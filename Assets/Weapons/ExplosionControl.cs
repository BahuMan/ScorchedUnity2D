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
    private float lastSize = 0f; //will used to detect when fireball was at its maximum

    private HashSet<GameObject> ToDealDamage;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("fireball");
        explosionStart = Time.time;
        ToDealDamage = new HashSet<GameObject>();
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

        //before fireball gets smaller again (=> at its maximum), deal damage:
        SetSize(curSize);
        if (curSize < lastSize)
        {
            //dealDamage();
            lastSize = 0f;
        }
        else {
            //store for next frame:
            lastSize = curSize;
        }

    }

    private void SetSize(float curSize)
    {
        foreach (Transform child in transform)
        {
            child.localScale = new Vector2(curSize, curSize);
        }
    }

    private void dealDamage()
    {
        try
        {
            foreach (GameObject go in ToDealDamage)
            {
                if (go is null) continue; //one of the objects in the trigger was the original missile
                ReceiveDamage rcv = go?.GetComponent<ReceiveDamage>();
                rcv?.RegisterDamage(this.gameObject);
            }
        }
        catch (MissingReferenceException mre)
        {
            Debug.LogException(mre);
        }
        finally
        {
            ToDealDamage.Clear();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Explosion blasted " + collision.gameObject.name);
            ReceiveDamage rcv = collision.GetComponent<ReceiveDamage>();
            if (rcv != null) rcv.RegisterDamage(this.gameObject);
        ToDealDamage.Add(collision.gameObject);
    }
}
