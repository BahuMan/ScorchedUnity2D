using UnityEngine;
using UnityEngine.Events;
public class ReceiveDamage : MonoBehaviour
{
    public UnityEvent<GameObject> OnDamageReceived;

    public void RegisterDamage(GameObject dealtBy)
    {
        OnDamageReceived.Invoke(dealtBy);
    }
}
