using UnityEngine;

public class MissileControl : MonoBehaviour
{
    [SerializeField]
    private ExplosionControl explosion;

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
    }

    private void FixedUpdate()
    {
        transform.right = _rigid.velocity;
    }
}
