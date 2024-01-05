using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class CoralCut : MonoBehaviour
{
    public Task task = Task.CoralCut;

    [SerializeField]
    private List<Stick> sticks = new List<Stick>();

    [SerializeField]
    private Scissors scissors;

    [SerializeField]
    private Animator textAnim;

    [SerializeField]
    private Button closeBtn;

    public bool isSuccess = false;

    private PlayerController myCharacter;

    private void Init()
    {
        if (GameSystem.Instance == null)
            return;

        if (myCharacter == null)
            foreach (var player in GameSystem.Instance.controllerList)
            {
                if (player.nickName.Equals(PhotonNetwork.NickName))
                    myCharacter = player;
                break;
            }

        textAnim.gameObject.SetActive(false);
        textAnim.SetBool("isSuccess", false);

        foreach (var stick in sticks)
            stick.OnReset();
    }

    public void OnShow()
    {
        if (isSuccess)
            return;

        Init();

        this.gameObject.SetActive(true);
        scissors.OnShow();
        myCharacter.isUI = true;
    }

    public void OnHide()
    {
        this.gameObject.SetActive(false);
        scissors.OnHide();

        if (InGameUIManager.Instance == null)
            return;

        InGameUIManager.Instance.TaskUI.OnHideTaskMission();
        myCharacter.isUI = false;
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
        closeBtn.interactable = false;

        textAnim.gameObject.SetActive(true);
        textAnim.SetBool("isSuccess", true);
        isSuccess = true;

        yield return new WaitForSeconds(3f);

        OnHide();
        closeBtn.interactable = true;
    }
}
