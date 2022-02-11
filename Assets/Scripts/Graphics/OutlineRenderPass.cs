using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlineRenderPass : ScriptableRenderPass {
    private string profilerTag;
    private Material material;
    private RenderTargetIdentifier cameraColorTargetIdent;
    private RenderTargetHandle tempTexture;

    public OutlineRenderPass(string profilerTag, RenderPassEvent renderPassEvent, Material material) {
        this.profilerTag = profilerTag;
        this.renderPassEvent = renderPassEvent;
        this.material = material;
    }

    public void setup(RenderTargetIdentifier cameraColorTargetIdent) {
        this.cameraColorTargetIdent = cameraColorTargetIdent;
    }

    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor) {
        cmd.GetTemporaryRT(tempTexture.id, cameraTextureDescriptor);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {
        CommandBuffer cmd = CommandBufferPool.Get(profilerTag);
        cmd.Clear();
        
        cmd.Blit(cameraColorTargetIdent, tempTexture.Identifier(), material, 0);
        cmd.Blit(tempTexture.Identifier(), cameraColorTargetIdent);
        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }

    public override void FrameCleanup(CommandBuffer cmd) {
        cmd.ReleaseTemporaryRT(tempTexture.id);
    }
}