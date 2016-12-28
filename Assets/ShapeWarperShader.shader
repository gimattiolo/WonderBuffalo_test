Shader "Unlit/ShapeWarperShader"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Alpha("Alpha", Range(0, 1)) = 0
		_Radius("Radius", Range(0, 10)) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Cull off

			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct vIn
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};


			float _Alpha;
			float _Radius;
			
			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert (vIn v)
			{
				v2f o;

#if !defined(SHADER_API_OPENGL)
				float3 nV = normalize(v.vertex.xyz);
				v.vertex.xyz = lerp(v.vertex.xyz, _Radius * nV, _Alpha);
				v.vertex.w = 1;
#endif

				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				//o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				//col.a = _Alpha;
				return col;
			}
			ENDCG
		}
	}
}
