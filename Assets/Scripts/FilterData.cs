using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

[Serializable]
public class FiltersResponse
{
    public string message;
    public bool success;
    public Filter[] data;
}

public class FilterData : MonoBehaviour
{
    [SerializeField] private GameObject filterButtonPrefab;
    [SerializeField] private Transform filterContent;
    [SerializeField] private TextMeshProUGUI categoryText; // Reference to the Text component

    private string apiUrl = "https://custom-iztj.onrender.com/api/admin/allFilters";

    // Global variables to store the selected filter name and url
    public static string SelectedFilterName { get; private set; }

    void OnEnable()
    {
        // Set the text of the categoryText component
        if (categoryText != null)
        {
            categoryText.text = Home.selectedCategoryName;
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found. Make sure to assign it in the Inspector.");
        }

        StartCoroutine(GetFiltersCoroutine());
    }

    IEnumerator GetFiltersCoroutine()
    {
        string jsonPayload = "{\"categoryName\":\"" + Home.selectedCategoryName + "\"}";
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonPayload);

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
        if (response != null && response.success && response.data != null)
        {
            foreach (Filter filter in response.data)
            {
                CreateFilterButton(filter);
            }
        }
        else
        {
            Debug.LogError("No filters found or error in the API response.");
        }
    }

    private void CreateFilterButton(Filter filter)
    {
        GameObject newButton = Instantiate(filterButtonPrefab, filterContent);

        TMP_Text textComponent = newButton.GetComponentInChildren<TMP_Text>();
        Button buttonComponent = newButton.GetComponent<Button>();

        if (textComponent != null && buttonComponent != null)
        {
            textComponent.text = filter.filterName;
            buttonComponent.onClick.AddListener(() => OnFilterButtonClick(filter.filterName, filter.filterUrl));
        }
        else
        {
            Debug.LogError("TextMeshPro or Button component not found in the filter button prefab.");
        }
    }

    private void OnFilterButtonClick(string filterName, string filterUrl)
    {
        SelectedFilterName = filterName;

        // Optionally, load the next scene using the scene name from the text view
        SceneManager.LoadScene(categoryText.text);
    }
}

[Serializable]
public class Filter
{
    public string filterName;
    public string filterUrl;
}