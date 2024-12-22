#ifndef UTILS_H
#define UTILS_H


#include <obj/model.h>



/**
 * GLSL-like three dimensional vector
 */
typedef struct vec3
{
    float x;
    float y;
    float z;
} vec3;

/**
 * Color with RGB components
 */
typedef struct Color
{
    float red;
    float green;
    float blue;
} Color;

/**
 * Material
 */
typedef struct Material
{
    struct Color ambient;
    struct Color diffuse;
    struct Color specular;
    float shininess;
} Material;

/**
 * Objects
*/


double degree_to_radian(double degree);

double radian_to_degree(double radian);

float clampf(float min, float num, float max);

float absf(float num);

void copy_Vector(vec3 *target, const vec3 *src);

void init_Vector(vec3 *vector);

void add_Vector(vec3 *vector1, vec3 *vector2);

void sub_Vector(vec3 *vector1, vec3 *vector2);

void scalar_Mult(vec3 *vector, float scalar);

void set_vector(vec3 *vector, float x, float y, float z);



#endif /* UTILS_H */
