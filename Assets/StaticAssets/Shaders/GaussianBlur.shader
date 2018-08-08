﻿Shader "Custom/GaussianBlur"
{
	Properties
	{
		[PerRendererData] _MainTex("_MainTex", 2D) = "white" {}
		_Lightness("_Lightness", Range(0,2)) = 1
		_Saturation("_Saturation", Range(-10,10)) = 1


	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Pass
		{
			ZWrite Off
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				half4 color : COLOR;
				half4 screenpos : TEXCOORD2;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 screenuv : TEXCOORD1;
				half4 color : COLOR;
				float2 screenpos : TEXCOORD2;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.screenuv = ((o.vertex.xy / o.vertex.w) + 1) * 0.5;
				o.color = v.color;
				o.screenpos = ComputeScreenPos(o.vertex);
				return o;
			}

			float2 safemul(float4x4 M, float4 v)
			{
				float2 r;

				r.x = dot(M._m00_m01_m02, v);
				r.y = dot(M._m10_m11_m12, v);

				return r;
			}

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;

			uniform float _Lightness;
            uniform float _Saturation;

            uniform sampler2D _Blur;

			float4 frag(v2f i) : SV_Target
			{
				float4 m = tex2D(_MainTex, i.uv);


				float2 uvWH = float2(_MainTex_TexelSize.z / _ScreenParams.x,_MainTex_TexelSize.w / _ScreenParams.y);
				uvWH.x *= _MainTex_TexelSize.x;
				uvWH.y *= _MainTex_TexelSize.y;



				float2 buv = float2(i.screenpos.x - (uvWH.x / 2),i.screenpos.y - (uvWH.y / 2));


                float4 blurColor = float4(0,0,0,1);
                blurColor = tex2D(_Blur,buv);


                blurColor.a = 1;
                            
				float4 finalColor = blurColor * i.color;
				finalColor.a = 1;


                finalColor.rgb *= _Lightness;


                float3 intensity = dot(finalColor.rgb, float3(0.299,0.587,0.114));
                finalColor.rgb = lerp(intensity, finalColor.rgb  , _Saturation);
                            
				return finalColor;
			}
			ENDCG
		}
	}

	Fallback "Sprites/Default"
}