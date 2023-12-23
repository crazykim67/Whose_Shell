using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DummyBtn : MonoBehaviour
{
    public LoginMenuUI loginMenuUI;

    public Button btn1;
    public Button btn2;
    public Button btn3;
    public Button btn4;

    private void Awake()
    {
        btn1.onClick.AddListener(() => { AutoLogin("crazykim97", "crazy6057"); });
        btn2.onClick.AddListener(() => { AutoLogin("qkrakfgo", "crazy6057"); });
        btn3.onClick.AddListener(() => { AutoLogin("rlavlfwnd2", "crazy6057"); });
        btn4.onClick.AddListener(() => { AutoLogin("rlavlfwnd117", "crazy6057"); });
    }

    public void AutoLogin(string _id, string password)
    {
        AuthManager.Instance.OnLogin(_id, loginMenuUI.lg_dropdown.options[loginMenuUI.lg_dropdown.value].text, password,
            loginMenuUI.OnHide, loginMenuUI.mainMenuUI.OnShow);
    }
}
