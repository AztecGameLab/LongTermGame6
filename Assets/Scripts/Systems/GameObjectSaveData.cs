using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game
{
    public class GameObjectSaveData : SaveData
    {
        public struct Data
        {
            
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 scale;
            public bool isObjectEnabled;
        }

        public override string GetID()
        {
            // new ID for every saved object
            string objectID = Guid.NewGuid().ToString();
            return objectID;
        }

        public override object WriteData()
        {
            // generate the data that represents this object's current state.
            Data data = new Data
            {
                position = transform.position,
                rotation = transform.rotation,
                scale = transform.localScale,
                isObjectEnabled = this.gameObject.activeInHierarchy,               
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
            this.gameObject.SetActive(savedData.isObjectEnabled);
        }
    }
}


