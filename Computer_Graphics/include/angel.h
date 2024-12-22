#ifndef ANGEL_H
#define ANGEL_H

#include "sceneobj.h"

typedef struct Angel
{
    /**
     * An angel is an enemy type, which can appear when the player blinks
     * Only moves when the player blinks, it can appear it can disappear at will, but if its close to the player it cannot disappear 
    */

    /*                  Angel settings                      */
    float               disappear_distance;
    float               damage;
    float               hidden_speed;
    float               visible_speed;
    float               appear_chance;          // 1/num (ex. 1/5 (20%))
    float               disappear_chance;       // 1/num (ex. 1/5 (20%))
    int                 has_appeared;
    sceneObject         body;

    /*                  Roaming settings                    */
    vec3                roam_position;
    float               roam_player_distance;
    float               maximum_roam_time;
    float               current_roam_time;

    /*                  Attack settings                     */
    float               attack_duration;
    float               attack_wait_time;
    float               attack_speed_multiplier;

} Angel;


void initialize_angel_enemy(void *_scene, Angel *angel);
void angel_blink(void *_scene, Angel *angel, int chance);
void angel_update(void *_scene, Angel *angel);
void angel_select_new_roam_position(void *_scene, Angel *angel);
void angel_move_to_roam_position(void *_scene, Angel *angel);

#endif