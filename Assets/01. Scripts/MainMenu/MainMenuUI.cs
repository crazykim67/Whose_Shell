using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MainMenuUI : MonoBehaviour
{
    [Header("MainMenu UI")]
    public GameObject mainMenu;

    public Button hostBtn;
    public Button onlineBtn;
    public Button optionBtn;
    public Button logoutBtn;

    [Header("Login Menu Script")]
    public LoginMenuUI loginMenuUI;

    [Header("Create Room Script")]
    public RoomCreateUI roomCreateUI;

    [Header("Online Menu Script")]
    public OnlineMenuUI onlineMenuUI;

    public void OnLogout()
    {
        AuthManager.Instance.OnLogOut();
        CircleTransitionController.Instance.CloseBlackScreen(OnHide, loginMenuUI.OnShow);
    }

    #region Show & Hide

    public void OnShow()
    {
        mainMenu.SetActive(true);
    }

    public void OnHide()
    {
        mainMenu.SetActive(false);
    }

    #endregion

    public void OnHost()
    {
        CircleTransitionController.Instance.CloseBlackScreen(OnHide, roomCreateUI.OnShow);
    }

    public void OnOnline()
    {
        CircleTransitionController.Instance.CloseBlackScreen(OnHide, onlineMenuUI.OnShow);
    }
}
