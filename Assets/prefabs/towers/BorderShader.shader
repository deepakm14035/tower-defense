Shader "Unlit/Borders"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _borderColor("border color", Color) = (0.0,0.0,0.0,1.0)
        _bt("border thickness", Float) = 1.0
    }
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
        Tags { "Queue" = "Transparent" "RenderType"="Transparent"  }

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
            float4 _borderColor;
            float _bt;

            v2f vert (appdata_full v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : COLOR
            {
                // sample the texture
                float4 col = tex2D(_MainTex, i.uv);
                col.rgb *= col.a;
                col *= i.color;
                //col.a = col.r < 0.1 && col.g < 0.1 && col.b < 0.1 ? 0.0 : 1.0;
                bool verOut = i.uv.y + (_bt * _MainTex_TexelSize.y) >= 1 || (i.uv.y - _bt * _MainTex_TexelSize.y) <= 0;
                bool horOut = i.uv.x + (_bt * _MainTex_TexelSize.x) >= 1 || (i.uv.x - _bt * _MainTex_TexelSize.x) <= 0;
                //bool upOut = _bt * _MainTex_TexelSize.y > _MainTex_TexelSize.w;
                //bool upOut = _bt * _MainTex_TexelSize.y > _MainTex_TexelSize.w;
                float colUp = verOut?0.0:tex2D(_MainTex, i.uv + float2(0, _bt* _MainTex_TexelSize.y)).a;
                float colUpRight = verOut ? 0.0 : tex2D(_MainTex, i.uv + float2(_bt * _MainTex_TexelSize.xy/1.4142)).a;
                float colDown = verOut ? 0.0 : tex2D(_MainTex, i.uv + float2(0, -_bt * _MainTex_TexelSize.y)).a;
                float colDownLeft = verOut ? 0.0 : tex2D(_MainTex, i.uv + float2(-_bt * _MainTex_TexelSize.xy / 1.4142)).a;
                float colLeft = horOut ? 0.0 : tex2D(_MainTex, i.uv + float2(-_bt * _MainTex_TexelSize.x,0)).a;
                float colRight = horOut ? 0.0 : tex2D(_MainTex, i.uv + float2(_bt * _MainTex_TexelSize.x,0)).a;
                float colUpLeft = verOut ? 0.0 : tex2D(_MainTex, i.uv + float2(-_bt * _MainTex_TexelSize.x / 1.4142, _bt * _MainTex_TexelSize.y / 1.4142)).a;
                float colDownRight = verOut ? 0.0 : tex2D(_MainTex, i.uv + float2(_bt * _MainTex_TexelSize.x / 1.4142,-_bt * _MainTex_TexelSize.y / 1.4142)).a;
                //col = (colUp < 0.1 || colDown < 0.1 || colRight < 0.1 || colLeft < 0.1) ? _borderColor : col;
                col.rgb = colUp* colUpRight* colDown* colDownLeft* colLeft* colRight* colUpLeft* colDownRight*col.rgb;
                //col.w = colRight;
                return col;
            }
            ENDCG
        }
    }
}
