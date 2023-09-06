using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace KWS
{
    public class ShorelineWavesPass : WaterPass
    {
        ShorelineWavesPassCore _pass;
        private RenderTargetIdentifier[] _mrt = new RenderTargetIdentifier[2];
        public ShorelineWavesPass(RenderPassEvent renderPassEvent, WaterSystem waterInstance)
        {
            WaterInstance = waterInstance;
            this.renderPassEvent = renderPassEvent;
        }
        void Initialize()
        {
            IsInitialized = true;
            _pass                   =  new ShorelineWavesPassCore(WaterInstance);
            _pass.OnSetRenderTarget += OnSetRenderTarget;
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

        private void OnSetRenderTarget(CommandBuffer cmd, Camera cam, RenderTexture rt1, RenderTexture rt2)
        {
            _mrt[0] = rt1;
            _mrt[1] = rt2;
            ConfigureTarget(_mrt, rt1.depthBuffer);
            KWS_SPR_CoreUtils.SetRenderTarget(cmd, _mrt, rt1.depthBuffer, ClearFlag.Color, Color.black);
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