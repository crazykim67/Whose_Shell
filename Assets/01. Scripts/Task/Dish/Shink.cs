using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shink : MonoBehaviour
{
    [SerializeField]
    private Image shinkImage;

    [SerializeField]
    private Sprite none_Plate;
    [SerializeField]
    private Sprite one_Plate;
    [SerializeField]
    private Sprite two_Plate;
    [SerializeField]
    private Sprite  three_Plate;
    [SerializeField]
    private Sprite four_Plate;
    [SerializeField]
    private Sprite five_Plate;

    public int plateCount = 0;

    public void OnThrow()
    {
        plateCount++;

        switch (plateCount)
        {
            case 1:
                {
                    shinkImage.sprite = one_Plate;
                    break;
                }
            case 2:
                {
                    shinkImage.sprite = two_Plate;
                    break;
                }
            case 3:
                {
                    shinkImage.sprite = three_Plate;
                    break;
                }
            case 4:
                {
                    shinkImage.sprite = four_Plate;
                    break;
                }
            case 5:
                {
                    shinkImage.sprite = five_Plate;
                    break;
                }
        }
    }

    public void OnReset()
    {
        shinkImage.sprite = none_Plate;

        plateCount = 0;
    }
}
