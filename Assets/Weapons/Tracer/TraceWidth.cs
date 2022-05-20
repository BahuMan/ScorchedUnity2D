using UnityEngine;

//this class aims to keep LineRenderer width in proportion to screen size
public class TraceWidth: MonoBehaviour
{
    [SerializeField] LineRenderer myLine;
    [SerializeField] float fudge = 1000f;

    private void Start()
    {
        mainUI_OnDimensionsChanged(new Rect());
    }
    private void OnEnable()
    {
        TriggerResizeEvent re = FindObjectOfType<TriggerResizeEvent>();
        if (re != null) re.OnDimensionsChanged.AddListener(mainUI_OnDimensionsChanged);
    }

    private void OnDisable()
    {
        TriggerResizeEvent re = FindObjectOfType<TriggerResizeEvent>();
        if (re != null) re.OnDimensionsChanged.RemoveListener(mainUI_OnDimensionsChanged);
    }

    private void mainUI_OnDimensionsChanged(Rect totalview)
    {
        float w = fudge / Camera.main.pixelWidth; // totalview.width / fudge;
        Debug.Log("Setting line width to " + w);
        myLine.startWidth = myLine.endWidth = w;
    }

}
