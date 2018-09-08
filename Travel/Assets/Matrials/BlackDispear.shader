Shader "UI/ImageGreyShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Fuck("Fuck", Range(0,1)) = 0
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
		CGPROGRAM
#pragma vertex vert     
#pragma fragment frag     
#include "UnityCG.cginc"     

		struct appdata_t
	{
		float4 vertex   : POSITION;
		float4 color    : COLOR;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f
	{
		float4 vertex   : SV_POSITION;
		fixed4 color : COLOR;
		half2 texcoord  : TEXCOORD0;
	};

	sampler2D _MainTex;
	fixed _Fuck;

	v2f vert(appdata_t IN)
	{
		v2f OUT;
		OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
		OUT.texcoord = IN.texcoord;   
		OUT.color = IN.color;
		return OUT;
	}

	fixed4 frag(v2f IN) : SV_Target
	{
		half4 color = tex2D(_MainTex, IN.texcoord) * IN.color;
		//float grey = dot(color.rgb, fixed3(0.22, 0.707, 0.071));
		//return half4(grey,grey,grey,color.a);
		clip(color.x-_Fuck);
		
		//if (color.x <= _Fuck )
		//{
			//return half4(0, 0, 0, 0);
			//discard;
			//return;
		//}
		return color;
	}
		ENDCG
	}
	}
}