using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game
{
    public class GameObjectSaveData : SaveData
    {
        struct Data
        {
            // all of the important state (position, rotation, is enabled, ect.)
            public Vector3 position;
            public Quaternion rotation;
            public bool isObjectEnabled;
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
            Data savedData = (Data)data;
            transform.position = savedData.position;
        }
    }
}


