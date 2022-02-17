using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private GameObject triggerObject;
    [SerializeField] private List<GameObject> objectsToSave;

    public void Start()
    {
        objectsToSave = new List<GameObject>();
    }
    //private GameObject saveData = Game.SaveData.WriteData
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(triggerObject))
        {
            objectsToSave.Add(this.gameObject);
            objectsToSave.Add(GameObject.Find("TowerEnemy"));
            objectsToSave.Add(GameObject.Find("ButcherEnemy"));
            Save();
        }
    }
    private void Save()
    {
     
    }

}
