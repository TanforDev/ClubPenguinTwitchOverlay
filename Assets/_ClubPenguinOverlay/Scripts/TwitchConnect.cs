using UnityEngine;
using System.Net.Sockets;
using System.IO;
using SimpleJSON;
using UnityEngine.Windows;

public class TwitchConnect : MonoBehaviour
{
    private TcpClient twitch;
    private StreamReader reader;
    private StreamWriter writer;

    const string URL = "irc.chat.twitch.tv";
    const int PORT = 6667;

    private float pingCounter;

    public delegate void TwitchConnectEventHandler(PlayerData player);
    public event TwitchConnectEventHandler EventPlayerRecieved;

    private void Awake()
    {
        ConnectToTwitch();
    }

    private void ConnectToTwitch()
    {
        twitch = new TcpClient(URL, PORT);
        reader = new StreamReader(twitch.GetStream());
        writer = new StreamWriter(twitch.GetStream());

        writer.WriteLine("PASS " + ConfigManager.Instance.Config.OAuth);
        writer.WriteLine("NICK " + ConfigManager.Instance.Config.User);
        writer.WriteLine("JOIN #" + ConfigManager.Instance.Config.Channel.ToLower());
        writer.WriteLine("CAP REQ :twitch.tv/tags");
        writer.WriteLine("CAP REQ :twitch.tv/commands");
        writer.WriteLine("CAP REQ :twitch.tv/membership");
        writer.Flush();
    }

    private void Update()
    {
        pingCounter += Time.deltaTime;
        if (pingCounter > 60)
        {
            writer.WriteLine("PING " + URL);
            writer.Flush();
            pingCounter = 0;
        }

        if (!twitch.Connected)
        {
            Debug.Log("ConnectToTwitch");
            ConnectToTwitch();
        }

        if (twitch.Available > 0)
        {
            string message = reader.ReadLine();

            if (message.Contains("PRIVMS"))
            {
                // Split the string using ';' as the delimiter
                string[] keyValuePairs = message.Split(';');

                // Create a new JSON object
                JSONObject json = new JSONObject();

                // Process each key-value pair and add it to the JSON object
                foreach (string pair in keyValuePairs)
                {
                    // Split each pair into key and value using '=' as the delimiter
                    string[] keyValue = pair.Split('=');

                    if (keyValue.Length == 2)
                    {
                        string key = keyValue[0].Trim();
                        string value = keyValue[1].Trim();
                        // Add the key-value pair to the JSON object
                        json.Add(key, value);
                    }
                }
                string playerName = json["display-name"];

                //Set player color
                string playerColorHex = json["color"];
                Color playerColor;
                if(!ColorUtility.TryParseHtmlString(playerColorHex, out playerColor))
                {
                    playerColor = Color.blue;
                }

                bool isSubscriber = json["subscriber"] == "1" ? true : false;
                bool isMod = json["mod"] == "1" ? true : false;

                //Set player message

                string chatMessage = "";
                string emoteId = "";

                if (!string.IsNullOrEmpty(json["emotes"]))
                {
                    emoteId = json["emotes"];
                    int emoteIndex = emoteId.IndexOf(':');
                    emoteId = emoteId.Substring(0, emoteIndex);
                }
                else
                {
                    chatMessage = json["user-type"];
                    int lastIndex = chatMessage.LastIndexOf(':');
                    chatMessage = chatMessage.Substring(lastIndex + 1).Trim();
                }

                PlayerData player = new PlayerData(playerName, playerColor, isSubscriber, isMod, chatMessage, emoteId);

                EventPlayerRecieved?.Invoke(player);
            }
        }
    }

}

[System.Serializable]
public struct PlayerData
{
    [SerializeField] private string name;
    [SerializeField] private Color color;
    [SerializeField] private bool isSubscriber;
    [SerializeField] private bool isMod;
    [SerializeField] private string message;
    [SerializeField] private string emoteId;

    public string Name => name;
    public Color Color => color;    
    public bool IsSubscriber => isSubscriber;
    public bool IsMod => isMod;
    public string Message => message;

    public string EmoteId => emoteId;

    public PlayerData(string name, Color color, bool isSubscriber, bool isMod, string message, string emoteId)
    {
        this.name = name;
        this.color = color;
        this.isSubscriber = isSubscriber;
        this.isMod = isMod;
        this.message = message;
        this.emoteId = emoteId;
    }
}