using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFinder : MonoBehaviour
{
    private CircleCollider2D coll;

    public List<PlayerController> players = new List<PlayerController>();

    public void SetKillRange(float range)
    {
        coll.radius = range;
    }

    private void Awake()
    {
        coll = GetComponent<CircleCollider2D>();
    }

    #region TrrigerEnter / Exit

    private void OnTriggerEnter2D(Collider2D coll)
    {
        var player = coll.GetComponent<PlayerController>();

        if(player != null && player.playerType == PlayerType.Turtle)
        {
            if (!players.Contains(player))
                players.Add(player);
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        var player = coll.GetComponent<PlayerController>();

        if(player != null && player.playerType == PlayerType.Turtle)
        {
            if(players.Contains(player))
                players.Remove(player);
        }
    }

    #endregion

    // 제일 근접한 플레이어 탐색
    public PlayerController GetNearTarget()
    {
        float dist = float.MaxValue;
        PlayerController nearPlayer = null;

        foreach (var player in players)
        {
            float newDist = Vector3.Distance(transform.position, player.transform.position);

            if(newDist < dist)
            {
                dist = newDist;
                nearPlayer = player;
            }
        }

        players.Remove(nearPlayer);
        return nearPlayer;
    }
}
