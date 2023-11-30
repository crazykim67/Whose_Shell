using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    #region Instance

    private static FadeController instance;

    public static FadeController Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new FadeController();
                return instance;
            }

            return instance;
        }
    }

    #endregion

    [SerializeField]
    private RawImage bg;

    [SerializeField]
    private float alphaValue = 0f;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            OnFadeIn(Test);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            OnFadeOut();
    }

    public void OnFadeIn(Action act = null)
    {
        if(act != null)
            StartCoroutine(FadeIn(act, 1f));
        else
            StartCoroutine(FadeIn());
    }

    public void OnFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeIn(Action act = null, float fadeSpeed = 0.8f)
    {
        bg.gameObject.SetActive(true);

        while (alphaValue <= 1)
        {
            alphaValue += Time.deltaTime * fadeSpeed;
            bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, alphaValue);
            yield return null;
        }

        alphaValue = 1f;
        if (act != null)
            act.Invoke();
    }

    public IEnumerator FadeOut(float fadeSpeed = 0.8f) 
    {
        bg.gameObject.SetActive(true);

        while (alphaValue > 0)
        {
            alphaValue -= Time.deltaTime * fadeSpeed;
            bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, alphaValue);
            yield return null;
        }

        bg.gameObject.SetActive(false);
        alphaValue = 0f;
    }

    public void Test()
    {
        Debug.Log("테스트");
    }
}
