Shader "Custom/WavySprite"{
	Properties{
		_MainTex("Texture",2D)="white"{}
		_Color("Tint",Color)=(1,1,1,1)
		_WaveDirection("Wave Direction",range(0,1))=0
		_StaticSide("Static Side",range(0,4))=3
		_WaveFrequency("Wave Frequency",float)=10
		_WaveForce("Wave Force",float)=0.1
		_WaveSpeed("Wave Speed",float)=1
	}
	SubShader
	{
		Tags{
			"Queue"="Transparent" 
			"IgnoreProjector"="True"
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
		}
		ZWrite Off 
		Lighting Off 
		Cull Off 
		Fog { Mode Off } 
		Blend One Zero
		Pass{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM

			#pragma exclude_renderers xbox360 ps3 flash
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata{
				float4 pos:POSITION;
				float2 uv:TEXCOORD0;
			};

			struct v2f{
				float4 pos:SV_POSITION;
				float2 uv:TEXCOORD0;
			};

			sampler2D _MainTex;
			float _WaveDirection;
			float _StaticSide;
			float _WaveFrequency;
			float _WaveForce;
			float _WaveSpeed;

			v2f vert(appdata v){
				v2f o;
				float3 unit=UnityObjectToClipPos(float3(1,1,0))-UnityObjectToClipPos(float3(0,0,0));
				float3 wpos=mul(unity_ObjectToWorld,v.pos).xyz;
				o.pos=UnityObjectToClipPos(v.pos);
				//Decide a static side
				float multiplier=0;
				if(_StaticSide==1) multiplier=1-v.uv.y; //Top
				if(_StaticSide==2) multiplier=1-v.uv.x; //Right
				if(_StaticSide==3) multiplier=v.uv.y; //Bottom
				if(_StaticSide==4) multiplier=v.uv.x; //Left
				if(_StaticSide==0) multiplier=1; //None
				//Horizontal waves
				if(_WaveDirection==0) o.pos.x+=sin((wpos.y*_WaveFrequency)-(_Time.a*_WaveSpeed))*(_WaveForce*unit.x*multiplier);
				//Vertical waves
				else o.pos.y+=sin((wpos.x*_WaveFrequency)-(_Time.a*_WaveSpeed))*(_WaveForce*unit.y*multiplier);

				o.uv=v.uv;
				return o;
			}

			fixed4 _Color;
			
			fixed4 frag (v2f i):SV_Target{
				fixed4 col=tex2D(_MainTex,i.uv)*_Color;
				return col;
			}
			ENDCG
		}
	}
}