#include "player.h"
#include "app.h"
#include "scene.h"

void initialize_player(void* _scene)
{
    Scene *scene = _scene; 

    scene->player.movement_speed = 2;
    scene->player.player_height = 1;
    
    scene->player.maximum_health = 100;
    scene->player.current_health = 100;

    scene->player.falling_speed = 0;
    scene->player.maximum_stamina = 5;
    scene->player.stamina = 5;
    scene->player.stamina_regeneration_speed = 0.5;
    scene->player.grounded = 1;
    
    scene->player.maximum_time_since_blink = 60;
    scene->player.elapsed_time_since_blink = 0;
    scene->player.blink_state_changed = 0;
    scene->player.is_blinking = 0;
}

void player_update(void* _scene, Player *player)
{
    Scene *scene = _scene; 

    if(!player->current_health) scene->game_state = 2;

    float sprint = getInput(SDL_SCANCODE_LSHIFT);
    float jump = getInput(SDL_SCANCODE_SPACE);
    float sneak = getInput(SDL_SCANCODE_LCTRL);
    float blink = getInput(SDL_SCANCODE_E);


    if(sprint) player->stamina -= scene->delta_time;
    else player->stamina += player->stamina_regeneration_speed * scene->delta_time;

    player->stamina = clampf(0, player->stamina, player->maximum_stamina);

    if(sprint && player->stamina != 0) sprint = sprint*3;
    else sprint = 1;

    float horizontal = (getInput(SDL_SCANCODE_S) - getInput(SDL_SCANCODE_W)) * scene->delta_time * scene->player.movement_speed * sprint;
    float vertical = (getInput(SDL_SCANCODE_D) - getInput(SDL_SCANCODE_A)) * scene->delta_time * scene->player.movement_speed * sprint ;

    float rotation = scene->player.camera_pointer->rotation.z;


    float moveX = sinf(degree_to_radian(rotation - 90)) * horizontal + cosf(degree_to_radian(rotation - 90)) * vertical;
    float moveY = sinf(degree_to_radian(rotation - 90)) * vertical - cosf(degree_to_radian(rotation - 90)) * horizontal;
    scene->player.camera_pointer->position.x = clampf(-PLAYGROUNDAREA/2, scene->player.camera_pointer->position.x + moveX, PLAYGROUNDAREA/2);
    scene->player.camera_pointer->position.y = clampf(-PLAYGROUNDAREA/2, scene->player.camera_pointer->position.y + moveY, PLAYGROUNDAREA/2);


    if(sneak)
    {
        player->player_height = 0.5f;
        if(player->grounded) scene->player.camera_pointer->position.z = player->player_height;
    }
    else player->player_height = 1.0f;



    if(player->grounded && jump)
    {
        player->grounded = 0;
        player->falling_speed = 0.05;
        if(sneak) player->falling_speed += 0.025;
    }

    scene->player.camera_pointer->position.z += player->falling_speed;
    scene->player.camera_pointer->position.z = clampf(player->player_height, scene->player.camera_pointer->position.z, 100);

    if(!player->grounded) 
    {
        player->falling_speed -= GRAVITY * scene->delta_time;
        if(absf(scene->player.camera_pointer->position.z - (player->player_height)) < 0.01f)
        {
            player->grounded = 1;
            player->falling_speed = 0;
        }
    }

    player_blink(player, blink);

    player->elapsed_time_since_blink = clampf(0, player->elapsed_time_since_blink + scene->delta_time, player->maximum_time_since_blink);
}

void player_blink(Player *player, int blink_state)
{
    if(blink_state != player->is_blinking)
    {
        player->blink_state_changed = 1;
        player->is_blinking = blink_state;
    }
    else player->blink_state_changed = 0;
    
    if(player->is_blinking)
    {
        player->elapsed_time_since_blink = 0;
    }
}

void player_damage(Player *player, float damage)
{
    player->current_health = clampf(0, player->current_health - damage,player->maximum_health);
    float fogColor[] = { 1.0 - player->current_health / player->maximum_health,0,0,1};
    glClearColor(fogColor[0], 0, 0, 1.0);
    glFogfv(GL_FOG_COLOR, fogColor); 
}