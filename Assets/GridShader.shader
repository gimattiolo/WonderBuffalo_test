Shader "Unlit/GridShader"
{
	Properties
	{
		_WorldPositionOffsets("World Position Offsets", 2D) = "white" {}
		_TextureBefore("Texture Before", 2D) = "white" {}
		_TextureAfter("Texture After", 2D) = "white" {}
		_TotalAmplitude("Total Amplitude", Range(0, 1000)) = 0
		_MaxClamp("Max Clamp", Range(0, 1000)) = 0
		_OffsetDirection("Offset Direction", Vector) = (0,0,0,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		//Pass{
		//	Color(0, 0, 0, 0)
		//}
		Pass
		{
			Cull off
			ZTest Always
			Fog { Mode Off}

			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct vIn
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				//float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 offset : COLOR0;
			};

			sampler2D _WorldPositionOffsets;
			uniform float4 _WorldPositionOffsets_ST; // Needed for TRANSFORM_TEX(v.texcoord, _DispTex)
			
			sampler2D _TextureBefore;
			uniform float4 _TextureBefore_ST;
			sampler2D _TextureAfter;

			float _TotalAmplitude;
			float4 _OffsetDirection;

			v2f vert (vIn v)
			{
				v2f o;

				// get texture value == vertex offset
				o.offset = float4(0.0, 0.0, 0.0, 1.0);

#if !defined(SHADER_API_OPENGL)
				float4 tex = tex2Dlod(_WorldPositionOffsets, float4(v.uv.xy, 0, 0));

				o.offset.b = _TotalAmplitude * (2.0 * tex.b - 1.0);

				v.vertex.y += o.offset.b;
#endif
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _WorldPositionOffsets);
				return o;
			}
			

			float _MaxClamp;
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the textures
				fixed4 colBefore = tex2D(_TextureBefore, i.uv);
				fixed4 colAfter = tex2D(_TextureAfter, i.uv);
			
				float a = abs(clamp(i.offset.b, -_MaxClamp, _MaxClamp) / _MaxClamp);

				return lerp(colAfter, colBefore, a);
				//return tex2D(_WorldPositionOffsets, i.uv);
				//return colBefore;// fixed4(0, 0, col.b, 1.0);
			}
			ENDCG
		}
	}
}
