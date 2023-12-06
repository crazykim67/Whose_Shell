using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroductText : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    public void SetAnim(bool _isAct)
    {
        anim.SetBool("isAct", _isAct);
    }
}
