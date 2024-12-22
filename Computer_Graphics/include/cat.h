#ifndef CAT_H
#define CAT_H

#include "sceneobj.h"
#include "camera.h"
#include "animation.h"

typedef struct Cat {

    /**
     * A cat is an enemy type, it works like this:
     * The cat selects a tree in the map, and goes to the selected tree
     * When it reaches the tree, it climbs on top, then waits for the player to get close
     * When the player is close enough, it jumps on the player scaring,
     * and possibly killing them
     * If the cat waits for a specified amount of time and the player is not
     * close in that interval, it leaves the tree and goes to an other
    */

    /*                  Cat settings                    */
    float               speed;
    float               jump_distance;
    float               stay_on_tree_max_time;
    float               stay_on_tree_time;
    float               latch_time;
    float               unlatch_time;
    float               latch_distance;
    float               damage;
    float               jump_length;
    float               maximum_attack_time;
    float               elapsed_attack_time;
    int                 is_attacking_player;
    int                 is_jumping_at_player;
    sceneObject         body;
    Animation           animation;

    /*                  Latched tree params             */
    int                 is_latching;
    int                 is_unlatching;
    int                 latched;
    int                 selected_tree_index;
    

} Cat;

void initialize_cat_enemy(void *_scene, Cat *cat);
void cat_update(void *_scene, Cat *cat);
void cat_move_towards_selected_tree(void *_scene, Cat *cat);
void cat_select_new_tree(void *_scene, Cat *cat);
void cat_create_jump_animation(void *_scene, Cat *cat);
void cat_create_latch_animation(void *_scene, Cat *cat);
void cat_create_unlatch_animation(void *_scene, Cat *cat);

#endif /* CAT_H */