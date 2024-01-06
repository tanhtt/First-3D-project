// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge Beta 0.17 
// Shader Forge (c) Joachim 'Acegikmo' Holmer
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.17;sub:START;pass:START;ps:lgpr:1,nrmq:1,limd:1,blpr:2,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:False,uamb:True,mssp:True,ufog:False,aust:True,igpj:True,qofs:0,lico:1,qpre:3,flbk:,rntp:2,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300;n:type:ShaderForge.SFN_Final,id:1,x:32528,y:32634|emission-15-OUT,alpha-23-OUT;n:type:ShaderForge.SFN_Color,id:2,x:33360,y:32505,ptlb:Main Color,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Tex2d,id:3,x:33360,y:32671,ptlb:Main Texture(R chanel),tex:8eb53b7e173f540459b34d09dc53824d,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:4,x:33140,y:32565|A-2-RGB,B-3-R;n:type:ShaderForge.SFN_Tex2d,id:5,x:33360,y:32875,ptlb:Distortion Texture(R chanel),tex:b5ee2b35697dfc943878a9e7d519e5d3,ntxv:0,isnm:False|UVIN-6-UVOUT;n:type:ShaderForge.SFN_Panner,id:6,x:33553,y:32875,spu:1,spv:0|UVIN-7-UVOUT,DIST-10-OUT;n:type:ShaderForge.SFN_TexCoord,id:7,x:33788,y:32761,uv:0;n:type:ShaderForge.SFN_Time,id:8,x:33934,y:32911;n:type:ShaderForge.SFN_ValueProperty,id:9,x:33934,y:33059,ptlb:Distortion Speed,v1:1;n:type:ShaderForge.SFN_Multiply,id:10,x:33738,y:32965|A-8-T,B-9-OUT;n:type:ShaderForge.SFN_Multiply,id:11,x:33194,y:32921|A-5-R,B-12-OUT;n:type:ShaderForge.SFN_ValueProperty,id:12,x:33360,y:33076,ptlb:Distortion Power,v1:1;n:type:ShaderForge.SFN_Multiply,id:13,x:33105,y:32754|A-3-R,B-11-OUT;n:type:ShaderForge.SFN_Multiply,id:15,x:32947,y:32658|A-4-OUT,B-13-OUT;n:type:ShaderForge.SFN_ComponentMask,id:21,x:32910,y:32853,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-15-OUT;n:type:ShaderForge.SFN_VertexColor,id:22,x:32915,y:33009;n:type:ShaderForge.SFN_Multiply,id:23,x:32756,y:32911|A-21-OUT,B-22-A;proporder:2-3-5-9-12;pass:END;sub:END;*/

Shader "Langvv/Eletricity_Additive_Gray_Scroll_U" {
    Properties {
        _MainColor ("Main Color", Color) = (0.5,0.5,0.5,1)
        _MainTextureRchanel ("Main Texture(R chanel)", 2D) = "white" {}
        _DistortionTextureRchanel ("Distortion Texture(R chanel)", 2D) = "white" {}
        _DistortionSpeed ("Distortion Speed", Float ) = 1
        _DistortionPower ("Distortion Power", Float ) = 1
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float4 _MainColor;
            uniform sampler2D _MainTextureRchanel; uniform float4 _MainTextureRchanel_ST;
            uniform sampler2D _DistortionTextureRchanel; uniform float4 _DistortionTextureRchanel_ST;
            uniform float _DistortionSpeed;
            uniform float _DistortionPower;
            struct VertexInput {
                float4 vertex : POSITION;
                float4 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_3 = tex2D(_MainTextureRchanel,TRANSFORM_TEX(i.uv0.rg, _MainTextureRchanel));
                float4 node_8 = _Time + _TimeEditor;
                float3 node_15 = ((_MainColor.rgb*node_3.r)*(node_3.r*(tex2D(_DistortionTextureRchanel,TRANSFORM_TEX((i.uv0.rg+(node_8.g*_DistortionSpeed)*float2(1,0)), _DistortionTextureRchanel)).r*_DistortionPower)));
                float3 emissive = node_15;
                float3 finalColor = emissive;
/// Final Color:
                return fixed4(finalColor,(node_15.r*i.vertexColor.a));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
