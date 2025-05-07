using UnityEngine;
using UnityEngine.UI;

public class ValidateOfArabic : MonoBehaviour
{
    #region Language Options
    [Header("Language Options")]
    [SerializeField] protected bool haveEnglishLetters ; // Flag to allow English letters
    [SerializeField] protected bool haveArabicLetters ; // Flag to allow Arabic letters
    public bool haveEnglishNumbers; // Flag to allow English numbers
    [SerializeField] protected bool haveArabicNumbers; // Flag to allow Arabic numbers
    [SerializeField] protected bool haveEnglishSymbols; // Flag to allow English symbols
    [SerializeField] protected bool haveArabicSymbols ; // Flag to allow Arabic symbols
    [SerializeField] protected bool allowSpaces; // Flag to allow space characters
    #endregion

    #region Character Limit
    [Header("Character Limit")]
    [SerializeField] protected int minCharacterLimit = 1;   // Minimum limit for the number of characters
    [SerializeField] protected int maxCharacterLimit = 100; // Maximum limit for the number of characters
    #endregion

    #region Validation Methods

    /// <summary>
    /// Filters the input text to only allow characters based on the selected language options.
    /// </summary>
    /// <param name="input">The input text to be filtered.</param>
    /// <returns>The filtered input text.</returns>
    public string FilterCharacters(string input)
    {
        char[] allowedChars = input.ToCharArray();
        for (int i = 0; i < allowedChars.Length; i++)
        {
            // Allow new line characters explicitly
            if (allowedChars[i] == '\n' || IsAllowedCharacter(allowedChars[i]))
            {
                continue; // Keep the character if it's a new line or allowed
            }

            allowedChars[i] = '\0'; // Replace disallowed characters with null character
        }
        return new string(allowedChars).Replace("\0", ""); // Remove null characters
    }

    /// <summary>
    /// Checks if a character is allowed based on the selected language options.
    /// </summary>
    /// <param name="c">The character to be checked.</param>
    /// <returns>True if the character is allowed, false otherwise.</returns>
    protected bool IsAllowedCharacter(char c)
    {
        // Check if the character is allowed based on the selected language options
        if (allowSpaces && c == ' ')
        {
            return true; // Allow space characters
        }
        if (haveEnglishLetters && IsEnglishLetter(c))
        {
            return true; // Allow English letters
        }
        if (haveEnglishNumbers && IsEnglishNumber(c))
        {
            return true; // Allow English numbers
        }
        if (haveEnglishSymbols && IsEnglishSymbol(c))
        {
            return true; // Allow English symbols
        }
        if (haveArabicLetters && IsArabicLetter(c))
        {
            return true; // Allow Arabic letters
        }
        if (haveArabicNumbers && IsArabicNumber(c))
        {
            return true; // Allow Arabic numbers
        }
        if (haveArabicSymbols && IsArabicSymbol(c))
        {
            return true; // Allow Arabic symbols
        }
        return false; // Disallow other characters
    }
    #endregion

    #region Character Check Methods
    /// <summary>
    /// Checks if a character is an English letter.
    /// </summary>
    /// <param name="c">The character to be checked.</param>
    /// <returns>True if the character is an English letter, false otherwise.</returns>
    protected bool IsEnglishLetter(char c)
    {
        return (c >= 0x0041 && c <= 0x005A) || (c >= 0x0061 && c <= 0x007A); // Uppercase A-Z and lowercase a-z
    }

    /// <summary>
    /// Checks if a character is an English number.
    /// </summary>
    /// <param name="c">The character to be checked.</param>
    /// <returns>True if the character is an English number, false otherwise.</returns>
    protected bool IsEnglishNumber(char c)
    {
        return c >= 0x0030 && c <= 0x0039; // Digits 0-9
    }

    /// <summary>
    /// Checks if a character is an English symbol.
    /// </summary>
    /// <param name="c">The character to be checked.</param>
    /// <returns>True if the character is an English symbol, false otherwise.</returns>
    protected bool IsEnglishSymbol(char c)
    {
        return (c >= 0x0020 && c <= 0x002F) || // Space and punctuation
               (c >= 0x003A && c <= 0x0040) || // Punctuation
               (c >= 0x005B && c <= 0x0060) || // Punctuation
               (c >= 0x007B && c <= 0x007E);   // Punctuation
    }

    /// <summary>
    /// Checks if a character is an Arabic letter.
    /// </summary>
    /// <param name="c">The character to be checked.</param>
    /// <returns>True if the character is an Arabic letter, false otherwise.</returns>
    protected bool IsArabicLetter(char c)
    {
        // Arabic letters range excluding Arabic-Indic digits
        return (c >= 0x0600 && c <= 0x065F) || (c >= 0x0670 && c <= 0x06FF);
    }

    /// <summary>
    /// Checks if a character is an Arabic number.
    /// </summary>
    /// <param name="c">The character to be checked.</param>
    /// <returns>True if the character is an Arabic number, false otherwise.</returns>
    protected bool IsArabicNumber(char c)
    {
        return c >= 0x0660 && c <= 0x0669; // Arabic-Indic digits 0-9
    }

    /// <summary>
    /// Checks if a character is an Arabic symbol.
    /// </summary>
    /// <param name="c">The character to be checked.</param>
    /// <returns>True if the character is an Arabic symbol, false otherwise.</returns>
    protected bool IsArabicSymbol(char c)
    {
        return (c >= 0x0600 && c <= 0x0603) || // Arabic symbols
               (c >= 0x060C && c <= 0x060D) || // Arabic comma and date separator
               (c == 0x061B) || // Arabic semicolon
               (c == 0x061F);   // Arabic question mark
    }
    #endregion
}
