using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachState : IState
{
    private float timer;
    private float timeLength;
    
    public void OnEnter(Bee bee)
    {
        timer = 0;
        timeLength = 3f;
    }

    public void OnExecute(Bee bee)
    {
        timer += Time.deltaTime;
        if (timer < timeLength)
        {
            bee.CountTimeWhenBouncing();
            bee.FlyToDoge();
        } else
        {
            bee.ChangeState(bee.findState);
        }
    }

    public void OnExit(Bee bee)
    {}
}
