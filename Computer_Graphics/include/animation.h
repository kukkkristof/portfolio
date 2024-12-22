#ifndef ANIMATION_H
#define ANIMATION_H

#include "utils.h"
#include "sceneobj.h"

#define LINEARCURVE 0
#define SINECURVE 1

#define FORWARDS 0
#define BACKWARDS 1

/**
 * Basic "animation" structure
 * Interpolate between 2 keyframes
*/
typedef struct
{
    
    /*                      Object to be animated           */
    sceneObject*            object;

    /*                      Animation settings              */
    int                     active;
    int                     repeating;
    int                     backwards_repetetion;
    int                     direction;
    int                     animation_curve;
    int                     animate_position;
    int                     animate_rotation;
    int                     animate_scale;

    /*                      Animation  start                */
    vec3                    start_position;
    vec3                    start_rotation;
    vec3                    start_scale;

    /*                      Animation  end                  */
    vec3                    target_position;
    vec3                    target_rotation;
    vec3                    target_scale;

    /*                      Timing variables                */
    float                   length;         // seconds
    float                   passed_time;    // seconds 
    float                   t;              // timePassed/length

} Animation;


void initialize_animation(Animation *animation, sceneObject *object);

void animation_progress(Animation *animation, float time);

#endif //ANIMATION_H