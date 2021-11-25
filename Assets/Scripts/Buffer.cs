using UnityEngine;

public class Buffer
{
    private float _startTime = float.NegativeInfinity;
    
    public void Queue()
    {
        _startTime = Time.time;
    }

    public bool IsQueued(float bufferTime)
    {
        float elapsedTime = Time.time - _startTime;
        return elapsedTime <= bufferTime;
    }

    public void Clear()
    {
        _startTime = float.NegativeInfinity;
    }
}