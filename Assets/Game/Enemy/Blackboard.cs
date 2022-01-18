using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy
{
    public class Blackboard : MonoBehaviour
    {
        public Dictionary<string, object> Data = new Dictionary<string, object>();
        public HashSet<string> Triggers = new HashSet<string>();
    }
}