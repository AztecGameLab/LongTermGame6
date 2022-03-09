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

        [SerializeField, HideInInspector] 
        private string id = Guid.NewGuid().ToString();
        
        public override string GetID()
        {
            return id;
        }

        public override object WriteData()
        {
            // generate the data that represents this object's current state.
            Data data = new Data
            {
                position = transform.position,
                rotation = transform.rotation,
                scale = transform.localScale,
                isObjectEnabled = gameObject.activeInHierarchy,               
            };
            return data;
        }
       
        public override void ReadData(object data)
        {
            // apply our saved data to our current object!
            Data savedData = (Data)data;
            transform.position = savedData.position;
            transform.rotation = savedData.rotation;
            transform.localScale = savedData.scale;
            gameObject.SetActive(savedData.isObjectEnabled);
        }
    }
}


