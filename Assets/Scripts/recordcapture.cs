using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System;
using TMPro;
using UnityEngine.Networking;

public class recordcapture: MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private bool isAudioRecording = true;
    public Button recordButton;
    public Canvas canvasToHide;

    private bool isRecording = false;
    private float timeHeldDown = 0.0f;

    //public variables for submit api
    public TMP_InputField doctorNameInputField; // Assuming doctorNameInputField is a TMP input field for doctor's name
    public TMP_InputField doctorSpecInputField; // Assuming doctorSpecInputField is a TMP input field for doctor's specialization
    public TMP_InputField doctorCityInputField; // Assuming doctorCityInputField is a TMP input field for doctor's city

    private const float holdThreshold = 1.0f; // Threshold for a long press

    void Start()
    {
        recordButton.onClick.AddListener(HandleClick); // Regular click
        SetUpScreenRecording();
    }

    void SetUpScreenRecording()
    {
        SmileSoftScreenRecordController.instance.SetStoredFolderName("CARE"); // Set folder name
        //SmileSoftScreenRecordController.instance.SetVideoStoringDestination(Application.dataPath); // Save in asset folder

        // Set screen size
        SmileSoftScreenRecordController.instance.SetVideoSize(Screen.width, Screen.height);
        SmileSoftScreenRecordController.instance.SetAudioCapabilities(isAudioRecording);
    }

    void Update()
    {
        if (isRecording)
        {
            timeHeldDown += Time.deltaTime;

            if (timeHeldDown >= holdThreshold)
            {
                // If we've passed the threshold, we are now recording
                StartScreenRecording();
                // Ensure we don't keep starting the recording
                isRecording = false;
            }
        }
    }

    void HandleClick()
    {
        // On a short tap, not a hold
        if (timeHeldDown < holdThreshold)
        {
            StartCoroutine(TakeScreenshot());
        }
    }

    private IEnumerator TakeScreenshot()
    {
        {
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
            SubmitDetails();
        }

    }

    public void StartScreenRecording()
    {
        SetFileName();
        canvasToHide.enabled = false;
        SmileSoftScreenRecordController.instance.StartRecording();
        Debug.Log("Started screenrecording");
    }

    public void StopScreenRecording()
    {
        string recordedFilePath = SmileSoftScreenRecordController.instance.StopRecording();
        canvasToHide.enabled = true;
        Debug.Log("Stopped Recording");
        SubmitDetails();
    }

    private void SetFileName()
    {
        System.DateTime now = System.DateTime.Now;
        string date = now.ToShortDateString().Replace('/', '_')
                    + now.ToLongTimeString().Replace(':', '_');
        string fileName = "Record_" + date;

        SmileSoftScreenRecordController.instance.SetVideoName(fileName);
    }

    // When the user presses down on the button
    public void OnPointerDown(PointerEventData eventData)
    {
        // Reset the time held down and flag the start of a potential recording
        timeHeldDown = 0.0f;
        isRecording = true;
    }

    // When the user releases the button
    public void OnPointerUp(PointerEventData eventData)
    {
        // If we were recording, stop the recording
        if (timeHeldDown >= holdThreshold)
        {
            StopScreenRecording();
        }

        // Reset recording flag
        isRecording = false;
    }
    public void SubmitDetails()
    {
        // Gather user-entered details
        string doctorName = doctorNameInputField.text;
        string doctorSpec = doctorSpecInputField.text;
        string doctorCity = doctorCityInputField.text;

        // Gather global variables
        string selectedCategoryName = Home.selectedCategoryName;
        string selectedFilterName = FilterData.SelectedFilterName;
        string MRID = Login.loginData;
        // Create a data object
        RecordData recordData = new RecordData()
        {
            mrID = MRID,
            doctorName = doctorName,
            doctorCatName = selectedCategoryName,
            doctorFilterName = selectedFilterName,
            doctorSpec = doctorSpec,
            doctorCity = doctorCity,
            doctorStatus = "Success"
        };

        // Serialize data object to JSON
        string jsonPayload = JsonUtility.ToJson(recordData);
        // logging the data
        Debug.Log("Sending data to API: " + jsonPayload);
        // Send JSON payload to API
        StartCoroutine(SendDataToAPI(jsonPayload));
    }

    private IEnumerator SendDataToAPI(string jsonPayload)
    {
        // Define API endpoint
        string apiUrl = "https://custom-iztj.onrender.com/api/doctor/staticUsage";

        // Create UnityWebRequest
        UnityWebRequest request = UnityWebRequest.PostWwwForm(apiUrl, "POST");

        // Set request headers
        request.SetRequestHeader("Content-Type", "application/json");

        // Attach JSON payload to request
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);

        // Send request and wait for response
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error sending request: " + request.error);
        }
        else
        {
            // Log response
            Debug.Log("Response: " + request.downloadHandler.text);
        }
    }

    // Data structure to hold record details
    [System.Serializable]
    public class RecordData
    {
        public string mrID;
        public string doctorName;
        public string doctorCatName;
        public string doctorFilterName;
        public string doctorSpec;
        public string doctorCity;
        public string doctorStatus;
    }
}