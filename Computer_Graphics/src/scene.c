#include "scene.h"
#include "app.h"
#include "cat.h"
#include "angel.h"
#include "perlin.h"

#include <obj/load.h>
#include <obj/draw.h>
#include <string.h>
#include <stdlib.h>
#include <math.h>
#include <time.h>

float light_intensity = 1;

void init_scene(Scene* scene)
{

    scene->game_state = 1;

    scene->info_texture = load_transparent_texture("assets/textures/help.png");
    scene->death_texture = load_transparent_texture("assets/textures/death.png");
    scene->end_texture = load_transparent_texture("assets/textures/keksz.png");

    glEnable(GL_ALPHA_TEST);
    glAlphaFunc(GL_GREATER, 0.5);
    glEnable(GL_FOG);

    scene->paper_count = 5;
    scene->found_papers = 0;

    srand(time(NULL));

    loadFromFile(scene);

    initialize_prefabs(scene);
    generate_trees(scene);
    generate_papers(scene);

    initialize_player(scene);
    initialize_cat_enemy(scene, &(scene->cat));
    initialize_angel_enemy(scene, &(scene->angel));
}

void set_lighting()
{
    float ambient_light[] =  { light_intensity, light_intensity, light_intensity, 1.0f };
    float diffuse_light[] =  { light_intensity, light_intensity, light_intensity, 1.0f };
    float specular_light[] = { light_intensity, light_intensity, light_intensity, 1.0f };
    float position[] = { 0.0f, 0.0f, 10.0f, 1.0f };

    glLightfv(GL_LIGHT0, GL_AMBIENT, ambient_light);
    glLightfv(GL_LIGHT0, GL_DIFFUSE, diffuse_light);
    glLightfv(GL_LIGHT0, GL_SPECULAR, specular_light);
    glLightfv(GL_LIGHT0, GL_POSITION, position);
}

void set_material(const Material* material)
{
    float ambient_material_color[] = {
        material->ambient.red,
        material->ambient.green,
        material->ambient.blue
    };

    float diffuse_material_color[] = {
        material->diffuse.red,
        material->diffuse.green,
        material->diffuse.blue
    };

    float specular_material_color[] = {
        material->specular.red,
        material->specular.green,
        material->specular.blue
    };

    glMaterialfv(GL_FRONT_AND_BACK, GL_AMBIENT, ambient_material_color);
    glMaterialfv(GL_FRONT_AND_BACK, GL_DIFFUSE, diffuse_material_color);
    glMaterialfv(GL_FRONT_AND_BACK, GL_SPECULAR, specular_material_color);

    glMaterialfv(GL_FRONT_AND_BACK, GL_SHININESS, &(material->shininess));
}

void update_scene(Scene* scene)
{
    float f1 = getInput(SDL_SCANCODE_F1);
    if(!f1 && scene->game_state)
    {

        float comma = getInput(SDL_SCANCODE_COMMA);
        float dot = getInput(SDL_SCANCODE_PERIOD);
        light_intensity += (dot- comma) * scene->delta_time;
        light_intensity = clampf(0.2, light_intensity, 0.8);

        scene->show_help = 0;
        player_update(scene, &(scene->player));
        cat_update(scene, &(scene->cat));
        angel_update(scene, &(scene->angel));

        check_all_collisions(scene);

        float F = getInput(SDL_SCANCODE_F);
        if(F > 0)
        {
            check_pickup(scene);
        }

        if(scene->player.blink_state_changed && scene->player.is_blinking) angel_blink(scene, &(scene->angel), rand()%100+1);


        if(scene->player.is_blinking)
        {
            glFogf(GL_FOG_DENSITY, 1000);
        }
        else {
            float fogDensity = clampf(0.02, scene->player.elapsed_time_since_blink / scene->player.maximum_time_since_blink, 1);
            glFogf(GL_FOG_DENSITY, fogDensity);
        }
    }
    else
    {
        scene->show_help = 1;
    }
}

