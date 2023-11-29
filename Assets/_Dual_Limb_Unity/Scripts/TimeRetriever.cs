using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeRetriever : MonoBehaviour
{
    [SerializeField] private string timerKey;
    [SerializeField] TextMeshProUGUI bestTimeText;

    private static List<TimeRetriever> timeRetrievers = new List<TimeRetriever>();

    // Start is called before the first frame update
    void Start()
    {
        timeRetrievers.Add(this);

        if (PlayerPrefs.HasKey(timerKey))
        {
            float savedTime = PlayerPrefs.GetFloat(timerKey);
            int minutes = Mathf.FloorToInt(savedTime / 60);
            int seconds = Mathf.FloorToInt(savedTime % 60);

            if (bestTimeText != null)
            {
                bestTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }

        }
        else
        {
            bestTimeText.text = string.Format("{0:00}:{1:00}", 0, 0);
        }
    }

    public void ResetAllTimes(AudioClip clip)
    {
        AudioSource[] sources = GameObject.Find("AudioManager").GetComponents<AudioSource>();
        
        for (int i = 0; i < sources.Length; i++)
        {
            if (sources[i].clip == null) 
            {
                sources[i].PlayOneShot(clip);
            }
        }
        
        timeRetrievers.ForEach(t => t.ResetTimesAndVisualize());
    }

    private void ResetTimesAndVisualize()
    {
        if (PlayerPrefs.HasKey(timerKey))
            PlayerPrefs.SetFloat(timerKey, 0);

        bestTimeText.text = string.Format("{0:00}:{1:00}", 0, 0);
    }
}
