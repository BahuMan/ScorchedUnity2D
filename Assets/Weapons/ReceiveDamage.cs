using UnityEngine;
using UnityEngine.Events;
public class ReceiveDamage : MonoBehaviour
{
    public UnityEvent<GameObject, int> OnDamageReceived;

    public void RegisterDamage(GameObject dealtBy, int dmg)
    {
        OnDamageReceived.Invoke(dealtBy, dmg);
    }
}
