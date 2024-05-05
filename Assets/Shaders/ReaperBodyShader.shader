Shader"Unlit/ReaperBodyShader"
{
    Properties
{
_MainTex ("Texture", 2D) = "white" {}
_ColourA ("ColourA", Color) = (1,1,1,1)
_ColourB ("ColourB", Color) = (1,1,1,1)
_FresnelColour ("Fresnel Colour", Color) = (1,1,1,1)
_FresnelStr ("Fresnel Strength", Range(0, 2)) = 0.5
_WaveAmp ("Wave Amplitude", Range(0, 0.2)) = 0.1 
_WaveFreq ("Wave Frequency", Range(0, 1)) = 0.1
}
SubShader
{
Tags 
{ 
    "RenderType"="Opaque"
    //"RenderType"="Transparent"
    //"Queue"="Transparent"
}

Pass
{
//ZWrite Off

//Blend DstColor Zero
//Cull Off

CGPROGRAM
#pragma vertex vert
#pragma fragment frag


#include "UnityCG.cginc"

#define TAU 6.28318

float4 _ColourA;
float4 _ColourB;
float4 _FresnelColour;
float _FresnelStr;
float _WaveAmp;
float _WaveFreq;

struct appdata
{
    float4 vertex : POSITION;
    float3 normals : NORMAL;
    float2 uv : TEXCOORD0;
    
};

struct v2f
{
    float3 normal : TEXCOORD0;
    float4 vertex : SV_POSITION;
    float2 uv : TEXCOORD1;
    float4 worldPos : TEXCOORD2;
};

sampler2D _MainTex;
float4 _MainTex_ST;
            

v2f vert(appdata v)
{
    v2f o;
    
    
    float wave = cos((v.vertex.y + _Time.y * _WaveFreq) * TAU + TAU) * _WaveAmp;
    
    v.vertex = v.vertex + wave * normalize(float4(v.normals, 0));
    //v.vertex = v.vertex + wave * float4(1, 0, 0, 0) * _WaveAmp;
    
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.normal = UnityObjectToWorldNormal(v.normals);
    o.uv = v.uv;
    o.worldPos = mul(unity_ObjectToWorld, v.vertex);
    
    return o;
}

float4 frag(v2f i) : SV_Target
{
    //float xOffset = cos(i.uv.x * TAU * 8) * 0.01;
    //float t = cos((i.uv.y + xOffset - _Time.y * 0.1) * TAU * 5) * 0.5 + 0.5;
    //t *= 1 - i.uv.y;

    //float topBottomRemover = abs(i.normal.y) < 0.999;
    //float waves = t * topBottomRemover;
    //float4 gradient = lerp(_ColourA, _ColourB, i.uv.y);

    //return gradient * waves;
    
    //return _ColourA;
    
    //return float4(i.uv, 0, 0);
    
    float3 normal = normalize(i.normal);
    float3 viewDirection = normalize( _WorldSpaceCameraPos - i.worldPos);
    
    float timeFresnelStr = _FresnelStr + cos(_Time.y * 1) * 0.2;
    float fresnel = saturate(timeFresnelStr - dot(viewDirection, normal));
    return _ColourA + fresnel * _FresnelColour;
    
}
ENDCG
}
}
}
