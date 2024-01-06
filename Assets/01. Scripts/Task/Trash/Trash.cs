using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trash : MonoBehaviour
{
    [SerializeField]
    private Vector2 initTr;

    [SerializeField]
    private Image trashImage;

    [SerializeField]
    private Sprite trashSprite;

    [SerializeField]
    private Sprite trashPickUp;

    public bool isThrow = false;

    public void OnPickUp()
    {
        trashImage.sprite = trashPickUp;
        trashImage.raycastTarget = false;
    }

    public void OnThrow(bool isTrash)
    {
        if (isTrash)
        {
            this.gameObject.SetActive(false);
            isThrow = true;
        }
        else
        {
            trashImage.raycastTarget = true;
            isThrow = false;
        }

        trashImage.rectTransform.anchoredPosition = initTr;
        trashImage.sprite = trashSprite;
    }

    public void OnReset()
    {
        trashImage.rectTransform.anchoredPosition = initTr;
        trashImage.sprite = trashSprite;
        trashImage.raycastTarget = true;
        this.gameObject.SetActive(true);
        isThrow = false;
    }
}
