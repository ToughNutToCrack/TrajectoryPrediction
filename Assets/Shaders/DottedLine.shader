Shader "TNTC/DottedLine"{
    Properties{
        _Rep("Repeat Count", float) = 5
        _Spacing("Spacing", float) = 0.5
        _Thickness("Thickness", float) = 0.5
        _Sharpness("Sharpness", Range(0, 100)) = 100
    }
    SubShader{
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float _Rep;
            float _Spacing;
            float _Thickness;
            float _Sharpness;

            struct appdata{
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR0;
            };

            struct v2f{
                float2 uv : TEXCOORD0;              
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR0;
            };

            v2f vert (appdata v){
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.uv.x = o.uv.x * _Rep * ( _Spacing);
  
                o.color = v.color;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target{
                i.uv.x = fmod(i.uv.x, _Spacing);
                float s = length(i.uv - float2(_Spacing, 1.0f) * 0.5);

                fixed4 color = i.color;
                color.a *= saturate((_Thickness - s) * _Sharpness);

                return color;
            }
            ENDCG
        }
    }
}