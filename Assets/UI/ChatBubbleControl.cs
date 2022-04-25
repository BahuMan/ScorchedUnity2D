using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ChatBubbleControl : MonoBehaviour
{
    public static ChatBubbleControl _instance;

    [SerializeField]
    private Text Chattext;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private Camera thiscam;
    private string[] FireLines;
    private string[] LastWords;
    public enum ChatMoment { FIRE, DIE }

    private void Start()
    {
        _instance = this;
        HideChatBubble();
        ReadChatFiles();
    }

    private void ReadChatFiles()
    {
        string fireText = File.ReadAllText(Application.streamingAssetsPath + "/fire.txt", System.Text.Encoding.UTF8);
        FireLines = fireText.Split("\r\n".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);

        string dieText = File.ReadAllText(Application.streamingAssetsPath + "/lastwords.txt", System.Text.Encoding.UTF8);
        LastWords = dieText.Split("\r\n".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
    }

    public void ShowChatBubble(Transform worldCoordinates, ChatMoment when)
    {
        string theText;
        if (when == ChatMoment.FIRE)
        {
            theText = FireLines[Random.Range(0, FireLines.Length)];
        }
        else
        {
            theText = LastWords[Random.Range(0, LastWords.Length)];
        }

        Vector3 wpos = worldCoordinates.position;
        Debug.Log("Chat " + theText + " at " + wpos);
        gameObject.SetActive(true);
        Vector3 screenpoint = thiscam.WorldToScreenPoint(wpos);
        Debug.Log("ShowChatBubble converted " + worldCoordinates.position + " to " + screenpoint);
        transform.position = screenpoint + offset;
        Chattext.text = theText;
        Chattext.CalculateLayoutInputHorizontal();
        Chattext.CalculateLayoutInputVertical();
        Chattext.LayoutComplete();

        ((RectTransform)this.transform).sizeDelta = Chattext.rectTransform.sizeDelta;
    }

    public void HideChatBubble()
    {
        gameObject.SetActive(false);
    }
}
