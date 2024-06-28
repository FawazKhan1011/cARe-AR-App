using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Video : MonoBehaviour
{
    [SerializeField] private bool isAudioRecording = true;
    [SerializeField] private Canvas canvasToHide;
    [SerializeField] private Button recordButton;
    private string _recordedFilePath;
    private bool isRecording = false;
    [SerializeField] private Text savedPathText;

    void Start()
    {
        Button recordButton = GetComponent<Button>(); // Assuming this script is attached to the button
        recordButton.onClick.AddListener(ToggleRecording);
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

    public void ToggleRecording()
    {
        if (isRecording)
        {
            StopRecording();
            isRecording = false;

            if (canvasToHide != null)
                canvasToHide.enabled = true; // Show canvas after recording
        }
        else
        {
            StartRecording();
            isRecording = true;

            if (canvasToHide != null)
                canvasToHide.enabled = false; // Hide canvas during recording
        }
    }

    public void StartRecording()
    {
        SetFileName();
        SmileSoftScreenRecordController.instance.StartRecording();
        Debug.Log("Started screenrecording");
    }

    public void StopRecording()
    {
        string recordedFilePath = SmileSoftScreenRecordController.instance.StopRecording();
        Debug.Log("Stopped Recording");

        // Add a log to show where the file is stored
        Debug.Log("File stored at: " + _recordedFilePath);
    }
    private void SetFileName()
    {
        System.DateTime now = System.DateTime.Now;
        string date = now.ToShortDateString().Replace('/', '_')
                    + now.ToLongTimeString().Replace(':', '_');
        string fileName = "Record_" + date;

        SmileSoftScreenRecordController.instance.SetVideoName(fileName);
    }
}