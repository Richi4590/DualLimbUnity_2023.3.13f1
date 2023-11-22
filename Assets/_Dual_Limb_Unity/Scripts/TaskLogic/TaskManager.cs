using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] GameObject UITaskPrefab;
    [SerializeField] GameObject taskElementsContainerParent;
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
            GameObject UITask = Instantiate(UITaskPrefab, taskElementsContainerParent.transform);
            UITaskValueReferences UITRef = UITask.GetComponent<UITaskValueReferences>();
            task.SetUIValueReferencer(UITRef);
            UITRef.TaskNameTextMesh.text = task.TaskName;
            UITRef.TaskDescTextMesh.text = task.TaskDescription;
        }
    }

    private void DisplayAllTasksDone()
    {
        // UI all tasks done maybe UI
    }
}
