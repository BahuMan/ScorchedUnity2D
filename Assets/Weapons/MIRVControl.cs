using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIRVControl : MonoBehaviour
{

    [SerializeField] Rigidbody2D reentryVehicle;
    [SerializeField] float horizontalSpread;

    private Rigidbody2D _rigid;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_rigid.velocity.y <= 0) //at top of parabola, split into 5
        {
            Debug.Log("MIRV split");
            for (int i=-2; i<3; ++i)
            {
                Rigidbody2D m = Instantiate<Rigidbody2D>(reentryVehicle);
                m.GetComponent<MissileControl>().firedBy = GetComponent<MissileControl>().firedBy;
                m.transform.position = this.transform.position;
                m.velocity = new Vector2(_rigid.velocity.x + i*horizontalSpread, 0);
                GameController._instance.addThingToDo(new WaitForDestruction(m.GetComponent<MissileControl>()));
                Destroy(this.gameObject);
            }
        }
    }
}
