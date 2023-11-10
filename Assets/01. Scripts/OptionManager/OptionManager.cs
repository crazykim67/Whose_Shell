using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionManager : MonoBehaviour
{
    #region Instance

    private static OptionManager instance;

    public static OptionManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new OptionManager();
                return instance;
            }

            return instance;
        }
    }

    #endregion

    public GameObject optionMenu;
    public GameObject emptyImage;

    public Button confirmBtn;

    private Animator anim;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        anim = GetComponent<Animator>();

        confirmBtn.onClick.AddListener(() => { OnHide(); });
    }

    #region Show & Hide

    public void OnShow()
    {
        anim.SetTrigger("Open");
        emptyImage.SetActive(true);
    }

    public void OnHide()
    {
        anim.SetTrigger("Close");
        emptyImage.SetActive(false);
    }

    #endregion
}
