using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "DiceData", menuName = "ScriptableObjects/DiceData", order = 2)]
public class DiceData : ScriptableObject
{
    [Header("주사위 각 면의 문양 Sprite배열")]
    public Sprite[] patternSpriteArray;

    [Header("주사위 각 면 문양의 이름 배열")]
    public string[] patternNameArray;
}
