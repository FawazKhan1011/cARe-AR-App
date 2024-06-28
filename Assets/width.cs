using UnityEngine;
using UnityEngine.UI;

public class ResizeRawImage : MonoBehaviour
{
    public RawImage rawImage;

    void Start()
    {
        ResizeImage();
    }

    void ResizeImage()
    {
        RectTransform rt = rawImage.rectTransform;
        CanvasScaler canvasScaler = GetComponentInParent<CanvasScaler>();

        if (canvasScaler != null)
        {
            // Get the reference resolution of the canvas scaler
            float referenceWidth = canvasScaler.referenceResolution.x;
            float referenceHeight = canvasScaler.referenceResolution.y;

            // Calculate the scaling factor for width and height
            float widthScale = Screen.width / referenceWidth;
            float heightScale = Screen.height / referenceHeight;

            // Apply the scaling factor to the size of the RawImage
            rt.sizeDelta = new Vector2(rt.sizeDelta.x * widthScale, rt.sizeDelta.y * heightScale);
        }
        else
        {
            Debug.LogWarning("CanvasScaler not found in parent. Make sure the RawImage is a child of a Canvas with CanvasScaler.");
        }
    }
}
