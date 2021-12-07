using UnityEngine;
using UnityEngine.Rendering;

public class VolumeBlender : MonoBehaviour
{
    [SerializeField] private Volume volume;
    [SerializeField] private float blendSpeed;
    [SerializeField] private bool invert;

    private float _targetWeight;

    private void Update()
    {
        float current = volume.weight;
        float target = _targetWeight;
        float maxDelta = blendSpeed >= 0 ? blendSpeed * Time.deltaTime : 1;
        
        volume.weight = Mathf.MoveTowards(current, target, maxDelta);
    }

    public void BlendWeight(float weight)
    {
        _targetWeight = invert ? 1 - weight : weight;
    }
}