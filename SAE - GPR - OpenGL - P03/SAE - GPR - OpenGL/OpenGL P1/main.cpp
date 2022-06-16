#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include <iostream>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>
#include "helpers/shader.h"


void framebuffer_size_callback(GLFWwindow* window, int width, int height);
void process_input(GLFWwindow* window, unsigned int VBO, unsigned int EBO);

float get_sin(double time);
float get_normalized_sin(double time);

void bind(unsigned int VBO, unsigned int EBO, float vertices[], unsigned int vertices_size, unsigned int indices[],
          unsigned int indices_size);

void swap_shape_triangle(unsigned int VBO, unsigned int EBO);
void swap_shape_rectangle(unsigned int VBO, unsigned int EBO);
void swap_shape_cube(unsigned int VBO, unsigned int EBO);
void swap_shape_real_pyramid(unsigned int VBO, unsigned int EBO);
void swap_shape_pyramid(unsigned int VBO, unsigned int EBO);

void set_transform(Shader shader);

//-------------------------------
// Settings
// ------------------------------

int window_width = 800;
int window_height = 800;

bool use_orto = true;
bool use_auto_fov = false;

float move_model_z = -10;
float move_model_x = 10;
float rotate_model = 0;

float move_cam_z = -10;
float move_cam_x = 0;

//-------------------------------
// Time
// ------------------------------

double last, now, dt;

int main()
{
#pragma region Init OpenGL
    //-------------------------------
    // Initialize and configure
    // ------------------------------

    glfwInit();
    glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 4);
    glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 4);
    glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

    //-------------------------------
    // Time
    // ------------------------------
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

    //glEnable(GL_CULL_FACE);
    //glFrontFace(GL_CW);

#pragma endregion

    Shader shader("vert.vs", "frag.fs");

    //-------------------------------
    // VBO, VAO, EBO
    // ------------------------------
    GLuint VAO, VBO, EBO;

    glGenVertexArrays(1, &VAO);
    glGenBuffers(1, &VBO);
    glGenBuffers(1, &EBO);

    glBindVertexArray(VAO); // Bind after generation, rebind when needed again
    swap_shape_triangle(VBO, EBO);

    // ------------------------------
    // Render Loop
    // ------------------------------
#pragma region Render loop

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
        process_input(window, VBO, EBO);

        //------------------------------
        // Render
        //------------------------------

        //const float t = get_normalized_sin(now);

        glClearColor(0, 0, 0, 1.0f);
        glClear(GL_COLOR_BUFFER_BIT);

        //------------------------------
        // Models / Vertices
        //------------------------------
        // glUseProgram(shaderProgram); // Which shader program should be used
        shader.use();
        set_transform(shader);
        glBindVertexArray(VAO);

        int size;
        glGetBufferParameteriv(GL_ELEMENT_ARRAY_BUFFER, GL_BUFFER_SIZE, &size);
        glDrawElements(GL_TRIANGLES, size / sizeof(GLuint), GL_UNSIGNED_INT, 0);


        //------------------------------
        // glfw: swap buffers and poll IO events (keys pressed/released, mouse moved etc.)
        //------------------------------

        glfwSwapBuffers(window);
        glfwPollEvents();
    }
#pragma endregion

    //------------------------------
    // glfw: terminate, clearing all previously allocated GLFW resources.
    //------------------------------
    glfwTerminate();
    return 0;
}


