using UnityEngine;

public enum KeyboardButtonType
{
    Character,
    Space,
    Delete,
    Clear
}

public class KeyboardButton : MonoBehaviour
{
    [Header("Button Properties")]
    public KeyboardButtonType buttonType = KeyboardButtonType.Character;
    [SerializeField] private string keyValue;
    [SerializeField] private ValidateOfTMPTextValue textEnglishValidator;
    [SerializeField] private ValidateOfTMPTextValue textArabicValidator;
    [SerializeField] private bool isAarbic = false;

    private float pressCooldown = 0.3f;
    private float lastPressTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Write") && Time.time - lastPressTime > pressCooldown && !isAarbic)
        {
            PressEnglishKey();
            lastPressTime = Time.time;
        }

        if (other.CompareTag("Write") && Time.time - lastPressTime > pressCooldown && isAarbic)
        {
            PressAarbicKey();
            lastPressTime = Time.time;
        }
    }

    public void PressEnglishKey()
    {
        if (textEnglishValidator == null)
            return;

        switch (buttonType)
        {
            case KeyboardButtonType.Character:
                textEnglishValidator.AddOrginalText(keyValue);
                break;

            case KeyboardButtonType.Space:
                textEnglishValidator.AddSpace();
                break;

            case KeyboardButtonType.Delete:
                textEnglishValidator.DeleteLastCharacter();
                break;
            case KeyboardButtonType.Clear:
                textEnglishValidator.ClearAllText();
                break;
        }
    }
    public void PressAarbicKey()
    {
        if (textArabicValidator == null)
            return;
        switch (buttonType)
        {
            case KeyboardButtonType.Character:
                textArabicValidator.AddOrginalText(keyValue);
                break;
            case KeyboardButtonType.Space:
                textArabicValidator.AddSpace();
                break;
            case KeyboardButtonType.Delete:
                textArabicValidator.DeleteLastCharacter();
                break;
            case KeyboardButtonType.Clear:
                textArabicValidator.ClearAllText();
                break;
        }
    }
}
