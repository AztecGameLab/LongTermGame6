using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game
{
    public class GameObjectSaveData : SaveData
    {
        public struct Data
        {
            // all of the important state (position, rotation, is enabled, ect.)
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 scale;
            public bool isObjectEnabled;
        }

        public override string GetID()
        {
            // create some kind of unique ID for this object, that we can use to look it up when we reload
            string uniqueID = this.gameObject.name + "1";
            
            return "placeholder unique ID";
        }

        public override object WriteData()
        {
            // generate the data that represents this object's current state.
            Data data = new Data
            {
                position = transform.position,
                rotation = transform.rotation,
                scale = transform.localScale,
                isObjectEnabled = this.gameObject.activeInHierarchy
            };
           // _instances.Add(data.position);
            return data;
            
            //_instances.Add(Data);
        }
       
        public override void ReadData(object data)
        {
            // apply our saved data to our current object!
            Data savedData = (Data)data;
            transform.position = savedData.position;
        }
    }
}


