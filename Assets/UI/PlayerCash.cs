using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class PlayerCash : MonoBehaviour
{
    public void SetCash(int cash)
    {
        GetComponent<Text>().text = $"Cash: {cash}";
    }
}