void check_all_collisions(Scene *scene)
{

    int i;
    int tmp;

    for(i = 0; i < scene->number_of_trees; i++)
    {
        check_collision(&(scene->player.camera_pointer->position), scene->trees[i], &(scene->player.grounded), scene->player.player_height);
        check_collision(&(scene->cat.body.position), scene->trees[i], &tmp, scene->cat.body.scale.z);
        if(scene->angel.has_appeared)
        {
            check_collision(&(scene->angel.body.position), scene->trees[i], &tmp, scene->angel.body.scale.z);
        }
    }

}

void render_scene(const Scene* scene)
{
    set_lighting();

    for(int i = 0; i < scene->object_count; i++)
    {
        render_object(&(scene->objects[i]));
    }

    for(int i = 0; i < scene->number_of_trees; i++)
    {
        render_object(&(scene->trees[i]));
        render_object(&(scene->leaves[i]));
    }

    render_object(&(scene->papers[scene->found_papers]));

    render_object(&(scene->cat.body));
    render_object(&(scene->trees[scene->cat.selected_tree_index]));
    render_object(&(scene->leaves[scene->cat.selected_tree_index]));

    if(scene->angel.has_appeared) render_object(&(scene->angel.body));

    if(scene->show_help && scene->game_state == 1)
        render_help_menu(scene);
    else if (scene->game_state == 2) {
        render_death_menu(scene);
    }
    else if(scene->game_state == 3) {
        render_finish_menu(scene);
    }
}

void initialize_prefabs(Scene *scene)
{

    initialize_sceneobject(&(scene->tree_prefab));
    initialize_sceneobject(&(scene->leaves_prefab));
    initialize_sceneobject(&(scene->paper_prefab));

    load_model(&(scene->tree_prefab.model), "assets/models/tree.obj");
    scene->tree_prefab.textureID = load_transparent_texture("assets/textures/tree.png");

    load_model(&(scene->leaves_prefab.model), "assets/models/leaves.obj");
    scene->leaves_prefab.textureID = load_transparent_texture("assets/textures/leaves.png");

    load_model(&(scene->paper_prefab.model), "assets/models/paper.obj");
    scene->paper_prefab.textureID = load_texture("assets/textures/FAKERWHATWASTHAT.jpeg");

    set_vector(&(scene->tree_prefab.scale.x), 0.3f, 0.3f, 0.3f);
    set_vector(&(scene->leaves_prefab.scale.x), 0.3f, 0.3f, 0.3f);
    set_vector(&(scene->paper_prefab.scale.x), 0.3f, 0.3f, 0.3f);


    scene->tree_prefab.colliderType = CYLINDERCOLLIDER;
    scene->tree_prefab.collisionRange = 0.95f;

}

