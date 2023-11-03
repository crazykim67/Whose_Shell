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

    public void OpenBlackScreen()
    {
        StartCoroutine(Transition(2, 0, 1));
    }

    public void CloseBlackScreen() 
    {
        StartCoroutine(Transition(2, 1, 0));
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

    private IEnumerator Transition(float duration, float beginRadius, float endRadius)
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
    }

}
