using UnityEngine;

// use this script Pandol objcet
public class PandolManager : MonoBehaviour
{
    public Animator controller;
    public AudioSource audioSource;

    public void StartPandol()
    {
        controller.enabled = true;
        audioSource.enabled = true;
    }

    public void StopPandol()
    {
        controller.enabled = false;
        audioSource.enabled = false;
    }
}
