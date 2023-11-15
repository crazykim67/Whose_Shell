using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomizeUI : MonoBehaviour
{
    public GameObject customUI;

    #region Show & Hide

    public void OnShow()
    {
        PlayerController.isUI = true;
        customUI.SetActive(true);


    }

    public void OnHide()
    {
        PlayerController.isUI = false;
        customUI.SetActive(false);


    }

    #endregion


}
