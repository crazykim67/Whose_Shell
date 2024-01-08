using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiderWeb : MonoBehaviour
{
    [SerializeField]
    private int dirtyCount = 3;

    public bool isClean = false;

    public bool isActive = false;

    [SerializeField]
    private float timer = 0f;

    [SerializeField]
    private Image spiderWebImage;

    [SerializeField]
    private Sprite normalWeb;

    [SerializeField]
    private Sprite littleWeb;

    [SerializeField]
    private Sprite emptyWeb;

    private void Update()
    {
        // 브러쉬로 거미줄 청소할 수 있는 쿨타임 = 1초
        if (isActive)
        {
            if(timer <= 1f)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
                isActive = false;
            }
        }
    }

    public void OnReset()
    {
        spiderWebImage.sprite = normalWeb;

        if (!spiderWebImage.enabled)
            spiderWebImage.enabled = true;

        if (!this.gameObject.activeSelf)
            this.gameObject.SetActive(true);

        dirtyCount = 3;
        isClean = false;
        isActive = false;
    }

    public void OnClean()
    {
        if (dirtyCount > 0)
            dirtyCount--;

        isActive = true;

        switch (dirtyCount)
        {
            case 2:
                {
                    spiderWebImage.sprite = littleWeb;
                    Debug.Log("한 번 지움");

                    break;
                }
            case 1:
                {
                    spiderWebImage.sprite = emptyWeb;
                    Debug.Log("두 번 지움");
                    break;
                }
            case 0:
                {
                    spiderWebImage.enabled = false;
                    this.gameObject.SetActive(false);
                    Debug.Log("완료");
                    isClean = true;
                    break;
                }
        }
    }
}
