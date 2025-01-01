#version 460

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec3 aNormal;
layout(location = 2) in vec2 aTexCoord;




uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat4 norm_matrix;


out vec3 varyingNormal; // eye-space vertex normal
out vec3 varyingVertPos;
out vec2 varyingTexCord;

void main()
{
    
    varyingVertPos = (vec4(aPosition,1.0)* model * view).xyz;
    varyingNormal = (vec4(aNormal, 1.0) * norm_matrix).xyz;
    varyingTexCord = aTexCoord;

    gl_Position = vec4(aPosition, 1.0)* model * view * projection;
}
