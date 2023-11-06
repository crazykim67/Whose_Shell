using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MainMenuUI : MonoBehaviour
{
    public GameObject mainMenu;

    public Button onlineBtn;
    public Button optionBtn;
    public Button logoutBtn;

    public LoginMenuUI loginMenuUI;

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
}
