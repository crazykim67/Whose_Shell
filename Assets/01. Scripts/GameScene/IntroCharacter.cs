using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class IntroCharacter : MonoBehaviour
{
    [SerializeField]
    private Image character;

    [SerializeField]
    private TextMeshProUGUI nickNameText;

    [SerializeField]
    private Material mat;

    public void SetIntroCharacter(string nickName, float hue)
    {
        Material _mat = new Material(mat);
        character.material = _mat;

        nickNameText.text = nickName;
        character.material.SetFloat("_Hue", hue);
    }
}
