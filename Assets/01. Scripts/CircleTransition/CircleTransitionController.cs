using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleTransitionController : MonoBehaviour
{
    #region Instance

    private static CircleTransitionController instance;

    public static CircleTransitionController Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new CircleTransitionController();
                return instance;
            }

            return instance;
        }
    }

    #endregion

    private Canvas canvas;

    [SerializeField]
    private Image blackScreen;

    [SerializeField]
    private float speed = 1f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        canvas = GetComponent<Canvas>();

        // 처음부터 실행
        blackScreen.material.SetFloat("_Radius", 0);
        OpenBlackScreen(false);
    }

    private void Start() => DrawCircleScreen();

    public void Update()
    {
        // Test
        if (Input.GetKeyDown(KeyCode.Alpha1))
            OpenBlackScreen();
        else if(Input.GetKeyDown(KeyCode.Alpha2)) 
            CloseBlackScreen();
    }

    public void OpenBlackScreen(bool _change = false)
    {
        StartCoroutine(Transition(2, 0, 1, _change));
    }

    public void CloseBlackScreen(Action act_1 = null, Action act_2 = null) 
    {
        if(act_1 == null && act_2 == null)
            StartCoroutine(Transition(2, 1, 0));
        else
            StartCoroutine(Transition(2, 1, 0, true, act_1, act_2));
    }

    public void DrawCircleScreen()
    {
        var canvasRect = canvas.GetComponent<RectTransform>().rect;
        var canvasWidth = canvasRect.width;
        var canvasHeight = canvasRect.height;

        var squareValue = 0f;

        if(canvasWidth > canvasHeight)
        // 세로
            squareValue = canvasWidth;
        // 가로
        else
            squareValue = canvasHeight;

        // 이미지 크기 조정
        blackScreen.rectTransform.sizeDelta = new Vector2(squareValue, squareValue);
    }

    private IEnumerator Transition(float duration, float beginRadius, float endRadius, bool _change = true, Action _act_1 = null, Action _act_2 = null)
    {
        var mat = blackScreen.material;
        float time = 0;
        while (time <= duration)
        {
            time += Time.deltaTime;
            var t = (time * speed) / duration;
            var radius = Mathf.Lerp(beginRadius, endRadius, t);

            mat.SetFloat("_Radius", radius);

            yield return null;
        }

        if (_act_1 != null)
            _act_1.Invoke();

        if (_act_2 != null)
            _act_2.Invoke();

        if (_change)
            OpenBlackScreen(false);
    }

}
