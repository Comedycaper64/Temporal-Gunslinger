Shader"Unlit/VelocityBarShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MaxVelocity ("Max Velocity", float) = 1
        _Velocity ("Velocity", float) = 0
        _DeltaVelocity ("Delta Velocity", float) = 0
        _PulseThreshold ("Pulse Threshold Velocity", float) = 0
        _PulseAmount ("Pulse Amount", float) = 0
        _PulseSpeed ("Pulse Speed", float) = 4
        _UnscaledTime ("Unscaled Time", float) = 0

        _FillColourStart ("Start Fill Colour", Color) = (1,1,1,1)
        _FillColourEnd ("End Fill Colour", Color) = (1,1,1,1)
        _EmptyColour ("Empty Colour", Color) = (0,0,0,0)
        _DeltaColour ("Delta Colour", Color) = (1,1,1,1)
        _PulseColour ("Pulse Colour", Color) = (1,1,1,1)
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

            sampler2D _MainTex;
            float _MaxVelocity;
            float _Velocity;
            float _DeltaVelocity;
            float _PulseThreshold;
            float _PulseAmount;
            float _PulseSpeed;
            float _UnscaledTime;

            float4 _FillColourStart;
            float4 _FillColourEnd;
            float4 _EmptyColour;
            float4 _DeltaColour;
            float4 _PulseColour;

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
                //float sdf = distance(i.uv, float2(0.5, 0.5)) * 2 - 1;            
                //clip(-sdf);
    
                //float innerSdf = distance(i.uv, float2(0.5, 0.5)) * 2 - _InnerLoss;
                //clip(innerSdf);
                //float compoundSdf = -sdf * innerSdf;
                //clip(compoundSdf);
    
   
                
                float radial = (atan2(uvsCentered.y, uvsCentered.x) / pi) * _MaxVelocity + _MaxVelocity / 1;
                //radial = ((radial * 0.5 + 0.5) + _Offset) % _MaxVelocity;
    
                float reveal = step(radial, _Velocity);
                float deltaReveal = step(radial, _DeltaVelocity);
    
                float tBarColour = saturate(InverseLerp(0.075, 1, i.uv.x));
                float4 barColour = lerp(_FillColourStart, _FillColourEnd, tBarColour);
                float4 texColour = tex2D(_MainTex, i.uv);
                //float barMask = _Velocity > i.uv.x * _MaxVelocity;
    
                float pulseStrength = saturate( InverseLerp(_PulseThreshold, 0, _Velocity) );
                float pulse = (cos(_UnscaledTime * _PulseSpeed) * _PulseAmount + 1) * pulseStrength;

                //return float4(barColour * reveal + _DeltaColour * deltaReveal, reveal);
                return texColour * (barColour * reveal + _DeltaColour * deltaReveal * (1 - reveal) + _EmptyColour * (1 - reveal) + _PulseColour * pulse * reveal);
}
            ENDCG
        }
    }
}
