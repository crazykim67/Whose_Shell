using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour
{
    [SerializeField]
    private Transform left;

    [SerializeField] 
    private Transform right;

    [SerializeField]
    private Transform top;

    [SerializeField]
    private Transform bottom;

    [SerializeField]
    private Image minimapImage;

    [SerializeField]
    private Image playerImage;

    [SerializeField]
    private PlayerController targetPlayer;

    private void Start()
    {
        Material mat = Instantiate(minimapImage.material);
        minimapImage.material = mat;
    }

    public void SetPlayer(PlayerController _player)
    {
        targetPlayer = _player;
    }

    private void Update()
    {
        if (targetPlayer == null)
            return;

        Vector2 mapArea = new Vector2(Vector3.Distance(left.position, right.position), Vector3.Distance(bottom.position, top.position));
        Vector2 charPos = new Vector2(Vector3.Distance(left.position, new Vector3(targetPlayer.transform.position.x, 0f, 0f)),
            Vector3.Distance(bottom.position, new Vector3(0f, targetPlayer.transform.position.y, 0f)));

        Vector2 normalPos = new Vector2(charPos.x / mapArea.x ,charPos.y / mapArea.y);

        playerImage.rectTransform.anchoredPosition = new Vector2((minimapImage.rectTransform.sizeDelta.x * normalPos.x),
             (minimapImage.rectTransform.sizeDelta.y * normalPos.y));
    }

    #region Show & Hide

    public void OnShow()
    {
        gameObject.SetActive(true);
    }

    public void OnHide()
    {
        gameObject.SetActive(false);
    }

    #endregion
}
