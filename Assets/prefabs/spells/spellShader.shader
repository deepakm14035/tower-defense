Shader "Unlit/Spell"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _rippleCount("ripple count", Float) = 2.0
        _blinkSpeed("blink speed", Float) = 2.0
        _threshold("threshold", Float) = 0.5
    }
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent"  }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"


            struct v2f
            {
                float4 pos : SV_POSITION;
                half2 uv : TEXCOORD0;
                float4 color : vertexColor;
        };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float _rippleCount;
            float _blinkSpeed;
            float _threshold;

            v2f vert(appdata_full v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : COLOR
            {
                // sample the texture
                float4 col = tex2D(_MainTex, i.uv);
                float newVal = abs(sin(length(i.uv.xy - 0.5)) * 3);
                //col.xyz*= abs(sin(length(i.uv.xy - 0.5))*3);
                //col.x = col.x < _threshold ? 0.0 : col.x;
                //col.y = col.y < _threshold ? 0.0 : col.y;
                //col.z = col.z < _threshold ? 0.0 : col.z;
                col.w *= newVal > _threshold? 1: 0.2+(abs(sin(_Time.x* _blinkSpeed)))/2.5;
                return col*i.color;
            }
            ENDCG
        }
    }
}
