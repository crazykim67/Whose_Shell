using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MapManager : MonoBehaviour
{
#region Instance

    private static MapManager instance;

    public static MapManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new MapManager();
                return instance;
            }

            return instance;
        }
    }

    #endregion

    private PhotonView pv;

    [Header("Lobby")]
    public GameObject lobbyShip;
    public SpriteSorter lobbySorter;

    [Header("InGame")]
    public GameObject inGameShip;
    public SpriteSorter inGameSorter;

    [Header("Spawn Area")]
    public List<Transform> spawnLobbyList = new List<Transform>();
    public List<Transform> spawnInGameList = new List<Transform>();

    [SerializeField]
    private TaskController taskController;
    public TaskController TaskController { get { return taskController; }  }

    private void Awake()
    {
        instance = this;
        pv = GetComponent<PhotonView>();
    }

    public void MapIndex(int index)
    {
        if (GameSystem.Instance == null)
            return;

        OnMap(index);
        StopAllCoroutines();
    }

    public void OnMap(int index)
    {
        if (GameSystem.Instance == null)
            return;

        List<PlayerController> playerList = GameSystem.Instance.controllerList;

        switch (index)
        {
            case 0:
                {
                    lobbyShip.SetActive(true);
                    inGameShip.SetActive(false);

                    foreach(var player in playerList)
                        player.sorting.SetSorter(lobbySorter);

                    SetTask(false);

                    for (int i = 0; i < playerList.Count; i++)
                    {
                        Transform spawnPos = spawnLobbyList[i];
                        pv.RPC("SetPosition", RpcTarget.All, i, spawnPos.position.x, spawnPos.position.y, spawnPos.position.z);
                    }
                    break;
                }
            case 1:
                {
                    lobbyShip.SetActive(false);
                    inGameShip.SetActive(true);

                    foreach (var player in playerList)
                        player.sorting.SetSorter(inGameSorter);

                    for (int i = 0; i < playerList.Count; i++)
                    {
                        Transform spawnPos = spawnInGameList[i];
                        pv.RPC("SetPosition", RpcTarget.All, i, spawnPos.position.x, spawnPos.position.y, spawnPos.position.z);
                    }
                    break;
                }
        }
    }

    [PunRPC]
    public void SetPosition(int index, float x, float y, float z)
    {
        List<PlayerController> playerList = GameSystem.Instance.controllerList;

        playerList[index].transform.position = new Vector3(x, y, z);
    }

    public void SetRpcPosition(int index, float x, float y, float z)
    {
        pv.RPC("SetPosition", RpcTarget.All, index, x, y, z);
    }

    public Transform[] SpawnArray()
    {
        Transform[] array = spawnInGameList.ToArray();

        return array;
    }

    public void SetTask(bool isInit)
    {
        if(isInit)
        {
            int randomCommon = Random.Range(0, TaskController.commonTasks.Count);
            int randomSimple = Random.Range(0, TaskController.simpleTasks.Count);

            pv.RPC("SetRpcTask", RpcTarget.All, randomCommon, randomSimple);
        }
        else
            pv.RPC("InitRpcTask", RpcTarget.All);
    }

    [PunRPC]
    public void SetRpcTask(int randomCommon, int randomSimple)
    {
        TaskController.SetTask(randomCommon, randomSimple);
    }

    [PunRPC]
    public void InitRpcTask()
    {
        TaskController.SetCommonTask(2, 0, true);
        TaskController.SetSimpleTask(3, 0, true);
    }
}
