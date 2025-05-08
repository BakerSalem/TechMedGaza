using TMPro;
using UnityEngine;

/// <summary>
/// ValidateOfTextValue is responsible for validating and fixing the input text
/// based on the selected language options (English and Arabic).
/// </summary>
public class ValidateOfTMPTextValue : ValidateOfArabic
{
    #region UI References
    [Header("UI References")]
    [SerializeField] private TMP_Text fixTxt; // Reference to the Text component to display the fixed text
    [SerializeField] private TMP_Text orgTxt;
    private string currentText = "";

    #endregion

    #region Unity Methods
    private void Start()
    {
        // Validate the initial text
        ValidateInput(orgTxt.text);
        ValidateInput(currentText);
    }
    #endregion

    #region Validation Methods

    public void AddOrginalText(string value)
    {
        currentText += value;
        orgTxt.text = currentText;
        ValidateInput(currentText);
        NetworkManager.Instance?.SendText(currentText);
    }

    /// <summary>
    /// Validates the input text based on the selected language options.
    /// </summary>
    /// <param name="input">The input text to be validated.</param>
    public void ValidateInput(string input)
    {
        string filteredInput = FilterCharacters(input);
        if (filteredInput.Length > maxCharacterLimit)
        {
            filteredInput = filteredInput.Substring(0, maxCharacterLimit);
        }
        Fix(filteredInput);
    }

    /// <summary>
    /// Appends a character or string to the current text, then validates and fixes it.
    /// </summary>
    public void DeleteLastCharacter()
    {
        if (currentText.Length > 0)
        {
            currentText = currentText.Substring(0, currentText.Length - 1);
            orgTxt.text = currentText;
            ValidateInput(currentText);
            NetworkManager.Instance?.SendText(currentText);
        }
    }
    /// <summary>
    /// Clears all input text.
    /// </summary>
    public void ClearAllText()
    {
        currentText = "";
        orgTxt.text = "";
        ValidateInput(currentText);
        NetworkManager.Instance?.SendText(currentText);
    }
    public void AddSpace()
    {
        AddOrginalText("  ");
    }
    public void ApplyRemoteText(string text)
    {
        currentText = text;
        orgTxt.text = text;
        ValidateInput(text);
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


