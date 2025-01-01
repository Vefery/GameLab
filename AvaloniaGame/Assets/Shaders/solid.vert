#version 300 es

precision mediump float;

uniform vec4 aColor;
in vec3 aPosition;
uniform mat4 aProj;

out vec4 bColor;

void main(void)
{
    gl_Position = vec4(aPosition, 1.0) * aProj;
    bColor = aColor;
}
