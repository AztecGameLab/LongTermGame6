using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlineFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class OutlineFeatureSettings
    {
        public bool isEnabled = true;
        public RenderPassEvent insertTiming = RenderPassEvent.AfterRendering;
        public Material material;
    }

    public OutlineFeatureSettings settings = new OutlineFeatureSettings();
    private RenderTargetHandle renderTargetHandle;
    private OutlineRenderPass renderPass;
    
    public override void Create()
    {
        renderPass = new OutlineRenderPass("Outline Pass", settings.insertTiming, settings.material);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (!settings.isEnabled) return;
        
        RenderTargetIdentifier cameraColorTargetIdent = renderer.cameraColorTarget;
        renderPass.setup(cameraColorTargetIdent);
        renderPass.ConfigureInput(ScriptableRenderPassInput.Normal);
        renderer.EnqueuePass(renderPass);
    }
}