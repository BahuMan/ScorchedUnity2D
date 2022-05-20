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
    [SerializeField] TraceShell traceShell;

    // Start is called before the first frame update
    IEnumerator Start()
    {

        yield return new WaitForSeconds(GetComponent<ExplosionControl>().ExplosionDuration/2f);
        for (int b=0; b<_projectilesCount; b++)
        {
            MissileControl mis = Instantiate<MissileControl>(_projectiles);
            TraceShell tc = Instantiate<TraceShell>(traceShell);
            mis.explosion = _funkyParticle.gameObject;
            Rigidbody2D rb = mis.GetComponent<Rigidbody2D>();
            rb.transform.SetPositionAndRotation(
                this.transform.position + Vector3.up,
                Quaternion.Euler(0, 0, Random.Range(45f, 135f))
            );
            rb.velocity = rb.transform.right * Random.Range(minPower, maxPower) / TankControl.ForceMultiplier;
            tc.Trace(mis.transform, Random.ColorHSV(0f, 1f, .7f, 1f, .5f, 1f, 1f, 1f));
            GameController._instance.addThingToDo(new WaitForDestruction(mis));
        }
    }

}
