Shader"Unlit/CircleProgressShader"
{
    Properties
    {
        _Colour ("Colour", Color) = (1,1,1,1)
        _Offset("Offset", Range(0,1)) = 0.25
        _BorderSize("Border Size", float) = 0
        _Progress ("Progress", Range(0,1)) = 0
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
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #define pi 3.141592

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
            float _Progress;
            float _Offset;
            float _BorderSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //Gives rounded edges to the square UV
                float sdf = distance(i.uv, float2(0.5, 0.5)) * 2 - 1;
                clip(-sdf);
    
                //Gives a border
                float borderSDF = sdf + _BorderSize;
                float borderMask = step(0, -borderSDF);
    
                //Turns the uvs radial
                float2 uvsCentered = i.uv * 2 - 1;
                float radial = atan2(uvsCentered.y, uvsCentered.x) / pi;
                radial = ((radial * 0.5 + 0.5) + _Offset) % 1.0;
    
                //Shows frags based on progress
                float reveal = step(radial,  _Progress);
  
    
                return _Colour * reveal * borderMask;
            }
            ENDCG
        }
    }
}
