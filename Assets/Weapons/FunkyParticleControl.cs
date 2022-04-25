using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ExplosionControl))]
public class FunkyParticleControl : MonoBehaviour
{

    private float startTime;
    void Start()
    {
        ExplosionControl std = GetComponent<ExplosionControl>();
        std.ExplosionSize = Random.Range(1f, 4f);
        std.ExplosionDuration = Random.Range(.5f, 1f);
        startTime = Time.time + Random.Range(0f, 1f);

        //foreach (var r in GetComponentsInChildren<SpriteRenderer>())
        //{
        //    r.color = Random.ColorHSV(0f, 1f, .5f, 1f, .8f, 1f);
        //}
    }

    void Update()
    {
        foreach (var r in GetComponentsInChildren<SpriteRenderer>())
        {
            float h, s, v;
            Color.RGBToHSV(r.color, out h, out s, out v);
            r.color = Color.HSVToRGB(Mathf.Repeat(Time.time-startTime*3f, 1f), s, v);
        }
    }
}
