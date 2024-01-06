using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public class Scissors : MonoBehaviour
{
    [SerializeField]
    private GraphicRaycaster gr;

    [SerializeField]
    private Image scissors;

    [SerializeField]
    private RectTransform scissorsRect;

    [SerializeField]
    private Sprite nonCut;
    [SerializeField]
    private Sprite cut;

    [SerializeField]
    private Stick currentStick;

    [SerializeField]
    private CoralCut coral;

    private void Update()
    {
        OnCut();
        ScissorsUpdate();
    }

    public void OnShow()
    {
        Cursor.visible = false;
    }

    public void OnHide()
    {
        Cursor.visible = true;
    }

    private void OnCut()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Raycast();

            if (currentStick != null && !currentStick.isCut)
            {
                scissors.sprite = cut;
                currentStick.Cut();
                coral.OnCheck();
            }
        }
        else if (Input.GetMouseButtonUp(0))
            scissors.sprite = nonCut;
    }

    private void ScissorsUpdate()
    {
        Vector2 mousePos = Input.mousePosition;
        scissorsRect.position = mousePos;
    }

    private void Raycast()
    {
        var ped = new PointerEventData(null);

        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);

        if (results.Count <= 0)
            return;

        if (results[0].gameObject.tag != "Stick")
            return;

        currentStick = results[0].gameObject.GetComponent<Stick>();
    }
}
