using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dust : MonoBehaviour
{
    [SerializeField]
    private Image dustImage;

    public bool isClean = false;

    public void OnReset()
    {
        dustImage.color = new Color(dustImage.color.r, dustImage.color.g, dustImage.color.b, 1f);
        this.gameObject.SetActive(true);
        isClean = false;
    }

    public void OnClean()
    {
        if (dustImage.color.a > 0f)
            dustImage.color = new Color(dustImage.color.r, dustImage.color.g, dustImage.color.b, dustImage.color.a - 0.005f);
        else if (dustImage.color.a <= 0f)
        {
            isClean = true;
            this.gameObject.SetActive(false);
        }
    }
}
