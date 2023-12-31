using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class EjectionUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ejectionResultText;

    [SerializeField]
    private Image ejectionPlayer;

    [SerializeField]
    private RectTransform top;
    [SerializeField]
    private RectTransform bottom;

    private void Start()
    {
        ejectionPlayer.material = Instantiate(ejectionPlayer.material);
    }

    public void OnShow(bool isEjection, float _hue, bool isTerrapin, int remainTerrapinCount)
    {
        if (GameSystem.Instance == null)
            return;

        string str = "";

        PlayerController ejectPlayer = null;

        if (isEjection)
        {
            foreach(var player in GameSystem.Instance.controllerList)
            {
                if(player.playerColor == _hue)
                {
                    ejectPlayer = player;
                    break;
                }
            }
            str = string.Format("{0}���� �ڶ�{1}\n�ڶ� {2}���� ���ҽ��ϴ�.", ejectPlayer.nickName, isTerrapin ? "�����ϴ�." : "�� �ƴϾ����ϴ�.", remainTerrapinCount);
        }
        else
        {
            str = string.Format("�ƹ��ϵ� �Ͼ�� �ʾҽ��ϴ�.\n�ڶ� {0}���� ���ҽ��ϴ�.", remainTerrapinCount);
        }

        gameObject.SetActive(true);
        StartCoroutine(ShowEjectionResult_Coroutine(ejectPlayer, str));
    }

    private IEnumerator ShowEjectionResult_Coroutine(PlayerController ejectPlayer, string str)
    {
        ejectionResultText.text = "";

        string forwardText = "";
        string backText = "";

        if(ejectPlayer != null)
        {
            ejectionPlayer.material.SetFloat("_Hue", ejectPlayer.playerColor);

            float timer = 0f;

            while(timer <= 1)
            {
                yield return null;
                timer += Time.deltaTime * 0.5f;

                ejectionPlayer.rectTransform.anchoredPosition = Vector2.Lerp(top.anchoredPosition, bottom.anchoredPosition, timer);
            }
        }

        backText = str;

        while(backText.Length != 0)
        {
            forwardText += backText[0];
            backText = backText.Remove(0, 1);
            ejectionResultText.text = string.Format("<color=#FFFFFF>{0}</color><color=#393957>{1}</color>", forwardText, backText);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void OnHide()
    {
        gameObject.SetActive(false);
    }
}
