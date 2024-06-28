using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;

public class FilterMaterialApplication : MonoBehaviour
{
    [SerializeField] private Material pimpleMaterial; // Reference to the Material component
    private string apiUrl = "https://custom-iztj.onrender.com/api/admin/allFilters"; // Replace with your actual API URL

    void Start()
    {
        // Set the pimpleMaterial to a "none" state or default state on start
        ResetPimpleMaterial();

        // Ensure that the FilterData script is accessible or replace it with the correct implementation.
        if (FilterData.SelectedFilterName != null)
        {
            // Log both global variables
            Debug.Log("Selected Category: " + Home.selectedCategoryName);
            Debug.Log("Selected Filter: " + FilterData.SelectedFilterName);

            // Start the coroutine to download the filter URL
            StartCoroutine(GetFilterUrlCoroutine());
        }
        else
        {
            Debug.LogError("SelectedFilterName not set. Make sure to set it from the FilterData script.");
        }
    }

    void ResetPimpleMaterial()
    {
        // Implement the logic to reset the pimpleMaterial to a "none" state or default state
        // For example, setting it to a default texture or color
        pimpleMaterial.mainTexture = null; // Assuming "none" state is no texture
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
                // Assuming you want to apply the filter URL to the Material
                ApplyFilterToMaterial(selectedFilter.filterUrl);
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

    private void ApplyFilterToMaterial(string filterUrl)
    {
        // Download the texture using the filterUrl and apply it to the Material
        StartCoroutine(DownloadTextureForMaterial(filterUrl));
    }

    IEnumerator DownloadTextureForMaterial(string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error downloading texture for Material: " + www.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                pimpleMaterial.mainTexture = texture;
            }
        }
    }
}

