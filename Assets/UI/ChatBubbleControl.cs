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

    private void Start()
    {
        _instance = this;
        HideChatBubble();
    }

    public void ShowChatBubble(Transform worldCoordinates, string theText)
    {
        Vector3 wpos = worldCoordinates.position;
        Debug.Log("Chat " + theText + " at " + wpos);
        gameObject.SetActive(true);
        Vector3 screenpoint = thiscam.WorldToScreenPoint(wpos);
        Debug.Log("ShowChatBubble converted " + worldCoordinates.position + " to " + screenpoint);
        transform.position = screenpoint + offset;
        Chattext.text = theText;
    }

    public void HideChatBubble()
    {
        gameObject.SetActive(false);
    }
}
