using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

[Serializable]
public class Category
{
    public string catID;
    public string catNAME;
}

public class Home : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform content;
    private string apiUrl = "https://custom-iztj.onrender.com/api/admin/getCategory";

    // Static variable to store the selected category name
    public static string selectedCategoryName;

    void Start()
    {
        StartCoroutine(GetCategoriesCoroutine());
    }

    IEnumerator GetCategoriesCoroutine()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error when trying to send request: " + request.error);
            yield break;
        }

        ProcessResponse(request.downloadHandler.text);
    }

    private void ProcessResponse(string json)
    {
        // Wrap the JSON array in an object
        string wrappedJson = "{\"categories\":" + json + "}";

        // Parse the wrapped JSON into a wrapper object
        var wrapper = JsonUtility.FromJson<Wrapper>(wrappedJson);

        if (wrapper != null && wrapper.categories != null && wrapper.categories.Length > 0)
        {
            foreach (Category category in wrapper.categories)
            {
                CreateCategoryButton(category.catNAME);
            }
        }
        else
        {
            Debug.LogError("No categories found in the API response.");
        }
    }

    [Serializable]
    private class Wrapper
    {
        public Category[] categories;
    }

    private void CreateCategoryButton(string categoryName)
    {
        GameObject newButton = Instantiate(buttonPrefab, content);

        TMP_Text textComponent = newButton.GetComponentInChildren<TMP_Text>();
        Button buttonComponent = newButton.GetComponent<Button>();

        if (textComponent != null && buttonComponent != null)
        {
            textComponent.text = categoryName;

            // Add an onClick listener to the button
            buttonComponent.onClick.AddListener(() => OnButtonClick(categoryName));
        }
        else
        {
            Debug.LogError("TextMeshPro or Button component not found in the button prefab.");
        }
    }

    private void OnButtonClick(string categoryName)
    {
        // Store the selected category name in the static variable
        selectedCategoryName = categoryName;

        // Load the next scene
        SceneManager.LoadScene("FilterSelect");
    }
}