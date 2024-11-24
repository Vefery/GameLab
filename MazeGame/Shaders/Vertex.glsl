#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec3 aNormal;
layout(location = 2) in vec2 aTexCoord;


uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 vColor;

void main()
{
    vColor = vec3(0.4, 0.4, 0.4);
    gl_Position = vec4(aPosition, 1.0)* model * view * projection;
}
