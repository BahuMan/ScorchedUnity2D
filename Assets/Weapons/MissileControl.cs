using UnityEngine;

public class MissileControl : MonoBehaviour
{
    [SerializeField]
    private ExplosionControl explosion;

    private float startTime;
    public TankControl firedBy;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Missile hit " + collision.transform.gameObject.name);
        MoveBorderToCameraEdge border = collision.gameObject.GetComponent<MoveBorderToCameraEdge>();
        if (border != null && border.GetBorderType() == MoveBorderToCameraEdge.BorderTypeEnum.BOUNCE)
        {
            Debug.Log("Missile BOINK");
            return; //allow bounce; no explosion
        }

        ExplosionControl fireball = Instantiate<ExplosionControl>(explosion, this.transform.position, Quaternion.identity);
        GameController._instance.addThingToDo(new WaitForDestruction(fireball));
        Destroy(this.gameObject);
    }

    private Rigidbody2D _rigid;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        startTime = Time.time;
    }

    private void FixedUpdate()
    {
        transform.right = _rigid.velocity;
        if (startTime + 10f < Time.time) Destroy(this.gameObject);
    }
}
