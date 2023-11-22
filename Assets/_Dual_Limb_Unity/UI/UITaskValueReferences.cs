using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITaskValueReferences : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI taskNameTextMesh;
    [SerializeField] private TextMeshProUGUI taskDescTextMesh;
    [SerializeField] private GameObject completionStatusObj;

    public TextMeshProUGUI TaskNameTextMesh { get { return taskNameTextMesh; } }
    public TextMeshProUGUI TaskDescTextMesh { get { return taskDescTextMesh; } }
    public GameObject CompletionStatusObj { get {  return completionStatusObj; } }
}
