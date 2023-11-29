using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI bestTimeText;
    public string timerKey;

    private float startTime;
    private bool isRunning = false;

    public static Timer Instance { get; private set; }

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        // Load the saved time from PlayerPrefs
        if (PlayerPrefs.HasKey(timerKey))
        {
            float savedTime = PlayerPrefs.GetFloat(timerKey);
            int minutes = Mathf.FloorToInt(savedTime / 60);
            int seconds = Mathf.FloorToInt(savedTime % 60);

            if (bestTimeText != null)
            {
                bestTimeText.text = "Best time: ";
                bestTimeText.text += string.Format("{0:00}:{1:00}", minutes, seconds);
            }
               
        }
        else
        {
            if (bestTimeText != null)
            {
                bestTimeText.text = "Best time: ";
                bestTimeText.text += string.Format("{0:00}:{1:00}", 0, 0);
            }
        }

        if (timerText != null)
            SetTime(0f);
    }

    private void Update()
    {
        if (isRunning)
        {
            float currentTime = Time.time - startTime;
            UpdateTimerText(currentTime);
        }
    }

    private void UpdateTimerText(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = "Time: ";
        timerText.text += string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartTimer()
    {
        if (!isRunning)
        {
            startTime = Time.time;
            isRunning = true;
        }
    }

    public void StopTimer()
    {
        if (isRunning)
        {
            float elapsedTime = Time.time - startTime;

            // Save the elapsed time to PlayerPrefs
            PlayerPrefs.SetFloat(timerKey, elapsedTime);
            PlayerPrefs.Save();

            float savedTime = PlayerPrefs.GetFloat(timerKey);
            int minutes = Mathf.FloorToInt(savedTime / 60);
            int seconds = Mathf.FloorToInt(savedTime % 60);

            if (bestTimeText != null)
            {
                bestTimeText.color = Color.yellow;
                bestTimeText.text = "Best time: ";
                bestTimeText.text += string.Format("{0:00}:{1:00}", minutes, seconds);
            }

            isRunning = false;
        }
    }

    public void ResetTimer()
    {
        SetTime(0f);

        // Remove the saved time from PlayerPrefs
        PlayerPrefs.DeleteKey(timerKey);
        PlayerPrefs.Save();
    }

    private void SetTime(float time)
    {
        startTime = Time.time - time;
        UpdateTimerText(time);
    }
}