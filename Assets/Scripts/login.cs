using System.Text;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class Login : MonoBehaviour
{
    public TMP_InputField MRIdField;
    public TMP_InputField passwordField;
    public TextMeshProUGUI messageText;

    private string loginApiUrl = "https://custom-iztj.onrender.com/api/mr/login";

    // Static string to store the login data globally
    public static string loginData;

    void Start()
    {
        // Reset loginData when the game starts
        loginData = "";
    }

    public void OnLoginButtonPressed()
    {
        StartCoroutine(LoginCoroutine());
    }

    IEnumerator LoginCoroutine()
    {
        string MRId = MRIdField.text;
        string password = passwordField.text;

        // Create the JSON data to send
        LoginData loginDataObj = new LoginData(MRId, password);
        string jsonData = JsonUtility.ToJson(loginDataObj);

        Debug.Log("Sending login request to server...");
        Debug.Log("Request payload: " + jsonData);

        // Create a new UnityWebRequest and set the method to POST
        using (UnityWebRequest www = new UnityWebRequest(loginApiUrl, "POST"))
        {
            byte[] jsonToSend = new UTF8Encoding().GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();

            // Set the content type
            www.SetRequestHeader("Content-Type", "application/json");

            // Send the request and wait for a response
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + www.error);
                messageText.text = "Network error: " + www.error;
            }
            else
            {
                Debug.Log("Received response from server: " + www.downloadHandler.text);

                // Parse the response
                LoginResponse response = JsonUtility.FromJson<LoginResponse>(www.downloadHandler.text);
                messageText.text = response.message;

                if (response.success)
                {
                    Debug.Log("Login successful! Redirecting to Home Page...");
                    // Store the data globally
                    loginData = response.data;
                    SceneManager.LoadScene("Home Page"); // Ensure this scene name is correct
                }
                else
                {
                    Debug.LogWarning("Login failed. Message: " + response.message);
                }
            }
        }
    }

    [System.Serializable]
    private class LoginData
    {
        public string MRId;
        public string password;

        public LoginData(string mrid, string pass)
        {
            MRId = mrid;
            password = pass;
        }
    }

    [System.Serializable]
    private class LoginResponse
    {
        public string message;
        public bool success;
        public string data;
    }
}
