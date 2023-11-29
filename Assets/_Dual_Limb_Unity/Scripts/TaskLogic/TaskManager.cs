using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TaskManager : MonoBehaviour
{
    [SerializeField] InputManager inputManager;
    [SerializeField] GameObject UITaskPrefab;
    [SerializeField] GameObject taskElementsContainerParent;
    [SerializeField] UIFader UIFader;
    [SerializeField] GameObject UIReturnToMenuLabel;
    [SerializeField] private AudioClip taskCompletedSound;
    public List<Task> tasks;


    // Start is called before the first frame update
    void Start()
    {
        if (tasks.Count <= 0) 
        {
            Debug.LogError("No tasks for Level assigned!");
        }

        GenerateUIOutOfTaskList();
    }

    private void GenerateUIOutOfTaskList()
    {
        foreach (Task task in tasks) 
        {
            task.taskCompleted += CheckForAllTasksDone;
            GameObject UITask = Instantiate(UITaskPrefab, taskElementsContainerParent.transform);
            UITaskValueReferences UITRef = UITask.GetComponent<UITaskValueReferences>();
            task.SetUIValueReferencer(UITRef);
            UITRef.TaskNameTextMesh.text = task.TaskName;
            UITRef.TaskDescTextMesh.text = task.TaskDescription;
        }
    }

    private async void CheckForAllTasksDone()
    {
        SoundPlayer.PlaySound(taskCompletedSound);

        if (tasks.TrueForAll(t => t.IsCompleted))
        {
            Timer.Instance.StopTimer();
            UIReturnToMenuLabel.SetActive(true);

            inputManager.DisconnectAllControllers();
            UIFader.Fade();

            await Utilities.WaitForSecondsAsync(2.0f);

            SceneManager.LoadScene("0_MainMenu");
        }
    }
}
