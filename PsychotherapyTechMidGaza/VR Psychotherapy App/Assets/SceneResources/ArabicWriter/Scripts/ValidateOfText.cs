using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ValidateOfTextValue is responsible for validating and fixing the input text
/// based on the selected language options (English and Arabic).
/// </summary>
public class ValidateOfText : ValidateOfArabic
{
    #region UI References
    [Header("UI References")]
    [SerializeField] private Text fixTxt; // Reference to the Text component to display the fixed text
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Validate the initial text
        ValidateInput(fixTxt.text);
    }
    #endregion

    #region Validation Method
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


