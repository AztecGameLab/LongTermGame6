using Game;
using UnityEngine;

namespace Systems
{
    public class RotationSystemSaveData : SaveData
    {
        [SerializeField] private RotationSystem system;
        
        public override object WriteData()
        {
            return system.CurrentRotation;
        }

        public override void ReadData(object data)
        {
            system.CurrentRotation = (Vector3) data;
        }
    }
}