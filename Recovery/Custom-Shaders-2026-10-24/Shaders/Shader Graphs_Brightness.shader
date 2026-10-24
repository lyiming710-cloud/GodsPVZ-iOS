Shader "Shader Graphs/Brightness"
{
    Properties
    {
        [NoScaleOffset] _MainTex ("MainTex", 2D) = "white" {}
        _Brightness ("Brightness", Range(1, 4)) = 1
        _Transparent ("Transparent", Range(0, 1)) = 1
        [HideInInspector] [NoScaleOffset] unity_Lightmaps ("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector] [NoScaleOffset] unity_LightmapsInd ("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector] [NoScaleOffset] unity_ShadowMasks ("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags { "QUEUE"="Transparent" "RenderType"="Transparent" "RenderPipeline"="UniversalPipeline" "ShaderGraphShader"="true" "UniversalMaterialType"="Lit" "ShaderGraphTargetId"="UniversalSpriteLitSubTarget" }
        Cull Off
        ZWrite Off
        ZTest LEqual
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "RecoveredBrightness.hlsl"
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_TexelSize;
            float _Brightness;
            float _Transparent;
        CBUFFER_END
        float4 _RendererColor;
        struct Attributes
        {
            float3 positionOS : POSITION;
            float4 tangentOS : TANGENT;
            float4 uv : TEXCOORD0;
            float4 color : COLOR;
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float4 uv : TEXCOORD0;
            float4 color : COLOR;
            float4 screenPosition : TEXCOORD1;
        };
        Varyings SpriteVertex(Attributes input)
        {
            Varyings output;
            output.positionCS = TransformObjectToHClip(input.positionOS);
            output.uv = input.uv;
            output.color = input.color * _RendererColor;
            float4 clip = output.positionCS;
            output.screenPosition = float4((float2(clip.x, clip.y * _ProjectionParams.x) + clip.w) * 0.5, clip.zw);
            return output;
        }
        half4 SampleMain(float2 uv)
        {
            return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
        }
        ENDHLSL
        Pass
        {
            Name "Sprite Lit"
            Tags { "LightMode"="Universal2D" }
            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex SpriteVertex
            #pragma fragment LitFragment
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_0 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_1 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_2 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_3 __
            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"
            #if USE_SHAPE_LIGHT_TYPE_0
                SHAPE_LIGHT(0)
            #endif
            #if USE_SHAPE_LIGHT_TYPE_1
                SHAPE_LIGHT(1)
            #endif
            #if USE_SHAPE_LIGHT_TYPE_2
                SHAPE_LIGHT(2)
            #endif
            #if USE_SHAPE_LIGHT_TYPE_3
                SHAPE_LIGHT(3)
            #endif
            half _HDREmulationScale;
            half _UseSceneLighting;
            half4 LitFragment(Varyings input) : SV_Target
            {
                float4 color = RecoveredBrightnessColor(SampleMain(input.uv.xy), input.color, _Brightness, _Transparent);
                float2 lightUV = input.screenPosition.xy / input.screenPosition.w;
                half3 modulate = 0;
                half3 additive = 0;
                #if USE_SHAPE_LIGHT_TYPE_0
                    half3 light0 = RecoveredBrightnessLight(SAMPLE_TEXTURE2D(_ShapeLightTexture0, sampler_ShapeLightTexture0, lightUV).rgb, _ShapeLightMaskFilter0, _ShapeLightInvertedFilter0);
                    modulate += light0 * _ShapeLightBlendFactors0.x;
                    additive += light0 * _ShapeLightBlendFactors0.y;
                #endif
                #if USE_SHAPE_LIGHT_TYPE_1
                    half3 light1 = RecoveredBrightnessLight(SAMPLE_TEXTURE2D(_ShapeLightTexture1, sampler_ShapeLightTexture1, lightUV).rgb, _ShapeLightMaskFilter1, _ShapeLightInvertedFilter1);
                    modulate += light1 * _ShapeLightBlendFactors1.x;
                    additive += light1 * _ShapeLightBlendFactors1.y;
                #endif
                #if USE_SHAPE_LIGHT_TYPE_2
                    half3 light2 = RecoveredBrightnessLight(SAMPLE_TEXTURE2D(_ShapeLightTexture2, sampler_ShapeLightTexture2, lightUV).rgb, _ShapeLightMaskFilter2, _ShapeLightInvertedFilter2);
                    modulate += light2 * _ShapeLightBlendFactors2.x;
                    additive += light2 * _ShapeLightBlendFactors2.y;
                #endif
                #if USE_SHAPE_LIGHT_TYPE_3
                    half3 light3 = RecoveredBrightnessLight(SAMPLE_TEXTURE2D(_ShapeLightTexture3, sampler_ShapeLightTexture3, lightUV).rgb, _ShapeLightMaskFilter3, _ShapeLightInvertedFilter3);
                    modulate += light3 * _ShapeLightBlendFactors3.x;
                    additive += light3 * _ShapeLightBlendFactors3.y;
                #endif
                return RecoveredBrightnessLit(color, modulate, additive, _HDREmulationScale, _UseSceneLighting);
            }
            ENDHLSL
        }
        Pass
        {
            Name "Sprite Normal"
            Tags { "LightMode"="NormalsRendering" }
            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex NormalVertex
            #pragma fragment NormalFragment
            struct NormalVaryings { float4 positionCS : SV_POSITION; float4 tangentWS : TEXCOORD0; float4 uv : TEXCOORD1; float3 viewNormal : TEXCOORD2; };
            NormalVaryings NormalVertex(Attributes input)
            {
                NormalVaryings output;
                output.positionCS = TransformObjectToHClip(input.positionOS);
                float3 tangent = mul((float3x3)unity_ObjectToWorld, input.tangentOS.xyz);
                tangent *= rsqrt(max(dot(tangent, tangent), 1.17549435e-38));
                output.tangentWS = float4(tangent, input.tangentOS.w);
                output.uv = input.uv;
                output.viewNormal = UNITY_MATRIX_V[2].xyz;
                return output;
            }
            half4 NormalFragment(NormalVaryings input) : SV_Target
            {
                return RecoveredBrightnessNormal(SampleMain(input.uv.xy), input.viewNormal, _Transparent);
            }
            ENDHLSL
        }
        Pass
        {
            Name "Sprite Forward"
            Tags { "LightMode"="UniversalForward" }
            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex SpriteVertex
            #pragma fragment ForwardFragment
            half4 ForwardFragment(Varyings input) : SV_Target
            {
                // The original forward program has neither alpha discard nor max(0).
                return RecoveredBrightnessColor(SampleMain(input.uv.xy), input.color, _Brightness, _Transparent);
            }
            ENDHLSL
        }
    }
    Fallback "Hidden/Shader Graph/FallbackError"
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
}
