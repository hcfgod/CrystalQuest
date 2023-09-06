using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace KWS
{
    public class DrawToPosteffectsDepthPass : WaterPass
    {
        DrawToPosteffectsDepthPassCore _pass;
        private RenderTargetIdentifier _depthRT;
        readonly RenderTargetIdentifier _cameraDepthTextureRT = new RenderTargetIdentifier(Shader.PropertyToID("_CameraDepthTexture"));

        public DrawToPosteffectsDepthPass(RenderPassEvent renderPassEvent, WaterSystem waterInstance)
        {
            WaterInstance = waterInstance;
            this.renderPassEvent = renderPassEvent;
        }

        void Initialize()
        {
            IsInitialized = true;
            _pass                   =  new DrawToPosteffectsDepthPassCore(WaterInstance);
            _pass.OnSetRenderTarget += OnSetRenderTarget;
        }
    
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!IsInitialized) Initialize();
            var cam = renderingData.cameraData.camera;
            var cmd = CommandBufferPool.Get(_pass.PassName);
            cmd.Clear();

#if UNITY_2022_1_OR_NEWER
            _depthRT = renderingData.cameraData.renderer.cameraDepthTargetHandle.rt;
#else
            _depthRT = renderingData.cameraData.renderer.cameraDepthTarget;
#endif

            _pass.Execute(cam, cmd, _cameraDepthTextureRT);

            context.ExecuteCommandBuffer(cmd); 
            CommandBufferPool.Release(cmd);
        }

        private void OnSetRenderTarget(CommandBuffer cmd, Camera cam)
        {
            ConfigureTarget(_cameraDepthTextureRT);
            KWS_SPR_CoreUtils.SetRenderTarget(cmd, _cameraDepthTextureRT);
        }

        public override void Release()
        {
            if (_pass != null)
            {
                _pass.OnSetRenderTarget -= OnSetRenderTarget;
                _pass.Release();
            }
          
            IsInitialized = false;
        }
    }
}
