using UnityEngine;
using UnityEngine.Events;

public class TankControl : MonoBehaviour
{
    private GenericPlayer Player;
    public Rigidbody2D shell;
    public Transform Gun;
    public Transform Muzzle;

    private int hP = 1000;
    private float angle = 0f;
    private float force;
    public const float ForceMultiplier = 10f;

    public UnityEvent<float> OnAngleChanged;
    public UnityEvent<float> OnForceChanged;
    public UnityEvent<int> OnHealthChanged;
    public UnityEvent<Rigidbody2D> OnShellFired;

    public void Fire()
    {
        Rigidbody2D projectile = Instantiate<Rigidbody2D>(shell);
        projectile.transform.position = Muzzle.position;
        projectile.transform.rotation = Muzzle.rotation;
        projectile.velocity = Muzzle.right * Force / ForceMultiplier;

        MissileControl m = projectile.GetComponent<MissileControl>();
        m.firedBy = this;
        GameController._instance.addThingToDo(new WaitForDestruction(m));
        if (OnShellFired != null) OnShellFired.Invoke(projectile);
    }

    private void Update()
    {
        UpdateGun(Angle);
    }

    private void UpdateGun(float a)
    {
        this.Gun.rotation = Quaternion.Euler(0f,0f, a);
    }

    public void OnDamageReceived(GameObject src, int dmg)
    {
        HP -= dmg;
        if (HP <= 0)
        {
            GameController._instance.addThingToDo(new TalkAndDie(this, null));
            Debug.Log("ow");
        }
        OnHealthChanged.Invoke(HP);
        if (this.force > this.hP)
        {
            this.force = this.hP;
            OnForceChanged.Invoke(this.force);
        }
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
            force = Mathf.Min(HP, value);
            OnForceChanged.Invoke(force);
        }
    }

    public void SetPlayer(GenericPlayer p, SimpleBehaviour.INode interaction)
    {
        if (OnMyTurn != null || Player != null) throw new UnityException("TankControl is being initialized second time by " + p.gameObject.name);
        this.Player = p;
        OnMyTurn = interaction;
    }

    public GenericPlayer GetPlayer()
    {
        return Player;
    }

    public SimpleBehaviour.INode GetInteraction()
    {
        if (OnMyTurn == null) Debug.LogError("TankControl was asked for interaction but nothing was set first");
        return OnMyTurn;
    }
}
