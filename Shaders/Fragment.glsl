#version 330 core

in vec3 vColor;
out vec4 FragColor;


uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    FragColor = vec4(vColor, 1.0);
}
