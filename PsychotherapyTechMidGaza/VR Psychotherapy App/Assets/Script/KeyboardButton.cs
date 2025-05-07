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
    [SerializeField] private ValidateOfInputField textValidator;

    private float pressCooldown = 0.3f;
    private float lastPressTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Write") && Time.time - lastPressTime > pressCooldown)
        {
            PressKey();
            lastPressTime = Time.time;
        }
    }

    public void PressKey()
    {
        if (textValidator == null)
            return;

        switch (buttonType)
        {
            case KeyboardButtonType.Character:
                textValidator.ValidateInput(keyValue);
                break;

            case KeyboardButtonType.Space:
                textValidator.ValidateInput(" ");
                break;

            case KeyboardButtonType.Delete:
                textValidator.DeleteLastCharacter();
                break;
            case KeyboardButtonType.Clear:
                textValidator.ClearAllText();
                break;
        }
    }
}
