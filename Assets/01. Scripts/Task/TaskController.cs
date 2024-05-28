using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskController : MonoBehaviour
{
    private int commonTaskCount;
    private int simpleTaskCount;

    public List<TaskObject> commonTasks = new List<TaskObject>();
    public List<TaskObject> simpleTasks = new List<TaskObject>();

    public void SetTask(int _randomCommon, int _randomSimple)
    {
        commonTaskCount = GameManager.Instance.ruleData.commonTask;
        simpleTaskCount = GameManager.Instance.ruleData.simpleTask;

        var manager = InGameUIManager.Instance;

        SetCommonTask(commonTaskCount, _randomCommon);
        SetSimpleTask(simpleTaskCount, _randomSimple);
    }

    public void SetCommonTask(int _count, int _random = 0, bool isReset = false)
    {
        // 공용 임무는 0 ~ 2 개
        switch (_count)
        {
            case 0:
                {
                    foreach(var task in commonTasks)
                        task.gameObject.SetActive(false);
                    break;
                }
            case 1:
                {
                    commonTasks[_random].gameObject.SetActive(false);

                    break;
                }
            case 2:
                {
                    foreach (var task in commonTasks)
                        task.gameObject.SetActive(true);
                    break;
                }
        }
        if(InGameUIManager.Instance != null && !isReset)
            InGameUIManager.Instance.TaskUI.SetCommonTaskList(_count, _random);
    }

    public void SetSimpleTask(int _count, int _random = 0, bool isReset = false)
    {
        // 개인 임무는 0 ~ 3개
        switch (_count)
        {
            case 0:
                {
                    foreach (var task in simpleTasks)
                        task.gameObject.SetActive(false);
                    break;
                }
            case 1:
                {
                    foreach (var task in simpleTasks)
                        task.gameObject.SetActive(false);

                    simpleTasks[_random].gameObject.SetActive(true);
                    break;
                }
            case 2:
                {
                    simpleTasks[_random].gameObject.SetActive(false);
                    break;
                }
            case 3:
                {
                    foreach (var task in simpleTasks)
                        task.gameObject.SetActive(true);
                    break;
                }
        }

        if(InGameUIManager.Instance != null && !isReset)
            InGameUIManager.Instance.TaskUI.SetSimpleTaskList(_count, _random);
    }
}
