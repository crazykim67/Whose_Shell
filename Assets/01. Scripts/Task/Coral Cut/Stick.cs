using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stick : MonoBehaviour
{
    [SerializeField]
    private Image stickImage;

    [SerializeField]
    private Vector2 initPos;

    [SerializeField]
    private Rigidbody2D rig;

    private float timer = 0f;

    public bool isCut = false;

    private void Update()
    {
        if(rig.bodyType == RigidbodyType2D.Dynamic)
        {
            if(timer <= 2f)
            {
                timer += Time.deltaTime;
            }
            else
            {
                this.gameObject.SetActive(false);
                rig.bodyType = RigidbodyType2D.Static;
                timer = 0f;
            }
        }
    }

    public void Cut()
    {
        if (rig.bodyType != RigidbodyType2D.Static)
            return;

        rig.bodyType = RigidbodyType2D.Dynamic;
        isCut = true;
    }

    public void OnReset()
    {
        isCut = false;

        rig.bodyType = RigidbodyType2D.Static;
        stickImage.rectTransform.anchoredPosition = initPos;
        timer = 0f; 

        this.gameObject.SetActive(true);
    }
}
