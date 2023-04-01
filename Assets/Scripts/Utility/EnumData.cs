using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HughEnumData
{
    public class EnumData { }

    /// <summary>
    /// InteractiveType 형식은 (사용될 테마, 사용될 오브젝트 이름) 형식을 따른다.
    /// </summary>
    public enum InteractiveType
    {
        None = 0,
        ThemeFirst_DoorLock,
        ThemeFirst_Dice,
        ThemeFirst_Tile_Pattern,
        ThemeFirst_Cube,
        ThemeSecond_Key,
        ThemeSecond_Note,
        Switch,
        Door,
    }
}
