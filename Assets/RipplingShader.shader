Shader "Unlit/RipplingShader"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}

		_Amplitude("Amplitude", Range(0, 1000)) = 1
		_TemporalOmega("Temporal Omega", Range(0, 1000)) = 0
		_SpatialOmega("Spatial Omega", Range(0, 1000)) = 0
		_Offset("Offset", Range(0, 1000)) = 0
		_TemporalDecay("Temporal Decay", Range(0, 1000)) = 0
		_SpatialDecay("Spatial Decay", Range(0, 1000)) = 0

		_T("Current Time", Float) = 0
		_T0("Initial Time", Float) = 0
		_TotalAmplitude("TotalAmplitude", Float) = 1
		_Center("Center", Vector) = (0, 0, 0, 1)
		//_BBox("Bounding Box Size", Vector) = (0, 0, 0, 1)
		_Accumulate("Debug Index", Float) = 0

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			

			

			#include "UnityCG.cginc"

			struct vIn
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (vIn v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			float4 _Center;
			//float4 _BBox;
			float _SpatialDecay;
			float _TemporalDecay; 
			float _Amplitude;
			float _TemporalOmega;
			float _SpatialOmega;
			float _Offset;
			float _T;
			float _T0;
			float _TotalAmplitude;
			float _Accumulate;

			fixed4 frag(v2f i) : SV_Target
			{
				// distance from center of the ripple
				float dist = distance(_Center.xy, i.uv.xy);

				float dt = _T - _T0;
				
				float wave = -cos((dt * _TemporalOmega) + (dist * _SpatialOmega) + _Offset);

				// calculate amplitude
				float amp = _Amplitude * exp(dist * _SpatialDecay + dt * _TemporalDecay);

				// normalize between -_Amplitude and _Amplitude
				float normDisp = lerp(amp * wave, _Amplitude, 0.5);
				normDisp /= _TotalAmplitude;

				//float currDisp = normDisp;
				//if (_Accumulate > 0.0)
				//{ 
				//	currDisp += tex2D(_MainTex, i.uv).b;
				//}

				// temporary solution
				// probably it is better with a blend option
				float currDisp = lerp(normDisp, normDisp + tex2D(_MainTex, i.uv).b, _Accumulate);

				return fixed4(currDisp, currDisp, currDisp, 1.0);
			}
			ENDCG
		}

	}
}
