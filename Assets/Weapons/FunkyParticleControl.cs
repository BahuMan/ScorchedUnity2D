using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ExplosionControl))]
public class FunkyParticleControl : MonoBehaviour
{
    void Start()
    {
        ExplosionControl std = GetComponent<ExplosionControl>();
        std.ExplosionSize = Random.Range(1f, 4f);
        std.ExplosionDuration = Random.Range(.5f, 1f);

        foreach (var r in GetComponentsInChildren<SpriteRenderer>())
        {
            r.color = Random.ColorHSV(0f, 1f, .5f, 1f, .8f, 1f);
        }
    }

}
