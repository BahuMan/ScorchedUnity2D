using UnityEngine;

public class MissileControl : MonoBehaviour
{
    [SerializeField]
    private ExplosionControl explosion;

    private float startTime;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Missile hit " + collision.transform.gameObject.name);
        ExplosionControl fireball = Instantiate<ExplosionControl>(explosion, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private Rigidbody2D _rigid;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        GameController ctrl = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        startTime = Time.time;
    }

    private void FixedUpdate()
    {
        transform.right = _rigid.velocity;
        if (startTime + 5f < Time.time) Destroy(this.gameObject);
    }
}
