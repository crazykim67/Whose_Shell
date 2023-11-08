using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OnlineMenuUI : MonoBehaviour
{
    public GameObject onlineMenu;

    public TMP_InputField nickNameInputField;
    public TMP_InputField roomCodeInputField;

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
        roomCodeInputField.text = "";
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

        if (roomCodeInputField.text.Equals(string.Empty))
        {
            OnCodeAnim();
            OnError("방 코드 입력 칸이 비어있습니다.");
            return;
        }

        if (roomCodeInputField.text.Length < 4)
        {
            OnCodeAnim();
            OnError("방 코드는 4글자여야 합니다.");
            return;
        }
    }

    public void onNickAnim()
    {
        nickAnim.SetTrigger("Error");
    }

    public void OnCodeAnim()
    {
        codeAnim.SetTrigger("Error");
    }

    public void OnError(string str)
    {
        errorText.enabled = true;
        errorText.text = str;
        isError = true;
        timer = 0;
    }
}
