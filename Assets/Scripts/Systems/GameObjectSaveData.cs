using UnityEngine;
using System;

namespace Game
{
    public class GameObjectSaveData : SaveData
    {
        [Serializable]
        public struct Data
        {
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 scale;
            public bool isObjectEnabled;
        }

        public override object WriteData()
        {
            Transform target = transform;
            
            // generate the data that represents this object's current state.
            Data data = new Data
            {
                position = target.position,
                rotation = target.rotation,
                scale = target.localScale,
                isObjectEnabled = gameObject.activeInHierarchy,               
            };
            return data;
        }
       
        public override void ReadData(object data)
        {
            Transform target = transform;
            
            // apply our saved data to our current object!
            Data savedData = (Data)data;
            target.position = savedData.position;
            target.rotation = savedData.rotation;
            target.localScale = savedData.scale;
            gameObject.SetActive(savedData.isObjectEnabled);
        }
    }
}


