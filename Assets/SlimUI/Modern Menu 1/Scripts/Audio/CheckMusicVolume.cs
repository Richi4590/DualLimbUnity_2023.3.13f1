using UnityEngine;
using System.Collections;

namespace SlimUI.ModernMenu{
	public class CheckMusicVolume : MonoBehaviour {

		[SerializeField] bool audioSourceOnSelf = true;
        [SerializeField] AudioSource audioSource;

        public void  Start (){

			if (audioSourceOnSelf)
			// remember volume level from last time
				GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
			else
                audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        }

		public void UpdateVolume (){
			if (audioSourceOnSelf)
				GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
			else
                audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");

        }
	}
}