void loadFromFile(Scene *scene)
{

    FILE *file;

    file = fopen("objectData.txt", "r");
    int objCount, transparent, colliderType;

    fscanf(file, "%d\n", &objCount);

    char model[255];
    char texture[255];

    float posX, posY, posZ,
          rotX, rotY, rotZ,
          scaleX, scaleY, scaleZ,
          matAmbR, matAmbG, matAmbB,
          matDiffR, matDiffG, matDiffB,
          matSpecR, matSpecG, matSpecB,
          matShin, colliderRange, colliderHeight;

    scene->objects = malloc(sizeof(sceneObject) * objCount);
    scene->object_count = objCount;
    for (int i = 0; i < objCount; i++)
    { 
        fscanf(file, "%s %f %f %f %f %f %f %f %f %f %s %f %f %f %f %f %f %f %f %f %f %d %d %f %f\n",
            &model,                             // Objects model
            &posX, &posY, &posZ,                // Object position in space
            &rotX, &rotY, &rotZ,                // Object rotation
            &scaleX, &scaleY, &scaleZ,          // Object scale
            &texture,                           // Objects texture
            &matAmbR, &matAmbG, &matAmbB,       // Objects material ambient
            &matDiffR, &matDiffG, &matDiffB,    // Objects material diffuse
            &matSpecR, &matSpecG, &matSpecB,    // Objects material specular
            &matShin,                           // Material shininess
            &transparent,                       // Texture transparency
            &colliderType,                      // Collider type
            &colliderRange, &colliderHeight);   // Collider range and height
        
        load_model(&(scene->objects[i].model), model);
        if(transparent) scene->objects[i].textureID = load_transparent_texture(texture);
        else scene->objects[i].textureID = load_texture(texture);

        scene->objects[i].position.x = posX;
	    scene->objects[i].position.y = posY;
	    scene->objects[i].position.z = posZ;
        
	    scene->objects[i].rotation.x = rotX;
	    scene->objects[i].rotation.y = rotY;
	    scene->objects[i].rotation.z = rotZ;

	    scene->objects[i].scale.x = scaleX;
	    scene->objects[i].scale.y = scaleY;
	    scene->objects[i].scale.z = scaleZ;

	    scene->objects[i].material.ambient.red = matAmbR;
	    scene->objects[i].material.ambient.green = matAmbG;
	    scene->objects[i].material.ambient.blue = matAmbB;

	    scene->objects[i].material.diffuse.red = matDiffR;
	    scene->objects[i].material.diffuse.green = matDiffG;
	    scene->objects[i].material.diffuse.blue = matDiffB;

	    scene->objects[i].material.specular.red = matSpecR;
	    scene->objects[i].material.specular.green = matSpecG;
	    scene->objects[i].material.specular.blue = matSpecB;

	    scene->objects[i].material.shininess = matShin;

        scene->objects[i].colliderType = colliderType;
        scene->objects[i].collisionRange = colliderRange;
        scene->objects[i].colliderHeight = colliderHeight;
    }


    fclose(file);

}

void generate_papers(Scene *scene)
{
    scene->papers = malloc(sizeof(sceneObject) * scene->paper_count);
    for(int i = 0; i < scene->paper_count; i++)
    {
        int selected_tree = rand()%scene->number_of_trees;
        memcpy(&(scene->papers[i]),&(scene->paper_prefab), sizeof(sceneObject));
        scene->papers[i].position.x = scene->trees[selected_tree].position.x + (rand() * 0.1f / RAND_MAX - 0.1f);
        scene->papers[i].position.y = scene->trees[selected_tree].position.y + (rand() * 0.1f / RAND_MAX - 0.1f);
        scene->papers[i].position.z = 0.8;
        int tmp;

        float range = scene->trees[selected_tree].collisionRange;
        scene->trees[selected_tree].collisionRange = 0.6f;
        check_collision(&(scene->papers[i].position), scene->trees[selected_tree], tmp, 1);
        scene->trees[selected_tree].collisionRange = range;
        float deg =  atan2f(scene->papers[i].position.y - scene->trees[selected_tree].position.y,
                            scene->papers[i].position.x - scene->trees[selected_tree].position.x);    
    
        scene->papers[i].rotation.z = radian_to_degree(deg);

    }
}

void generate_trees(Scene *scene)
{
    srand(time(NULL));

    scene->number_of_trees = 0;

    int gridSize = 100;

    float minX = -PLAYGROUNDAREA / 2;
    float minY = -PLAYGROUNDAREA / 2;
    float step = PLAYGROUNDAREA / gridSize;

    float cutoff = 0.995;
    
    int seed = rand();

    for (int x = 0; x < gridSize; x++)
    {
        for (int y = 0; y < gridSize; y++)
        {
            if(Perlin_Get2d(minX + x * step, minY + y * step, 5, 1, seed) > cutoff) scene->number_of_trees++;
        }
    }
    scene->trees = malloc(sizeof(sceneObject) * scene->number_of_trees);
    scene->leaves = malloc(sizeof(sceneObject) * scene->number_of_trees);
    int tmp = 0;
    for (int x = 0; x < gridSize; x++)
    {
        for (int y = 0; y < gridSize; y++)
        {
            if(Perlin_Get2d(minX + x * step, minY + y * step, 5, 1, seed) > cutoff)
            {
                memcpy(&(scene->trees[tmp]),&(scene->tree_prefab), sizeof(sceneObject));
                memcpy(&(scene->leaves[tmp]),&(scene->leaves_prefab), sizeof(sceneObject));
                scene->trees[tmp].position.x = minX + x * step;
                scene->trees[tmp].position.y = minY + y * step;

                scene->leaves[tmp].position.x = minX + x * step;
                scene->leaves[tmp].position.y = minY + y * step;
                tmp++;
            }
        }
    }
}

