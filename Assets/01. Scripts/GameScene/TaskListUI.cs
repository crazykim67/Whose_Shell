using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TaskListUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private float offest;

    [SerializeField]
    private RectTransform taskListRect;

    private bool isOpen = true;

    [SerializeField]
    private float timer;

    public void OnPointerClick(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(OpenAndHideUI());
    }

    private IEnumerator OpenAndHideUI()
    {
        isOpen = !isOpen;

        if (timer != 0f)
            timer = 1f - timer;

        while (timer <= 1f)
        {
            timer += Time.deltaTime * 2f;

            float start = isOpen ? -taskListRect.sizeDelta.x : offest;
            float dest = isOpen ? offest : -taskListRect.sizeDelta.x;

            taskListRect.anchoredPosition = new Vector2(Mathf.Lerp(start, dest, timer), taskListRect.anchoredPosition.y);

            yield return null;
        }
    }
}
