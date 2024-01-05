using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskObject : MonoBehaviour
{
    public Task task = Task.None;

    [SerializeField]
    private GameObject outLine;

    [SerializeField]
    private PlayerController currentPlayer;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (GameSystem.Instance == null)
            return;

        if (IsSuccessful())
            return;

        currentPlayer = coll.GetComponent<PlayerController>();

        if (currentPlayer != null && currentPlayer.pv.IsMine && (currentPlayer.playerType & PlayerType.Ghost) != PlayerType.Ghost)
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
        if (InGameUIManager.Instance == null)
            return;

        InGameUIManager.Instance.OnTask(task);
        currentPlayer.isUI = true;
    }

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
        }

        return isSuccss;
    }
}
