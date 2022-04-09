using UnityEngine;
using UnityEngine.UI;

public class WindController : MonoBehaviour
{
    [SerializeField] float MaxWind;
    [SerializeField] bool VariableWind;

    [SerializeField] AreaEffector2D _effector;
    [SerializeField] InputField windInput;
    [SerializeField] RectTransform WindIcon;

    private void Start()
    {
        float windSpeed = Random.Range(0, MaxWind);
        bool fromLeft = Random.Range(0, 2) == 0 ? true : false;
        _effector.forceMagnitude = windSpeed;
        windInput.text = Mathf.RoundToInt(windSpeed).ToString();
        if (fromLeft)
        {
            _effector.forceAngle = 0f;
            WindIcon.localScale = Vector3.one;
        }
        else
        {
            _effector.forceAngle = 180f;
            WindIcon.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
}
