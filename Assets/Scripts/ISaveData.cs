// ReSharper disable All

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class SaveData : MonoBehaviour
    {
        private static HashSet<SaveData> _instances = new HashSet<SaveData>();
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
    
    public class GameObjectSaveData : SaveData
    {
        struct Data
        {
            // all of the important state (position, rotation, is enabled, ect.)
            public Vector3 position;
        }
        
        public override string GetID()
        {
            // create some kind of unique ID for this object, that we can use to look it up when we reload
            return "placeholder unique ID";
        }

        public override object WriteData()
        {
            // generate the data that represents this object's current state.
            return new Data { position = transform.position };
        }

        public override void ReadData(object data)
        {
            // apply our saved data to our current object!
            Data savedData = (Data) data;
            transform.position = savedData.position;
        }
    }
}