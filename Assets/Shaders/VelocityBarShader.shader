Shader"Unlit/VelocityBarShader"
{
    Properties
    {
        _MaxVelocity ("Max Velocity", float) = 1
        _Velocity ("Velocity", Range(-300, 0)) = 0
        _DeltaVelocity ("Delta Velocity", Range(-300, 0)) = 0
        _FillColourStart ("Start Fill Colour", Color) = (1,1,1,1)
        _FillColourEnd ("End Fill Colour", Color) = (1,1,1,1)
        _EmptyColour ("Empty Colour", Color) = (0,0,0,0)
        _DeltaColour ("Delta Colour", Color) = (1,1,1,1)
        _InnerLoss ("Inner Circle Missing", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

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

            float _MaxVelocity;
            float _Velocity;
            float _DeltaVelocity;
            float _InnerLoss;           
            float4 _FillColourStart;
            float4 _FillColourEnd;
            float4 _EmptyColour;
            float4 _DeltaColour;

            float InverseLerp(float a, float b, float v)
            {
                return (v - a) / (b - a);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //float valueLerp = saturate(InverseLerp(_MaxVelocity / 5, _MaxVelocity - (_MaxVelocity / 5), _Velocity));
                float2 uvsCentered = i.uv * 2 - 1;
                float sdf = distance(i.uv, float2(0.5, 0.5)) * 2 - 1;
    
                clip(-sdf);
    
                float innerSdf = distance(i.uv, float2(0.5, 0.5)) * 2 - _InnerLoss;
                clip(innerSdf);
    
                
                float radial = (atan2(uvsCentered.y, uvsCentered.x) / pi) * _MaxVelocity;
                //radial = ((radial * 0.5 + 0.5) ) % _MaxVelocity;
    
                float reveal = step(radial, _Velocity);
                float deltaReveal = step(radial, _DeltaVelocity);
    
                float4 barColour = lerp(_FillColourStart, _FillColourEnd, i.uv.x);
                //float barMask = _Velocity > i.uv.x * _MaxVelocity;

                //return float4(barColour * reveal + _DeltaColour * deltaReveal, reveal);
    return barColour * reveal + _DeltaColour * deltaReveal * (1 - reveal);
}
            ENDCG
        }
    }
}
