using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering.LookDev;
using UnityEngine.UI;

public class Bubble : MonoBehaviour
{
    [SerializeField] private float activeTime = 3;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Image emoteImage;
    [SerializeField] private RawImage emoteRawImage;
    [SerializeField] private AudioClip fartClip;

    private RectTransform rectTransform;
    private Vector3 initialSize;

    private Coroutine disableBubble;
    private Coroutine downloadEmote;

    [SerializeField] private Sprite[] emotes;

    private void OnEnable()
    {
        SetInitialReferences();
    }


    private void OnDisable()
    {
        if (disableBubble != null)
        {
            StopCoroutine(disableBubble);
            disableBubble = null;
        }

        if (downloadEmote != null)
        {
            StopCoroutine(downloadEmote);
            downloadEmote = null;
        }
    }

    private void SetInitialReferences()
    {
        rectTransform = GetComponent<RectTransform>();
        initialSize = rectTransform.localScale;

        rectTransform.localScale = Vector3.zero;
    }
    public void SayEmote(string emoteId)
    {
        if (downloadEmote != null)
        {
            StopCoroutine(downloadEmote);
            downloadEmote = null;
        }
        downloadEmote = StartCoroutine(GetTexture(emoteId, PlaceEmote));
    }

    public void Say(string message)
    {
        rectTransform.localScale = initialSize;

        if (!message.ToLower().StartsWith("!emote"))
        {
            messageText.SetText(message);
            emoteImage.gameObject.SetActive(false);
            emoteRawImage.gameObject.SetActive(false);
        }
        else
        {
            PlaceEmote(message.ToLower());
        }

        if(disableBubble != null)
        {
            StopCoroutine(disableBubble);
            disableBubble = null;
        }

        disableBubble = StartCoroutine(DisableBubbleRoutine());
    }

    private IEnumerator DisableBubbleRoutine()
    {
        yield return new WaitForSeconds(activeTime);
        rectTransform.localScale = Vector3.zero;
    }

    private void PlaceEmote(string message)
    {
        string[] lines = message.Split(' ');
        string code = lines[lines.Length - 1];

        Sprite emote = null;
        switch (code)
        {
            case "1" or ":d":
                emote = emotes[0];
                break;
            case "2" or "happy" or ":)":
                emote = emotes[1];
                break;
            case "3" :
                emote = emotes[2];
                break;
            case "4" or "sad" or ":(":
                emote = emotes[3];
                break;
            case "5" or ":o":
                emote = emotes[4];
                break;
            case "6" or ":p":
                emote = emotes[5];
                break;
            case "7":
                emote = emotes[6];
                break;
            case "8":
                emote = emotes[7];
                break;
            case "9":
                emote = emotes[8];
                break;
            case "y":
                emote = emotes[9];
                break;
            case "0" or ":/":
                emote = emotes[10];
                break;
            case "k" or "cake":
                emote = emotes[11];
                break;
            case "h" or "heart" or "love":
                emote = emotes[12];
                break;
            case "b" or "bulb" or "idea":
                emote = emotes[13];
                break;
            case "c" or "coffee":
                emote = emotes[14];
                break;
            case "g" or "game":
                emote = emotes[15];
                break;
            case "o" or "popcorn":
                emote = emotes[16];
                break;
            case "z" or "pizza":
                emote = emotes[17];
                break;
            case "q":
                emote = emotes[18];
                break;
            case "l":
                emote = emotes[19];
                break;
            case "f" or "flower":
                emote = emotes[20];
                break;
            case "n" or "night":
                emote = emotes[21];
                break;
            case "p" or "puffle":
                emote = emotes[22];
                break;
            case "m" or "money" or "coin":
                emote = emotes[23];
                break;
            case "t" or "fart":
                AudioSource.PlayClipAtPoint(fartClip, Vector3.zero, 1);
                emote = emotes[24];
                break;
            case "d" or "sun":
                emote = emotes[25];
                break;
            case "w":
                emote = emotes[26];
                break;
            case "i" or "iglu":
                emote = emotes[27];
                break;
        }

        if (emote == null) return;

        messageText.SetText("");
        emoteRawImage.gameObject.SetActive(false);

        emoteImage.sprite = emote;
        emoteImage.gameObject.SetActive(true);
    }

    private void PlaceEmote(Texture2D texture)
    {
        Say("");
        emoteRawImage.texture = texture;
        emoteRawImage.gameObject.SetActive(true);
    }

    IEnumerator GetTexture(string emoteID, Action<Texture2D> onSuccess)
    {
        string url = $"https://static-cdn.jtvnw.net/emoticons/v1/{emoteID}/3.0";
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        Texture2D img = DownloadHandlerTexture.GetContent(www);;
        onSuccess?.Invoke(img);

    }
}
