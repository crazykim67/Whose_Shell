using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    #region Instance

    private static InGameUIManager instance;

    public static InGameUIManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new InGameUIManager();
                return instance;
            }

            return instance;
        }
    }

    #endregion

    [SerializeField]
    private GameObject uiObject;

    [SerializeField]
    private MinimapManager minimap;

    private void Awake() => instance = this;

    public void OnSet(PlayerController player)
    {
        minimap.SetPlayer(player);
        Debug.Log("Width : " + Screen.width + "Height : " + Screen.height);
    }

    public void StartGameSetting()
    {
        this.uiObject.SetActive(true);
    }

    public void EndGameSetting()
    {
        this.uiObject.SetActive(false);
    }
}
