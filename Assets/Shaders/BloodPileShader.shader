Shader"Unlit/BloodPileShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WaveHeight ("Wave Height", Range(0, 1)) = 0.5
        _WaveAmount ("Wave Amount", Range(0, 50)) = 0.5
        _WaveSpeed ("Wave Speed", Range(0, 10)) = 0.5
        _LowColour ("Low Colour", Color) = (1,1,1,1)
        _HighColour ("High Colour", Color) = (1,1,1,1)
        _UVX ("UV X Augment", Range(0, 10)) = 1
        _UVY ("UV Y Augment", Range(0, 10)) = 1
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

            Blend SrcAlpha
            OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normals : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float waveValue : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _WaveHeight;
            float _WaveAmount;
            float _WaveSpeed;
            float4 _LowColour;
            float4 _HighColour;
            float _UVX;
            float _UVY;

            float InverseLerp(float a, float b, float v)
            {
                return (v - a) / (b - a);
            }

            v2f vert (appdata v)
            {
                v2f o;
    
                float wave = sin((v.uv.x * _UVX + v.uv.y * _UVY) * _WaveAmount + _Time.y * _WaveSpeed) * _WaveHeight;
    
                v.vertex = v.vertex + wave * normalize(float4(v.normals, 0));
                o.waveValue = wave;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float lerpValue = InverseLerp(-_WaveHeight, _WaveHeight, i.waveValue);
                //float stepLerp = step(0.5, lerpValue);
                float4 lerpColour = lerp(_LowColour, _HighColour, lerpValue);
                //float4 col = step
            
                return lerpColour;
}
            ENDCG
        }
    }
}
