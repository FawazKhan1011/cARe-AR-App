using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputFieldCharacterLimit : MonoBehaviour
{
    public TMP_InputField inputField;
    public int characterLimit = 21; // Set your desired character limit here

    void Start()
    {
        if (inputField != null)
        {
            inputField.characterLimit = characterLimit;
        }
    }
}
