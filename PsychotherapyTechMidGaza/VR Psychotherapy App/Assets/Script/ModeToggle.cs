using UnityEngine;

public class ModeToggle : MonoBehaviour
{
    public GameObject DrawBoard;
    public GameObject WriteBoard;
    public GameObject EngKey;
    public GameObject ArbKey;

    public void ShowDrawBoard()
    {
        DrawBoard.SetActive(true);
        WriteBoard.SetActive(false);
    }
    public void ShowWriteBoard()
    {
        WriteBoard.SetActive(true);
        DrawBoard.SetActive(false);
    }
    public void ShowEngKey()
    {
        EngKey.SetActive(true);
        ArbKey.SetActive(false);
    }
    public void ShowArbKey()
    {
        ArbKey.SetActive(true);
        EngKey.SetActive(false);
    }
}

