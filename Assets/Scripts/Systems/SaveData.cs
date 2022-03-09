using System;
using System.Collections.Generic;
using UnityEngine;


namespace Game
{
    public abstract class SaveData : MonoBehaviour
    {
        [SerializeField, HideInInspector] 
        private string id = Guid.NewGuid().ToString();
        
        private static HashSet<SaveData> _instances = new HashSet<SaveData>();
        public static IEnumerable<SaveData> Instances => _instances;
       
        private void Awake()
        {
            _instances.Add(this);
        }

        private void OnDestroy()
        {
            _instances.Remove(this);
        }

        public string GetID()
        {
            return id;
        }
        
        public abstract object WriteData();
        public abstract void ReadData(object data);
    }
}
