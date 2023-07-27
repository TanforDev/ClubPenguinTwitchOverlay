using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class PenguinManager : MonoBehaviour
{
    [SerializeField] private GameObject penguinPrefab;

    private Transform entityHolder;

    private Dictionary<string, Penguin> penguinDic;

    public List<Penguin> PenguinList
    {
        get
        {
            List<Penguin> penguinList = new List<Penguin>(penguinDic.Values);
            return penguinList;
        }
    }

    private TwitchConnect twitchConnect;


    //Testing
    [SerializeField] private bool generateFakePenguins = false;
    [SerializeField] private int fakePenguinsCount = 10;

    private void Awake()
    {
        SetInitialReferences();
        twitchConnect.EventPlayerRecieved += PlayerRecieved;
    }

    private void OnDisable()
    {
        twitchConnect.EventPlayerRecieved -= PlayerRecieved;
    }

    private void SetInitialReferences()
    {
        twitchConnect = GetComponent<TwitchConnect>();
        penguinDic = new Dictionary<string, Penguin>();

        entityHolder = GameObject.FindGameObjectWithTag("EntityHolder").transform;

        //Testing
        if (generateFakePenguins && Application.isEditor)
        {
            for (int i = 0; i < fakePenguinsCount; i++)
            {
                float r = Random.Range(0.0f, 1.0f);
                float g = Random.Range(0.0f, 1.0f);
                float b = Random.Range(0.0f, 1.0f);
                Color color = new Color(r, g, b);
                bool isSub = Random.Range(0, 2) == 0 ? false : true;
                bool isMod = Random.Range(0, 2) == 0 ? false : true;
                PlayerData fakeData = new PlayerData("player" + i, color, isSub, isMod, "!afk", "");

                PlayerRecieved(fakeData);
            }
        }
    }

    private void PlayerRecieved(PlayerData incommingData)
    {
        string key = incommingData.Name.ToLower();
        if (!penguinDic.ContainsKey(key))
        {
            if (penguinDic.Count >= ConfigManager.Instance.Config.MaxPenguinCount) return;

            if (ConfigManager.Instance.Config.JoinCommandToJoin == true)
            {
                if(incommingData.Message.ToLower() == "!join")
                {
                    penguinDic.Add(key, CreateNewPenguin());
                    penguinDic[key].UpdateData(incommingData);
                }
            }
            else
            {
                penguinDic.Add(key, CreateNewPenguin());
                penguinDic[key].UpdateData(incommingData);
            }
        }
        else
        {
            penguinDic[key].UpdateData(incommingData);
        }
    }

    private Penguin CreateNewPenguin()
    {
        Penguin newPenguin = Instantiate(penguinPrefab, entityHolder).GetComponent<Penguin>();
        return newPenguin;
    }

    public void RemovePenguin(string name)
    {
        if (penguinDic.ContainsKey(name))
        {
            penguinDic.Remove(name.ToLower());
        }
    }

    public Penguin GetPenguin(string name)
    {
        if (penguinDic.ContainsKey(name))
        {
            return penguinDic[name];
        }
        return null;
    }
    public Penguin GetRandomPenguin()
    {
        return penguinDic.Values.ElementAt(Random.Range(0, penguinDic.Count));
    }

}
