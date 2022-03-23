using UnityEngine;

public class TankControl : MonoBehaviour
{
    public Rigidbody2D shell;
    public Transform Gun;
    public Transform Muzzle;

    public int HP = 1000;
    public float angle = 0f;
    public float force;

    public void Fire()
    {
        Rigidbody2D projectile = Instantiate<Rigidbody2D>(shell);
        projectile.transform.position = Muzzle.position;
        projectile.transform.rotation = Muzzle.rotation;
        projectile.velocity = Muzzle.right * force;

        GameController._instance.addThingToDo(new WaitForDestruction(projectile.GetComponent<MissileControl>()));
    }

    private void Update()
    {
        UpdateGun(angle);
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
