using System.Collections.Generic;
using UnityEngine;

public class BlurRenderer : MonoBehaviour
{
    [Range(0, 25)] [SerializeField] private int _iterations;

    [Range(0, 5)] [SerializeField] private int _downRes;

    public RenderTexture BlurTexture;

    public List<Camera> SkipsCameras;

    private Material _material;

    private RenderTexture _renderTexture;

    // blur renderTexture and set it in the BlurTexture
    private void _blurRenderTexture()
    {
        int width = _renderTexture.width >> _downRes;
        int height = _renderTexture.height >> _downRes;

        RenderTexture rt = RenderTexture.GetTemporary(width, height);

        Graphics.Blit(_renderTexture, rt);

        for (int i = 0; i < _iterations; i++)
        {
            RenderTexture rt2 = RenderTexture.GetTemporary(width, height);
            Graphics.Blit(rt, rt2, _material);
            RenderTexture.ReleaseTemporary(rt);
            rt = rt2;
        }
        Shader.SetGlobalTexture("_Blur", rt);

        Graphics.Blit(rt, BlurTexture);
    }

    // make screenShot by all cameras an set it in the renderTexture
    private void _makeRenderTexture()
    {
        List<Camera> cameras = new List<Camera>(Camera.allCameras);

        _renderTexture = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
        RenderTexture.active = _renderTexture;
        foreach (Camera cam in cameras)
        {
            if (SkipsCameras.Contains(cam))
                continue;

            float fov = cam.fieldOfView;
            cam.targetTexture = _renderTexture;
            cam.Render();
            cam.targetTexture = null;
            cam.fieldOfView = fov;
        }
        RenderTexture.active = null;
    }

    public void SetBlur()
    {
        if (_material == null)
        {
            _material = new Material(Shader.Find("Hidden/GaussianBlur"));
        }
        if (BlurTexture != null)
        {
            _makeRenderTexture();
            _blurRenderTexture();
        }
    }
}
