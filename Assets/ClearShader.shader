Shader "Unlit/ClearShader"
{
	Properties
	{
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		Pass{
			Color(0, 0, 0, 0)
		}
		//Pass
		//{
			
		
			//CGPROGRAM
			//#pragma vertex vert
			//#pragma fragment frag
			//
			//#include "UnityCG.cginc"

			//struct vIn
			//{
			//	float4 vertex : POSITION;
			//	float2 uv : TEXCOORD0;
			//};

			//struct v2f
			//{
			//	float2 uv : TEXCOORD0;
			//	float4 vertex : SV_POSITION;
			//};

			//sampler2D _MainTex;
			//float4 _MainTex_ST;
			//v2f vert (vIn v)
			//{
			//	v2f o;
			//	o.vertex = UnityObjectToClipPos(v.vertex);
			//	o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			//	return o;
			//}
			//
			//fixed4 frag (v2f i) : SV_Target
			//{
			//	return fixed4(0.0, 0.0, 0.0, 1.0);
			//}
			//ENDCG
		//}
	}
}
