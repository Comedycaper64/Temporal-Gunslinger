Shader"Unlit/WeakPointShader"
{
    Properties
    {
       _Color ("Colour", color) = (1,1,1,1) 
       _FlashSpeed ("Flash Speed", Range(0, 10)) = 0.5
       _UnscaledTime ("Unscaled Time", float) = 0
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float waveValue : TEXCOORD1;
            };

            float4 _Color;
            float _FlashSpeed;
            float _UnscaledTime;

            v2f vert (appdata v)
            {
                v2f o;
                float wave = cos(v.uv.x + v.uv.y + _UnscaledTime * _FlashSpeed) * 0.25 + 0.5;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.waveValue = wave;
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                
                return float4(_Color.r, _Color.g, _Color.b, i.waveValue);
                //return _Color * i.waveValue;
}
            ENDCG
        }
    }
}
