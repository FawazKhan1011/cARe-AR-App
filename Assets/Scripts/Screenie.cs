using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;

public class Screenie : MonoBehaviour
{
    public Canvas canvasToHide; // Assign the canvas that needs to be hidden during screenshot
    public Button captureButton; // Use 'Button' from 'UnityEngine.UI' namespace

    private bool isCapturing = false;

    void Start()
    {
        // Add a listener to the button's click event
        captureButton.onClick.AddListener(OnCaptureButtonClick);
    }
    void OnCaptureButtonClick()
    {
        if (!isCapturing)
        {
            StartCoroutine(CaptureAndSaveScreenshot());
        }
    }

    IEnumerator CaptureAndSaveScreenshot()
    {
        isCapturing = true;

        // Hide the canvas
        canvasToHide.enabled = false;

        // Wait for end of frame to ensure canvas is hidden before taking screenshot
        yield return new WaitForEndOfFrame();

        // Generate a unique filename with current date and time
        string fileName = "screenshot_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";

        string filePath = Path.Combine(Application.persistentDataPath, fileName);

#if UNITY_ANDROID || UNITY_IOS
        // Capture screenshot using ReadPixels method
        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenTexture.Apply();

        // Convert texture to bytes and save as PNG file
        byte[] bytes = screenTexture.EncodeToPNG();
        File.WriteAllBytes(filePath, bytes);

        // Save screenshot to gallery if file exists
        if (File.Exists(filePath))
        {
            NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(filePath, "My Screenshots", fileName);
            Debug.Log("Permission result: " + permission);
        }
#endif

        // Show the canvas again after capturing and saving the screenshot
        canvasToHide.enabled = true;

        isCapturing = false;
    }
}
