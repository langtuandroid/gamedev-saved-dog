using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void OnEnter(Bee bee);
    void OnExecute(Bee bee);
    void OnExit(Bee bee);
}
