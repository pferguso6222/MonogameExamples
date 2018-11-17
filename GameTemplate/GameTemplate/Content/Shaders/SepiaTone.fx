sampler s0;

float _attenuation = 800.0; // 800.0
float _linesFactor = 0.04; // 0.04



float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
    return float4(1, 0, 0, 1);
}

float4 mainPS( float2 texCoord:TEXCOORD0, in float2 screenPos:VPOS ) : COLOR0
{
    float4 color = tex2D( s0, texCoord );
    float scanline = sin( texCoord.y * _linesFactor ) * _attenuation;
	//sepia tone
    float r = color.r;
	float g = color.g;
	float b = color.b;
	
	color.r = (r * .393) + (g * .769) + (b * .189);
	color.g = (r * .349) + (g * .686) + (b * .168);
	color.b = (r * .272) + (g * .534) + (b * .131);
	
    return color;
}



technique Scanlines
{
    pass P0
    {
        PixelShader = compile ps_3_0 mainPS();
    }
};