using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceShell : MonoBehaviour
{
    [SerializeField] float timeStep = 0.1f;
    [SerializeField] LineRenderer myLine;
    
    private Transform _projectile;
    private float _nextStep;
    private Color _playerColor;

    public void Trace(Transform theProjectile, Color playerColor)
    {

        _nextStep = Time.time + timeStep;
        _projectile = theProjectile;
        myLine.positionCount = 2;
        myLine.SetPosition(0, theProjectile.position);
        myLine.SetPosition(1, theProjectile.position);
        myLine.startColor = playerColor - new Color(0,0,0,.5f);
        myLine.endColor = playerColor - new Color(0, 0, 0, .5f);
        _playerColor = playerColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (_projectile == null)
        {
            myLine.Simplify(0.05f);
            Destroy(this); //only this script, not the actual LineRenderer!
            return;
        }

        //if the x-position changed sign, the projectile wrapped around
        //and we need to split the trace in 2
        if ((_projectile.position.x * myLine.GetPosition(myLine.positionCount - 1).x) < 0)
        {
            Debug.Log("splitting trace");
            TraceShell newTrace = Instantiate<TraceShell>(this);
            newTrace.Trace(_projectile, _playerColor);
            Destroy(this);
            return;
        }

        if (Time.time > _nextStep)
        {
            myLine.positionCount++;
        }

        myLine.SetPosition(myLine.positionCount - 1, _projectile.position);
    }

}
