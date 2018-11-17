#if OPENGL
    #define SV_POSITION POSITION
    #define VS_SHADERMODEL vs_3_0
    #define PS_SHADERMODEL ps_3_0
#else
    #define VS_SHADERMODEL vs_4_0_level_9_1
    #define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix WorldViewProjection;
sampler2D TextureSampler: register(s0); // added

struct VertexShaderInput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TexCoords: TEXCOORD0; // added
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TexCoords: TEXCOORD0; // added
}; 

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;

    output.Position = mul(input.Position, WorldViewProjection);
    output.Color = input.Color;
    output.TexCoords = input.TexCoords; // added

    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    return tex2D(TextureSampler, input.TexCoords);
}

technique BasicColorDrawing
{
    pass P0
    {
        //VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
   }
};