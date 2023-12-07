using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingSprite : MonoBehaviour
{
    public enum ESortingType
    {
        Static, Update
    }

    [SerializeField]
    private ESortingType type;

    private SpriteSorter sorter;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        sorter = FindAnyObjectByType<SpriteSorter>();

        if(type == ESortingType.Static)
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sortingOrder = sorter.GetSortingOrder(gameObject);
    }

    private void Update()
    {
        if (sorter == null)
            return;

        if(type == ESortingType.Update) 
            spriteRenderer.sortingOrder = sorter.GetSortingOrder(gameObject);
    }

    public void SetSorter(SpriteSorter _sorter)
    {
        this.sorter = _sorter;

        if (type == ESortingType.Static)
            spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sortingOrder = sorter.GetSortingOrder(gameObject);
    }
}
