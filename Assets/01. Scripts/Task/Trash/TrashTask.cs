using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class TrashTask : TaskScript
{
    [SerializeField]
    private GraphicRaycaster gr;

    [SerializeField]
    private Trash currentTrash;

    [SerializeField]
    private List<Trash> trashes = new List<Trash>();

    [SerializeField]
    private TrashCan trashCan;

    private void Update()
    {
        TrashMove();

        if (Input.GetMouseButtonDown(0))
            Raycast();
        else if (Input.GetMouseButtonUp(0))
        {
            PickOff();
            OnCheck();
        }
    }

    public override void Init()
    {
        base.Init();

        foreach (var trash in trashes)
            trash.OnReset();

        trashCan.OnReset();
    }

    public override void OnShow()
    {
        base.OnShow();
    }

    public override void OnHide()
    {
        base.OnHide();
    }

    public void OnCheck()
    {
        int throwing = 0;

        foreach(var trash in trashes)
        {
            if(trash.isThrow)
                throwing++;
        }

        if (throwing == 3)
            StartCoroutine(OnSuccess());
    }

    public IEnumerator OnSuccess()
    {
        myCharacter.isUI = false;
        closeBtn.interactable = false;

        if(myCharacter.taskObject != null)
            myCharacter.taskObject = null;  

        textAnim.gameObject.SetActive(true);
        textAnim.SetBool("isSuccess", true);
        curTaskText.OnTaskCheck(true);
        isSuccess = true;

        if (InGameUIManager.Instance != null)
        {
            InGameUIManager.Instance.OnSuccess();
            InGameUIManager.Instance.TaskTypeCheck((int)taskType);
        }

        yield return new WaitForSeconds(3f);

        OnHide();
        closeBtn.interactable = true;
    }

    private void Raycast()
    {
        var ped = new PointerEventData(null);

        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);

        if (results.Count <= 0)
            return;

        if (results[0].gameObject.tag != "Trash")
            return;

        currentTrash = results[0].gameObject.GetComponent<Trash>();

        currentTrash.OnPickUp();
    }

    private void PickOff()
    {
        if (currentTrash == null)
            return;

        var ped = new PointerEventData(null);

        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);

        if (results.Count <= 0)
            return;

        if (results[0].gameObject.tag != "TrashCan")
            currentTrash.OnThrow(false);
        else
        {
            currentTrash.OnThrow(true);
            trashCan.OnThrow();
        }

        currentTrash = null;
    }

    private void TrashMove()
    {
        if (currentTrash == null)
            return;

        currentTrash.transform.position = Input.mousePosition;
    }
}
