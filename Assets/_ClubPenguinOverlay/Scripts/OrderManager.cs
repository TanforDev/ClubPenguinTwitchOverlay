using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    private PenguinManager penguinManager;
    private SnowballManager snowballManager;

    private void OnEnable()
    {
        penguinManager = GetComponent<PenguinManager>();
        snowballManager = GetComponent<SnowballManager>();
    }

    private void Update()
    {
        List<Entity> entityList = new List<Entity>();

        entityList.AddRange(penguinManager.PenguinList);
        entityList.AddRange(snowballManager.SnowballList);

        // Sort the list based on the Y position using LINQ
        entityList.Sort((a, b) => a.RectTransform.anchoredPosition.y.CompareTo(b.RectTransform.anchoredPosition.y));

        // Set the sorted list as the new hierarchy order
        for (int i = 0; i < entityList.Count; i++)
        {
            entityList[i].RectTransform.SetSiblingIndex(entityList.Count - 1 - i);
        }
    }
}
