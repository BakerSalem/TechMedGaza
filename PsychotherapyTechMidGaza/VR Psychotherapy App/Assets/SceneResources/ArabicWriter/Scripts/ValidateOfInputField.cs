using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

/// <summary>
/// ValidateOfInputFieldValue is responsible for validating and fixing the input text
/// in an InputField based on the selected language options (English and Arabic).
/// </summary>
public class ValidateOfInputField : ValidateOfArabic
{
    #region UI References
    [Header("UI References")]
    [SerializeField] private TMP_Text fixTxt; // Reference to the Text component to display the fixed text
    [SerializeField] private TMP_InputField inputField; // Reference to the InputField component
    private string currentText = "";
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Set the character limit for the InputField
        inputField.characterLimit = maxCharacterLimit;
    }
    #endregion

    #region Validation Method
    /// <summary>
    /// Validates the input text based on the selected language options.
    /// </summary>
    /// <param name="input">The input text to be validated.</param>
    public void ValidateInput(string input)
    {
        // Append the new input to the existing text

        // Filter the combined input
        string filteredInput = FilterCharacters(input);

        // Enforce character limits
        if (filteredInput.Length > maxCharacterLimit)
        {
            filteredInput = filteredInput.Substring(0, maxCharacterLimit);
        }

        // Update the InputField and fix the filtered input
        currentText += filteredInput;
        Fix(currentText);
    }

    /// <summary>
    /// Deletes the last character from the current text, then validates and fixes it.
    /// </summary>
    public void DeleteLastCharacter()
    {
        if (currentText.Length > 0)
        {
            currentText = currentText.Substring(0, currentText.Length - 1); // Remove from internal buffer
            Fix(currentText);
        }
    }
    /// <summary>
    /// Clears all input text.
    /// </summary>
    public void ClearAllText()
    {
        currentText = "";
        Fix(currentText);
    }
    #endregion

    #region Fix Method
    /// <summary>
    /// Fixes the input text for display based on the selected language options.
    /// </summary>
    /// <param name="txt">The text to be fixed.</param>
    public void Fix(string txt)
    {
        if (haveArabicLetters)
        {
            fixTxt.text = ArabicSupport.Fix(txt, true, true);
        }
        else
        {
            fixTxt.text = txt; // No need to fix for English, just display the text
        }
        //Debug.Log("Fixed: " + txt);
    }
    #endregion
}