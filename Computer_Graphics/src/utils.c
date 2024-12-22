#include "utils.h"

#include <math.h>
#include <obj/draw.h>
#include <gl/gl.h>

double degree_to_radian(double degree)
{
	return degree * M_PI / 180.0;
}

double radian_to_degree(double radian)
{
	return radian / M_PI * 180.0;
}

float clampf(float min, float num, float max)
{

	return (min > num) ? min : ((max < num) ? max : num);
}

float absf(float num)
{
	return num>0?num:num*-1;
}


void init_Vector(vec3 *vector)
{
	vector->x = 0;
	vector->y = 0;
	vector->z = 0;
}

void copy_Vector(vec3 *target, const vec3 *src)
{
	target->x = src->x;
	target->z = src->z;
	target->y = src->y;
}

void add_Vector(vec3 *vector1, vec3 *vector2)
{
	vector1->x += vector2->x;
	vector1->y += vector2->y;
	vector1->z += vector2->z;
}

void sub_Vector(vec3 *vector1, vec3 *vector2)
{
	vector1->x -= vector2->x;
	vector1->y -= vector2->y;
	vector1->z -= vector2->z;
}


void scalar_Mult(vec3 *vector, float scalar)
{
	vector->x *= scalar;
	vector->y *= scalar;
	vector->z *= scalar;
}


void set_vector(vec3 *vector, float x, float y, float z)
{
	vector->x = x;
	vector->y = y;
	vector->z = z;
}