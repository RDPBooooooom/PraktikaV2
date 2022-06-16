#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include <iostream>


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

//-------------------------------
// Prepared Shaders
// ------------------------------
const char* vertex_shader_source = "#version 400 core\n"
    "layout (location = 0) in vec3 aPos;\n"
    "void main()\n"
    "{\n"
    " gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);\n"
    "}\0";
const char* fragment_shader_source = "#version 400 core\n"
    "out vec4 FragColor;\n"
    "void main()\n"
    "{\n"
    " FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);\n"
    "}\0";

//-------------------------------
// Settings
// ------------------------------

constexpr int window_width = 800;
constexpr int window_height = 800;

//-------------------------------
// Time
// ------------------------------

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

    //-------------------------------
    // Compile prepared Shaders
    // ------------------------------
#pragma region Vertex Shader
    // Compile vertex shader
    int vertexShader = glCreateShader(GL_VERTEX_SHADER);
    glShaderSource(vertexShader, 1, &vertex_shader_source, NULL);
    glCompileShader(vertexShader);
    // check for shader compile errors
    int success = 1;
    char infoLog[512];
    glGetShaderiv(vertexShader, GL_COMPILE_STATUS, &success);
    if (!success)
    {
        glGetShaderInfoLog(vertexShader, 512, NULL, infoLog);
        std::cout << "Exception: Vertex shader compilation failed.\n" << infoLog << std::endl;
    }
#pragma endregion
#pragma region Fragment Shader
    // Compile fragment shader
    const int fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
    glShaderSource(fragmentShader, 1, &fragment_shader_source, NULL);
    glCompileShader(fragmentShader);
    // check for shader compile errors
    glGetShaderiv(fragmentShader, GL_COMPILE_STATUS, &success);
    if (!success)
    {
        glGetShaderInfoLog(fragmentShader, 512, NULL, infoLog);
        std::cout << "Exception: Fragment shader compilation failed.\n" << infoLog << std::endl;
    }
#pragma endregion

    //-------------------------------
    // Build and link prepared Shader
    // ------------------------------
#pragma region Build and Link
    // Link shaders
    const int shaderProgram = glCreateProgram();
    glAttachShader(shaderProgram, vertexShader);
    glAttachShader(shaderProgram, fragmentShader);
    glLinkProgram(shaderProgram);
    // Check for linking errors
    glGetProgramiv(shaderProgram, GL_LINK_STATUS, &success);
    if (!success)
    {
        glGetProgramInfoLog(shaderProgram, 512, NULL, infoLog);
        std::cout << " Exception: Program linking failed.\\n" << infoLog << std::endl;
    }
    // Free allocated memory for shaders
    glDeleteShader(vertexShader);
    glDeleteShader(fragmentShader);
#pragma endregion

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

        const float t = get_normalized_sin(now);

        glClearColor(t, 0, 0, 1.0f);
        glClear(GL_COLOR_BUFFER_BIT);

        //------------------------------
        // Models / Vertices
        //------------------------------
        glUseProgram(shaderProgram); // Which shader program should be used
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

    //------------------------------
    // glfw: terminate, clearing all previously allocated GLFW resources.
    //------------------------------
    glfwTerminate();
    return 0;
}

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
}

//------------------------------
// glfw: whenever the window size changed (by OS or user resize input) this callback function executes
//------------------------------
void framebuffer_size_callback(GLFWwindow* window, int width, int height)
{
    glViewport(0, 0, width, height);
}

//------------------------------
// Bind
//------------------------------
void bind(unsigned int VBO, unsigned int EBO, float vertices[], unsigned int vertices_size, unsigned int indices[],
          unsigned int indices_size)
{
    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, vertices_size, vertices, GL_STATIC_DRAW);

    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, indices_size, indices, GL_STATIC_DRAW);

    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), (void*)0);
    glEnableVertexAttribArray(0);
}

//------------------------------
// Swap Shapes
//------------------------------
void swap_shape_triangle(unsigned int VBO, unsigned int EBO)
{
    float vertices_triangle[] = {
        -0.5, 0.5f, 0.0f, // left_top
        -0.5f, -0.5f, 0.0f, // left_down
        0.5f, 0.5f, 0.0f // right_top
    };
    unsigned int indices_triangle[] = {
        0, 2, 1
    };

    bind(VBO, EBO, vertices_triangle, sizeof(vertices_triangle), indices_triangle, sizeof(indices_triangle));
}

void swap_shape_rectangle(unsigned int VBO, unsigned int EBO)
{
    float vertices_rectangle[] = {
        -0.5, 0.5f, 1.0f, // left_top
        -0.5f, -0.5f, 1.0f, // left_down
        0.5f, -0.5f, 1.0f, // right_down
        0.5f, 0.5f, 1.0f, // right_top
    };
    unsigned int indices_rectangle[] = {
        0, 3, 1,
        1, 3, 2
    };

    bind(VBO, EBO, vertices_rectangle, sizeof(vertices_rectangle), indices_rectangle, sizeof(indices_rectangle));
}

void swap_shape_cube(unsigned int VBO, unsigned int EBO)
{
    float vertices_cube[] = {
        -0.5, 0.5f, 0.0f, // front_top_Left
        0.5f, 0.5f, 0.0f, //front_top_right
        -0.5f, -0.5f, 0.0f, // front_bot_left
        0.5f, -0.5f, 0.0f, // front_bot_right
        -0.4f, 0.6f, 0.5f, // back_top_left
        0.6f, 0.6f, 0.5f, // back_top_right
        -0.4f, -0.4f, 0.5f, // back_bot_left
        0.6f, -0.4f, 0.5f, // back_bot_right
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
    float vertices_pyramid[] = {
        -0.5f, -0.5f, 1.0f, // back_left
        -0.5f, -0.5f, 0.5f, // front_left
        0.5f, -0.5f, 0.5f, // front_right
        0.5f, -0.5f, 1.0f, // back_right
        0.0f, 0.5f, 0.75f // middle_top
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
    float vertices_pyramid[] = {
        -0.1f, -0.5f, 0.0f, // front_corner
        0.5f, 0.0f, 1.0f, // right_corner
        -0.5f, 0.0f, 1.0f, // left_corner
        0.0f, 0.5f, 0.5f // top
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
