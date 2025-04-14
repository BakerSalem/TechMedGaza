using UnityEngine;

public class VoiceToggle : MonoBehaviour
{
    public GameObject voiceAudio;

    public void StartVoice()
    {
        voiceAudio.SetActive(true);
    }
    public void StopVoice()
    {
        voiceAudio.SetActive(false);
    }
}

