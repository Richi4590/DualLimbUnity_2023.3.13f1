using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITask
{
    public bool IsCompleted { get; }
    public string TaskName { get;}
    public string TaskDescription { get;}
    public TaskType TaskType { get; }

}
