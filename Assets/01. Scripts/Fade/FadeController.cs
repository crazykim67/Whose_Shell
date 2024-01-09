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

    public Coroutine runningCorouine;

    private void Awake()
    {
        instance = this;
    }

    public void OnFadeIn(float _speed = 0.8f, Action act = null)
    {
        if(act != null)
            StartCoroutine(FadeIn(act, 1f));
        else
            StartCoroutine(FadeIn(null, _speed));
    }

    public void OnFadeOut(float _speed)
    {
        StartCoroutine(FadeOut(_speed));
    }

    public IEnumerator FadeIn(Action act = null, float fadeSpeed = 0.8f)
    {
        alphaValue = 0f;
        bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, alphaValue);
        bg.gameObject.SetActive(true);

        while (alphaValue <= 1)
        {
            alphaValue += Time.deltaTime * fadeSpeed;
            bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, alphaValue);
            yield return null;
        }

        alphaValue = 1f;
        if (act != null)
        {
            act.Invoke();
            OnFadeOut(0.5f);
        }
    }

    public IEnumerator FadeOut(float fadeSpeed = 0.8f) 
    {
        alphaValue = 1f;
        bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, alphaValue);
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

}
