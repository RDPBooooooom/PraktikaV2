Shader "SAE/Constant Color"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        struct Input
        {
			float4 color : COLOR;// Vertex color
        };

        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = _Color.xyz;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
