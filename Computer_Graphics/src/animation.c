#include "animation.h"
#include "utils.h"

#include <math.h>

void initialize_animation(Animation *animation, sceneObject *object)
{

    animation->passed_time = 0;
    animation->direction = FORWARDS;

    animation->length = 5;

    animation->animate_position = 1;
    animation->animate_rotation = 1;
    animation->animate_scale = 1;

    animation->object = object;

}

void animation_progress(Animation *animation, float time)
{

    if(animation->active)
    {

        animation->passed_time = clampf(0, animation->passed_time + time * (animation->direction?-1:1), animation->length);
        animation->t = animation->passed_time / animation->length;

        if(animation->animation_curve == SINECURVE)
        {
            
            animation->t = sinf(degree_to_radian(animation->t * 90.0f));
            
        }

        sceneObject *objPtr = animation->object;

        if(animation->animate_position)
        {
            
            vec3 realPos;
            init_Vector(&realPos);
            add_Vector(&realPos, &(animation->target_position));
            sub_Vector(&realPos, &(animation->start_position));
            scalar_Mult(&realPos, animation->t);
            add_Vector(&realPos, &(animation->start_position));
            copy_Vector(&(objPtr->position), &realPos);
            
        }

        if(animation->animate_rotation)
        {
            vec3 realRot;
            init_Vector(&realRot);
            add_Vector(&realRot, &(animation->target_rotation));
            sub_Vector(&realRot, &(animation->start_rotation));
            scalar_Mult(&realRot, animation->t);
            add_Vector(&realRot, &(animation->start_rotation));
            copy_Vector(&(objPtr->rotation), &realRot);
        }

        if(animation->animate_scale)
        {
            vec3 realScale;
            init_Vector(&realScale);
            add_Vector(&realScale, &(animation->target_scale));
            sub_Vector(&realScale, &(animation->start_scale));
            scalar_Mult(&realScale, animation->t);
            add_Vector(&realScale, &(animation->start_scale));
            copy_Vector(&(objPtr->scale), &realScale);
        }

        if((animation->repeating && ((animation->direction == FORWARDS && animation->t == 1) || (animation->direction == BACKWARDS && animation->t == 0))))
        {
            if(animation->backwards_repetetion)
                animation->direction = !(animation->direction);
            else
                animation->passed_time = 0;
        }

    }

}