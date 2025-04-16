Shader "Joey/BlackWhiteNoir_URP"
{
    Properties
    {
        _GrainIntensity("Grain Intensity", Range(0.0, 1.0)) = 0.5
        _DarkThreshold("Dark Threshold", Range(0.0, 0.3)) = 0.08
        _MidThreshold("Mid Threshold", Range(0.0, 0.5)) = 0.3
        _Alpha("Overall Transparency", Range(0.0, 1.0)) = 0.8
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };

            TEXTURE2D_X(_CameraOpaqueTexture);
            SAMPLER(sampler_CameraOpaqueTexture);

            float _GrainIntensity;
            float _DarkThreshold;
            float _MidThreshold;
            float _Alpha;

            float3 RGBToHSV(float3 c)
            {
                float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
                float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));
                float d = q.x - min(q.w, q.y);
                float e = 1.0e-10;
                return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
            }

            float random(float2 co)
            {
                return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = IN.uv;
                OUT.screenPos = ComputeScreenPos(OUT.positionHCS);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
                float4 screenColor = SAMPLE_TEXTURE2D_X(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, screenUV);

                float3 hsv = RGBToHSV(screenColor.rgb);
                float grain = (random(screenUV * _Time.y * 60.0) - 0.5) * _GrainIntensity;

                // Noir tone levels (grayscale)
                float4 color7 = float4(0.0, 0.0, 0.0, _Alpha);               // Deep black
                float4 color8 = float4(0.1, 0.1, 0.1, _Alpha);               // Charcoal
                float4 color16 = float4(0.3, 0.3, 0.3, _Alpha);              // Mid gray
                float4 color20 = float4(0.95, 0.95, 0.95, _Alpha);           // Almost white

                float brightness = hsv.z + grain;

                float4 finalColor = (brightness < _MidThreshold ?
                                        (brightness < _DarkThreshold ?
                                            color7 : color8)
                                    : (brightness < 0.6 ? color16 : color20));

                return finalColor;
            }

            ENDHLSL
        }
    }
}
