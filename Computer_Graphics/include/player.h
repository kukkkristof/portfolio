#ifndef PLAYER_H
#define PLAYER_H

#include "camera.h"

typedef struct Player
{

    /*                  Player settings                 */
    Camera*             camera_pointer;
    float               movement_speed;
    float               player_height;
    

    /*                  Health                          */
    float               maximum_health;
    float               current_health;

    /*                  Movement in space               */
    float               falling_speed;
    float               maximum_stamina;
    float               stamina;
    float               stamina_regeneration_speed;
    int                 grounded;

    /*                  Blink mechanic                  */
    float               maximum_time_since_blink;
    float               elapsed_time_since_blink;
    int                 blink_state_changed;
    int                 is_blinking;

} Player;

void initialize_player(void* scene);
void player_update(void* scene, Player *player);
void player_blink(Player *player, int blink_state);
void player_damage(Player *player, float damage);


#endif /* PLAYER_H */