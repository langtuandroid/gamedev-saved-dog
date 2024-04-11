using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindAdvanceState : IState
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
            bee.FlyToFinalPointOnLine();
        }
        else
        {
            bee.ChangeState(new ApproachState());
        }
    }

    public void OnExit(Bee bee)
    {
        
    }
}
