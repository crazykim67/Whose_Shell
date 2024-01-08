using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class OuttroUIManager : MonoBehaviour
{
    #region Instance

    private static OuttroUIManager instance;

    public static OuttroUIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new OuttroUIManager();
                return instance;
            }

            return instance;
        }
    }

    #endregion

    [SerializeField]
    private PhotonView pv;

    [SerializeField]
    private OuttroUIController controller;
    public OuttroUIController Controller { get { return controller; } }

    private void Awake()
    {
        instance = this;
        pv = GetComponent<PhotonView>();
    }
}
