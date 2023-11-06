using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomCreateUI : MonoBehaviour
{
    public GameObject roomCreateUI;

    [Header("MainMenu UI")]
    public MainMenuUI mainMenuUI;

    #region Show & Hide

    public void OnShow()
    {
        roomCreateUI.SetActive(true);
    }

    public void OnHide()
    {
        roomCreateUI.SetActive(false);
    }

    #endregion

    public void OnCancel()
    {
        CircleTransitionController.Instance.CloseBlackScreen(OnHide, mainMenuUI.OnShow);
    }
}
