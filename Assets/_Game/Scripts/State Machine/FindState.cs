using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindState : IState
{
    private float timer;
    private float timeLength;

    public void OnEnter(Bee bee)
    {
        timer = 0;
        timeLength = 0.5f;
    }

    public void OnExecute(Bee bee)
    {
        timer += Time.deltaTime;
        if (timer < timeLength)
        {
            bee.CountTimeWhenBouncing();
            bee.FlyRandomToLine();
        }
        else
        {
            bee.ChangeState(bee.approachState);
        }
    }

    public void OnExit(Bee bee)
    {
        
    }
}
