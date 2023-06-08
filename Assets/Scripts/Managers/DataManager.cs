using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using HughUtility;
using HughGenerics;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

public class DataManager : Singleton<DataManager>
{
    //[SerializeField] private TextAsset GameProgressJsonData;
    //Json Data 관련
    private string GameDataFileName = "GameProgressData.json";
    private string jsonFilePath = "/Resources/Data/";

    //Game Session 관련
    private string userSession = "EFU_login";
    public int SaveThemeIndex { get; set; } = 1;
    private void Start()
    {
        ReadDialogueCSV();
    }
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
            string movePath = Path.Combine(Application.dataPath + jsonFilePath, GameDataFileName);
            File.WriteAllText(movePath, jsonData);
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
        var json = Resources.Load<TextAsset>("Data/GameProgressData");
        GameProgressData data = JsonUtility.FromJson<GameProgressData>(json.ToString());
        return data;

        /*
        string movePath = Path.Combine(Application.dataPath + jsonFilePath, GameDataFileName);
        GameProgressData data = new GameProgressData();
        if (File.Exists(movePath))
        {
            string jsonData = File.ReadAllText(movePath);
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

    #region CSV Data

    //테마1 캐릭터, 대사 리스트 담기
    [HideInInspector] public List<string> ThemeFirstCharacter = new List<string>();
    [HideInInspector] public List<string> ThemeFirstContext = new List<string>();

    //테마2 캐릭터, 대사 리스트 담기
    [HideInInspector] public List<string> ThemeSecondCharacter = new List<string>();
    [HideInInspector] public List<string> ThemeSecondContext = new List<string>();

    //테마3 캐릭터, 대사 리스트 담기
    [HideInInspector] public List<string> ThemeThirdCharacter = new List<string>();
    [HideInInspector] public List<string> ThemeThirdContext = new List<string>();

    private void ReadDialogueCSV()
    {
        string shopFile = "Dialogue";
        List<Dictionary<string, string>> csvDataList = CSVReader.ReadFile(shopFile);

        for (int i = 0; i < csvDataList.Count; i++)
        {
            string theme = csvDataList[i]["Theme"].ToString();
            if (theme == "1")
            {
                ThemeFirstCharacter.Add(csvDataList[i]["Character"].ToString());
                ThemeFirstContext.Add(csvDataList[i]["Context"].ToString());
            }
            else if (theme == "2")
            {
                ThemeSecondCharacter.Add(csvDataList[i]["Character"].ToString());
                ThemeSecondContext.Add(csvDataList[i]["Context"].ToString());
            }
            else if (theme == "3")
            {
                ThemeThirdCharacter.Add(csvDataList[i]["Character"].ToString());
                ThemeThirdContext.Add(csvDataList[i]["Context"].ToString());
            }
        }
    }
    #endregion
}
