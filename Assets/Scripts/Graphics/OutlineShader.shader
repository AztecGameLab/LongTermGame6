Shader "Custom/Outline"
    {
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (1, 1, 1, 1) // color of the outline
        _OutlineIntensity ("Outline Intensity", Range(1, 50)) = 1.0 // degree to which outline glows
        _OutlineSize ("Outline Size", Range(0, 0.005)) = 0.002 // size of the outline
        _DepthThreshold ("Depth Threshold", Range(0, 0.10)) = 0.05
        _NormalThreshold ("Normal Threshold", Range(0, 1)) = 1.0
        
//        [Toggle] _DoDepthEdgeDetection ("Depth Edge Detection", Float) = 1.0
//        [Toggle] _DoNormalEdgeDetection ("Normal Edge Detection", Float) = 1.0
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11, OpenGL ES 2.0 because it uses unsized arrays
#pragma exclude_renderers d3d11 gles
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
            float _NormalThreshold;
            float _DepthThreshold;
            // float _DoDepthEdgeDetection;
            // float _DoNormalEdgeDetection;
            UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
            UNITY_DECLARE_TEX2D(_CameraNormalsTexture);
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



            
            
            // detect if at edge using the camera normals
            bool detectEdgeFromNormal(float2 offsets[4], float2 screenPosition)
            {
                float3 normals[4];
                for (int j = 0; j < 4; j++) normals[j] = UNITY_SAMPLE_TEX2D(_CameraNormalsTexture, screenPosition + offsets[j]);
                const float3 normalFiniteDifference0 = normals[1] - normals[0];
                const float3 normalFiniteDifference1 = normals[3] - normals[2];
                float edgeNormal = sqrt(dot(normalFiniteDifference0, normalFiniteDifference0) + dot(normalFiniteDifference1, normalFiniteDifference1));
                return edgeNormal > _NormalThreshold;
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
            
            // detect if at edge using the depth buffer
            bool detectEdgeFromDepth(v2f i, float2 offsets[4])
            {
                // const float THRESHOLD = 0.05;
                
                // depth sampling
                float4 adjacentDepths = 0;
                float4 localDepth = getDepth(i, 0);
                for (int j = 0; j < 4; j++) adjacentDepths[j] = getDepth(i, float4(offsets[j], 0, 0));

                float depth = max(localDepth, getMax(adjacentDepths));
                return depth > _DepthThreshold;
            }




            

            fixed4 frag (v2f i) : SV_Target
            {
                float2 offsets[] = {
                    float2(-_OutlineSize, 0),
                    float2(_OutlineSize, 0),
                    float2(0, -_OutlineSize),
                    float2(0, _OutlineSize),
                };
                
                // screen color
                const float2 screenPosition = i.screenPos.xy / i.screenPos.w;
                const fixed4 screenColor = UNITY_SAMPLE_TEX2D(_CameraOpaqueTexture, screenPosition);

                // edge detection
                const bool atDepthEdge = detectEdgeFromDepth(i, offsets);
                const bool atNormalEdge = detectEdgeFromNormal(offsets, screenPosition);
                bool atEdge = atDepthEdge && atNormalEdge;
                
                // mixing
                fixed4 col = (atEdge) ? _OutlineColor * _OutlineIntensity : screenColor;
                col = lerp(screenColor, col, _OutlineColor[3]); // apply opacity of color

                // apply fog?
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
