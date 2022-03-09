// seulement si glew32s.lib
// #define GLEW_STATIC 1
#include "GL/glew.h"
#include "GLFW/glfw3.h"
#include "GLShader.h"

#include <stdio.h>

uint32_t g_BasicShader;

bool Initialise() {
    GLenum ret = glewInit();

    g_BasicShader = CreateShaderProgram("basic.vs", "basic.fs");

    return true;
}

void Terminate() {
    ;
}

void Render() {
    glClear(GL_COLOR_BUFFER_BIT);
}

int main(void)
{
    GLFWwindow* window;

    /* Initialize the library */
    if (!glfwInit())
        return -1;

    /* Create a windowed mode window and its OpenGL context */
    window = glfwCreateWindow(640, 480, "Hello World", NULL, NULL);
    if (!window)
    {
        glfwTerminate();
        return -1;
    }

    /* Make the window's context current */
    glfwMakeContextCurrent(window);

    Initialise();

    /* Loop until the user closes the window */
    while (!glfwWindowShouldClose(window))
    {
        /* Render here */
        Render();

        /* Swap front and back buffers */
        glfwSwapBuffers(window);

        /* Poll for and process events */
        glfwPollEvents();
    }

    Terminate();

    glfwTerminate();
    return 0;
}