void check_pickup(Scene *scene)
{
    scene->papers[scene->found_papers].colliderType = BOXCOLLIDER;
    scene->papers[scene->found_papers].collisionRange = 2;
    scene->papers[scene->found_papers].colliderHeight = 2;

    float ray_length = 2;
    for(int i = 0; i <= 10; i++)
    {
        vec3 ray_position;
        ray_position.x = scene->player.camera_pointer->position.x + sinf(degree_to_radian(scene->player.camera_pointer->rotation.z)) * (ray_length / 10) * i;
        ray_position.y = scene->player.camera_pointer->position.y + cosf(degree_to_radian(scene->player.camera_pointer->rotation.z)) * (ray_length / 10) * i;
        ray_position.z = scene->player.camera_pointer->position.z + cosf(degree_to_radian(-(scene->player.camera_pointer->rotation.z) + 90)) * (ray_length / 10) * i;
        int tmp;
        if(check_collision(&ray_position, scene->papers[scene->found_papers], &tmp, 0.1))
        {
            scene->found_papers++;
        }
    }
    scene->papers[scene->found_papers].colliderType = NOCOLLIDER;
    scene->papers[scene->found_papers].collisionRange = 0;
    scene->papers[scene->found_papers].colliderHeight = 0;

    if(scene->found_papers == scene->paper_count)
    {
        scene->game_state = 3;
    }

}

void render_UI_texture(int texture_id, int left, int up, int right, int down) { 
    glPushAttrib(GL_ENABLE_BIT|GL_TEXTURE_BIT|GL_LIGHTING_BIT);

    glDisable(GL_LIGHTING);
    glDisable(GL_DEPTH_TEST);

    glEnable(GL_BLEND);
    glEnable(GL_TEXTURE_2D);
    glEnable(GL_ALPHA_TEST);

    GLint viewport[4];
    glGetIntegerv(GL_VIEWPORT,viewport);

    glMatrixMode(GL_PROJECTION);

    glColor3f(1,1,1);

    glPushMatrix();

        glLoadIdentity();
        glOrtho(0, viewport[2], 0, viewport[3], -1, 1);

        glMatrixMode(GL_MODELVIEW);
        glPushMatrix();

            glLoadIdentity();

            glBindTexture(GL_TEXTURE_2D, texture_id);
            glBegin(GL_QUADS);

                glTexCoord2f(0.0f, 1.0f); glVertex2f(left,down);
                glTexCoord2f(1.0f, 1.0f); glVertex2f(right,down);
                glTexCoord2f(1.0f, 0.0f); glVertex2f(right,up);
                glTexCoord2f(0.0f, 0.0f); glVertex2f(left,up);

            glEnd();

        glPopMatrix();

        glMatrixMode(GL_PROJECTION);
    glPopMatrix();

    glMatrixMode(GL_MODELVIEW);
    glPopAttrib();

    glEnable(GL_LIGHTING);
    glEnable(GL_DEPTH_TEST);
}

void render_help_menu(Scene *scene)
{
    GLint viewport[4];
    glGetIntegerv(GL_VIEWPORT,viewport);
    render_UI_texture(scene->info_texture, 0, viewport[3], viewport[2], 0);
}
void render_death_menu(Scene *scene)
{
    GLint viewport[4];
    glGetIntegerv(GL_VIEWPORT,viewport);
    render_UI_texture(scene->death_texture, 0, viewport[3], viewport[2], 0);
}
void render_finish_menu(Scene *scene)
{
    GLint viewport[4];
    glGetIntegerv(GL_VIEWPORT,viewport);
    render_UI_texture(scene->end_texture, 0, viewport[3], viewport[2], 0);
}