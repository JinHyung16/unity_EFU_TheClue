using HughGenerics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private string GameDataFileName = "GameProgressData.json";
    private string filePath = "/Resources/Data";
    private GameProgressData gameProgressData;

    #region PlayerPrefab Control
    private string userSession = "EFU.UserSession";
    
    /// <summary>
    /// 유저가 새게임을 최초 1회 시작하면 value로 1 저장
    /// </summary>
    public void SetUserLoginRecord()
    {
        PlayerPrefs.SetInt(userSession, 1);
    }

    /// <summary>
    /// 이전의 User가 최초 1회 게임을 시작한적 있는지 불러온다.
    /// </summary>
    /// <returns> 해당 값이 1이면 true, 없거나 0이면 false return </returns>
    public bool GetUserLoginRecord()
    {
        var isSession = PlayerPrefs.GetInt(userSession);
        if (isSession == 1)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region Json Data Control
    /// <summary>
    /// 현재 게임의 진행 상황을 Josn으로 저장합니다.
    /// 테마 단위로 저장합니다.
    /// </summary>
    public void SaveData(string theme)
    {
        LoadDataInJson();
        if (gameProgressData == null)
        {
            gameProgressData = new GameProgressData
            {
                ThemeName = theme
            };

            string jsonData = JsonUtility.ToJson(gameProgressData);
            string path = Path.Combine(Application.dataPath + filePath, GameDataFileName);
            File.WriteAllText(path, jsonData);
        }
        else
        {
            string jsonData = JsonUtility.ToJson(gameProgressData);
            JsonUtility.FromJsonOverwrite(jsonData, gameProgressData);
        }

#if UNITY_EDITOR
        Debug.Log("DataManager: 게임 데이터 저장 완료");
#endif
    }

    public string LoadData()
    {
        LoadDataInJson();
        return gameProgressData.ThemeName;
    }

    /// <summary>
    /// Json으로 저장된 게임 진행 상황 데이터를 읽어옵니다.
    /// 해당 테마의 첫 번째부터 시작합니다.
    /// </summary>
    private void LoadDataInJson()
    {
        string path = Path.Combine(Application.dataPath + filePath, GameDataFileName);
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            gameProgressData = JsonUtility.FromJson<GameProgressData>(jsonData);
#if UNITY_EDITOR
            Debug.Log("DataManager: 저장된 게임 데이터 불러오기 완료");
#endif
        }
    }
    #endregion
}
