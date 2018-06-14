Shader "Sprites/SpriteStencilMask" {
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
		
		_OutLine("isOutline",Range(0.0,1.0)) = 0.0
		_LineColor("Line Color", Color) = (1,1,1,1)
		_OutLineSpreadX("Outline Spread", Range(0,0.03)) = 0.003
		_OutLineSpreadY("Outline Spread", Range(0,0.03)) = 0.003
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

		Cull Off
		Lighting Off
		ZWrite On
		Blend One OneMinusSrcAlpha
		Pass
	{
		stencil{
		Ref 1
		Comp Always
		Pass Replace
		}
		CGPROGRAM
#pragma vertex SpriteVert
#pragma fragment frag
#pragma target 2.0
#pragma multi_compile_instancing
#pragma multi_compile _ PIXELSNAP_ON
#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
#include "UnitySprites.cginc"

	/*	struct appdata_t
	{
		float4 vertex   : POSITION;
		float4 color    : COLOR;
		float2 texcoord : TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2f
	{
		float4 vertex   : SV_POSITION;
		fixed4 color : COLOR;
		float2 texcoord  : TEXCOORD0;
		UNITY_VERTEX_OUTPUT_STEREO
	};

	fixed4 _Color;

	v2f vert(appdata_t IN)
	{
		v2f OUT;
		UNITY_SETUP_INSTANCE_ID(IN);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
		OUT.vertex = UnityObjectToClipPos(IN.vertex);
		OUT.texcoord = IN.texcoord;
		OUT.color = IN.color * _Color;
#ifdef PIXELSNAP_ON
		OUT.vertex = UnityPixelSnap(OUT.vertex);
#endif

		return OUT;
	}*/

	//sampler2D _MainTex;
	//sampler2D _AlphaTex;

//	fixed4 SampleSpriteTexture(float2 uv)
//	{
//		fixed4 color = tex2D(_MainTex, uv);
//
//#if ETC1_EXTERNAL_ALPHA
//		// get the color from an external texture (usecase: Alpha support for ETC1 on android)
//		color.a = tex2D(_AlphaTex, uv).r;
//#endif //ETC1_EXTERNAL_ALPHA
//
//		return color;
//	}


	fixed4 _LineColor;
	float _OutLineSpreadX;
	float _OutLineSpreadY;
	float _Outline;

	fixed4 frag(v2f IN) : SV_Target
	{
		fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
	c.rgb *= c.a;
	if (_Outline > 0.0f) {
		fixed4 TempColor = tex2D(_MainTex, IN.texcoord + float2(_OutLineSpreadX, 0.0)) + tex2D(_MainTex, IN.texcoord - float2(_OutLineSpreadX, 0.0));
		TempColor = TempColor + tex2D(_MainTex, IN.texcoord + float2(0.0, _OutLineSpreadY)) + tex2D(_MainTex, IN.texcoord - float2(0.0, _OutLineSpreadY));
		if (TempColor.a > 0.1) {
			TempColor.a = 1.0f;
		}
		fixed4 AlphaColor = fixed4(1, 1, 1, TempColor.a);
		fixed4 mainColor = AlphaColor * _LineColor;
		mainColor.rgb *= mainColor.a;
		if (c.a > 0.95) {
			mainColor = c;
		}
		return mainColor;
	}
	else {
		return c;
	}
	}
		ENDCG
	}
	}
}
