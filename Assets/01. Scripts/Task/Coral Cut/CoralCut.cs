using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class CoralCut : TaskScript
{
    [SerializeField]
    private List<Stick> sticks = new List<Stick>();

    [SerializeField]
    private Scissors scissors;

    public override void Init()
    {
        base.Init();

        foreach (var stick in sticks)
            stick.OnReset();
    }

    public override void OnShow()
    {
        base.OnShow();

        scissors.OnShow();
    }

    public override void OnHide()
    {
        base.OnHide();
        scissors.OnHide();
    }

    public void OnCheck()
    {
        int cutting = 0;

        foreach (var stick in sticks)
            if (stick.isCut)
                cutting++;

        if (cutting == 3)
            StartCoroutine(OnSuccess());
    }

    public IEnumerator OnSuccess()
    {
        myCharacter.isUI = false;
        closeBtn.interactable = false;

        textAnim.gameObject.SetActive(true);
        textAnim.SetBool("isSuccess", true);
        isSuccess = true;

        if(InGameUIManager.Instance != null)
        {
            InGameUIManager.Instance.OnSuccess();
            InGameUIManager.Instance.TaskTypeCheck((int)taskType);
        }

        yield return new WaitForSeconds(3f);

        OnHide();
        closeBtn.interactable = true;
    }
}
