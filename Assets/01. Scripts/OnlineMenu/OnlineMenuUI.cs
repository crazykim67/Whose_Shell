using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OnlineMenuUI : MonoBehaviour
{
    public GameObject onlineMenu;

    public TMP_InputField nickNameInputField;

    public Animator nickAnim;
    public Animator codeAnim;

    public TextMeshProUGUI errorText;

    private float timer;
    private bool isError;

    public MainMenuUI mainMenuUI;

    private void Update()
    {
        if (isError)
        {
            if(timer < 3)
                timer += Time.deltaTime;
            else
            {
                timer = 0;
                isError = false;
                errorText.enabled = false;
            }
        }
    }

    #region Show & Hide

    public void OnShow()
    {
        nickNameInputField.text = "";
        onlineMenu.SetActive(true);
    }

    public void OnHide()
    {
        onlineMenu.SetActive(false);
    }

    #endregion

    public void OnCancel()
    {
        CircleTransitionController.Instance.CloseBlackScreen(OnHide, mainMenuUI.OnShow);
    }

    // 방 입장
    public void OnJoin()
    {
        if (nickNameInputField.text.Equals(string.Empty))
        {
            onNickAnim();
            OnError("닉네임 입력 칸이 비어있습니다.");
            return;
        }
    }

    public void onNickAnim()
    {
        nickAnim.SetTrigger("Error");
    }

    public void OnError(string str)
    {
        errorText.enabled = true;
        errorText.text = str;
        isError = true;
        timer = 0;
    }
}
