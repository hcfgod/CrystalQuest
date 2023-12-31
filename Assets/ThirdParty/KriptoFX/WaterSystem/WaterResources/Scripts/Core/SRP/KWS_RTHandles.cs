using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

namespace KWS
{
    /// <summary>
    /// Default instance of a RTHandleSystem
    /// </summary>
    public static class KWS_RTHandles
    {
        static KWS_RTHandleSystem s_DefaultInstance = new KWS_RTHandleSystem();

        /// <summary>
        /// Maximum allocated width of the default RTHandle System
        /// </summary>
        public static int maxWidth { get { return s_DefaultInstance.GetMaxWidth(); } }
        /// <summary>
        /// Maximum allocated height of the default RTHandle System
        /// </summary>
        public static int maxHeight { get { return s_DefaultInstance.GetMaxHeight(); } }
        /// <summary>
        /// Current properties of the default RTHandle System
        /// </summary>
        public static RTHandleProperties rtHandleProperties { get { return s_DefaultInstance.rtHandleProperties; } }

        /// <summary>
        /// Allocate a new fixed sized RTHandle with the default RTHandle System.
        /// </summary>
        /// <param name="width">With of the RTHandle.</param>
        /// <param name="height">Heigh of the RTHandle.</param>
        /// <param name="slices">Number of slices of the RTHandle.</param>
        /// <param name="depthBufferBits">Bit depths of a depth buffer.</param>
        /// <param name="colorFormat">GraphicsFormat of a color buffer.</param>
        /// <param name="filterMode">Filtering mode of the RTHandle.</param>
        /// <param name="wrapMode">Addressing mode of the RTHandle.</param>
        /// <param name="dimension">Texture dimension of the RTHandle.</param>
        /// <param name="enableRandomWrite">Set to true to enable UAV random read writes on the texture.</param>
        /// <param name="useMipMap">Set to true if the texture should have mipmaps.</param>
        /// <param name="autoGenerateMips">Set to true to automatically generate mipmaps.</param>
        /// <param name="isShadowMap">Set to true if the depth buffer should be used as a shadow map.</param>
        /// <param name="anisoLevel">Anisotropic filtering level.</param>
        /// <param name="mipMapBias">Bias applied to mipmaps during filtering.</param>
        /// <param name="msaaSamples">Number of MSAA samples for the RTHandle.</param>
        /// <param name="bindTextureMS">Set to true if the texture needs to be bound as a multisampled texture in the shader.</param>
        /// <param name="useDynamicScale">Set to true to use hardware dynamic scaling.</param>
        /// <param name="memoryless">Use this property to set the render texture memoryless modes.</param>
        /// <param name="name">Name of the RTHandle.</param>
        /// <returns></returns>
        public static KWS_RTHandle Alloc(
            int width,
            int height,
            int slices = 1,
            DepthBits depthBufferBits = DepthBits.None,
            bool useDepthOnlyFormat = false,
            GraphicsFormat colorFormat = GraphicsFormat.R8G8B8A8_SRGB,
            FilterMode filterMode = FilterMode.Point,
            TextureWrapMode wrapMode = TextureWrapMode.Repeat,
            TextureDimension dimension = TextureDimension.Tex2D,
            bool enableRandomWrite = false,
            bool useMipMap = false,
            bool autoGenerateMips = true,
            int mipMapCount = 0,
            bool isShadowMap = false,
            int anisoLevel = 1,
            float mipMapBias = 0,
            MSAASamples msaaSamples = MSAASamples.None,
            bool bindTextureMS = false,
            bool useDynamicScale = false,
            RenderTextureMemoryless memoryless = RenderTextureMemoryless.None,
            VRTextureUsage vrUsage = VRTextureUsage.None,
            string name = ""
        )
        {
            return s_DefaultInstance.Alloc(
                width,
                height,
                slices,
                depthBufferBits,
                useDepthOnlyFormat,
                colorFormat,
                filterMode,
                wrapMode,
                dimension,
                enableRandomWrite,
                useMipMap,
                autoGenerateMips,
                mipMapCount,
                isShadowMap,
                anisoLevel,
                mipMapBias,
                msaaSamples,
                bindTextureMS,
                useDynamicScale,
                memoryless,
                vrUsage,
                name
            );
        }