#pragma region Input
//------------------------------
// Process all input: query GLFW whether relevant keys are pressed/released this frame and react accordingly
//------------------------------
void process_input(GLFWwindow* window, unsigned int VBO, unsigned int EBO)
{
    if (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS)
    {
        glfwSetWindowShouldClose(window, true);
    }

    //------------------------------
    // Change Render Mode
    //------------------------------
    if (glfwGetKey(window, GLFW_KEY_L) == GLFW_PRESS)
    {
        glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);
    }
    else if (glfwGetKey(window, GLFW_KEY_F) == GLFW_PRESS)
    {
        glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);
    }

    //------------------------------
    // Change Shape
    //------------------------------
    if (glfwGetKey(window, GLFW_KEY_1) == GLFW_PRESS)
    {
        swap_shape_triangle(VBO, EBO);
    }
    else if (glfwGetKey(window, GLFW_KEY_2) == GLFW_PRESS)
    {
        swap_shape_rectangle(VBO, EBO);
    }
    else if (glfwGetKey(window, GLFW_KEY_3) == GLFW_PRESS)
    {
        swap_shape_cube(VBO, EBO);
    }
    else if (glfwGetKey(window, GLFW_KEY_4) == GLFW_PRESS)
    {
        swap_shape_pyramid(VBO, EBO);
    }
    else if (glfwGetKey(window, GLFW_KEY_5) == GLFW_PRESS)
    {
        swap_shape_real_pyramid(VBO, EBO);
    }

    //------------------------------
    // Move Model
    //------------------------------
    if (glfwGetKey(window, GLFW_KEY_W))
    {
        move_model_z++;
    }
    else if (glfwGetKey(window, GLFW_KEY_S))
    {
        move_model_z--;
    }
    else if (glfwGetKey(window, GLFW_KEY_A))
    {
        move_model_x--;
    }
    else if (glfwGetKey(window, GLFW_KEY_D))
    {
        move_model_x++;
    }
    if (glfwGetKey(window, GLFW_KEY_Q))
    {
        rotate_model++;
    }
    else if (glfwGetKey(window, GLFW_KEY_E))
    {
        rotate_model--;
    }

    //------------------------------
    // Change Camera type
    //------------------------------
    if (glfwGetKey(window, GLFW_KEY_O))
    {
        use_orto = true;
    }
    else if (glfwGetKey(window, GLFW_KEY_P))
    {
        use_orto = false;
    }

    //------------------------------
    // Move Camera
    //------------------------------
    if (glfwGetKey(window, GLFW_KEY_UP))
    {
        move_cam_z++;
    }
    else if (glfwGetKey(window, GLFW_KEY_DOWN))
    {
        move_cam_z--;
    }
    else if (glfwGetKey(window, GLFW_KEY_LEFT))
    {
        move_cam_x++;
    }
    else if (glfwGetKey(window, GLFW_KEY_RIGHT))
    {
        move_cam_x--;
    }

    //------------------------------
    // Use Auto zoom (in/out)
    //------------------------------
    if (glfwGetKey(window, GLFW_KEY_COMMA))
    {
        use_auto_fov = true;
    }
    else if (glfwGetKey(window, GLFW_KEY_PERIOD))
    {
        use_auto_fov = false;
    }

    //------------------------------
    // Use auto rotate model
    //------------------------------
    if (glfwGetKey(window, GLFW_KEY_R))
    {
        rotate_model += static_cast<float>(dt) * 45.0f;
    }
}
#pragma endregion
#pragma region Helper methods
//------------------------------
// glfw: whenever the window size changed (by OS or user resize input) this callback function executes
//------------------------------
void framebuffer_size_callback(GLFWwindow* window, int width, int height)
{
    glViewport(0, 0, width, height);
    glfwGetWindowSize(window, &window_width, &window_height);
}

//------------------------------
// Bind
//------------------------------
void bind(unsigned int VBO, unsigned int EBO, glm::vec3 vertices[], unsigned int vertices_size, unsigned int indices[],
          unsigned int indices_size)
{
    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, vertices_size, vertices, GL_STATIC_DRAW);

    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, indices_size, indices, GL_STATIC_DRAW);

    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), (void*)0);
    glEnableVertexAttribArray(0);
}
#pragma endregion
#pragma region Shapes
//------------------------------
// Swap Shapes
//------------------------------
void swap_shape_triangle(unsigned int VBO, unsigned int EBO)
{
    glm::vec3 vertices_triangle[] = {
        glm::vec3(-0.5, 0.5f, 0.0f), // left_top
        glm::vec3(-0.5f, -0.5f, 0.0f), // left_down
        glm::vec3(0.5f, 0.5f, 0.0f) // right_top
    };
    unsigned int indices_triangle[] = {
        0, 2, 1
    };

    bind(VBO, EBO, vertices_triangle, sizeof(vertices_triangle), indices_triangle, sizeof(indices_triangle));
}

void swap_shape_rectangle(unsigned int VBO, unsigned int EBO)
{
    glm::vec3 vertices_rectangle[] = {
        glm::vec3(-0.5, 0.5f, 1.0f), // left_top
        glm::vec3(-0.5f, -0.5f, 1.0f), // left_down
        glm::vec3(0.5f, -0.5f, 1.0f), // right_down
        glm::vec3(0.5f, 0.5f, 1.0f), // right_top
    };
    unsigned int indices_rectangle[] = {
        0, 3, 1,
        1, 3, 2
    };

    bind(VBO, EBO, vertices_rectangle, sizeof(vertices_rectangle), indices_rectangle, sizeof(indices_rectangle));
}

void swap_shape_cube(unsigned int VBO, unsigned int EBO)
{
    glm::vec3 vertices_cube[] = {
        glm::vec3(-0.5, 0.5f, 0.0f), // front_top_Left
        glm::vec3(0.5f, 0.5f, 0.0f), //front_top_right
        glm::vec3(-0.5f, -0.5f, 0.0f), // front_bot_left
        glm::vec3(0.5f, -0.5f, 0.0f), // front_bot_right
        glm::vec3(-0.5f, 0.5f, 1.0f), // back_top_left
        glm::vec3(0.5f, 0.5f, 1.0f), // back_top_right
        glm::vec3(-0.5f, -0.5f, 1.0f), // back_bot_left
        glm::vec3(0.5f, -0.5f, 1.0f), // back_bot_right
    };
    unsigned int indices_cube[] = {
        // TOP
        0, 4, 1,
        1, 4, 5,
        // Front
        2, 0, 3,
        3, 0, 1,
        // BOT
        6, 2, 7,
        7, 2, 3,
        // Right
        7, 3, 5,
        5, 3, 1,
        // LEFT
        4, 0, 6,
        6, 0, 2,
        // BACK
        4, 6, 5,
        5, 6, 7
    };

    bind(VBO, EBO, vertices_cube, sizeof(vertices_cube), indices_cube, sizeof(indices_cube));
}

