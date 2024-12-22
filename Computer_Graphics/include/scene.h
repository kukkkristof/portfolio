#ifndef SCENE_H
#define SCENE_H

#include "player.h"
#include "cat.h"
#include "angel.h"
#include "utils.h"
#include "texture.h"
#include "animation.h"
#include "sceneobj.h"

#include <obj/model.h>

#define GRAVITY 0.1
#define PLAYGROUNDAREA 100

typedef struct Scene
{

    /*                      Scene information                       */
    int                     object_count;
    float                   delta_time;
    int                     number_of_trees;
    int                     paper_count;
    int                     found_papers;
    Player                  player;
    Cat                     cat;
    Angel                   angel;
    int                     show_help;
    int                     show_death;
    int                     show_end;
    int                     info_texture;
    int                     death_texture;
    int                     end_texture;
    int                     game_state;

    /*
     *  Prefabs are used to create multiple instances of the same object
     */

    /*                      Prefabs                                 */
    sceneObject             tree_prefab;
    sceneObject             leaves_prefab;
    sceneObject             paper_prefab;

    /*                      Static objects in the scene             */
    sceneObject*            objects;
    sceneObject*            trees;
    sceneObject*            leaves;
    sceneObject*            papers;

} Scene;

/**
 * Initialize the scene by loading models.
 */
void init_scene(Scene* scene);

/**
 * Set the lighting of the scene.
 */
void set_lighting();


/**
 * Set the currently used material.
 */
void set_material(const Material* material);

/**
 * Update the scene.
 */
void update_scene(Scene* scene);

/**
 * Render the scene (enemies, static objects, objectives).
 */
void render_scene(const Scene* scene);


void check_all_collisions(Scene *scene);
void initialize_prefabs(Scene *scene);
void loadFromFile(Scene *scene);
void check_pickup(Scene *scene);

void render_UI_texture(int texture_id, int left, int up, int right, int down);
void render_help_menu(Scene *scene);
void render_death_menu(Scene *scene);
void render_finish_menu(Scene *scene);

#endif /* SCENE_H */
