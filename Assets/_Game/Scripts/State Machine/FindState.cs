using UnityEngine;

public class FindState : IState
{
    private float timerValue;
    private float timeValueLength;

    public void OnEnter(Bee bee)
    {
        timerValue = 0;
        timeValueLength = 0.5f;
    }

    public void OnExecute(Bee bee)
    {
        timerValue += Time.deltaTime;
        if (timerValue < timeValueLength)
        {
            bee.CountTimeWhenBouncing();
            bee.FlyRandomToLine();
        } else
        {
            bee.ChangeState(bee.approachState);
        }
    }

    public void OnExit(Bee bee) {}
}
