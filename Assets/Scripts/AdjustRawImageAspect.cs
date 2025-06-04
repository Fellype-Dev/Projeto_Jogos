using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AdjustRawImageAspect : MonoBehaviour
{
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    public CanvasScaler canvasScaler;

    void Start()
    {
        if (videoPlayer != null)
            videoPlayer.prepareCompleted += OnVideoPrepared;
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        // Pega a resolução do vídeo
        float videoWidth = vp.texture.width;
        float videoHeight = vp.texture.height;

        // Pega a resolução de referência do Canvas
        Vector2 referenceResolution = canvasScaler.referenceResolution;

        // Pega a resolução real da tela
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Calcula proporção do vídeo e da tela
        float videoAspect = videoWidth / videoHeight;
        float screenAspect = (screenWidth / referenceResolution.x) * referenceResolution.x / ((screenHeight / referenceResolution.y) * referenceResolution.y);

        // Ajusta o tamanho do RawImage para caber na tela sem distorcer
        RectTransform rt = rawImage.rectTransform;
        if (screenAspect > videoAspect)
        {
            // Tela mais larga que o vídeo
            float height = rt.sizeDelta.y;
            float width = height * videoAspect;
            rt.sizeDelta = new Vector2(width, height);
        }
        else
        {
            // Tela mais alta que o vídeo
            float width = rt.sizeDelta.x;
            float height = width / videoAspect;
            rt.sizeDelta = new Vector2(width, height);
        }
    }
}