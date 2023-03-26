using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "TileData", menuName = "ScriptableObjects/TileData", order = 3)]
public class TileData : ScriptableObject
{
    public GameObject[] tileObjArray;
}
