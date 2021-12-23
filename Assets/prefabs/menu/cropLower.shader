Shader "Unlit/CropCustom"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _lowerBound("bound", Float) = 0.5
        _innerDist("inner dist", Float) = 0.5
        _outerDist("outer dist", Float) = 0.5
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
            float _lowerBound;
            float _innerDist;
            float _outerDist;

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
                if (distance((0.5, 0.5), i.uv.xy) < _innerDist) return (0.0, 0.0, 0.0, 0.0);
                if (distance((0.5, 0.5), i.uv.xy) > _outerDist) return (0.0, 0.0, 0.0, 0.0);
                if (i.uv.y < _lowerBound) return (0.0, 0.0, 0.0, 0.0);
                return col*i.color;
            }
            ENDCG
        }
    }
}
