using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSfxPlayer : MonoBehaviour
{
    [SerializeField]
    private string soundName;

    [SerializeField]
    private bool playOnEnable;

    private void OnEnable()
    {
        if (playOnEnable)
            Play();
    }

    public void Play()
    {
        AudioPlayer.Instance.PlaySFX(soundName);
    }

    public void Play(string name)
    {
        AudioPlayer.Instance.PlaySFX(name);
    }
}
