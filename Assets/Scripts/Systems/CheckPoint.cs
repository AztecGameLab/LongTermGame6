using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private GameObject triggerObject;
    [SerializeField] private Game.GameObjectSaveData gameObjectSaveDataScript;

    public void Start()
    {
        
    }
    //private GameObject saveData = Game.SaveData.WriteData
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(triggerObject))
        {
            gameObjectSaveDataScript.WriteData();
        }
    }


}
