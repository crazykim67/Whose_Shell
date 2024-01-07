using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CleanShellTask : TaskScript
{
    [SerializeField]
    private List<Dust> dusts = new List<Dust>();

    [SerializeField]
    private Cloth cloth;

    [SerializeField]
    private GameObject head;

    public override void Init()
    {
        base.Init();

        foreach (var dust in dusts)
            dust.OnReset();
    }

    public override void OnShow()
    {
        base.OnShow();
        cloth.OnShow();
    }

    public override void OnHide()
    {
        base.OnHide();
        cloth.OnHide();
    }

    public void OnCheck()
    {
        int cleaning = 0;

        foreach(var dust in dusts)
            if (dust.isClean)
                cleaning++;

        if(cleaning == 4)
            StartCoroutine(OnSuccess());
    }

    public IEnumerator OnSuccess()
    {
        myCharacter.isUI = false;
        closeBtn.interactable = false;

        textAnim.gameObject.SetActive(true);
        textAnim.SetBool("isSuccess", true);
        isSuccess = true;

        head.SetActive(true);

        if (InGameUIManager.Instance != null)
        {
            InGameUIManager.Instance.OnSuccess();
            InGameUIManager.Instance.TaskTypeCheck((int)taskType);
        }

        yield return new WaitForSeconds(3f);

        OnHide();
        closeBtn.interactable = true;
    }
}
