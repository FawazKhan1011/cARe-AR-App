using UnityEngine;
using TMPro;

public class PasswordRecoveryPanel : MonoBehaviour
{
    public GameObject passwordRecoveryPanel;
    public TMP_InputField idInputField;

    public void ShowPanel()
    {
        // Set the panel active or adjust CanvasGroup alpha as needed
        passwordRecoveryPanel.SetActive(true);
    }

    public void HidePanel()
    {
        // Set the panel inactive or adjust CanvasGroup alpha as needed
        passwordRecoveryPanel.SetActive(false);
    }

    public void SubmitButtonClicked()
    {
        // Access the input field value
        string userId = idInputField.text;

        // Add logic for submitting the password recovery form
        // You can use the userId variable for processing
        // Implement password recovery logic here
    }

    public void CloseButtonClicked()
    {
        // Handle the "Close" button click to clear the input field and close the panel
        ClearInputField();
        HidePanel();
    }

    private void ClearInputField()
    {
        // Clear the text in the ID input field
        idInputField.text = "";
    }
}
