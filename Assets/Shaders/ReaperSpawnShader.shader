Shader"Unlit/ReaperSpawnShader"
{
Properties
{
    _Colour ("Colour", Color) = (1,1,1,1)
    _BorderColour ("Border Colour", Color) = (1,1,1,1)
    _BorderSize("Border Size", Range(0, 0.5)) = 0.1
    _Size ("Size", Range(0,1)) = 1
    _BreatheTime ("Breathe Time", float) = 0.1
    _BreatheAmount ("Breathe Amount", float) = 0.1
}
SubShader
{
    Tags 
    {
        //"RenderType"="Opaque" 
        "RenderType"="Transparent" 
        "Queue"="Transparent"
    }

    Pass
    {
        ZWrite Off

        Blend SrcAlpha OneMinusSrcAlpha
        //Blend DstColor Zero
            
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag

#include "UnityCG.cginc"
#define pi 3.141592
#define TAU 6.28318

struct appdata
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float2 uv : TEXCOORD0;
    float4 vertex : SV_POSITION;
    float3 worldPos : TEXCOORD1;
};

float4 _Colour;
float4 _BorderColour;
float _Size;
float _Offset;
float _BorderSize;
float _BreatheTime;
float _BreatheAmount;

float InverseLerp(float a, float b, float v)
{
    return (v - a) / (b - a);
}

v2f vert(appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.worldPos = mul(unity_ObjectToWorld, v.vertex);
    o.uv = v.uv;
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
    //Gives rounded edges to the square UV
    float sdf = distance(i.uv, float2(0.5, 0.5)) * 2 - 1;
    clip(-sdf);
    
    float2 uvsCentered = i.uv * 2 - 1;
    
    
    float uvDistance = distance(uvsCentered, float2(0, 0));
    //float uvDistance = distance(uvsCentered, float2(0, 0)) + cos(radial + _Time.y * TAU) * 0.1;
  
    float radial = atan2(uvsCentered.y, uvsCentered.x) / pi;
    
    float borderBreathe = cos(_Time.y * _BreatheTime) * _BreatheAmount - 0.5;
    
    //Makes it get bigger + smaller 
    float sizeAlter = _Size + borderBreathe * 0.01;
    //float sizeAlter = _Size;
                //Gives a border
    //float borderWidth = 0.1;
    
    //float borderSDF = _Size - _BorderSize;
    float borderSDF = (sizeAlter - _BorderSize) + sin(((radial + uvDistance) + (_Time.y * -0.15)) * 10 * TAU) * 0.1 - 0.1;
    
    //return borderSDF;
    
    //float borderMask = step(borderSDF - _BorderSize, uvDistance);
    float borderMask = step(borderSDF, uvDistance);
    clip(-borderMask);
    
    borderMask = step(borderSDF - _BorderSize, uvDistance);
    
    borderMask = borderMask + borderBreathe;
    
    float3 cameraPosition = _WorldSpaceCameraPos;
    
    float clampMax = 3;
    
    float alphaMod = clamp(abs(distance(cameraPosition, i.worldPos)), 0.1, clampMax) / clampMax;
    
    alphaMod = saturate(InverseLerp(0.4, 1, alphaMod));
 
    
    //return borderMask;
    
                //Turns the uvs radial
  
    //radial = ((radial * 0.5 + 0.5)) % 1.0;
    
                //Shows frags based on progress
    float reveal = step(uvDistance, sizeAlter);
    
    float uvDistanceLerp = saturate(InverseLerp(0.4, 0.6, uvDistance));
    
    float3 colorLerp = lerp(_Colour, _BorderColour, uvDistanceLerp);
    
    float3 output = colorLerp * reveal + (float3)_BorderColour * borderMask;
    
    return float4(output.rgb, alphaMod);
    //return _Colour * reveal + _BorderColour * borderMask;
}
            ENDCG
        }
    }
}
