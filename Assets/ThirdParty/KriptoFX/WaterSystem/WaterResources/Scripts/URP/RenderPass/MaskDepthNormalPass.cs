using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace KWS
{
    public class MaskDepthNormalPass : WaterPass
    {
        MaskDepthNormalPassCore _pass;
        private RenderTargetIdentifier[] _mrt = new RenderTargetIdentifier[2];
        public MaskDepthNormalPass(RenderPassEvent renderPassEvent, WaterSystem waterInstance)
        {
            WaterInstance = waterInstance;
            this.renderPassEvent = renderPassEvent;
        }

        public void Initialize()
        {
            IsInitialized = true;

            _pass = new MaskDepthNormalPassCore(WaterInstance);
            _pass.OnInitializedRenderTarget += OnInitializedRenderTarget;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!IsInitialized) Initialize();
            var cam = renderingData.cameraData.camera; 
            var cmd = CommandBufferPool.Get(_pass.PassName);
            cmd.Clear();
            _pass.Execute(cam, cmd);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
        private void OnInitializedRenderTarget(CommandBuffer cmd, KWS_RTHandle rt1, KWS_RTHandle rt2, KWS_RTHandle rt3)
        {
            _mrt[0] = rt1;
            _mrt[1] = rt2;
            ConfigureTarget(_mrt, rt3.rt);
            //ConfigureClear(UnityEngine.Rendering.ClearFlag.All, Color.black); 
            KWS_SPR_CoreUtils.SetRenderTarget(cmd, rt1, rt2, rt3, ClearFlag.All, Color.black); //by some reason, configure target cause flickering in the editor view
        }

        public override void Release()
        {
            if (_pass != null)
            {
                _pass.OnInitializedRenderTarget -= OnInitializedRenderTarget;
                _pass.Release();
            }
            IsInitialized = false;
        }
    }
}