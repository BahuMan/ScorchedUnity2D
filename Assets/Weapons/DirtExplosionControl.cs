using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtExplosionControl : MonoBehaviour
{

    [SerializeField]
    private Collider2D explosionShape;

    [Tooltip("Maximum size of the explosion")]
    public float ExplosionSize = 2f;

    [Tooltip("How long the fireball will be visible")]
    public float ExplosionDuration = 3f;

    [Tooltip("When and how the fireball will grow/shrink")]
    public AnimationCurve explosionGrowth;

    private float explosionStart;
    private float lastSize;
    private bool dirtyDeedsDone = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("fireball");
        explosionStart = Time.time;
        SetSize(0f);
        lastSize = 0f;
        dirtyDeedsDone = false;
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

        //at max size, trigger dirt:
        if (lastSize > curSize) TriggerDirt();
        else lastSize = curSize;


        SetSize(curSize);
    }

    private void TriggerDirt()
    {
        if (dirtyDeedsDone) return;
        Debug.Log("Dirt!");
        FindObjectOfType<TerrainGenerator>().AddDirt(explosionShape);
        lastSize = 0f;
        dirtyDeedsDone =true; //cheap! \nn/
    }

    private void SetSize(float curSize)
    {
        explosionShape.transform.localScale = new Vector2(curSize, curSize);
    }

}
