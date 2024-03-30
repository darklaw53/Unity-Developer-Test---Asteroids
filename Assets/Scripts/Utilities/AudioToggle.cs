using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioToggle : MonoBehaviour
{
    public BoolSO mutedSO;
    public AudioMixer audioMixer;
    public Sprite muted, unmuted;
    public Image icon;

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