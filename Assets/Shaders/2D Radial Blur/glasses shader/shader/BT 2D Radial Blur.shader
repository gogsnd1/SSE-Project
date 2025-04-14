Shader "BloodTea/ScreenSpaceRadialBlur TwT"
{
	Properties
	{
		_MainTex("Discord: bloodtea", 2D) = "white" {}
		_CenterX("Blur Center X", Range(0, 1)) = 0.5
		_CenterY("Blur Center Y", Range(0, 1)) = 0.5
		_BlurStrength("Blur Strength", Range(0, 1)) = 0.5
		_BlurSamples("Blur Samples", Range(1, 16)) = 8
		_Animate("Animate Blur", Float) = 0 // Toggle for animation (0 = off, 1 = on)
		_AnimSpeed("Animation Speed", Range(0, 10)) = 2 // Animation speed fucked up TwT
		_AnimAmplitude("Animation Amplitude", Range(0, 1)) = 0.1 // Range of oscillation
		_Alpha("Alpha", Range(0, 1)) = 1.0 // Alpha for transparency
	}

		SubShader
		{
			Tags { "RenderType" = "Transparent" "Queue" = "Overlay" }

			// Enable blending for transparency
			Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				sampler2D _MainTex;	
				float4 _MainTex_TexelSize;    // Needed for texture size (1/width, 1/height)
				float _CenterX, _CenterY;      // Center of radial blur
				float _BlurStrength;           // Strength of blur
				int _BlurSamples;              // Number of blur samples
				float _Animate;                // Toggle for animation
				float _AnimSpeed;              // Speed of the animation
				float _AnimAmplitude;          // Amplitude of the oscillation (how far the blur center moves)
				float _Alpha;                  // Alpha for transparency

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

				
				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					return o;
				}

				
				fixed4 frag(v2f i) : SV_Target
				{
					// Base texture color
					fixed4 color = tex2D(_MainTex, i.uv);

				// Animate the center if _Animate is enabled
				float2 center = float2(_CenterX, _CenterY);

				// If animation is enabled, adjust center.x with sine wave oscillation over time RAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
				if (_Animate > 0.5)
				{
					
					float timeValue = _Time.y * _AnimSpeed;
					center.x += sin(timeValue) * _AnimAmplitude;
				}

				// Get direction from the center to the current pixel
				float2 direction = i.uv - center;

				// Initialize blur color and sample count
				fixed4 blurColor = 0;
				float totalWeight = 0;

				// Perform radial blur with several samples along the direction vector
				for (int j = 0; j < _BlurSamples; j++)
				{
					float sampleWeight = (1.0 - float(j) / float(_BlurSamples)) * _BlurStrength;
					float sampleOffset = sampleWeight * length(direction);

					// Sample the texture along the blur direction
					blurColor += tex2D(_MainTex, i.uv - direction * sampleOffset);
					totalWeight += sampleWeight;
				}

				
				blurColor /= totalWeight;

				
				fixed4 finalColor = lerp(color, blurColor, _BlurStrength);

				
				finalColor.a *= _Alpha;

				return finalColor;
			}
			ENDCG
		}
		}
}
