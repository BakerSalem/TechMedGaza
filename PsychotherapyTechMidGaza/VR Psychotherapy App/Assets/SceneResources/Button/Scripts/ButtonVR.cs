using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonVR : MonoBehaviour
{
    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    GameObject presser;
    AudioSource sound;
    bool isPressed;
    float pressCooldown = 0.3f;
    float lastPressTime;

    void Start()
    {
        sound = GetComponent<AudioSource>();
        isPressed = false;
        lastPressTime = -pressCooldown;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed && other.CompareTag("Write") && Time.time - lastPressTime > pressCooldown)
        {
            button.transform.localPosition = new Vector3(0, 0.003f, 0);
            presser = other.gameObject;
            onPress.Invoke();
            sound.Play();
            isPressed = true;
            lastPressTime = Time.time;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isPressed && other.gameObject == presser && other.CompareTag("Write"))
        {
            button.transform.localPosition = new Vector3(0, 0.015f, 0);
            onRelease.Invoke();
            isPressed = false;
        }
    }

}
