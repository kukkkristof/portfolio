#include "cat.h"
#include "angel.h"
#include "scene.h"
#include <obj/load.h>
#include <stdlib.h>
#include <time.h>
#include <math.h>

void initialize_cat_enemy(void *_scene, Cat *cat)
{
    Scene *scene = _scene;

    initialize_sceneobject(&(cat->body));
    load_model(&(cat->body.model), "assets/models/cat.obj");
    cat->body.textureID = load_texture("assets/textures/cube.png");
    cat->body.scale.x = 1;
    cat->body.scale.y = 1;
    cat->body.scale.z = 1;
    cat->body.position.z = 0;

    cat->is_latching = 0;
    cat->is_unlatching = 0;
    cat->is_jumping_at_player = 0;
    cat->is_attacking_player = 0;

    cat->latched = 0;
    cat->damage = 10;
    cat->jump_distance = 10;
    cat->jump_length = 0.1;
    cat->speed = 100;
    cat->stay_on_tree_max_time = 5;
    cat->stay_on_tree_time = 0;
    cat->latch_time = 2;
    cat->unlatch_time = 1;
    cat->latch_distance = 2;
    cat->maximum_attack_time = 3;
    cat->elapsed_attack_time = 0;
    
    cat_select_new_tree(scene, cat);

}
void cat_update(void *_scene, Cat *cat)
{
    Scene *scene = _scene;


    if(cat->is_attacking_player)
    {
        cat->elapsed_attack_time += scene->delta_time;
        player_blink(&(scene->player), 1);
        scene->player.blink_state_changed = 0;
        if(cat->elapsed_attack_time > cat->maximum_attack_time)
        {
            angel_blink(scene, &(scene->angel), rand()%100+1);
            player_damage(&(scene->player), 10);
            cat->body.position.z = 0;
            cat->is_attacking_player = 0;
            cat->elapsed_attack_time = 0;
        }
    }
    else if(cat->is_jumping_at_player)
    {
        animation_progress(&(cat->animation), scene->delta_time);
        if(cat->animation.t == 1)
        {
            cat->is_attacking_player = 1;
            cat->is_jumping_at_player = 0;
            cat->animation.active = 0;
            cat->stay_on_tree_time = 0;
            cat_select_new_tree(scene, cat);
        }
    }
    else if(cat->latched)
    {
        if(cat->is_latching)
        {
            animation_progress(&(cat->animation), scene->delta_time);
            if(cat->animation.t == 1)
            {
                cat->is_latching = 0;
                cat->animation.active = 0;
            }
        }
        else if(cat->is_unlatching)
        {
            animation_progress(&(cat->animation), scene->delta_time);
            if(cat->animation.t == 1)
            {
                cat->is_unlatching = 0;
                cat->latched = 0;
                cat->animation.active = 0;
                cat->stay_on_tree_time = 0;
                cat_select_new_tree(scene, cat);
            }
        }
        else
        {
            cat->stay_on_tree_time += scene->delta_time;

            if(cat->stay_on_tree_time >= cat->stay_on_tree_max_time)
            {
                cat->stay_on_tree_time = 0;
                cat->is_unlatching = 1;
                cat_create_unlatch_animation(scene, cat);
                cat->animation.active = 1;
            }
            else
            {
                float distance_from_player = pow(
                pow(scene->player.camera_pointer->position.x - cat->body.position.x, 2)
                + pow(scene->player.camera_pointer->position.y - cat->body.position.y, 2),
                0.5);

                if(distance_from_player <= cat->jump_distance)
                {
                    cat->is_jumping_at_player = 1;
                    cat->latched = 0;
                    cat->stay_on_tree_time = 0;
                    cat_create_jump_animation(scene, cat);
                    cat->animation.active = 1;
                }

            }
        }
        
    }
    else
    {
        cat_move_towards_selected_tree(scene, cat);

        float distance_from_tree = pow(
        pow(scene->trees[cat->selected_tree_index].position.x - cat->body.position.x, 2)
        + pow(scene->trees[cat->selected_tree_index].position.y - cat->body.position.y, 2),
        0.5);
    
        float deg =  atan2f(cat->body.position.y - scene->trees[cat->selected_tree_index].position.y,
                            cat->body.position.x - scene->trees[cat->selected_tree_index].position.x);    
    
        cat->body.rotation.z = radian_to_degree(deg);

        if(distance_from_tree < cat->latch_distance)
        {
            cat->latched = 1;
            cat->is_latching = 1;
            cat_create_latch_animation(scene, cat);
            cat->animation.active = 1;
        }

    }
}
void cat_move_towards_selected_tree(void *_scene, Cat *cat)
{
    Scene *scene = _scene;
    cat->body.position.x += clampf(-cat->speed,
    scene->trees[cat->selected_tree_index].position.x - cat->body.position.x,
    cat->speed) * scene->delta_time;

    cat->body.position.y += clampf(-cat->speed,
    scene->trees[cat->selected_tree_index].position.y - cat->body.position.y,
    cat->speed) * scene->delta_time;
}
void cat_select_new_tree(void *_scene, Cat *cat)
{
    Scene *scene = _scene;
    cat->selected_tree_index = rand() % scene->number_of_trees;
    cat_create_latch_animation(scene, cat);
}
void cat_create_jump_animation(void *_scene, Cat *cat)
{
    Scene *scene = _scene;
    initialize_animation(&(cat->animation), &(cat->body));

    copy_Vector(&(cat->animation.start_position),
                &(scene->trees[cat->selected_tree_index].position));

    copy_Vector(&(cat->animation.target_position),
                &(scene->player.camera_pointer->position));

    cat->animation.animate_rotation = 0;
    cat->animation.animate_scale = 0;

    cat->animation.start_position.z = 5;
    cat->animation.length = cat->jump_length;
    cat->animation.animation_curve = LINEARCURVE;
}
void cat_create_latch_animation(void *_scene, Cat *cat)
{
    Scene *scene = _scene;
    initialize_animation(&(cat->animation), &(cat->body));

    copy_Vector(&(cat->animation.start_position),
                &(scene->trees[cat->selected_tree_index].position));

    copy_Vector(&(cat->animation.target_position),
                &(scene->trees[cat->selected_tree_index].position));

    cat->animation.animate_rotation = 0;
    cat->animation.animate_scale = 0;

    cat->animation.target_position.z = 5;
    cat->animation.length = cat->latch_time;
    cat->animation.animation_curve = SINECURVE;

}
void cat_create_unlatch_animation(void *_scene, Cat *cat)
{
    Scene *scene = _scene;
    initialize_animation(&(cat->animation), &(cat->body));

    copy_Vector(&(cat->animation.start_position),
                &(scene->trees[cat->selected_tree_index].position));

    copy_Vector(&(cat->animation.target_position),
                &(scene->trees[cat->selected_tree_index].position));

    cat->animation.animate_rotation = 0;
    cat->animation.animate_scale = 0;
    
    cat->animation.start_position.z = 5;

    cat->animation.length = cat->unlatch_time;
    cat->animation.animation_curve = SINECURVE;
}