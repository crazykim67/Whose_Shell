using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    public Button colorBtn;

    public float hue;

    public void SetInteractable(bool isInter)
    {
        colorBtn.interactable = isInter;
    }

}
