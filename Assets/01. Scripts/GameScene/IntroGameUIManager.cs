using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class IntroGameUIManager : MonoBehaviour
{
    #region Instacne

    private static IntroGameUIManager instance;

    public static IntroGameUIManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new IntroGameUIManager();
                return instance;
            }
            return instance;
        }
    }

    #endregion

    private PhotonView pv;

    [SerializeField]
    private IntroUIController controller;

    public IntroUIController Controller { get { return controller; } }

    public void Awake()
    {
        instance = this;
        pv = GetComponent<PhotonView>();
    }


}