        public static KWS_RTHandle AllocVR(
            int                     width,
            int                     height,
            DepthBits               depthBufferBits    = DepthBits.None,
            bool                    useDepthOnlyFormat = false,
            GraphicsFormat          colorFormat        = GraphicsFormat.R8G8B8A8_SRGB,
            FilterMode              filterMode         = FilterMode.Point,
            TextureWrapMode         wrapMode           = TextureWrapMode.Repeat,
            bool                    enableRandomWrite  = false,
            bool                    useMipMap          = false,
            bool                    autoGenerateMips   = true,
            int                     mipMapCount        = 0,
            bool                    isShadowMap        = false,
            int                     anisoLevel         = 1,
            float                   mipMapBias         = 0,
            MSAASamples             msaaSamples        = MSAASamples.None,
            bool                    bindTextureMS      = false,
            bool                    useDynamicScale    = false,
            RenderTextureMemoryless memoryless         = RenderTextureMemoryless.None,
            string                  name               = ""
        )
        {
            var vrUsage   = WaterSystem.IsSinglePassStereoEnabled ? UnityEngine.XR.XRSettings.eyeTextureDesc.vrUsage : VRTextureUsage.None;
            var dimension = WaterSystem.IsSinglePassStereoEnabled ? TextureDimension.Tex2DArray : TextureDimension.Tex2D;
            var slices    = WaterSystem.IsSinglePassStereoEnabled ? 2 : 1;
            return s_DefaultInstance.Alloc(
                width,
                height,
                slices,
                depthBufferBits,
                useDepthOnlyFormat,
                colorFormat,
                filterMode,
                wrapMode,
                dimension,
                enableRandomWrite,
                useMipMap,
                autoGenerateMips,
                mipMapCount,
                isShadowMap,
                anisoLevel,
                mipMapBias,
                msaaSamples,
                bindTextureMS,
                useDynamicScale,
                memoryless,
                vrUsage,
                name
            );
        }


        public static KWS_RTHandle AllocVR(
            Vector2                 scaleFactor,
            DepthBits               depthBufferBits    = DepthBits.None,
            bool                    useDepthOnlyFormat = false,
            GraphicsFormat          colorFormat        = GraphicsFormat.R8G8B8A8_SRGB,
            FilterMode              filterMode         = FilterMode.Point,
            TextureWrapMode         wrapMode           = TextureWrapMode.Repeat,
            bool                    enableRandomWrite  = false,
            bool                    useMipMap          = false,
            bool                    autoGenerateMips   = true,
            int                     mipMapCount        = 0,
            bool                    isShadowMap        = false,
            int                     anisoLevel         = 1,
            float                   mipMapBias         = 0,
            MSAASamples             msaaSamples        = MSAASamples.None,
            bool                    bindTextureMS      = false,
            bool                    useDynamicScale    = false,
            RenderTextureMemoryless memoryless         = RenderTextureMemoryless.None,
            string                  name               = ""
        )
        {
            var vrUsage   = WaterSystem.IsSinglePassStereoEnabled ? UnityEngine.XR.XRSettings.eyeTextureDesc.vrUsage : VRTextureUsage.None;
            var dimension = WaterSystem.IsSinglePassStereoEnabled ? TextureDimension.Tex2DArray : TextureDimension.Tex2D;
            var slices    = WaterSystem.IsSinglePassStereoEnabled ? 2 : 1;
            return s_DefaultInstance.Alloc(
                scaleFactor,
                slices,
                depthBufferBits,
                useDepthOnlyFormat,
                colorFormat,
                filterMode,
                wrapMode,
                dimension,
                enableRandomWrite,
                useMipMap,
                autoGenerateMips,
                mipMapCount,
                isShadowMap,
                anisoLevel,
                mipMapBias,
                msaaSamples,
                bindTextureMS,
                useDynamicScale,
                memoryless,
                vrUsage,
                name
            );
        }

        public static KWS_RTHandle AllocVR(
            ScaleFunc               scaleFunc,
            DepthBits               depthBufferBits    = DepthBits.None,
            bool                    useDepthOnlyFormat = false,
            GraphicsFormat          colorFormat        = GraphicsFormat.R8G8B8A8_SRGB,
            FilterMode              filterMode         = FilterMode.Point,
            TextureWrapMode         wrapMode           = TextureWrapMode.Repeat,
            bool                    enableRandomWrite  = false,
            bool                    useMipMap          = false,
            bool                    autoGenerateMips   = true,
            bool                    isShadowMap        = false,
            int                     anisoLevel         = 1,
            float                   mipMapBias         = 0,
            int                     mipMapCount        = 0,
            MSAASamples             msaaSamples        = MSAASamples.None,
            bool                    bindTextureMS      = false,
            bool                    useDynamicScale    = false,
            RenderTextureMemoryless memoryless         = RenderTextureMemoryless.None,
            string                  name               = ""
        )
        {
            var vrUsage   = WaterSystem.IsSinglePassStereoEnabled ? UnityEngine.XR.XRSettings.eyeTextureDesc.vrUsage : VRTextureUsage.None;
            var dimension = WaterSystem.IsSinglePassStereoEnabled ? TextureDimension.Tex2DArray : TextureDimension.Tex2D;
            var slices    = WaterSystem.IsSinglePassStereoEnabled ? 2 : 1;
            return s_DefaultInstance.Alloc(
                scaleFunc,
                slices,
                depthBufferBits,
                useDepthOnlyFormat,
                colorFormat,
                filterMode,
                wrapMode,
                dimension,
                enableRandomWrite,
                useMipMap,
                autoGenerateMips,
                mipMapCount,
                isShadowMap,
                anisoLevel,
                mipMapBias,
                msaaSamples,
                bindTextureMS,
                useDynamicScale,
                memoryless,
                vrUsage,
                name
            );
        }

