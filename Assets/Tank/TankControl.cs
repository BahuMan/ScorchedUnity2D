using UnityEngine;
using UnityEngine.Events;

public class TankControl : MonoBehaviour
{
    public Rigidbody2D shell;
    public Transform Gun;
    public Transform Muzzle;

    private int hP = 1000;
    private float angle = 0f;
    private float force;

    public UnityEvent<float> OnAngleChanged;
    public UnityEvent<float> OnForceChanged;

    public void Fire()
    {
        Rigidbody2D projectile = Instantiate<Rigidbody2D>(shell);
        projectile.transform.position = Muzzle.position;
        projectile.transform.rotation = Muzzle.rotation;
        projectile.velocity = Muzzle.right * Force;

        GameController._instance.addThingToDo(new WaitForDestruction(projectile.GetComponent<MissileControl>()));
    }

    private void Update()
    {
        UpdateGun(Angle);
    }

    private void UpdateGun(float a)
    {
        this.Gun.rotation = Quaternion.Euler(0f,0f, a);
    }

    public void OnDamageReceived(GameObject src)
    {
        HP = 0;
        Debug.Log("ow");
    }

    private SimpleBehaviour.INode OnMyTurn = null;

    public int HP { get => hP; set => hP = value; }
    public float Angle { 
        get => angle;
        set { 
            angle = value;
            OnAngleChanged.Invoke(angle);
        } 
    }
    public float Force {
        get => force;
        set {
            force = value;
            OnForceChanged.Invoke(force);
        } }

    public void SetInteraction(SimpleBehaviour.INode interaction)
    {
        if (OnMyTurn != null) throw new UnityException("ScorchedUnity2D: can't set two interactions for one tank " + this.gameObject.name);
        OnMyTurn = interaction;
    }

    public SimpleBehaviour.INode GetInteraction()
    {
        if (OnMyTurn == null) Debug.LogError("TankControl was asked for interaction but nothing was set first");
        return OnMyTurn;
    }
}
