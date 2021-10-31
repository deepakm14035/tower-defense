Shader "Unlit/Laser"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _threshold("threshold", Float) = 0.5
        _color("color", Color) = (0.0,0.0,0.0,1.0)
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
            float4 _color;

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
                float4 col = _color;
                col.w *= abs(tan(i.uv.y*3.14));
                return col*i.color;
            }
            ENDCG
        }
    }
}
