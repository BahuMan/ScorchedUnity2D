using System;
using UnityEngine;
using UnityEngine.Events;

public class TriggerResizeEvent : MonoBehaviour
{
    [Tooltip("register here to receive rectangle with visible world coordinates whenever window resizes")]
    public UnityEvent<Rect> OnDimensionsChanged;

    private void OnEnable()
    {
        OnRectTransformDimensionsChange();
    }

    private void OnRectTransformDimensionsChange()
    {

        Vector3 bottomleft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 topright = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 1f));
        Rect newdim = new Rect((bottomleft.x + topright.x) / 2f, (bottomleft.y + topright.y) / 2f, (topright.x - bottomleft.x) / 2f, (topright.y - bottomleft.y) / 2f);
        //Debug.Log("Bottomleft = " + bottomleft + ", topright = " + topright);
        //Debug.Log("resized to " + newdim);  
        if (OnDimensionsChanged != null) OnDimensionsChanged.Invoke(newdim);
    }
}
