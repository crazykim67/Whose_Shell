using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class GameRuleItem : MonoBehaviour
{
    [SerializeField]
    private GameObject inactiveObj;

    private void Start() => MasterCheck();

    public void OnEnable()
    {
        MasterCheck();
    }

    public void MasterCheck()
    {
        if (!PhotonNetwork.IsMasterClient)
            inactiveObj.SetActive(false);
        else
            inactiveObj.SetActive(true);
    }
}
