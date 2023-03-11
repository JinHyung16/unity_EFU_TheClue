using HughGenerics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Build;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    //Json Data 관련
    private string GameDataFileName = "GameProgressData.json";
    private string filePath = "/Resources/Data";
    public bool IsDataLoad { get; private set; } = false;
    
    public GameProgressData[] GameProgressDataArray { get; private set; } //Load해온 GameProgressData를 담고 있는다.
    //Game Session 관련
    private string userSession = "EFU_login";

    /// <summary>
    /// 현재 게임의 진행 상황을 Josn으로 저장합니다.
    /// 테마 단위로 저장합니다.
    /// </summary>
    public void SaveData(int themeIndex, string theme)
    {
        string checkPath = Path.Combine(Application.dataPath + filePath, GameDataFileName);

        GameProgressData[] jsonDataArray = new GameProgressData[themeIndex];
        for (int i = 0; i < themeIndex; i++)
        {
            jsonDataArray[i] = new GameProgressData();
            jsonDataArray[i].themeName = theme;
            if (File.Exists(checkPath))
            {

                string jsonData = JsonUtility.ToJson(jsonDataArray[i]);
                string path = Path.Combine(Application.dataPath + filePath, GameDataFileName);
                File.WriteAllText(path, jsonData);
            }
            else
            {
                string jsonData = JsonUtility.ToJson(jsonDataArray[i]);
                JsonUtility.FromJsonOverwrite(jsonData, jsonDataArray[i]);
            }
        }
    }

    /// <summary>
    /// Json으로 저장된 게임 진행 상황 데이터를 읽어옵니다.
    /// 해당 테마의 첫 번째부터 시작합니다.
    /// </summary>
    public void LoadData()
    {
        string path = Path.Combine(Application.dataPath + filePath, GameDataFileName);
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            for (int i = 0; i < jsonData.Length; i++)
            {
                GameProgressDataArray[i] = JsonUtility.FromJson<GameProgressData>(jsonData);
            }
            IsDataLoad = true;
#if UNITY_EDITOR
            Debug.Log("DataManager: 저장된 게임 데이터 불러오기 완료");
#endif
        }
        else
        {
            IsDataLoad = false;
#if UNITY_EDITOR
            Debug.Log("DataManager: 저장된 게임 데이터 존재하지 않아 불러오기 실패");
#endif
            return;
        }
    }


    /// <summary>
    /// 유저가 게임을 진행하고 게임을 끄면, 플레이 했다는 내용을 기록한다.
    /// </summary>
    public void SaveUserSession()
    {
        if (!PlayerPrefs.HasKey(userSession))
        {
            PlayerPrefs.SetInt(userSession, 1);
        }
    }

    /// <summary>
    /// 이전의 유저가 게임을 진행한적이 있는지 체크한다.
    /// </summary>
    /// <returns></returns>
    public bool GetUserSession()
    {
        var session = PlayerPrefs.GetInt(userSession);
        if (session == 1)
        {
            return true;
        }
        return false;
    }
}
