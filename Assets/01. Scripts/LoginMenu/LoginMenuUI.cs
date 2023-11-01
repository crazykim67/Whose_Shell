using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginMenuUI : MonoBehaviour
{

    [Header("Login Menu")]
    public GameObject loginMenu;
    public TMP_InputField lg_IdInputField;
    public TMP_InputField lg_PassInputField;
    public TMP_Dropdown lg_dropdown;
    public Button lg_loginBtn;
    public Button lg_registerBtn;
    public Button exitBtn;

    [Header("Register Menu")]
    public GameObject registerMenu;
    public TMP_InputField rg_IdInputField;
    public TMP_InputField rg_PassInputField;
    public TMP_InputField rg_PassCheckInputField;
    public TMP_Dropdown rg_dropdown;
    public Button rg_registerBtn;
    public Button rg_backBtn;

    private void Awake()
    {
        lg_loginBtn.onClick.AddListener(OnLogin);
        lg_registerBtn.onClick.AddListener(OnRegisterMenu);
        rg_backBtn.onClick.AddListener(OnLoginMenu);
        rg_registerBtn.onClick.AddListener(OnRegist);
    }

    // 로그인 버튼
    public void OnLogin()
    {
        if (lg_IdInputField.Equals(string.Empty))
        {
            Debug.Log("ID Input Field is Empty...!");
            return;
        }
        else if (lg_IdInputField.text.Equals(string.Empty))
        {
            Debug.Log("Password Input Field is Empty...!");
            return;
        }

        // 파이어베이스 로그인
        AuthManager.Instance.OnLogin(lg_IdInputField.text, lg_dropdown.options[lg_dropdown.value].text, lg_PassInputField.text);
    }

    // 회원가입 완료 버튼
    public void OnRegist()
    {
        // 아이디 입력칸 비어있음
        if (rg_IdInputField.text.Equals(string.Empty))
        {
            Debug.Log("ID Input Field is Empty...!");
            return;
        }
        // 아이디 입력 값 8자 미만 15자 초과했음
        else if (rg_IdInputField.text.Length < 8 || rg_IdInputField.text.Length > 15)
        {
            Debug.Log("ID Must at least 8 char and at most 15 char...!");
            return;
        }
        // 비밀번호 입력칸 비어있음
        else if (rg_PassInputField.Equals(string.Empty))
        {
            Debug.Log("Password Input Field is Empty...!");
            return;
        }
        // 비밀번호 입력 값 8자 미만 15자 초과했음
        else if (rg_PassInputField.text.Length < 8 || rg_PassInputField.text.Length > 15)
        {
            Debug.Log("Password Must at least 8 char and at most 15 char...!");
            return;
        }
        // 비밀번호 확인 입력칸 비어있음
        else if (rg_PassCheckInputField.text.Equals(string.Empty))
        {
            Debug.Log("Password Check Input Field is Empty...!");
            return;
        }
        // 비밀번호 확인 입력 값이 비밀번호 입력 값과 일치하지 않음
        else if (!rg_PassCheckInputField.text.Equals(rg_PassInputField.text))
        {
            Debug.Log("Passsword Input Field is Different from PasswordCheck Input Field");
            return;
        }

        AuthManager.Instance.OnRegister($"{rg_IdInputField.text}@{rg_dropdown.options[rg_dropdown.value].text}", rg_PassInputField.text);
        OnLoginMenu();
    }

    // 로그인화면 세팅
    public void OnLoginMenu()
    {
        loginMenu.SetActive(true);
        lg_IdInputField.text = "";
        lg_PassInputField.text = "";
        lg_dropdown.value = 0;

        registerMenu.SetActive(false);
    }

    // 회원가입화면 세팅
    public void OnRegisterMenu()
    {
        loginMenu.SetActive(false);

        registerMenu.SetActive(true);
        rg_IdInputField.text = "";
        rg_PassInputField.text = "";
        rg_PassCheckInputField.text = "";
        rg_dropdown.value = 0;
    }
}
