using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnceOnCollision : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [Range(0f, 1f)]
    [SerializeField] private float volume = 1f;
    private bool audioPlayed = false;


    private void OnCollisionEnter(Collision collision)
    {
        if (!audioPlayed && collision.gameObject.GetComponent<ConfigurableJoint>() != null)
        {
            SoundPlayer.PlaySound(clip, volume);
            audioPlayed = true;
        }
    }
}
