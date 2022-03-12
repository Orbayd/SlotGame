using System.Collections;
using System.Collections.Generic;
using SpykeGames.Showcase.Core;
using SpykeGames.Showcase.Core.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "SlotMachineConfig", menuName = "ScriptableObjects/SlotMachineConfig", order = 1)]
public class SlotMachineConfig : ScriptableObject
{

    [Range(0.1f, 10.0f)]
    public float DelayRange;
    public float FastStopTime = 0.001f * 100.0f;
    public float NormalStop = 1.0f;
    public float SlowStop = 2.25f;

    public float GetStopDelay(StopDelayType delayType)
    {
        var delay = 0f;
        switch (delayType)
        {
            case StopDelayType.Immediate:
                delay = FastStopTime;
                break;
            case StopDelayType.Normal:
                delay = NormalStop;
                break;
            case StopDelayType.Slow:
                delay = SlowStop;
                break;
            default: break;
        }
        return delay;
    }

}
