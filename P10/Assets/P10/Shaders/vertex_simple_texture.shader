Shader "SAE/Vertex Simple Texture"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Amplitude("Amplitude", Float) = 1
        _Frequency("Frequency", Float) = 1
        _WobbleStrength("Wobble Strength", Range(0,6)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert

        #pragma target 3.0

        struct Input
        {
			float4 color : COLOR;// Vertex color
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        fixed4 _Color;

        float _Amplitude;
        float _Frequency;
        float _WobbleStrength;
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c;
        }

        void vert(inout appdata_full v)
        {
            v.vertex.xyz += v.normal * sin(v.texcoord * _WobbleStrength) * _Amplitude;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
