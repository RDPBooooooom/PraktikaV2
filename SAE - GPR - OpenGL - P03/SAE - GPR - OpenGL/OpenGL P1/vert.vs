#version 400 core
layout (location = 0) in vec3 pos;
// Properties
uniform mat4 mvp;
void main()
{
    gl_Position =  mvp * vec4(pos, 1.0);
}