#version 460


struct Material
{
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float shininess;
};


struct SpotLight
{
    vec3 position;
    vec3 direction;
    float cutoffAngle;
    float outerCutoff;
    float intensity;
    float constant;
    float linear;
    float quadratic;
    vec3 color;
};


out vec4 FragColor;


uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform vec3 globalAmbient;

uniform vec3 viewPos;  // Camera position
uniform Material material;
uniform SpotLight spotLight;
uniform sampler2D u_texture;


in vec3 varyingNormal; // eye-space vertex normal
in vec3 varyingVertPos;
in vec2 varyingTexCord;


vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir, Material material, vec3 globalAmbient);


void main()
{
    vec3 norm = normalize(varyingNormal);
    vec3 viewDir = normalize(viewPos -  varyingVertPos);
    
    vec3 result = CalcSpotLight(spotLight, norm, varyingVertPos, viewDir, material, globalAmbient);

    FragColor =  texture(u_texture, varyingTexCord) * vec4(result, 1.0);
}


vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir, Material material, vec3 globalAmbient)
{
    // Direction from fragment to light
    vec3 lightDir = normalize(light.position - fragPos);

    // Spotlight intensity based on angle between light direction and fragment direction
    float theta = dot(lightDir, normalize(-light.direction));

    // Check if fragment is inside the spotlight cone
    float epsilon = light.cutoffAngle - light.outerCutoff;

    float intensity = clamp((theta - light.outerCutoff) / epsilon, 0.0, 1.0);


    // Ambient lighting
    vec3 ambient = (globalAmbient * material.ambient).xyz; // 0.1 * light.color;

    // Diffuse lighting (only if within cone)
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 diffuse = diff * light.color * material.diffuse.xyz * intensity;

    // Specular lighting
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = spec * light.color * material.specular.xyz * intensity;

    // Attenuation
    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance +
                               light.quadratic * (distance * distance));

    // Combine results
    ambient *= attenuation;
    diffuse *= attenuation;
    specular *= attenuation;

    vec3 result = vec3(0, 0, 0);

    result += ambient;
    result += diffuse;
    result += specular;

    return result * light.intensity;
}