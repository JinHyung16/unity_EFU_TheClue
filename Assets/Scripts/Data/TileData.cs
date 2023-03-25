using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "TileData", menuName = "ScriptableObjects/TileData", order = 3)]
public class TileData : ScriptableObject
{
    public Texture[] tileTextureArray;
    public Sprite[] tileSpriteArray;
    public Color[] tileEmissionColorArray; //빨강 노랑 파랑 이렇게 저장되어야함
}
