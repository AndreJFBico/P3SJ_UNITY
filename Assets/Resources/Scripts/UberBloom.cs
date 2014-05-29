using UnityEngine;
using System.Collections;

public class UberBloom : PostEffectsBase {

	public enum UberResolution {
		Low = 0,
		High = 1,
	}

	public enum UberBlurType {
		Standard = 0,
		Sgx = 1,
	}

	[Range(0.0f, 1.5f)] public float threshhold = 0.25f;

	[Range(0.0f, 2.5f)]	public float intensity = 0.75f;

	[Range(0.25f, 5.5f)] public float blurSize = 1.0f;

	UberResolution resolution = UberResolution.Low;

	[Range(1, 4)] public int blurIterations = 1;

	public UberBlurType blurType = UberBlurType.Standard;

	public Shader fastBloomShader;
	private Material fastBloomMaterial = null;

	bool CheckResources () 
	{	
		CheckSupport (false);	
	
		fastBloomMaterial = CheckShaderAndCreateMaterial (fastBloomShader, fastBloomMaterial);
		
		if(!isSupported)
			ReportAutoDisable ();
		return isSupported;				
	}

	void OnDisable() 
	{
		if(fastBloomMaterial)
			DestroyImmediate (fastBloomMaterial);
	}

	void OnRenderImage (RenderTexture source, RenderTexture destination) {	
		if(CheckResources() == false) {
			Graphics.Blit (source, destination);
			return;
		}

        int divider = resolution == UberResolution.Low ? 4 : 2;
        float widthMod = resolution == UberResolution.Low ? 0.5f : 1.0f;

		fastBloomMaterial.SetVector ("_Parameter", new Vector4 (blurSize * widthMod, 0.0f, threshhold, intensity));
		source.filterMode = FilterMode.Bilinear;

		var rtW = source.width/divider;
		var rtH = source.height/divider;

		// downsample
		RenderTexture rt = RenderTexture.GetTemporary (rtW, rtH, 0, source.format);
		rt.filterMode = FilterMode.Bilinear;
		Graphics.Blit (source, rt, fastBloomMaterial, 1);

        var passOffs = blurType == UberBlurType.Standard ? 0 : 2;
		
		for(int i = 0; i < blurIterations; i++) {
			fastBloomMaterial.SetVector ("_Parameter", new Vector4 (blurSize * widthMod + (i*1.0f), 0.0f, threshhold, intensity));

			// vertical blur
			RenderTexture rt2 = RenderTexture.GetTemporary (rtW, rtH, 0, source.format);
			rt2.filterMode = FilterMode.Bilinear;
			Graphics.Blit (rt, rt2, fastBloomMaterial, 2 + passOffs);
			RenderTexture.ReleaseTemporary (rt);
			rt = rt2;

			// horizontal blur
			rt2 = RenderTexture.GetTemporary (rtW, rtH, 0, source.format);
			rt2.filterMode = FilterMode.Bilinear;
			Graphics.Blit (rt, rt2, fastBloomMaterial, 3 + passOffs);
			RenderTexture.ReleaseTemporary (rt);
			rt = rt2;
		}
		
		fastBloomMaterial.SetTexture ("_Bloom", rt);

		Graphics.Blit (source, destination, fastBloomMaterial, 0);

		RenderTexture.ReleaseTemporary (rt);
	}
}