        public static KWS_RTHandle Alloc(
            Vector2 scaleFactor,
            int slices = 1,
            DepthBits depthBufferBits = DepthBits.None,
            bool useDepthOnlyFormat = false,
            GraphicsFormat colorFormat = GraphicsFormat.R8G8B8A8_SRGB,
            FilterMode filterMode = FilterMode.Point,
            TextureWrapMode wrapMode = TextureWrapMode.Repeat,
            TextureDimension dimension = TextureDimension.Tex2D,
            bool enableRandomWrite = false,
            bool useMipMap = false,
            bool autoGenerateMips = true,
            int mipMapCount = 0,
            bool isShadowMap = false,
            int anisoLevel = 1,
            float mipMapBias = 0,
            MSAASamples msaaSamples = MSAASamples.None,
            bool bindTextureMS = false,
            bool useDynamicScale = false,
            RenderTextureMemoryless memoryless = RenderTextureMemoryless.None,
            VRTextureUsage vrUsage = VRTextureUsage.None,
            string name = ""
        )
        {
            return s_DefaultInstance.Alloc(
                scaleFactor,
                slices,
                depthBufferBits,
                useDepthOnlyFormat,
                colorFormat,
                filterMode,
                wrapMode,
                dimension,
                enableRandomWrite,
                useMipMap,
                autoGenerateMips,
                mipMapCount,
                isShadowMap,
                anisoLevel,
                mipMapBias,
                msaaSamples,
                bindTextureMS,
                useDynamicScale,
                memoryless,
                vrUsage,
                name
            );
        }

        /// <summary>
        /// Allocate a new automatically sized RTHandle for the default RTHandle System.
        /// </summary>
        /// <param name="scaleFunc">Function used for the RTHandle size computation.</param>
        /// <param name="slices">Number of slices of the RTHandle.</param>
        /// <param name="depthBufferBits">Bit depths of a depth buffer.</param>
        /// <param name="colorFormat">GraphicsFormat of a color buffer.</param>
        /// <param name="filterMode">Filtering mode of the RTHandle.</param>
        /// <param name="wrapMode">Addressing mode of the RTHandle.</param>
        /// <param name="dimension">Texture dimension of the RTHandle.</param>
        /// <param name="enableRandomWrite">Set to true to enable UAV random read writes on the texture.</param>
        /// <param name="useMipMap">Set to true if the texture should have mipmaps.</param>
        /// <param name="autoGenerateMips">Set to true to automatically generate mipmaps.</param>
        /// <param name="isShadowMap">Set to true if the depth buffer should be used as a shadow map.</param>
        /// <param name="anisoLevel">Anisotropic filtering level.</param>
        /// <param name="mipMapBias">Bias applied to mipmaps during filtering.</param>
        /// <param name="msaaSamples">Number of MSAA samples.</param>
        /// <param name="bindTextureMS">Set to true if the texture needs to be bound as a multisampled texture in the shader.</param>
        /// <param name="useDynamicScale">Set to true to use hardware dynamic scaling.</param>
        /// <param name="memoryless">Use this property to set the render texture memoryless modes.</param>
        /// <param name="name">Name of the RTHandle.</param>
        /// <returns></returns>
        public static KWS_RTHandle Alloc(
            ScaleFunc scaleFunc,
            int slices = 1,
            DepthBits depthBufferBits = DepthBits.None,
            bool useDepthOnlyFormat = false,
            GraphicsFormat colorFormat = GraphicsFormat.R8G8B8A8_SRGB,
            FilterMode filterMode = FilterMode.Point,
            TextureWrapMode wrapMode = TextureWrapMode.Repeat,
            TextureDimension dimension = TextureDimension.Tex2D,
            bool enableRandomWrite = false,
            bool useMipMap = false,
            bool autoGenerateMips = true,
            bool isShadowMap = false,
            int anisoLevel = 1,
            float mipMapBias = 0,
            int mipMapCount = 0,
            MSAASamples msaaSamples = MSAASamples.None,
            bool bindTextureMS = false,
            bool useDynamicScale = false,
            RenderTextureMemoryless memoryless = RenderTextureMemoryless.None,
            VRTextureUsage vrUsage = VRTextureUsage.None,
            string name = ""
        )
        {
            return s_DefaultInstance.Alloc(
                scaleFunc,
                slices,
                depthBufferBits,
                useDepthOnlyFormat,
                colorFormat,
                filterMode,
                wrapMode,
                dimension,
                enableRandomWrite,
                useMipMap,
                autoGenerateMips,
                mipMapCount,
                isShadowMap,
                anisoLevel,
                mipMapBias,
                msaaSamples,
                bindTextureMS,
                useDynamicScale,
                memoryless,
                vrUsage,
                name
            );
        }

