#include "scene.h"
#include <obj/load.h>

void initialize_angel_enemy(void *_scene, Angel *angel)
{
    Scene *scene = _scene; 
    initialize_sceneobject(&(angel->body));
    load_model(&(angel->body.model), "assets/models/angelGood.obj");
    angel->body.textureID = load_texture("assets/textures/angel.jpg");

    angel->has_appeared = 0;

    angel->disappear_distance = 20;
    angel->disappear_chance = 1.0/10;
    angel->appear_chance = 1.0/60;
    angel->maximum_roam_time = 2;
    angel->current_roam_time = 0;
    
    angel->damage = 999;
    angel->hidden_speed = 1000;
    angel->visible_speed = 30;

    angel->attack_duration = 0;
    angel->attack_wait_time = 2;
    angel->attack_speed_multiplier = 1;

}

void angel_update(void *_scene, Angel *angel)
{
    Scene *scene = _scene; 
    if(!(angel->has_appeared))
    {
        float distance_from_player = pow(
                pow(scene->player.camera_pointer->position.x - angel->body.position.x, 2)
                + pow(scene->player.camera_pointer->position.y - angel->body.position.y, 2),
                0.5);
        
        if(angel->current_roam_time >= angel->maximum_roam_time)
        {
            angel->current_roam_time = 0;
            angel_select_new_roam_position(scene, angel);
            angel_move_to_roam_position(scene, angel);
        }
        else
        {
            angel_move_to_roam_position(scene, angel);
            angel->current_roam_time += scene->delta_time;
        }
    }
    else
    {
        angel->attack_duration += scene->delta_time;

        float distance_from_player = pow(
                pow(scene->player.camera_pointer->position.x - angel->body.position.x, 2)
                + pow(scene->player.camera_pointer->position.y - angel->body.position.y, 2),
                0.5);
        
        if(scene->player.is_blinking && distance_from_player < angel->disappear_distance)
        {
            if( angel->attack_duration >= angel->attack_wait_time)
            {
            angel->body.position.x += clampf(-angel->visible_speed,
                scene->player.camera_pointer->position.x - angel->body.position.x,
                angel->visible_speed) * scene->delta_time;

                angel->body.position.y += clampf(-angel->visible_speed,
                scene->player.camera_pointer->position.y - angel->body.position.y,
                angel->visible_speed) * scene->delta_time;
            }
            float deg = atan2f(angel->body.position.y - scene->player.camera_pointer->position.y,
            angel->body.position.x - scene->player.camera_pointer->position.x);    
            angel->body.rotation.z = radian_to_degree(deg-90);
        }

        distance_from_player = pow(
                pow(scene->player.camera_pointer->position.x - angel->body.position.x, 2)
                + pow(scene->player.camera_pointer->position.y - angel->body.position.y, 2),
                0.5);


        if(distance_from_player <= 1)
        {
            player_damage(&(scene->player), angel->damage);
        }
    }
}

void angel_blink(void *_scene, Angel *angel, int chance)
{
    Scene *scene = _scene;
    if(1.0/chance <= angel->appear_chance && !angel->has_appeared)
    {
        
    
        sceneObject push_object;
        push_object.position.x = scene->player.camera_pointer->position.x; 
        push_object.position.y = scene->player.camera_pointer->position.y;
        push_object.colliderType = CYLINDERCOLLIDER;
        push_object.collisionRange = angel->disappear_distance-0.2;
        int tmp;
        check_collision(&(angel->body.position), push_object, &tmp, 1);

        float deg = atan2f(angel->body.position.y - scene->player.camera_pointer->position.y,
            angel->body.position.x - scene->player.camera_pointer->position.x);    

        angel->body.rotation.z = radian_to_degree(deg-90);

        angel->has_appeared = 1;
    }
    else if(1.0/chance <= angel->disappear_chance && angel->has_appeared)
    {
        float distance_from_player = pow(
                pow(scene->player.camera_pointer->position.x - angel->body.position.x, 2)
                + pow(scene->player.camera_pointer->position.y - angel->body.position.y, 2),
                0.5);
        if(distance_from_player >= angel->disappear_distance)
        {
            angel->attack_duration = 0;
            angel->has_appeared = 0;
        }
    }
}

void angel_select_new_roam_position(void *_scene, Angel *angel)
{
    Scene *scene = _scene; 
    int randDeg = rand() % 360;
    float xpos = scene->player.camera_pointer->position.x + sinf(degree_to_radian(randDeg)) * angel->disappear_distance * 0.5;
    float ypos = scene->player.camera_pointer->position.y + cosf(degree_to_radian(randDeg)) * angel->disappear_distance * 0.5;
    angel->roam_position.x = xpos;
    angel->roam_position.y = ypos;
}

void angel_move_to_roam_position(void *_scene, Angel *angel)
{
    Scene *scene = _scene; 
    angel->body.position.x += clampf(-angel->hidden_speed,
    angel->roam_position.x - angel->body.position.x,
    angel->hidden_speed) * scene->delta_time;

    angel->body.position.y += clampf(-angel->hidden_speed,
    angel->roam_position.y - angel->body.position.y,
    angel->hidden_speed) * scene->delta_time;

    
}