using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using HughUtility;
using HughGenerics;

public class DataManager : Singleton<DataManager>
{
    [SerializeField] private TextAsset GameProgressJsonData;
    //Json Data 관련
    private string GameDataFileName = "GameProgressData.json";
    private string jsonFilePath = "/Resources/Data/";

    //Game Session 관련
    private string userSession = "EFU_login";

    /// <summary>
    /// 현재 게임의 진행 상황을 Josn으로 저장합니다.
    /// 테마 단위로 저장합니다.
    /// </summary>
    public void SaveData(int clearIndex)
    {
        string path = Path.Combine(Application.dataPath + jsonFilePath, GameDataFileName);
        GameProgressData data = new GameProgressData
        {
            themeClearIndex = clearIndex,
        };
        string saveData = JsonUtility.ToJson(data);
        File.WriteAllText(path, saveData);

        /*
        string checkPath = Path.Combine(Application.dataPath + jsonFilePath, GameDataFileName);
        GameProgressData data = new GameProgressData
        {
            themeClearIndex = clearIndex,
        };

        if (!File.Exists(checkPath))
        {
            string jsonData = JsonUtility.ToJson(data);
            string path = Path.Combine(Application.dataPath + jsonFilePath, GameDataFileName);
            File.WriteAllText(path, jsonData);
        }
        else
        {
            string jsonData = JsonUtility.ToJson(data);
            JsonUtility.FromJsonOverwrite(jsonData, data);
        }
        */
    }

    /// <summary>
    /// Json으로 저장된 게임 진행 상황 데이터를 읽어옵니다.
    /// 해당 테마의 첫 번째부터 시작합니다.
    /// </summary>
    public GameProgressData LoadData()
    {
        GameProgressData data = new GameProgressData();
        string json = GameProgressJsonData.ToString();
        data = JsonUtility.FromJson<GameProgressData>(json);
        return data;
        /*
        string path = Path.Combine(Application.dataPath + jsonFilePath, GameDataFileName);
        GameProgressData data = new GameProgressData();
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            data = JsonUtility.FromJson<GameProgressData>(jsonData);
#if UNITY_EDITOR
            Debug.Log("DataManager: 저장된 게임 데이터 불러오기 완료");
#endif
            return data;
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("DataManager: 저장된 게임 데이터 존재하지 않아 불러오기 실패");
#endif
            return null;
        }
        */
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
