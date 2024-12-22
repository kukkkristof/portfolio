#include "app.h"

#include <SDL2/SDL_image.h>
#include <stdio.h>

float axisArray[SDL_NUM_SCANCODES];

#define MOUSESENSITIVITY 4

void handle_app_events(App* app)
{
    SDL_Event event;
    static bool rotateCamera = false;
    int width;
    int height;

    while (SDL_PollEvent(&event)) {
        switch (event.type) {
        case SDL_KEYDOWN:
            switch (event.key.keysym.scancode) {
            case SDL_SCANCODE_ESCAPE:
                app->is_running = false;
                break;
            default:
                axisArray[event.key.keysym.scancode] = 1;
                break;
            }
            break;
        case SDL_KEYUP:
            axisArray[event.key.keysym.scancode] = 0;
            break;
        case SDL_MOUSEBUTTONDOWN:
            if(event.button.button == SDL_BUTTON_RIGHT)
            {

                rotateCamera = !rotateCamera;

                SDL_ShowCursor(!rotateCamera);
                SDL_SetWindowGrab(app->window, rotateCamera);
                SDL_SetRelativeMouseMode(rotateCamera);
            }
            break;
        case SDL_MOUSEMOTION:
            if (rotateCamera) {
                rotate_camera(&(app->camera), -event.motion.xrel * app->scene.delta_time * MOUSESENSITIVITY,
                                              -event.motion.yrel * app->scene.delta_time * MOUSESENSITIVITY);
            }
            break;
        
        case SDL_WINDOWEVENT:
            switch (event.window.event)
            {
                case SDL_WINDOWEVENT_RESIZED:
                    SDL_GetWindowSize(app->window, &width,&height);
                    reshape(width, height);
                break;
            }
            break;

        case SDL_QUIT:
            app->is_running = false;
            break;
        default:
            break;
        }
    }
}

float getInput(int axis)
{
    if(axis < SDL_NUM_SCANCODES)
    {
        return axisArray[axis];
    }
    return 0;
}

#pragma region  DONT TOUCH

void init_app(App* app, int width, int height)
{

    

    int error_code;
    int inited_loaders;

    app->is_running = false;

    error_code = SDL_Init(SDL_INIT_EVERYTHING);
    if (error_code != 0) {
        printf("[ERROR] SDL initialization error: %s\n", SDL_GetError());
        return;
    }

    app->window = SDL_CreateWindow(
        "P2MZHY Graphics Assigment!",   
        SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED,
        width, height,
        SDL_WINDOW_OPENGL|SDL_WINDOW_RESIZABLE);
    if (app->window == NULL) {
        printf("[ERROR] Unable to create the application window!\n");
        return;
    }
    SDL_SetWindowMinimumSize(app->window, width, height);

    inited_loaders = IMG_Init(IMG_INIT_PNG);
    if (inited_loaders == 0) {
        printf("[ERROR] IMG initialization error: %s\n", IMG_GetError());
        return;
    }

    app->gl_context = SDL_GL_CreateContext(app->window);
    if (app->gl_context == NULL) {
        printf("[ERROR] Unable to create the OpenGL context!\n");
        return;
    }

    init_opengl();
    reshape(width, height);

    init_camera(&(app->camera));
    init_scene(&(app->scene));

    app->scene.player.camera_pointer = &(app->camera);

    app->is_running = true;
    app->uptime = 0;
}

void update_app(App* app)
{
    double current_time;

    current_time = (double)SDL_GetTicks() / 1000;
    app->scene.delta_time = current_time - app->uptime;
    app->uptime = current_time;
    update_scene(&(app->scene));
}

void init_opengl()
{
    glShadeModel(GL_SMOOTH);

    glEnable(GL_NORMALIZE);
    glEnable(GL_AUTO_NORMAL);

    glClearColor(0, 0, 0, 1.0);

    glMatrixMode(GL_MODELVIEW);
    glLoadIdentity();

    glEnable(GL_DEPTH_TEST);

    glClearDepth(1.0);

    glEnable(GL_TEXTURE_2D);

    glEnable(GL_LIGHTING);
    glEnable(GL_LIGHT0);
}

void reshape(GLsizei width, GLsizei height)
{

    glViewport(0, 0, width, height);

    glMatrixMode(GL_PROJECTION);
    glLoadIdentity();

    float LeftRight = width / 2.0f /10000;
    float TopBottom = height / 2.0f /10000;

    glFrustum(
        -LeftRight, LeftRight,
        -TopBottom, TopBottom,
        .1, 1000
    );
}

void render_app(App* app)
{
    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
    glMatrixMode(GL_MODELVIEW);

    glPushMatrix();
    set_view(&(app->camera));
    render_scene(&(app->scene));
    glPopMatrix();

    if (app->camera.is_preview_visible) {
        show_texture_preview();
    }

    SDL_GL_SwapWindow(app->window);
}

void destroy_app(App* app)
{
    if (app->gl_context != NULL) {
        SDL_GL_DeleteContext(app->gl_context);
    }

    if (app->window != NULL) {
        SDL_DestroyWindow(app->window);
    }

    SDL_Quit();
}

#pragma endregion