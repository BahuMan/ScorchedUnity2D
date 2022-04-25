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
    //private string[] LastWords;
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
        //TextAsset FireTextFile = Resources.Load<TextAsset>("fire");
        //if (FireTextFile == null) throw new System.Exception("could not find resource 'fire'");
        //if (FireTextFile.text == null) throw new System.Exception("Could find, but not read resource 'fire'");
        FireLines = fireText.Split("\r\n".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);

        //TextAsset LastWordsTextfile = Resources.Load<TextAsset>("lastwords");
        //if (LastWordsTextfile == null) throw new System.Exception("could not find resource 'lastwords'");
        //if (LastWordsTextfile.text == null) throw new System.Exception("could find, but not read resource 'lastwords;");
        //LastWords = LastWordsTextfile.text.Split("\r\n".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
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
            theText = worldCoordinates.gameObject.name;
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