void swap_shape_real_pyramid(unsigned int VBO, unsigned int EBO)
{
    glm::vec3 vertices_pyramid[] = {
        glm::vec3(-0.5f, -0.5f, 1.0f), // back_left
        glm::vec3(-0.5f, -0.5f, 0.0f), // front_left
        glm::vec3(0.5f, -0.5f, 0.0f), // front_right
        glm::vec3(0.5f, -0.5f, 1.0f), // back_right
        glm::vec3(0.0f, 0.5f, 0.5f) // middle_top
    };
    unsigned int indices_pyramid[] = {
        // BOT
        0, 3, 1,
        1, 3, 2,
        // Front
        1, 4, 2,
        // LEFT
        0, 1, 4,
        // Back
        0, 3, 4,
        // Right
        3, 4, 2
    };

    bind(VBO, EBO, vertices_pyramid, sizeof(vertices_pyramid), indices_pyramid, sizeof(indices_pyramid));
}

void swap_shape_pyramid(unsigned int VBO, unsigned int EBO)
{
    glm::vec3 vertices_pyramid[] = {
        glm::vec3(-0.1f, -0.5f, 0.0f), // front_corner
        glm::vec3(0.5f, 0.0f, 1.0f), // right_corner
        glm::vec3(-0.5f, 0.0f, 1.0f), // left_corner
        glm::vec3(0.0f, 0.5f, 0.5f) // top
    };
    unsigned int indices_pyramid[] = {
        0, 1, 2,
        //Sides
        0, 2, 3, // left
        1, 3, 2, // back
        0, 3, 1 // right
    };

    bind(VBO, EBO, vertices_pyramid, sizeof(vertices_pyramid), indices_pyramid, sizeof(indices_pyramid));
}
#pragma endregion

//------------------------------
// Transform
//------------------------------
void set_transform(Shader shader)
{
    //-------------------------------
    // Camera
    // ------------------------------
    glm::mat4 camera_translate = glm::mat4(1.0f);
    glm::mat4 camera_rotate = glm::mat4(1.0f);
    glm::mat4 camera_scale = glm::mat4(1.0f);

    //-------------------------------
    // Object
    // ------------------------------
    glm::mat4 model_translate = glm::mat4(1.0f);
    glm::mat4 model_rotate = glm::mat4(1.0f);
    glm::mat4 model_scale = glm::mat4(1.0f);


    //-------------------------------
    // Some tweaks for better visibility based on mode
    // ------------------------------

    float scale = 5;
    if (use_orto)
    {
        scale = 50;
    }
    
    //-------------------------------
    // Translate & Scale & Rotate
    // ------------------------------


    model_translate = glm::translate(model_translate, glm::vec3(move_model_x, 0, move_model_z));
    model_rotate = glm::rotate(model_rotate, glm::radians(rotate_model), glm::vec3(0, 1, 0));
    model_scale = glm::scale(model_scale, glm::vec3(scale, scale, scale));

    camera_translate = glm::translate(camera_translate, glm::vec3(move_cam_x, 0, move_cam_z));
    camera_rotate = glm::rotate(camera_rotate, glm::radians(0.0f), glm::vec3(0, 0, 1));
    camera_scale = glm::scale(camera_scale, glm::vec3(1, 1, 1));

    // Init matrices & Model matrix verschieben

    glm::mat4 model = model_translate * model_rotate * model_scale;
    glm::mat4 view = camera_translate * camera_rotate * camera_scale;

    glm::mat4 projection = glm::mat4(1.0f);

    if (use_orto)
    {
        //-------------------------------
        // Orthographic Cam
        // ------------------------------
        float width = window_width / 2.0f;
        float height = window_height / 2.0f;
        
        projection = glm::ortho(-width, width, -height, height,
                                0.1f, 100.0f);
    }
    else
    {
        //-------------------------------
        // Perspective Cam
        // ------------------------------
        float aspectRatio = (float)window_width / (float)window_height;

        float degree;
        if (use_auto_fov)
        {
            degree = glm::mix(45, 90, get_normalized_sin(now));
        }
        else
        {
            degree = 60;
        }

        projection = glm::perspective(glm::radians(degree), aspectRatio, 0.1f, 1000.0f);
    }

    // Retrieve the matrix uniform location IDs
    shader.setMat4("mvp", projection * view * model);
}

#pragma region Sinus
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
#pragma endregion
