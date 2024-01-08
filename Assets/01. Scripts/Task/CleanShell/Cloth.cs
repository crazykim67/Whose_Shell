using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cloth : MonoBehaviour
{
    [SerializeField]
    private GraphicRaycaster gr;

    [SerializeField]
    private RectTransform clothRect;

    [SerializeField]
    private Dust currentDust;

    [SerializeField]
    private CleanShellTask cleanShellTask;

    private bool isShake;
    private Vector2 oldMouseAxis;

    private void Update()
    {
        OnCleaning();
        ClothUpdate();
    }

    public void OnShow() => Cursor.visible = false;

    public void OnHide() => Cursor.visible = true;

    public void OnCleaning()
    {
        if (Input.GetMouseButton(0))
        {
            Raycast();

            if (currentDust != null && !currentDust.isClean)
            {
                Vector2 mouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                isShake = Mathf.Sign(mouseAxis.x) != Mathf.Sign(oldMouseAxis.x) ||
                    Mathf.Sign(mouseAxis.y) != Mathf.Sign(oldMouseAxis.y);

                if (isShake)
                {
                    currentDust.OnClean();
                    cleanShellTask.OnCheck();
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if(currentDust != null)
                currentDust = null;

            isShake = false;
            oldMouseAxis = Vector2.zero;
        }
    }

    private void ClothUpdate()
    {
        Vector2 mousePos = Input.mousePosition;
        clothRect.position = mousePos;
    }

    private void Raycast()
    {
        if (currentDust != null)
            return;

        var ped = new PointerEventData(null);

        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);

        if (results.Count <= 0)
            return;

        if (results[0].gameObject.tag != "Dust")
            return;

        currentDust = results[0].gameObject.GetComponent<Dust>();
    }
}
