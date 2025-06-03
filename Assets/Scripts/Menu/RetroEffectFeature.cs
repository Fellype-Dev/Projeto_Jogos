using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RetroEffectFeature : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        Material material;

        public CustomRenderPass(Material mat)
        {
            this.material = mat;
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (material == null) return;

            CommandBuffer cmd = CommandBufferPool.Get("RetroEffectPass");
            RenderTargetIdentifier source = renderingData.cameraData.renderer.cameraColorTarget;
            cmd.Blit(source, source, material);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    public Material retroMaterial;
    CustomRenderPass pass;

    public override void Create()
    {
        pass = new CustomRenderPass(retroMaterial);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (retroMaterial != null)
            renderer.EnqueuePass(pass);
    }
}
