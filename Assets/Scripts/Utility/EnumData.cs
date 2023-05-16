using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HughEnumData
{
    public class EnumData { }

    /// <summary>
    /// InteractiveTypeToDoor 형식은 (사용될 테마, 사용될 오브젝트 이름) 형식을 따른다.
    /// </summary>
    public enum InteractiveType
    {
        None = 0,

        //테마1, 2 사용
        DoorLock,
        Switch,
        Door,

        ThemeFirst_Dice,
        ThemeFirst_Tile_Pattern,
        ThemeFirst_Cube,

        ThemeSecond_Key,
        ThemeSecond_NPC,
        ThemeSecond_ShowCase,
        ThemeSecond_WristWatch, //손목시계
        ThemeSecond_WallClock, //벽걸이 시계

        ThemeThird_Btn_GetKey,
        ThemeThird_Btn_CallEnemiesRegion02,
        ThemeThird_Btn_CallNPCRegion03,
        ThemeThird_Btn_CallNPCRegion04,
    }

    public enum AnimationType
    {
        None = 0,

        P_PickUp,
        P_EnterCode,
        P_OpenDoor,
        P_Died,
    }
}
