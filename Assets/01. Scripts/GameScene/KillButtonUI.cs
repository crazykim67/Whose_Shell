using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class KillButtonUI : MonoBehaviour
{
    [SerializeField]
    private Button killBtn;

    [SerializeField]
    private TextMeshProUGUI cooldownText;

    [SerializeField]
    private PlayerController targetPlayer;

    private void Update()
    {
        if (targetPlayer == null)
            return;

        if (!targetPlayer.isKill)
        {
            cooldownText.text = targetPlayer.KillCooldown > 0 ? ((int)targetPlayer.KillCooldown).ToString() : "";
            killBtn.interactable = false;
        }
        else
        {
            cooldownText.text = "";
            killBtn.interactable = true;
        }
    }

    public void OnShow(PlayerController player)
    {
        killBtn.gameObject.SetActive(true);
        targetPlayer = player;
    }

    public void OnHide()
    {
        killBtn.gameObject.SetActive(false);
    }

    public void OnKill()
    {
        targetPlayer.Kill();
    }
}
