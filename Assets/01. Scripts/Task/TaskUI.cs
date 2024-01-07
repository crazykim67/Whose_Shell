using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;


public enum Task
{
    None,
    CoralCut,
    Trash,
    Dish,
}

public class TaskUI : MonoBehaviour
{
    public Image progress;

    [SerializeField]
    private CoralCut coralCut;
    public CoralCut CoralCut { get { return coralCut; } }

    [SerializeField]
    private TrashTask trashTask;
    public TrashTask TrashTask { get { return trashTask; } }

    [SerializeField]
    private DishTask dishTask;
    public DishTask DishTask { get { return dishTask; } }

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
                    trashTask.OnShow();
                    break;
                }
            case Task.Dish:
                {
                    this.gameObject.SetActive(true);
                    DishTask.OnShow();
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
}
