#ifndef SCENEOBJ_H
#define SCENEOBJ_H

#include "utils.h"
#include <obj/model.h>


/**/
#define NOCOLLIDER 0
#define SPHERECOLLIDER 1
#define CYLINDERCOLLIDER 2
#define BOXCOLLIDER 3

typedef struct sceneObject
{

    /*          Transform           */    
    vec3        position;
    vec3        rotation;
    vec3        scale;

    /*          Rendering           */
    int         textureID;
    Model       model;
    Material    material;

    /*          Collider settings   */
    int         colliderType;
    float       collisionRange;
    float       colliderHeight;

} sceneObject;

void initialize_sceneobject(sceneObject *object);

void render_object(sceneObject *object);

int check_collision(vec3 *check_point, const sceneObject collision_object, int *grounded, float check_point_height);
#endif /* SCENEOBJ_H */