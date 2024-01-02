using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRewindableAction
{
    void Execute();
    void Undo();
    float GetTimestamp();
    void SetTimestamp(float timestamp);
}
