using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballManager : Singleton<SnowballManager>
{
    [SerializeField] private GameObject snowballPrefab;
    private List<Snowball> pool;
    public List<Snowball> SnowballList => pool;

    private int currentSnowball;

    private void OnEnable()
    {
        pool = new List<Snowball>();
        for (int i = 0; i < ConfigManager.Instance.Config.MaxSnowballCount; i++)
        {
            Snowball snowball = Instantiate(snowballPrefab).GetComponent<Snowball>();
            snowball.RectTransform.SetParent(GameObject.FindGameObjectWithTag("EntityHolder").transform);
            snowball.gameObject.SetActive(false);
            pool.Add(snowball);
        }
    }

    public Snowball GetSnowball()
    {
        Snowball snowball = pool[currentSnowball];
        currentSnowball = (currentSnowball + 1) % pool.Count;
        snowball.gameObject.SetActive(true);
        return snowball;
    }
}
