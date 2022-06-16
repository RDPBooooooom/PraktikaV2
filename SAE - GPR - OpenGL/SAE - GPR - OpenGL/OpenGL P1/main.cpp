#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include <iostream>


void framebuffer_size_callback(GLFWwindow* window, int width, int height);
void process_input(GLFWwindow* window);

float get_sin(double time);
float get_normalized_sin(double time);

// settings

constexpr int window_width = 800;
constexpr int window_height = 600;

double last, now, dt;

int main()
{
    //-------------------------------
    // Initialize and configure
    // ------------------------------
    glfwInit();
    glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 4);
    glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 4);
    glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

    
    now = glfwGetTime();

    // ------------------------------
    // GLFW Window creation
    // ------------------------------

    GLFWwindow* window = glfwCreateWindow(window_width, window_height, "SAE OpenGL", NULL, NULL);
    if (window == nullptr)
    {
        std::cout << "Failed to create GLFW window" << std::endl;
        glfwTerminate();
        return -1;
    }
    glfwMakeContextCurrent(window);


    // ------------------------------
    // GLAD: load all OpenGL function pointers
    // ------------------------------
    if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress))
    {
        std::cout << "Failed to initialize GLAD" << std::endl;
        return -1;
    }

    glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);

    // ------------------------------
    // Render Loop
    // ------------------------------
    while (!glfwWindowShouldClose(window))
    {
        // ------------------------------
        // Time
        // ------------------------------
        last = now;
        now = glfwGetTime();
        dt = now - last;

        //------------------------------
        // Input
        //------------------------------
        process_input(window);

        //------------------------------
        // Render
        //------------------------------

        const float t = get_normalized_sin(now);

        std::cout << t;
        std::cout << "\n";

        glClearColor(t, 0, 0, 1.0f);
        glClear(GL_COLOR_BUFFER_BIT);

        //------------------------------
        // glfw: swap buffers and poll IO events (keys pressed/released, mouse moved etc.)
        //------------------------------
        
        glfwSwapBuffers(window);
        glfwPollEvents();
        process_input(window);
    }

    //------------------------------
    // glfw: terminate, clearing all previously allocated GLFW resources.
    //------------------------------
    glfwTerminate();
    return 0;
}

//------------------------------
// Process all input: query GLFW whether relevant keys are pressed/released this frame and react accordingly
//------------------------------
void process_input(GLFWwindow* window)
{
    if (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS)
    {
        glfwSetWindowShouldClose(window, true);
    }
}

//------------------------------
// glfw: whenever the window size changed (by OS or user resize input) this callback function executes
//------------------------------
void framebuffer_size_callback(GLFWwindow* window, int width, int height)
{
    glViewport(0, 0, width, height);
}

//------------------------------
// Get Sinus
//------------------------------
float get_sin(const double time)
{
    return sin(time);
}

float get_normalized_sin(const double time)
{
    return (get_sin(time) + 1) / 2;
}
