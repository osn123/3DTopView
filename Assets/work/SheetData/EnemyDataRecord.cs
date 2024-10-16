using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public EnemyDataRecord[] enemies;

    [System.Serializable]
    public class EnemyDataRecord
    {
        public int id;
        public string name;
        public int hp;
        public int attack;
        public int defense;
        public string dropItem;
        public string spawnArea;
    }
}