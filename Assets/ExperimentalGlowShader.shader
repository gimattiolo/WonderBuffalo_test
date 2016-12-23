Shader "Outlined/Silhouetted Diffuse" {
	Properties{
		_Color("Main Color", Color) = (.5,.5,.5,1)

		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Outline("Outline width", Range(0.0, 0.03)) = .005

		_MainTex("Base (RGB)", 2D) = "white" { }

	_ColorTint("ColorTint", Color) = (1,1,1,1)
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimPower("Rim Power", Range(0, 5)) = 1
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
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

		float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
		float2 offset = TransformViewToProjection(norm.xy);

		o.pos.xy += offset * o.pos.z * _Outline;
		o.color = _OutlineColor;
		return o;
	}
	ENDCG

	SubShader{
		Tags{ "Queue" = "Transparent" }

		// note that a vertex shader is specified here but its using the one above
		Pass{
			Name "OUTLINE"
			Tags{ "LightMode" = "Always" }
			Cull Off
			ZWrite Off
			ZTest Always
			ColorMask RGB // alpha not used

							// you can choose what kind of blending mode you want for the outline
			Blend SrcAlpha OneMinusSrcAlpha // Normal
											//Blend One One // Additive
											//Blend One OneMinusDstColor // Soft Additive
											//Blend DstColor Zero // Multiplicative
											//Blend DstColor SrcColor // 2x Multiplicative

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			half4 frag(v2f i) :COLOR{
				return i.color;
			}
			ENDCG
		}

		Pass{
			Name "BASE"
			ZWrite On
			ZTest LEqual
			Blend SrcAlpha OneMinusSrcAlpha
			Material{
				Diffuse[_Color]
				Ambient[_Color]
			}
			Lighting On
			SetTexture[_MainTex]{
				ConstantColor[_Color]
				Combine texture * constant
			}
			SetTexture[_MainTex]{
				Combine previous * primary DOUBLE
			}


		//CGPROGRAM
		//#pragma target 3.0
		//#pragma vertex vert
		//#pragma fragment frag
		//#include "UnityCG.cginc"

		//struct v2f2
		//{
		//	float2 uv : TEXCOORD0;
		//	// in clip space
		//	float4 vertex : SV_POSITION;
		//	// in world space
		//	float4 vertexW : COLOR0;
		//	float4 normalW : NORMAL;
		//};

		//sampler2D _MainTex;
		//uniform float4 _MainTex_ST;
		//float4 _ColorTint;
		//float4 _RimColor;
		//float _RimPower;

		//v2f2 vert(appdata_base v) {
		//	v2f2 o;
		//	o.vertex = UnityObjectToClipPos(v.vertex);
		//	o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
		//	o.normalW = mul(unity_ObjectToWorld, float4(v.normal, 0.0));
		//	o.vertexW = mul(unity_ObjectToWorld, v.vertex);
		//	return o;
		//}

		//fixed4 frag(v2f2 i) : SV_Target
		//{
		//	fixed4 col;
		//	// sample the texture
		//	col = tex2D(_MainTex, i.uv).rgba * _ColorTint;


		//	half rim = 1.0 - saturate(dot(normalize(i.vertexW), i.normalW));
		//	return col * _RimColor.rgb * pow(rim, _RimPower);
		//}
		//ENDCG

//		CGPROGRAM
//
//#pragma surface surf Lambert
//
//
//
//		struct Input {
//		float4 color : COLOR;
//		float2 uv_MainTex;
//		float3 viewDir;
//		float3 worldNormal;
//	};
//
//	float4 _ColorTint;
//	float4 _RimColor;
//	float _RimPower;
//	sampler2D _MainTex;
//
//	void surf(Input i, inout SurfaceOutput o) {
//		o.Albedo = tex2D(_MainTex, i.uv_MainTex).rgb * _ColorTint;
//		half rim = 1.0 - saturate(dot(normalize(i.viewDir), i.worldNormal));
//		o.Emission = _RimColor.rgb * pow(rim, _RimPower);
//
//	}
//	ENDCG
	
		}

	}

	Fallback "Diffuse"
}