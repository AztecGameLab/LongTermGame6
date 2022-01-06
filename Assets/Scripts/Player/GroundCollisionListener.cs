using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// todo : seek higher knowledge for better algorithms here, my code smell sense is tingling
// todo: use ConnectedCollider and velocity to implement cool surfing slide sound

[RequireComponent(typeof(Collider))]
public class GroundCollisionListener : GroundCheck
{
    public override bool IsGrounded => _collidersToNormals
        .Any(pair => Vector3.Angle(-gravityDirection, pair.Value) <= slopeLimitDegrees);
    
    public override Vector3 ContactNormal => _collidersToNormals.Count > 0 ? _collidersToNormals
        .Select(pair => pair.Value)
        .Aggregate((prevVector, curVector) => prevVector + curVector) / _collidersToNormals.Count : Vector3.zero;
    
    public override Collider ConnectedCollider => _collidersToNormals.Count > 0 ? _collidersToNormals.First().Key : null;

    private Dictionary<Collider, Vector3> _collidersToNormals = new Dictionary<Collider, Vector3>();

    private void UpdateCollisionDictionary(Collider targetCollider, Vector3 normal)
    {
        if (_collidersToNormals.ContainsKey(targetCollider) == false)
            _collidersToNormals.Add(targetCollider, normal);

        else _collidersToNormals[targetCollider] = normal;
    }
    
    private void OnCollisionStay(Collision other)
    {
        UpdateCollisionDictionary(other.collider, other.GetContact(0).normal);
    }

    private void OnCollisionExit(Collision other)
    {
        _collidersToNormals.Remove(other.collider);
    }
}