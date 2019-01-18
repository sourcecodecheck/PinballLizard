// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Gradient_3Color_Outline" {
	Properties{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
	_Color("Main Color", Color) = (.5,.5,.5,1)
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Outline("Outline width", Range(0.0, 0.03)) = .005
		_MainTex("Base (RGB)", 2D) = "white" { }
		_ColorTop("Top Color", Color) = (1,1,1,1)
		_ColorMid("Mid Color", Color) = (1,1,1,1)
		_ColorBot("Bot Color", Color) = (1,1,1,1)
		_Middle("Middle", Range(0.001, 0.999)) = 1
	}
		CGINCLUDE
#include "UnityCG.cginc"

			struct appdata {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
		};

		struct v2f {
			float4 pos : POSITION;
			float4 color : COLOR;
		};

		uniform float _Outline;
		uniform float4 _OutlineColor;

		v2f vert(appdata v) {
			// just make a copy of incoming vertex data but scaled according to normal direction
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);

			float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
			float2 offset = TransformViewToProjection(norm.xy);

			o.pos.xy += offset * o.pos.z * _Outline;
			o.color = _OutlineColor;
			return o;
		}
		ENDCG

		SubShader{
			Tags {"Queue" = "Background"  "IgnoreProjector" = "True"}
			LOD 100

			ZWrite On

			Pass {
			CGPROGRAM
			#pragma vertex vert  
			#pragma fragment frag
			#include "UnityCG.cginc"

			half4 frag(v2f i) :COLOR {
			return i.color;
			}

			fixed4 _ColorTop;
			fixed4 _ColorMid;
			fixed4 _ColorBot;
			float  _Middle;

			struct v2f {
				float4 pos : SV_POSITION;
				float4 texcoord : TEXCOORD0;
			};

			v2f vert(appdata_full v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				return o;
			}

			fixed4 frag(v2f i) : COLOR {
				fixed4 c = lerp(_ColorBot, _ColorMid, i.texcoord.y / _Middle) * step(i.texcoord.y, _Middle);
				c += lerp(_ColorMid, _ColorTop, (i.texcoord.y - _Middle) / (1 - _Middle)) * step(_Middle, i.texcoord.y);
				c.a = 1;
				return c;
			}
			ENDCG
			}

			Pass {
			Name "BASE"
			ZWrite On
			ZTest LEqual
			Blend SrcAlpha OneMinusSrcAlpha
			Material {
				Diffuse[_Color]
				Ambient[_Color]
			}
			Lighting On
			SetTexture[_MainTex] {
				ConstantColor[_Color]
				Combine texture * constant
			}
			SetTexture[_MainTex] {
				Combine previous * primary DOUBLE
			}
		}
		}

			SubShader{
				Tags { "Queue" = "Transparent" }

				Pass {
					Name "OUTLINE"
					Tags { "LightMode" = "Always" }
					Cull Front
					ZWrite Off
					ZTest Always
					ColorMask RGB

				// you can choose what kind of blending mode you want for the outline
				Blend SrcAlpha OneMinusSrcAlpha // Normal
				//Blend One One // Additive
				//Blend One OneMinusDstColor // Soft Additive
				//Blend DstColor Zero // Multiplicative
				//Blend DstColor SrcColor // 2x Multiplicative

				CGPROGRAM
				#pragma vertex vert
				#pragma exclude_renderers gles xbox360 ps3
				ENDCG
				SetTexture[_MainTex] { combine primary }
			}

			Pass {
				Name "BASE"
				ZWrite On
				ZTest LEqual
				Blend SrcAlpha OneMinusSrcAlpha
				Material {
					Diffuse[_Color]
					Ambient[_Color]
				}
				Lighting On
				SetTexture[_MainTex] {
					ConstantColor[_Color]
					Combine texture * constant
				}
				SetTexture[_MainTex] {
					Combine previous * primary DOUBLE
				}
			}
			}

				Fallback "Diffuse"
}
		}
}