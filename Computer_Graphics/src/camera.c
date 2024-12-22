#include "camera.h"
#include "utils.h"
#include "texture.h"

#include <GL/gl.h>

#include <math.h>

int texture = 0;

void init_camera(Camera* camera)
{

    //texture = load_texture("assets/textures/cube.png");

    camera->position.x = 0.0;
    camera->position.y = 0.0;
    camera->position.z = 1;
    camera->rotation.x = 0.0;
    camera->rotation.y = 0.0;
    camera->rotation.z = 0.0;
    camera->speed.x = 0.0;
    camera->speed.y = 0.0;
    camera->speed.z = 0.0;

    camera->is_preview_visible = 0;
}

void set_view(const Camera* camera)
{
    glMatrixMode(GL_MODELVIEW);
    glLoadIdentity();

    glRotatef(-(camera->rotation.x + 90), 1.0, 0, 0);
    glRotatef(-(camera->rotation.z - 90), 0, 0, 1.0);
    glTranslatef(-camera->position.x, -camera->position.y, -camera->position.z);
}

void rotate_camera(Camera* camera, double horizontal, double vertical)
{
    camera->rotation.z += horizontal;
    camera->rotation.x += vertical;

    if (camera->rotation.z < 0) {
        camera->rotation.z += 360.0;
    }

    if (camera->rotation.z > 360.0) {
        camera->rotation.z -= 360.0;
    }

    if(camera->rotation.x < -90) camera->rotation.x = -90;
    else if(camera->rotation.x > 90) camera->rotation.x = 90;  
    //camera->rotation.x = clampf(-90, camera->rotation.x, 90);

}

void show_texture_preview()
{
    glDisable(GL_LIGHTING);
    glDisable(GL_DEPTH_TEST);
    glEnable(GL_COLOR_MATERIAL);

    glMatrixMode(GL_MODELVIEW);
    glLoadIdentity();

    glBindTexture(GL_TEXTURE_2D, texture);

    glColor3f(1, 1, 1);

    glBegin(GL_QUADS);
    glTexCoord2f(0, 0);
    glVertex3f(0, 0, -3);

    glTexCoord2f(0, 1);
    glVertex3f(0, 1, -3);

    glTexCoord2f(1, 1);
    glVertex3f(1, 1, -3);

    glTexCoord2f(1, 0);
    glVertex3f(1, 0, -3);
    glEnd();

    glDisable(GL_COLOR_MATERIAL);
    glEnable(GL_LIGHTING);
    glEnable(GL_DEPTH_TEST);
}
