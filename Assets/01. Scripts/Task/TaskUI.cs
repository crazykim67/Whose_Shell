using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Burst.Intrinsics;

public enum Task
{
    None,
    CoralCut,
    Trash,
    Dish,
    CleanShell,
    SpiderWeb,
}

public enum TaskType
{
    None,
    Common,
    Simple,
}

public class TaskUI : MonoBehaviour
{
    public Image progress;

    public RectTransform taskRect;
    public GameObject taskText;

    [SerializeField]
    private List<TaskText> taskTextList = new List<TaskText>();

    [SerializeField]
    private CoralCut coralCut;
    public CoralCut CoralCut { get { return coralCut; } }

    [SerializeField]
    private TrashTask trashTask;
    public TrashTask TrashTask { get { return trashTask; } }

    [SerializeField]
    private DishTask dishTask;
    public DishTask DishTask { get { return dishTask; } }

    [SerializeField]
    private CleanShellTask cleanShellTask;
    public CleanShellTask CleanShellTask { get { return cleanShellTask; } }

    [SerializeField]
    private SpiderWebTask spiderWebTask;
    public SpiderWebTask SpiderWebTask { get { return spiderWebTask; } }


    public bool isPlaying = false;

    public void OnTaskMission(Task _task)
    {
        if (isPlaying)
            return;

        switch (_task)
        {
            case Task.None:
                {
                    break;
                }
            case Task.CoralCut:
                {
                    this.gameObject.SetActive(true);
                    CoralCut.OnShow();
                    break;
                }
                case Task.Trash: 
                {
                    this.gameObject.SetActive(true);
                    TrashTask.OnShow();
                    break;
                }
            case Task.Dish:
                {
                    this.gameObject.SetActive(true);
                    DishTask.OnShow();
                    break;
                }
            case Task.CleanShell:
                {
                    this.gameObject.SetActive(true);
                    CleanShellTask.OnShow();
                    break;
                }
            case Task.SpiderWeb:
                {
                    this.gameObject.SetActive(true);
                    SpiderWebTask.OnShow();
                    break;
                }
        }

        isPlaying = true;
    }

    public void OnHideTaskMission()
    {
        this.gameObject.SetActive(false);
        isPlaying = false;
    }

    public void OnSuccess()
    {
        progress.fillAmount = progress.fillAmount + ((float)Math.Truncate(InGameUIManager.Instance.taskAmount * 1000) / 1000);
    }

    public void TaskTypeCheck(TaskType type)
    {
        if (GameManager.Instance == null)
            return;

        if (InGameUIManager.Instance == null)
            return;

        var game = GameManager.Instance;
        var inGame = InGameUIManager.Instance;

        switch (type)
        {
            case TaskType.None:
                    break;
            case TaskType.Common:
                {
                    game.successCommonTask = game.successCommonTask + 1;
                    break;
                }
            case TaskType.Simple:
                {
                    game.successSimpleTask = game.successSimpleTask + 1;
                    break;
                }
        }

        int totalSuccessTask = game.successCommonTask + game.successSimpleTask;
        int totalTask = inGame.commonTaskCount + inGame.simpleTaskCount;

        //
        if (totalSuccessTask == totalTask)
        {
            // 거북이 승리
            OuttroUIManager.Instance.Controller.OnOuttro(0);
        }
    }

    public void Init()
    {
        OnTaskListReset();
        progress.fillAmount = 0;
        CoralCut.Init();
        TrashTask.Init();
        DishTask.Init();
        CleanShellTask.Init();
        SpiderWebTask.Init();
    }

    public void SetCommonTaskList(int common, int random = 0)
    {
        switch(common) 
        {
            case 0:
                {
                    break;
                }
            case 1:
                {
                    if(random == 0)
                        SetTaskText("공용임무 : 쓰레기 버리기", trashTask);
                    else
                        SetTaskText("공용임무 : 산호초 자르기", coralCut);
                    break;
                }
            case 2:
                {
                    SetTaskText("공용임무 : 산호초 자르기", coralCut);
                    SetTaskText("공용임무 : 쓰레기 버리기", trashTask);
                    break;
                }
        }
    }
    public void SetSimpleTaskList(int simple, int random = 0)
    {
        switch (simple)
        {
            case 0:
                {
                    break;
                }
            case 1:
                {
                    if (random == 0)
                        SetTaskText("간단한 임무 : 접시 모으기", dishTask);
                    else if(random == 1)
                        SetTaskText("간단한 임무 : 등껍질 닦기", cleanShellTask);
                    else
                        SetTaskText("간단한 임무 : 거미줄 청소하기", spiderWebTask);

                    break;
                }
            case 2:
                {
                    if(random == 0)
                    {
                        SetTaskText("간단한 임무 : 등껍질 닦기", cleanShellTask);
                        SetTaskText("간단한 임무 : 거미줄 청소하기", spiderWebTask);
                    }
                    else if(random == 1)
                    {
                        SetTaskText("간단한 임무 : 접시 모으기", dishTask);
                        SetTaskText("간단한 임무 : 거미줄 청소하기", spiderWebTask);
                    }
                    else
                    {
                        SetTaskText("간단한 임무 : 접시 모으기", dishTask);
                        SetTaskText("간단한 임무 : 등껍질 닦기", cleanShellTask);
                    }
                    break;
                }
            case 3:
                {
                    SetTaskText("간단한 임무 : 등껍질 닦기", cleanShellTask);
                    SetTaskText("간단한 임무 : 접시 모으기", dishTask);
                    SetTaskText("간단한 임무 : 등껍질 닦기", cleanShellTask);
                    break;
                }
        }
    }   

    private void SetTaskText(string text, TaskScript taskScript) 
    { 
        TaskText _taskText = Instantiate(taskText, taskRect).GetComponent<TaskText>();
        _taskText.SetTask(text);
        _taskText.OnTaskCheck(false);
        taskTextList.Add(_taskText);
        taskScript.curTaskText = _taskText;
    }

    public void OnTaskListReset()
    {
        foreach(var taskText in taskTextList)
            Destroy(taskText.gameObject);

        taskTextList.Clear();
    }

    public IEnumerator Rebuilder()
    {
        yield return null;

        LayoutRebuilder.ForceRebuildLayoutImmediate(taskRect);
    }
}
