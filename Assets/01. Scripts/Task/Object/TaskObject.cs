using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskObject : MonoBehaviour
{
    public Task task = Task.None;

    public TaskType taskType = TaskType.None;

    [SerializeField]
    private GameObject outLine;

    [SerializeField]
    private PlayerController currentPlayer;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (IsSuccessful())
            return;

        currentPlayer = coll.GetComponent<PlayerController>();

        if (currentPlayer != null && currentPlayer.pv.IsMine && (currentPlayer.playerType & PlayerType.Terrapin) != PlayerType.Terrapin)
        {
            outLine.SetActive(true);
            currentPlayer.taskObject = this;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if(currentPlayer != null)
        {
            currentPlayer.taskObject = null;
            currentPlayer = null;
        }

        outLine.SetActive(false);
    }

    public void OnShow()
    {
        InGameUIManager.Instance.OnTask(task);
        currentPlayer.isUI = true;
    }

    // 이미 처리한 미션인지 아닌지 체크
    public bool IsSuccessful()
    {
        bool isSuccss = false;

        switch (task)
        {
            case Task.None:
                {
                    isSuccss = false;
                    break;
                }
            case Task.CoralCut:
                {
                    isSuccss = InGameUIManager.Instance.TaskUI.CoralCut.isSuccess ? true : false;
                    break;
                }
            case Task.Trash:
                {
                    isSuccss = InGameUIManager.Instance.TaskUI.TrashTask.isSuccess ? true : false;
                    break;
                }
            case Task.Dish:
                {
                    isSuccss = InGameUIManager.Instance.TaskUI.DishTask.isSuccess ? true : false;
                    break;
                }
            case Task.CleanShell:
                {
                    isSuccss = InGameUIManager.Instance.TaskUI.CleanShellTask.isSuccess ? true : false;
                    break;
                }
            case Task.SpiderWeb:
                {
                    isSuccss = InGameUIManager.Instance.TaskUI.SpiderWebTask.isSuccess ? true : false;
                    break;
                }
        }

        return isSuccss;
    }
}
