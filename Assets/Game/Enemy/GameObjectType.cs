using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy
{
    [CreateAssetMenu(fileName = "New Type", menuName = "Type", order = 0)]
    public class Type : ScriptableObject { }

    public class GameObjectType : MonoBehaviour
    {
        [SerializeField] private Type[] types;

        private HashSet<Type> _typeSet;

        private void Awake()
        {
            _typeSet = new HashSet<Type>(types);
        }

        public bool IsType(Type type)
        {
            return _typeSet.Contains(type);
        }
    }
}