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
			{
				if (audioSource == null)
				{
                    AudioSource[] sources = GameObject.Find("AudioManager").GetComponents<AudioSource>();

                    for (int i = 0; i < sources.Length; i++)
                    {
                        if (sources[i].clip != null)
                        {
                            audioSource = sources[i];
                        }
                    }
                }


                audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
            }
        }

		public void UpdateVolume (){
			if (audioSourceOnSelf)
				GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
			else
            {
                if (audioSource == null)
                {
                    AudioSource[] sources = GameObject.Find("AudioManager").GetComponents<AudioSource>();

                    for (int i = 0; i < sources.Length; i++)
                    {
                        if (sources[i].clip != null)
                        {
                            audioSource = sources[i];
                        }
                    }
                }

                audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
            }


        }
	}
}