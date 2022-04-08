using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlineObjectsFeature : RenderObjects {
    private RenderObjectsPass rpoPass;

    // public OutlineObjectsFeatureSettings newSettings = new OutlineObjectsFeatureSettings();
    public RenderPassEvent insertTiming;
    public LayerMask layerMask;
    public Material mainMaterial;


    [System.Serializable]
    public class OutlineObjectsFeatureSettings : RenderObjectsSettings
    {
        public RenderPassEvent insertTiming = RenderPassEvent.AfterRenderingTransparents;
        // public Material material;
        public int layerMask = 1 << 7;
    }

    public override void Create()
    {
        rpoPass = new RenderObjectsPass("Outline Objects Pass", insertTiming, new string[0], RenderQueueType.Opaque, layerMask, new CustomCameraSettings());
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        rpoPass.overrideMaterial = mainMaterial;
        rpoPass.ConfigureInput(ScriptableRenderPassInput.Normal);
        renderer.EnqueuePass(rpoPass);
    }
}