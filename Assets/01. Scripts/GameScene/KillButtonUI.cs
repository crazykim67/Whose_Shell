using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class KillButtonUI : MonoBehaviour
{
    [SerializeField]
    private Button killBtn;

    public void OnShow()
    {
        killBtn.gameObject.SetActive(true);
    }

}
