#include "sceneobj.h"
#include "texture.h"

void initialize_sceneobject(sceneObject *object)
{
	init_Vector(&(object->position));
	init_Vector(&(object->rotation));

	object->scale.x = 1;
	object->scale.y = 1;
	object->scale.z = 1;

	object->material.ambient.red = 1;
	object->material.ambient.green = 1;
	object->material.ambient.blue = 1;

	object->material.diffuse.red = 1;
	object->material.diffuse.green = 1;
	object->material.diffuse.blue = 1;

	object->material.specular.red = 1;
	object->material.specular.green = 1;
	object->material.specular.blue = 1;

	object->material.shininess = 1;

}

void render_object(sceneObject *object)
{
	glPushMatrix();

	glTranslatef(object->position.x, object->position.y, object->position.z);

	glRotatef(object->rotation.x, 1, 0, 0);
	glRotatef(object->rotation.y, 0, 1, 0);
	glRotatef(object->rotation.z, 0, 0, 1);

	glScalef(object->scale.x, object->scale.y, object->scale.z);

	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glBindTexture(GL_TEXTURE_2D, object->textureID);
	draw_model(&(object->model));

	glPopMatrix();
}

int check_collision(vec3 *check_point, const sceneObject collision_object, int *grounded, float check_point_height)
{
	vec3 push_out_position;
	init_Vector(&push_out_position);
	push_out_position.x = check_point->x;
	push_out_position.y = check_point->y;
	push_out_position.z = check_point->z;

	if(collision_object.colliderType != NOCOLLIDER)
	{

		if(collision_object.colliderType == CYLINDERCOLLIDER)
		{
			
			float dist = powf(powf(collision_object.position.x - check_point->x,2) + powf(collision_object.position.y - check_point->y,2), 0.5f);

			if(dist < collision_object.collisionRange)
			{
				float deltaX = collision_object.position.x - check_point->x;
				float deltaY = collision_object.position.y - check_point->y;

				float angle = atan2f(deltaY, deltaX);

				push_out_position.x = collision_object.position.x - collision_object.collisionRange * cosf(angle);
				push_out_position.y = collision_object.position.y - collision_object.collisionRange * sinf(angle);

				copy_Vector(check_point, &push_out_position);

			}
			return 1;
		}

		if(collision_object.colliderType == BOXCOLLIDER)
		{    

   			// Calculate player's feet and head positions along the Z-axis
   			float playerFeetZ = check_point->z - check_point_height;
   			float playerHeadZ = check_point->z;

   			// Calculate cube's bounding box
   			float cubeMinX = collision_object.position.x - collision_object.collisionRange / 2.0f;
   			float cubeMaxX = collision_object.position.x + collision_object.collisionRange / 2.0f;
   			float cubeMinY = collision_object.position.y - collision_object.collisionRange / 2.0f;
   			float cubeMaxY = collision_object.position.y + collision_object.collisionRange / 2.0f;
   			float cubeMinZ = collision_object.position.z - collision_object.colliderHeight / 2.0f;
   			float cubeMaxZ = collision_object.position.z + collision_object.colliderHeight / 2.0f;

   			// Check for collision
   			int collisionX = (check_point->x >= cubeMinX) && (check_point->x <= cubeMaxX);
   			int collisionY = (check_point->y >= cubeMinY) && (check_point->y <= cubeMaxY);
   			int collisionZ = (playerFeetZ <= cubeMaxZ) && (playerHeadZ >= cubeMinZ);

   			int collision = collisionX && collisionY && collisionZ;

   			// Determine collision point for future reference
   			if (collision) {
   			    float dxMin = check_point->x - cubeMinX;
       			float dxMax = cubeMaxX - check_point->x;
       			float dyMin = check_point->y - cubeMinY;
       			float dyMax = cubeMaxY - check_point->y;
       			float dzMin = playerFeetZ - cubeMinZ;
       			float dzMax = cubeMaxZ - playerHeadZ;

       			float minDist = dxMin;
       			push_out_position.x = cubeMinX;
				push_out_position.y = check_point->y;
				push_out_position.z = check_point->z;

       			if (dxMax < minDist) {
       			    minDist = dxMax;
       			    push_out_position.x = cubeMaxX;
					push_out_position.y = check_point->y;
					push_out_position.z = check_point->z;
       			}
       			if (dyMin < minDist) {
       			    minDist = dyMin;
       			    push_out_position.x = check_point->x;
					push_out_position.y = cubeMinY;
					push_out_position.z = check_point->z;
       			}
       			if (dyMax < minDist) {
       			    minDist = dyMax;
       			    push_out_position.x = check_point->x;
					push_out_position.y = cubeMaxY;
					push_out_position.z = check_point->z;
       			}
       			if (dzMin < minDist) {
       			    minDist = dzMin;
       			    push_out_position.x = check_point->x;
					push_out_position.y = check_point->y;
					push_out_position.z = cubeMinZ;
       			}
       			if (dzMax < minDist) {
       			    minDist = dzMax;
       			    push_out_position.x = check_point->x;
					push_out_position.y = check_point->y;
					push_out_position.z = cubeMaxZ + check_point_height;
       			}
				copy_Vector(check_point, &push_out_position);
   			}

			if(collision)
			{
   				if (fabs(playerFeetZ - cubeMaxZ) < 0.1) {
   				    *grounded = 1;
   				} else {
   				    *grounded = 0;
   				}
			}

   			return collision;
		}
		
	}
	return 0;
}