Shader "Custom/Texture"
{
	Properties
	{
		_MainTex1("Texture", 2D) = "white" {}
	}
		SubShader
		{
			Tags {"Queue" = "Transparent"}

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"


				sampler2D _MainTex1;


				struct v2f
				{
					float4 pos : SV_POSITION;
					float4 uv : TEXCOORD0;
				};



				v2f vert(appdata_base v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = v.texcoord;

					return o;
				}


				fixed4 frag(v2f i) : SV_Target
				{

					//i.uv.x += _SinTime.w

					float tex1 = tex2D(_MainTex1, i.uv);
				

					return tex1;
				}
				ENDCG
			}
		}
}