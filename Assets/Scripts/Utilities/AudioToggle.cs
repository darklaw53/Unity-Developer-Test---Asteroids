using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioToggle : MonoBehaviour
{
    [Header("Images and Sprites")]
    public Sprite muted;
    public Sprite unmuted;
    public Image icon;

    [Header("Scriptable Object")]
    public BoolSO mutedSO;

    [Header("Audio")]
    public AudioMixer audioMixer;

    bool isMuted = false;

    private void Start()
    {
        if (mutedSO._bool)
        {
            audioMixer.SetFloat("Volume", -80f);
            icon.sprite = muted;
        }
        else
        {
            audioMixer.SetFloat("Volume", 0f);
            icon.sprite = unmuted;
        }
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        if (isMuted)
        {
            audioMixer.SetFloat("Volume", -80f);
            icon.sprite = muted;
            mutedSO._bool = true;
        }
        else
        {
            audioMixer.SetFloat("Volume", 0f);
            icon.sprite = unmuted;
            mutedSO._bool = false;
        }
    }
}