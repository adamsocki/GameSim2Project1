Shader "Unlit/SceneFader"
{
   Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            
            //sampler2D _MainTex;
            //float4 _MainTex_ST;
            float4 _Color;
            
            struct MeshData
            {
                float4 vertex : POSITION;
                float3 normals : NORMAL;
                
                float2 uv0 : TEXCOORD0;
            };

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };


            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex) ;
                return o;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
                return _Color;
            }
            ENDCG
            }
        }
    
        
}
