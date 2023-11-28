using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    public static SoundPlayer Instance { get; private set; }

    private void Start()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }



    public static void PlaySound(AudioClip clip)
    {
        Instance.audioSource.PlayOneShot(clip);
    }

}
