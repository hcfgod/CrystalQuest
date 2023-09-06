#if ENVIRO_HDRP
using System.Collections;
using UnityEngine.Rendering;

namespace UnityEngine.Rendering.HighDefinition
{
    class EnviroHDRPSkyRenderer : SkyRenderer
    {
        Material skyMat; 
        MaterialPropertyBlock m_PropertyBlock = new MaterialPropertyBlock();

        public EnviroHDRPSkyRenderer()
        {
        }

        public override void Build()
        {
            if(skyMat == null)
               skyMat = CoreUtils.CreateEngineMaterial(Shader.Find("Enviro/HDRP/Sky"));
        }
 
        public override void Cleanup()
        {
            CoreUtils.Destroy(skyMat);
        }

        public override void RenderSky(BuiltinSkyParameters builtinParams, bool renderForCubemap, bool renderSunDisk)
        {
            if (Enviro.EnviroManager.instance == null || Enviro.EnviroManager.instance.Sky == null)
                return;

            if (skyMat == null)
                Build();

            Enviro.EnviroManager.instance.Sky.UpdateSkybox(skyMat);

            var enviroSky = builtinParams.skySettings as EnviroHDRPSky;
            Shader.SetGlobalMatrix("_PixelCoordToViewDirWS", builtinParams.pixelCoordToViewDirMatrix);
            Shader.SetGlobalFloat("_EnviroSkyIntensity", GetSkyIntensity(enviroSky, builtinParams.debugSettings));

            m_PropertyBlock.SetMatrix("_PixelCoordToViewDirWS", builtinParams.pixelCoordToViewDirMatrix);
           // m_PropertyBlock.SetFloat("_EnviroSkyIntensity", GetSkyIntensity(enviroSky, builtinParams.debugSettings));
            
            if (builtinParams.hdCamera.camera.cameraType != CameraType.Reflection)
                Enviro.EnviroManager.instance.Sky.mySkyboxMat = skyMat; 

            CoreUtils.DrawFullScreen(builtinParams.commandBuffer, skyMat, m_PropertyBlock, renderForCubemap ? 0 : 1);
            }
        }
    }
#endif
