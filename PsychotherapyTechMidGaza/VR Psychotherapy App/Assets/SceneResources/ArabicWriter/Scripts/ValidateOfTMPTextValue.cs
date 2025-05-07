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
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Validate the initial text
        ValidateInput(fixTxt.text);
    }
    #endregion

    #region Validation Methods
    /// <summary>
    /// Validates the input text based on the selected language options.
    /// </summary>
    /// <param name="input">The input text to be validated.</param>
    public void ValidateInput(string input)
    {
        // Filter the input based on the selected language options
        string filteredInput = FilterCharacters(input);

        // Enforce character limits
        if (filteredInput.Length > maxCharacterLimit)
        {
            filteredInput = filteredInput.Substring(0, maxCharacterLimit);
        }

        //if (filteredInput.Length < minCharacterLimit)
        //{
        //    Debug.LogWarning($"Input text is shorter than the minimum limit of {minCharacterLimit} characters.");
        //    return;
        //}

        // Fix and display the filtered input
        Fix(filteredInput);
    }

    /// <summary>
    /// Appends a character or string to the current text, then validates and fixes it.
    /// </summary>
    public void AppendInput(string input)
    {
        string newText = fixTxt.text + input;
        string filteredInput = FilterCharacters(newText);

        if (filteredInput.Length > maxCharacterLimit)
            filteredInput = filteredInput.Substring(0, maxCharacterLimit);

        Fix(filteredInput);
    }

    /// <summary>
    /// Deletes the last character from the current text, then validates and fixes it.
    /// </summary>
    public void DeleteLastCharacter()
    {
        if (fixTxt.text.Length > 0)
        {
            string newText = fixTxt.text.Substring(0, fixTxt.text.Length - 1);
            string filteredInput = FilterCharacters(newText);

            Fix(filteredInput);
        }
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
            fixTxt.text += ArabicSupport.Fix(txt, true, true);
        }
        else
        {
            fixTxt.text = txt; // No need to fix for English, just display the text
        }
        //Debug.Log("Fixed: " + txt);
    }
    #endregion
}


