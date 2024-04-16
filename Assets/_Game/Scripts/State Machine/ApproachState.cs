using UnityEngine;

public class ApproachState : IState
{
    private float timerValue;
    private float timeValueLength;
    
    public void OnEnter(Bee bee)
    {
        timerValue = 0;
        timeValueLength = 3f;
    }

    public void OnExecute(Bee bee)
    {
        timerValue += Time.deltaTime;
        if (timerValue < timeValueLength)
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
