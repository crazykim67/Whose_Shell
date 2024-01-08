using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Brush : MonoBehaviour
{
    [SerializeField]
    private GraphicRaycaster gr;

    [SerializeField]
    private RectTransform brushRect;

    [SerializeField]
    private SpiderWeb currentSpiderWeb;

    [SerializeField]
    private SpiderWebTask spiderWebTask;

    private void Update()
    {
        OnCleaning();
        BrushUpdate();
    }

    public void OnShow() => Cursor.visible = false;

    public void OnHide() => Cursor.visible = true;

    public void OnCleaning()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Raycast();

            if(currentSpiderWeb != null && !currentSpiderWeb.isClean && !currentSpiderWeb.isActive)
            {
                currentSpiderWeb.OnClean();
                spiderWebTask.OnCheck();
            }
        }
        else if(Input.GetMouseButtonUp(0)) 
        {
            if (currentSpiderWeb != null)
                currentSpiderWeb = null;
        }
    }

    private void BrushUpdate()
    {
        Vector2 mousePos = Input.mousePosition;
        brushRect.position = mousePos;
    }

    private void Raycast() 
    {
        if (currentSpiderWeb != null)
            return;

        var ped = new PointerEventData(null);

        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);

        if (results.Count <= 0)
            return;

        if (results[0].gameObject.tag != "SpiderWeb")
        {
            if (currentSpiderWeb != null)
                currentSpiderWeb = null;

            return;
        }

        currentSpiderWeb = results[0].gameObject.GetComponent<SpiderWeb>();
    }
}
