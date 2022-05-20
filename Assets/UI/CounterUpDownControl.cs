using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CounterUpDownControl : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] Button UpButton;
    [SerializeField] Button DownButton;

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ExecuteEvents.Execute(UpButton.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            ExecuteEvents.Execute(DownButton.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }
    }
}
