using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Plate : MonoBehaviour
{
    [SerializeField]
    private Vector2 initTr;

    [SerializeField]
    private Image plateImage;

    public bool isPlate = false;

    public void OnPickUp() => plateImage.raycastTarget = false;

    public void OnThrow(bool _isPlate)
    {
        if (_isPlate)
        {
            this.gameObject.SetActive(false);
            isPlate = true;
        }
        else
        {
            plateImage.raycastTarget = true;
            isPlate = false;
        }

        plateImage.rectTransform.anchoredPosition = initTr;
    }

    public void OnReset()
    {
        plateImage.rectTransform.anchoredPosition = initTr;
        plateImage.raycastTarget = true;
        this.gameObject.SetActive(true);
        isPlate = false;
    }
}
