using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionControl : MonoBehaviour
{

    [Tooltip("Maximum size of the explosion")]
    public float ExplosionSize = 2f;

    [Tooltip("How long the fireball will be visible")]
    public float ExplosionDuration = 3f;

    [Tooltip("When and how the fireball will grow/shrink")]
    public AnimationCurve explosionGrowth;

    private float explosionStart;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("fireball");
        explosionStart = Time.time;
        SetSize(0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > explosionStart + ExplosionDuration)
        {
            Destroy(this.gameObject);
            return;
        }

        //calculate current size according to animationCurve:
        float curTime = Time.time - explosionStart;
        float curSize = ExplosionSize * explosionGrowth.Evaluate(curTime / ExplosionDuration);
        SetSize(curSize);
    }

    private void SetSize(float curSize)
    {
        foreach (Transform child in transform)
        {
            child.localScale = new Vector2(curSize, curSize);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Explosion blasted " + collision.gameObject.name);
        ReceiveDamage rcv = collision.GetComponent<ReceiveDamage>();
        if (rcv != null) rcv.RegisterDamage(this.gameObject, 500);
    }
}
