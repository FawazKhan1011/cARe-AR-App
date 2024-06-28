using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class askeris : MonoBehaviour
{
    public TMP_InputField passwordInput;
    public TMP_Text passwordText;
    private bool isPasswordVisible = true; // Assuming the password is initially visible

    void Start()
    {
        // Ensure the visibility state is consistent on start
        UpdatePasswordVisibility();
    }

    public void TogglePasswordVisibility()
    {
        // Toggle the visibility state
        isPasswordVisible = !isPasswordVisible;

        // Update the visibility immediately when the button is clicked
        UpdatePasswordVisibility();
    }

    private void UpdatePasswordVisibility()
    {
        // Set the content type of the TMP_InputField based on the visibility state
        passwordInput.contentType = isPasswordVisible ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;

        // Set the text of the TMP_Text based on the entered password
        passwordText.text = isPasswordVisible ? passwordInput.text : new string('*', passwordInput.text.Length);
    }
}
