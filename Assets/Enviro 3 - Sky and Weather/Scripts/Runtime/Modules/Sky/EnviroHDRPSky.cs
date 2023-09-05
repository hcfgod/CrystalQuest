#if ENVIRO_HDRP
using System;
using UnityEngine.Rendering.HighDefinition;

namespace UnityEngine.Rendering.HighDefinition
{
    [VolumeComponentMenu("Sky/Enviro 3 Skybox")]
    [SkyUniqueID(990)] 
    public class EnviroHDRPSky : SkySettings
    {

        public override int GetHashCode()
        {
            int hash = base.GetHashCode();

            unchecked
            {
               // hash = hash * 23 + GetHashCode();
            }

            return hash;
        }
 
        public override Type GetSkyRendererType() { return typeof(EnviroHDRPSkyRenderer); }
    }
}
#endif
