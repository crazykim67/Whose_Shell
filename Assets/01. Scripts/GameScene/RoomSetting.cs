using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomSetting : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI roomCodeText;

    public Button startBtn;
    public TextMeshProUGUI errorText;

    private bool isError;
    private float timer;

    public GameObject optionMenu;
    public Button settingBtn;

    private bool isMenu = false;

    [Header("Player Count UI")]
    public TextMeshProUGUI playerCount;

    private void Awake()
    {
        settingBtn.onClick.AddListener(() => { OnShowSetting(); });
    }

    private void Update()
    {
        if (GameManager.Instance == null)
            return;

        if (isError)
        {
            if(timer <= 3)
                timer += Time.deltaTime;
            else
            {
                timer = 0;
                isError = false;
                errorText.enabled = false;
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isMenu)
                OnOptionMenuShow();
            else
                OnOptionMenuHide();
        }
    }



    public override void OnJoinedRoom()
    {
        MasterCheck();
        UpdatePlayerCount();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        MasterCheck();
    }

    public void MasterCheck()
    {
        if (!PhotonNetwork.InRoom)
            return;

        if (PhotonNetwork.IsMasterClient)
        {
            roomCodeText.text = $"Room Code : {PhotonNetwork.CurrentRoom.Name}";
            roomCodeText.enabled = true;
            startBtn.gameObject.SetActive(true);
            errorText.gameObject.SetActive(true);
        }
        else
        {
            roomCodeText.enabled = false;
            startBtn.gameObject.SetActive(false);
            errorText.gameObject.SetActive(false);
        }
    }

    public void OnStart()
    {
        if (GameManager.Instance == null)
            return;

        if (GameSystem.Instance == null)
            return;

        if (!PhotonNetwork.InRoom)
            return;

        //if(GameManager.Instance.terrapinCount == 1)
        //{
        //    if(PhotonNetwork.CurrentRoom.PlayerCount < 4)
        //    {
        //        OnError("게임 인원이 최소 4명 이상이어야 합니다.");
        //        return;
        //    }
        //}
        //else if(GameManager.Instance.terrapinCount == 2)
        //{
        //    if(PhotonNetwork.CurrentRoom.PlayerCount < 7)
        //    {
        //        OnError("게임 인원이 최소 7명 이상이어야 합니다.");
        //        return;
        //    }
        //}
        //else
        //{
        //    if(PhotonNetwork.CurrentRoom.PlayerCount < 9)
        //    {
        //        OnError("게임 인원이 최소 9명 이상이어야 합니다.");
        //        return;
        //    }
        //}

        GameSystem.Instance.OnStart();
    }

    public void OnError(string str)
    {
        isError = true;
        errorText.text = str;
        errorText.enabled = true;
    }

    #region OptionMenu

    public void OnOptionMenuShow()
    {
        optionMenu.SetActive(true);
        isMenu = true;
    }

    public void OnOptionMenuHide()
    {
        if (OptionManager.Instance == null)
            return;
        if(OptionManager.Instance.emptyImage.activeSelf)
            OnHideSetting();

        optionMenu.SetActive(false);
        isMenu = false;
    }

    public void OnShowSetting()
    {
        if (OptionManager.Instance == null)
            return;

        OptionManager.Instance.OnShow();
    }

    public void OnHideSetting()
    {
        if (OptionManager.Instance == null)
            return;

        OptionManager.Instance.OnHide();
    }

    public void OnExit()
    {
        if (!PhotonNetwork.InRoom)
            return;

        FirebaseManager.Instance.UpdatePlayerCount(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.CurrentRoom.PlayerCount - 1);
        NetworkManager.Instance.LeftRoom();
    }

    #endregion

    #region Player Count Setting

    public void UpdatePlayerCount()
    {
        if (!PhotonNetwork.InRoom)
            return;

        playerCount.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerCount();
        StartCheck();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerCount();
        StartCheck();
    }

    // 플레이어 수에 따라 Start 버튼 활성 / 비활성 화
    public void StartCheck()
    {
        if (!PhotonNetwork.InRoom)
            return;

        if (!PhotonNetwork.IsMasterClient)
            return;

        if (GameManager.Instance == null)
            return;

        //if (GameManager.Instance.terrapinCount == 1)
        //{
        //    if (PhotonNetwork.CurrentRoom.PlayerCount < 4)
        //        startBtn.interactable = false;
        //    else
        //        startBtn.interactable = true;
        //}
        //else if (GameManager.Instance.terrapinCount == 2)
        //{
        //    if (PhotonNetwork.CurrentRoom.PlayerCount < 7)
        //        startBtn.interactable = false;
        //    else
        //        startBtn.interactable = true;
        //}
        //else
        //{
        //    if (PhotonNetwork.CurrentRoom.PlayerCount < 9)
        //        startBtn.interactable = false;
        //    else
        //        startBtn.interactable = true;
        //}
    }

    #endregion
}
