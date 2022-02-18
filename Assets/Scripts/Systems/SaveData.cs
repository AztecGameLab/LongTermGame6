using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game
{
    public abstract class SaveData : MonoBehaviour
    {
        protected static HashSet<SaveData> _instances = new HashSet<SaveData>();
        public static IReadOnlyCollection<SaveData> Instances => _instances;
       
        private void Awake()
        {
            _instances.Add(this);
        }

        private void OnDestroy()
        {
            _instances.Remove(this);
        }

        public abstract string GetID();
        public abstract object WriteData();
        public abstract void ReadData(object data);
    }
}
