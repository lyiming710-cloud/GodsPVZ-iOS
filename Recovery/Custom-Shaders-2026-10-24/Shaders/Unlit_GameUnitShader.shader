Shader "Unlit/GameUnitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AngleX ("AngleX", Float) = 0
        _AngleY ("AngleY", Float) = 0
        _ScaleX ("ScaleX", Float) = 1
        _ScaleY ("ScaleY", Float) = 1
        _Alpha ("Alpha", Float) = 1
        _IsVisible ("IsVisible", Range(-1, 0)) = 0
        _Exposure ("Exposure", Float) = 1
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "QUEUE"="Transparent" "RenderType"="Transparent" }
        Cull Off
        ZWrite Off
        ZTest LEqual
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "RecoveredGameUnit.hlsl"
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _AngleX, _AngleY, _ScaleX, _ScaleY;
            float _Alpha, _IsVisible, _Exposure;
            float4 _Color;
            struct Attributes { float4 position : POSITION; float2 uv : TEXCOORD0; float4 color : COLOR; };
            struct Varyings { float2 uv : TEXCOORD0; float4 position : SV_POSITION; float4 color : COLOR; };
            Varyings vert(Attributes input)
            {
                Varyings output;
                float2 xy = RecoveredGameUnitXY(input.position.xy, _AngleX, _AngleY, _ScaleX, _ScaleY);
                float4 world = mul(unity_ObjectToWorld, float4(xy, 0.0, 1.0));
                output.position = mul(UNITY_MATRIX_VP, world);
                output.uv = input.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                output.color = input.color * _Color;
                return output;
            }
            half4 frag(Varyings input) : SV_Target
            {
                half4 texel = tex2D(_MainTex, input.uv);
                return RecoveredGameUnitColor(texel, input.color, _Exposure, _Alpha, _IsVisible);
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}