        /// <summary>
        /// Allocate a RTHandle from a regular Texture for the default RTHandle system.
        /// </summary>
        /// <param name="tex">Input texture</param>
        /// <returns>A new RTHandle referencing the input texture.</returns>
        public static KWS_RTHandle Alloc(Texture tex)
        {
            return s_DefaultInstance.Alloc(tex);
        }

        /// <summary>
        /// Allocate a RTHandle from a regular RenderTexture for the default RTHandle system.
        /// </summary>
        /// <param name="tex">Input texture</param>
        /// <returns>A new RTHandle referencing the input texture.</returns>
        public static KWS_RTHandle Alloc(RenderTexture tex)
        {
            return s_DefaultInstance.Alloc(tex);
        }

        /// <summary>
        /// Allocate a RTHandle from a regular render target identifier for the default RTHandle system.
        /// </summary>
        /// <param name="tex">Input render target identifier.</param>
        /// <returns>A new RTHandle referencing the input render target identifier.</returns>
        public static KWS_RTHandle Alloc(RenderTargetIdentifier tex)
        {
            return s_DefaultInstance.Alloc(tex);
        }

        /// <summary>
        /// Allocate a RTHandle from a regular render target identifier for the default RTHandle system.
        /// </summary>
        /// <param name="tex">Input render target identifier.</param>
        /// <param name="name">Name of the render target.</param>
        /// <returns>A new RTHandle referencing the input render target identifier.</returns>
        public static KWS_RTHandle Alloc(RenderTargetIdentifier tex, string name)
        {
            return s_DefaultInstance.Alloc(tex, name);
        }

        private static KWS_RTHandle Alloc(KWS_RTHandle tex)
        {
            Debug.LogError("Allocation a RTHandle from another one is forbidden.");
            return null;
        }

        /// <summary>
        /// Initialize the default RTHandle system.
        /// </summary>
        /// <param name="width">Initial reference rendering width.</param>
        /// <param name="height">Initial reference rendering height.</param>
        public static void Initialize(int width, int height, bool scaledRTsupportsMSAA, MSAASamples scaledRTMSAASamples)
        {
            s_DefaultInstance.Initialize(width, height);
        }

        /// <summary>
        /// Release memory of a RTHandle from the default RTHandle System
        /// </summary>
        /// <param name="rth">RTHandle that should be released.</param>
        public static void Release(KWS_RTHandle rth)
        {
            s_DefaultInstance.Release(rth);
        }


        /// <summary>
        /// Sets the reference rendering size for subsequent rendering for the default RTHandle System
        /// </summary>
        /// <param name="width">Reference rendering width for subsequent rendering.</param>
        /// <param name="height">Reference rendering height for subsequent rendering.</param>
        public static void SetReferenceSize(int width, int height, MSAASamples msaaSamples)
        {
            s_DefaultInstance.SetReferenceSize(width, height);
        }

        /// <summary>
        /// Reset the reference size of the system and reallocate all textures.
        /// </summary>
        /// <param name="width">New width.</param>
        /// <param name="height">New height.</param>
        public static void ResetReferenceSize(int width, int height)
        {
            s_DefaultInstance.ResetReferenceSize(width, height);
        }

        /// <summary>
        /// Returns the ratio against the current target's max resolution
        /// </summary>
        /// <param name="width">width to utilize</param>
        /// <param name="height">height to utilize</param>
        /// <returns> retruns the width,height / maxTargetSize.xy ratio. </returns>
        public static Vector2 CalculateRatioAgainstMaxSize(int width, int height)
        {
            return s_DefaultInstance.CalculateRatioAgainstMaxSize(new Vector2Int(width, height));
        }
    }
}
