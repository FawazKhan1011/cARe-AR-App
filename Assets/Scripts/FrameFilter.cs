using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Collections;

public class FilterApplication : MonoBehaviour
{
    [SerializeField] private RawImage filterRawImage; // Reference to the RawImage component
    private string apiUrl = "https://custom-iztj.onrender.com/api/admin/allFilters"; // Replace with your actual API URL

    void OnEnable()
    {
        // Ensure that the FilterData script is accessible or replace it with the correct implementation.
        if (FilterData.SelectedFilterName != null)
        {
            // Log both global variables
            Debug.Log("Selected Category: " + Home.selectedCategoryName);
            Debug.Log("Selected Filter: " + FilterData.SelectedFilterName);
            Debug.Log("MR id: " + Login.loginData);
            // Start the coroutine to download the filter URL
            StartCoroutine(GetFilterUrlCoroutine());
        }
        else
        {
            Debug.LogError("SelectedFilterName not set. Make sure to set it from the FilterData script.");
        }
    }

    IEnumerator GetFilterUrlCoroutine()
    {
        // Create the payload with category name
        string jsonPayload = "{\"categoryName\":\"" + Home.selectedCategoryName + "\"}";
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonPayload);

        // Create and send the UnityWebRequest
        using (UnityWebRequest www = new UnityWebRequest(apiUrl, "POST"))
        {
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonBytes);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                ProcessResponse(www.downloadHandler.text);
            }
        }
    }

    private void ProcessResponse(string json)
    {
        FiltersResponse response = JsonUtility.FromJson<FiltersResponse>(json);
        if (response != null && response.success && response.data != null && response.data.Length > 0)
        {
            // Find the correct filter URL based on the selected filter name
            Filter selectedFilter = Array.Find(response.data, filter => filter.filterName == FilterData.SelectedFilterName);

            if (selectedFilter != null)
            {
                // Assuming you want to apply the filter URL to the RawImage
                ApplyFilterToRawImage(selectedFilter.filterUrl);
            }
            else
            {
                Debug.LogError("Selected filter not found in the API response.");
            }
        }
        else
        {
            Debug.LogError("No filter URL found or error in the API response.");
        }
    }
    private void ApplyFilterToRawImage(string filterUrl)
    {
        // Download the texture using the filterUrl and apply it to the RawImage
        StartCoroutine(DownloadTextureForRawImage(filterUrl));
    }

    IEnumerator DownloadTextureForRawImage(string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error downloading texture for RawImage: " + www.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                if (filterRawImage != null)
                {
                    filterRawImage.texture = texture;
                }
                else
                {
                    Debug.LogError("RawImage component is not assigned. Make sure to assign it in the Inspector.");
                }
            }
        }
    }
}
