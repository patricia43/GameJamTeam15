Shader "Custom/LiquidTiltURP"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        [Space]
        _FillAmount ("Fill Amount", Range(0, 1)) = 0.5
        _Tilt ("Tilt Intensity", Range(-1, 1)) = 0.0 
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "RenderPipeline" = "UniversalPipeline"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            Name "Universal2D"
            Tags { "LightMode" = "Universal2D" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float4 color      : COLOR;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 color      : COLOR;
                float2 uv         : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float _FillAmount;
                float _Tilt;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                OUT.color = IN.color * _Color;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Unlit Sprite Texture sampling
                half4 c = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv) * IN.color;

                // Tilt calculation: Find the height of the liquid line at the current UV x coordinate.
                // We center uv.x at 0.5 so it pivots perfectly in the middle of the sprite.
                float lineY = _FillAmount + ((IN.uv.x - 0.5) * _Tilt);

                // Branchless math for performance: step() returns 0 if uv.y is greater than lineY, and 1 otherwise.
                c.a *= step(IN.uv.y, lineY);

                // Pre-multiply alpha as standard for Unity 2D transparent passes
                c.rgb *= c.a;
                
                return c;
            }
            ENDHLSL
        }
    }
}
