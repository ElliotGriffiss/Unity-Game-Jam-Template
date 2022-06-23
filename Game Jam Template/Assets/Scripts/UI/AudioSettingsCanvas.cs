using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingsCanvas : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject CanvasParent;
    [SerializeField] private AudioMixer AudioMixer;

    private void Update()
    {
        if (Input.GetKeyUp("space"))
        {
            CanvasParent.SetActive(!CanvasParent.activeInHierarchy);
        }
    }

    public void OnMusicSliderUpdated(float Value)
    {
        Debug.Log(Value);
        AudioMixer.SetFloat("musicLevel", Value);
    }

    public void OnSFXSliderUpdated(float Value)
    {
        AudioMixer.SetFloat("soundEffectLevel", Value);
    }
}
