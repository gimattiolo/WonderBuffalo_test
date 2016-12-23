Shader "Custom/GlowSurfaceShader" {

	Properties {
		_ColorTint ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimPower("Rim Power", Range(0, 5)) = 1
	}

	SubShader{
		Tags{ "RenderType" = "Opaque" }

		CGPROGRAM

		#pragma surface surf Lambert

		struct Input {
			float4 color : COLOR;
			float2 uv_MainTex;
			float3 viewDir;
			float3 worldNormal;
		};

		sampler2D _MainTex;
		float4 _ColorTint;
		float4 _RimColor;
		float _RimPower;

		void surf(Input i, inout SurfaceOutput o) {
			o.Albedo = tex2D(_MainTex, i.uv_MainTex).rgb * _ColorTint;
			half rim = 1.0 - saturate(dot(normalize(i.viewDir), i.worldNormal));
			o.Emission = _RimColor.rgb * pow(rim, _RimPower);

		}
		ENDCG
	}
}

