Shader "Custom/Outline"
    {
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (1, 1, 1, 1) // color of the outline
        _OutlineIntensity ("Outline Intensity", Range(1, 50)) = 1.0 // degree to which outline glows
        _OutlineSize ("Outline Size", Range(0, 0.005)) = 0.002 // size of the outline
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD0;
            };

            float _OutlineSize;
            float4 _OutlineColor;
            float _OutlineIntensity;
            UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
            UNITY_DECLARE_TEX2D(_CameraOpaqueTexture);

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                COMPUTE_EYEDEPTH(o.screenPos.z);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            // gets the depth value at i.screenpos
            float getDepth(v2f i, float4 offset)
            {
                float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos + offset)));
                return sceneZ - i.screenPos.z;
            }

            // returns max value in a float4
            float getMax(float4 values)
            {
                float maxValue = 0;
                for (int i = 0; i < 4; i++) maxValue = max(maxValue, values[i]);
                return maxValue;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                const float THRESHOLD = 0.05;
                
                // depth sampling
                float4 adjacentDepths = 0;
                float4 localDepth = getDepth(i, 0);
                adjacentDepths[0] = getDepth(i, float4(-_OutlineSize, 0, 0, 0));
                adjacentDepths[1] = getDepth(i, float4(_OutlineSize, 0, 0, 0));
                adjacentDepths[2] = getDepth(i, float4(0, -_OutlineSize, 0, 0));
                adjacentDepths[3] = getDepth(i, float4(0, _OutlineSize, 0, 0));

                // screen color
                float2 screenPosition = i.screenPos.xy / i.screenPos.w;
                fixed4 screenColor = UNITY_SAMPLE_TEX2D(_CameraOpaqueTexture, screenPosition);

                // mixing
                float depth = max(localDepth, getMax(adjacentDepths));
                fixed4 col = (depth > THRESHOLD) ? _OutlineColor * _OutlineIntensity : screenColor;
                col = lerp(screenColor, col, _OutlineColor[3]); // apply opacity of color
                
                // apply fog?
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
