using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWebTask : TaskScript
{
    [SerializeField]
    private List<SpiderWeb> spiderWebs = new List<SpiderWeb>();

    [SerializeField]
    private Brush brush;

    public override void Init()
    {
        base.Init();

        foreach(var spiderWeb in spiderWebs)
            spiderWeb.OnReset();
    }

    public override void OnShow()
    {
        base.OnShow();
        brush.OnShow();
    }

    public override void OnHide()
    {
        base.OnHide();
        brush.OnHide();
    }

    public void OnCheck()
    {
        int clean = 0;

        foreach (var spiderWeb in spiderWebs)
            if (spiderWeb.isClean)
                clean++;

        if(clean == 4)
            StartCoroutine(OnSuccess());
    }

    public IEnumerator OnSuccess()
    {
        myCharacter.isUI = false;
        closeBtn.interactable = false;

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
}
