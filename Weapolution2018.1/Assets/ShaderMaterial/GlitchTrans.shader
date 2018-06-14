Shader "Custom/GlitchTrans"
{
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_LuminosityAmount("GrayScale Amount", Range(0.0, 1.0)) = 1.0
		_glitchY("ahahyeeY",float) = 0.0
		_glitchHeight("height",float) = 0.0
		_glitchColumnGap("gap", float) = 0.0
		_glitchOffsetX("offsetX", float) = 0.0
		_glitchColOffestR("offsetR", float) = 0.0
		_glitchColOffestG("offsetG", float) = 0.0
		_glitchColOffestB("offsetB", float) = 0.0
		_glitchWhiteChange("offsetB", float) = 0.0
	}
		SubShader{
		Pass{
		CGPROGRAM
#pragma vertex vert_img  
#pragma fragment frag  

#include "UnityCG.cginc"  

		uniform sampler2D _MainTex;
	fixed _LuminosityAmount;
	fixed _glitchY;
	fixed _glitchHeight;
	fixed _glitchColumnGap;
	fixed _glitchOffsetX;
	fixed _glitchColOffestR;
	fixed _glitchColOffestG;
	fixed _glitchColOffestB;
	fixed _glitchWhiteChange;
	float2 _offset;
	//fixed4 renderTex;
	fixed4 finalColor;


	fixed4 frag(v2f_img i) : COLOR
	{
		//Get the colors from the RenderTexture and the uv's  
		//from the v2f_img struct  
		for (int col = 0; col < 2; col++) {
			fixed posY;
			posY = sin(_glitchY);
			if (posY < 0.0) { posY *= -1.0; }
			posY += ((float)col * _glitchColumnGap);
			while (posY > 1.0) { posY -= 1.0; }
			//while (posY < -0.0) { posY += 1.0; }

			fixed top = posY + _glitchHeight;
			//_glitchHeight = (_glitchY + _glitchHeight);
			if (top > 1.0) { top = 1.0; }
			if (i.uv.y >= posY && i.uv.y <= top) {
				_offset.x = i.uv.x + _glitchOffsetX;
				if (_offset.x > 1.0) { _offset.x -= 1.0; }
				if (_offset.x < 0.0) { _offset.x += 1.0; }
				_offset.y = i.uv.y;
				finalColor = tex2D(_MainTex, _offset);
				
				/*finalColor.x = tex2D(_MainTex, _offset + _glitchColOffestR).x;
				finalColor.y = tex2D(_MainTex, _offset + _glitchColOffestG).y;
				finalColor.z = tex2D(_MainTex, _offset + _glitchColOffestB).z;*/
				
				break;
				//if (col > 0) { finalColor = (1.0*(float)col, 1.0*(float)col, 1.0*(float)col, 1.0*(float)col); }
			}
			else {
				finalColor = tex2D(_MainTex, i.uv );

				fixed2 tempR = fixed2(i.uv.x + _glitchColOffestR, i.uv.y);
				fixed2 tempG = fixed2(i.uv.x + _glitchColOffestG, i.uv.y);
				fixed2 tempB = fixed2(i.uv.x + _glitchColOffestB, i.uv.y);
				finalColor.x = tex2D(_MainTex, tempR).x;
				finalColor.y = tex2D(_MainTex, tempG).y;
				finalColor.z = tex2D(_MainTex, tempB).z;

				/*finalColor.x = tex2D(_MainTex, i.uv + _glitchColOffestR).x;
				finalColor.y = tex2D(_MainTex, i.uv + _glitchColOffestG).y;
				finalColor.z = tex2D(_MainTex, i.uv + _glitchColOffestB).z;*/
			}
		}
	//if (i.uv.x > 0.3 && i.uv.x < 0.8) { renderTex = tex2D(_MainTex, _offset); }
	//else { renderTex = tex2D(_MainTex, i.uv ); }
	//Apply the Luminosity values to our render texture  
	//float luminosity = 0.299 * renderTex.r + 0.587 * renderTex.g + 0.114 * renderTex.b;
	//fixed4 finalColor = lerp(renderTex, luminosity, _LuminosityAmount);
	if (_glitchWhiteChange > 0.0) {
		finalColor.x += _glitchWhiteChange;
		finalColor.y += _glitchWhiteChange;
		finalColor.z += _glitchWhiteChange;
		finalColor.w += _glitchWhiteChange;
	}
	
	return finalColor;
	}

		ENDCG
	}
	}
		FallBack "Diffuse"
}
