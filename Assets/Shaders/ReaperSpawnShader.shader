Shader"Unlit/ReaperSpawnShader"
{
Properties
{
    _Colour ("Colour", Color) = (1,1,1,1)
    _BorderColour ("Border Colour", Color) = (1,1,1,1)
    _BorderSize("Border Size", Range(0, 0.5)) = 0.1
    _Size ("Size", Range(0,1)) = 1
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

        //Blend SrcAlpha OneMinusSrcAlpha
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
};

float4 _Colour;
float4 _BorderColour;
float _Size;
float _Offset;
float _BorderSize;

v2f vert(appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
                //Gives rounded edges to the square UV
    float sdf = distance(i.uv, float2(0.5, 0.5)) * 2 - 1;
    clip(-sdf);
    
    float2 uvsCentered = i.uv * 2 - 1;
    
    
    float radial = atan2(uvsCentered.y, uvsCentered.x) / pi;
    
    float uvDistance = distance(uvsCentered, float2(0, 0));
    //float uvDistance = distance(uvsCentered, float2(0, 0)) + cos(radial + _Time.y * TAU) * 0.1;
  
    
                //Gives a border
    //float borderWidth = 0.1;
    float borderSDF = _Size - _BorderSize;
    
    //return borderSDF;
    
    //float borderMask = step(borderSDF - _BorderSize, uvDistance);
    float borderMask = step(borderSDF, uvDistance);
    clip(-borderMask);
    
    borderMask = step(borderSDF - _BorderSize, uvDistance);
    
    
    //return borderMask;
    
                //Turns the uvs radial
  
    //radial = ((radial * 0.5 + 0.5)) % 1.0;
    
                //Shows frags based on progress
    float reveal = step(uvDistance, _Size);
  
    
    return _Colour * reveal + _BorderColour * borderMask;
}
            ENDCG
        }
    }
}