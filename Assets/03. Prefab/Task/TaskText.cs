using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI taskText;
    [SerializeField]
    private GameObject taskCheck;

    public void SetTask(string _text) => taskText.text = _text;

    public void OnTaskCheck(bool isCheck) => taskCheck.SetActive(isCheck);
}
