using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunkyExplosionControl : MonoBehaviour
{

    [SerializeField] ExplosionControl _funkyParticle;
    [SerializeField] MissileControl _projectiles;
    [SerializeField] int _projectilesCount = 5;
    [SerializeField] float minPower = 200;
    [SerializeField] float maxPower = 800;

    // Start is called before the first frame update
    IEnumerator Start()
    {

        yield return new WaitForSeconds(GetComponent<ExplosionControl>().ExplosionDuration/2f);
        for (int b=0; b<_projectilesCount; b++)
        {
            MissileControl mis = Instantiate<MissileControl>(_projectiles);
            mis.explosion = _funkyParticle.gameObject;
            Rigidbody2D rb = mis.GetComponent<Rigidbody2D>();
            rb.transform.position = this.transform.position + Vector3.up;
            rb.transform.rotation = Quaternion.Euler(0, 0, Random.Range(45f, 135f));
            rb.velocity = rb.transform.right * Random.Range(minPower, maxPower) / TankControl.ForceMultiplier;
            GameController._instance.addThingToDo(new WaitForDestruction(mis));
        }
    }

}
