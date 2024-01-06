using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashCan : MonoBehaviour
{
    [SerializeField]
    private Image trashCan;

    [SerializeField]
    private Sprite empty;

    [SerializeField]
    private Sprite little;

    [SerializeField]
    private Sprite medium;

    [SerializeField]
    private Sprite full;

    public int trashCount = 0;

    public void OnReset()
    {
        trashCan.sprite = empty;
        trashCount = 0;
    }

    public void OnThrow()
    {
        trashCount++;

        if (trashCount == 1)
            trashCan.sprite = little;
        else if(trashCount == 2)
            trashCan.sprite = medium;
        else
            trashCan.sprite = full;
    }
}
