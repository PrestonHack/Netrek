﻿Shader "Fading/Surface/ScreenFading/MetalBumpedDouble"
{
    
	Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
		_MetallicGlossMap("Metallic", 2D) = "white" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}
        _Occlusion ("Occlusion Map", 2D) = "white" {}
		[Toggle] _inverse("inverse", Float) = 0
		[HideInInspector][Toggle(SCREENDISSOLVE)] _dissolve("dissolveTexture", Float) = 1
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType"="Clipping" }
        LOD 200

		Cull Front

        CGPROGRAM
 
        //#pragma surface surf Standard nolightmap no alpha:fade

		#pragma surface surf NoLighting nolightmap

		#pragma multi_compile __ FADE_PLANE FADE_SPHERE
		#pragma shader_feature SCREENDISSOLVE
		#include "CGIncludes/section_clipping_CS.cginc"

        #pragma target 3.0
 
        sampler2D _MainTex;
 
        struct Input {
            float2 uv_MainTex;
			float3 worldPos;
			float4 screenPos;
        };
 
        half _Glossiness;
        fixed4 _Color;


		 fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
         {
             fixed4 c;
             c.rgb = s.Albedo * 0.5; 
             c.a = s.Alpha;
             return c;
         }

 
        void surf (Input IN, inout SurfaceOutput o)
        {
			#if (FADE_PLANE || FADE_SPHERE) && SCREENDISSOLVE 

			float4 fade = PLANE_FADE(IN.worldPos);

			float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			screenUV *= float2(_ScreenNoiseScale*_ScreenParams.x/_ScreenNoise_TexelSize.z,_ScreenNoiseScale*_ScreenParams.y/_ScreenNoise_TexelSize.z);
			float f = tex2D (_ScreenNoise, screenUV).r;

			bool eval = f>=fade.a&&fade.a<1;
			if(eval&&_inverse==0) discard;
			if(!eval&&_inverse==1) discard;

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
			
			o.Albedo *= fade.rgb*2;

			#else
					
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;

			#endif
        }

        ENDCG


		Cull Back
   
        CGPROGRAM
 
        #pragma surface surf Standard addshadow

		#pragma multi_compile __ FADE_PLANE FADE_SPHERE
		#pragma shader_feature SCREENDISSOLVE
		#include "CGIncludes/section_clipping_CS.cginc"

        #pragma target 3.0
 
        sampler2D _MainTex;
		sampler2D _MetallicGlossMap;
		sampler2D _BumpMap;
        sampler2D _Occlusion;
 
        struct Input {
            float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 worldPos;
			float4 screenPos;
        };
 
        half _Glossiness;
        fixed4 _Color;
 
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            
			#if (FADE_PLANE || FADE_SPHERE) && SCREENDISSOLVE 

			float4 fade = PLANE_FADE(IN.worldPos);

			float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			screenUV *= float2(_ScreenNoiseScale*_ScreenParams.x/_ScreenNoise_TexelSize.z,_ScreenNoiseScale*_ScreenParams.y/_ScreenNoise_TexelSize.z);
			float f = tex2D (_ScreenNoise, screenUV).r;

			bool eval = f>=fade.a&&fade.a<1;
			if(_inverse==1) eval = !eval;
			if(eval) discard;

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 Occ = tex2D(_Occlusion, IN.uv_MainTex);  
			o.Albedo = c.rgb * Occ.rgb;
			fixed4 metal = tex2D(_MetallicGlossMap, IN.uv_MainTex);
			o.Metallic = metal.r;
			o.Smoothness = metal.a * _Glossiness;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            o.Alpha = c.a;
			
			o.Albedo *= fade.rgb*2;

			#else
					
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 Occ = tex2D(_Occlusion, IN.uv_MainTex);  
			o.Albedo = c.rgb * Occ.rgb;
			fixed4 metal = tex2D(_MetallicGlossMap, IN.uv_MainTex);
			o.Metallic = metal.r;
			o.Smoothness = metal.a * _Glossiness;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            o.Alpha = c.a;

			#endif
        }

        ENDCG
    }
    FallBack "Standard"
}
