using UnityEngine;

public class AspectRatioHandler : MonoBehaviour
{
    void Start()
    {
        AdaptToAspectRatio();
    }

    void AdaptToAspectRatio()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        float screenRatio = (float)Screen.width / Screen.height;

        // Example adaptation based on screen ratio
        if (screenRatio > 0.5f && screenRatio < 0.6f)
        {
            // Adjust size, position, etc., based on this aspect ratio
            rectTransform.sizeDelta = new Vector2(200, 200);
            rectTransform.anchoredPosition = new Vector2(0, 0);
        }
        else if (screenRatio > 0.6f && screenRatio < 0.7f)
        {
            // Adjust for a different aspect ratio
            rectTransform.sizeDelta = new Vector2(150, 150);
            rectTransform.anchoredPosition = new Vector2(-50, -50);
        }
        // ... Add more conditions as needed
    }
}
