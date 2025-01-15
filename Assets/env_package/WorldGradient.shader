Shader "Custom/WorldGradient"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _SkyColor ("Sky Color", Color) = (0.5,0.7,1,1)
        _GradientStart ("Gradient Start", Float) = 0.0
        _GradientEnd ("Gradient End", Float) = 10.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata_t
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            float4 _BaseColor;
            float4 _SkyColor;
            float _GradientStart;
            float _GradientEnd;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // Calculate gradient factor
                float gradientFactor = saturate((i.worldPos.y - _GradientStart) / (_GradientEnd - _GradientStart));

                // Blend between base color and sky color
                float4 finalColor = lerp(_BaseColor, _SkyColor, gradientFactor);

                return finalColor;
            }
            ENDCG
        }
    }
}
