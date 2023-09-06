using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace KWS
{
    public class UnderwaterPass: WaterPass
    {
        UnderwaterPassCore _pass;
        private RenderTargetIdentifier _cameraColor;

        public UnderwaterPass(RenderPassEvent renderPassEvent, WaterSystem waterInstance)
        {
            WaterInstance = waterInstance;
            this.renderPassEvent = renderPassEvent;
        }
        void Initialize()
        {
            IsInitialized = true;
            _pass                   =  new UnderwaterPassCore(WaterInstance);
            _pass.OnSetRenderTarget += OnSetRenderTarget;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!IsInitialized) Initialize();
            var cam = renderingData.cameraData.camera;
            var cmd = CommandBufferPool.Get(_pass.PassName);
            cmd.Clear();

#if UNITY_2022_1_OR_NEWER
            _cameraColor = renderingData.cameraData.renderer.cameraColorTargetHandle.rt;
#else
            _cameraColor = renderingData.cameraData.renderer.cameraColorTarget;
#endif
            _pass.Execute(cam, cmd, _cameraColor);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
      
        private void OnSetRenderTarget(CommandBuffer cmd, RenderTargetIdentifier rt)
        {
            ConfigureTarget(rt);
            KWS_SPR_CoreUtils.SetRenderTarget(cmd, rt);
        }

        public override void Release()
        {
            if (_pass != null)
            {
                _pass.OnSetRenderTarget            -= OnSetRenderTarget;
                _pass.Release();
            }

          
            IsInitialized = false;
        }
    }
}