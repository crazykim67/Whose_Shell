using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DishTask : TaskScript
{
    [SerializeField]
    private GraphicRaycaster gr;

    [SerializeField]
    private List<Plate> plates = new List<Plate>();

    [SerializeField]
    private Plate currentPlate;

    public GameObject water;

    [SerializeField]
    private Shink shink;

    private void Update()
    {
        PlateMove();

        if (Input.GetMouseButtonDown(0))
        {
            Raycast();
            OnWaterTap();
        }
        else if (Input.GetMouseButtonUp(0))
            PickOff();
    }

    public override void Init()
    {
        base.Init();

        foreach (var plate in plates)
            plate.OnReset();

        shink.OnReset();
    }

    public override void OnShow()
    {
        base.OnShow();
    }

    public override void OnHide()
    {
        base.OnHide();
    }

    public IEnumerator OnSuccess()
    {
        myCharacter.isUI = false;
        closeBtn.interactable = false;

        if (myCharacter.taskObject != null)
            myCharacter.taskObject = null;

        textAnim.gameObject.SetActive(true);
        textAnim.SetBool("isSuccess", true);
        curTaskText.OnTaskCheck(true);
        isSuccess = true;

        if (InGameUIManager.Instance != null)
        {
            InGameUIManager.Instance.OnSuccess();
            InGameUIManager.Instance.TaskTypeCheck((int)taskType);
        }

        yield return new WaitForSeconds(3f);

        OnHide();
        closeBtn.interactable = true;
    }

    public void PlateMove()
    {
        if (currentPlate == null)
            return;

        currentPlate.transform.position = Input.mousePosition;
    }

    private void Raycast()
    {
        var ped = new PointerEventData(null);

        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);

        if (results.Count <= 0)
            return;

        if (results[0].gameObject.tag != "Plate")
            return;

        currentPlate = results[0].gameObject.GetComponent<Plate>();

        currentPlate.OnPickUp();
    }

    private void PickOff()
    {
        if (currentPlate == null)
            return;

        var ped = new PointerEventData(null);

        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);

        if (results.Count <= 0)
            return;

        if (results[0].gameObject.tag != "Shink")
            currentPlate.OnThrow(false);
        else
        {
            currentPlate.OnThrow(true);
            shink.OnThrow();
        }

        currentPlate = null;
    }

    public void OnWaterTap()
    {
        if(shink.plateCount == 5)
        {
            var ped = new PointerEventData(null);

            ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(ped, results);

            if (results.Count <= 0)
                return;

            if (results[0].gameObject.tag == "WaterTap")
            {
                water.SetActive(true);
                StartCoroutine(OnSuccess());
            }
        }
    }
}
