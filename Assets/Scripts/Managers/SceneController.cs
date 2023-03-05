using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using HughGenerics;

public class SceneController : Singleton<SceneController>
{
    public string CurSceneName { get; private set; } //현재 내가 머물고있는 Scene이름
    public string LoadSceneName { get; private set; } //내가 이동할 Scene이름

    /// <summary>
    /// Loading Scene을 제외한 모든 Scene의 있는 Manager에서 호출한다.
    /// 현재 내가 있는 Scene의 이름을 받아놓는다.
    /// </summary>
    public void SetCurScene()
    {
        CurSceneName = SceneManager.GetActiveScene().name;
    }

    #region 비동기 Scene 이동 처리 Functions
    /// <summary>
    /// 기본적으로 호출하는 함수
    /// 이동할 Scene의 이름을 받아 저장해두고 LoadingScene으로 이동한 뒤, 미리 받아놓은 Scene으로 이동시킨다.
    /// </summary>
    /// <param name="loadSceneName"> 이동할 씬 이름을 받는다 </param>
    public void LoadScene(string loadSceneName)
    {
        GameManager.GetInstance.DespawnPlayer();

        this.CurSceneName = "LoadingScene";
        this.LoadSceneName = loadSceneName;

        SceneManager.LoadScene("LoadingScene");
    }

    public async UniTask LoadScenario()
    {
        AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync("ScenarioScene");
        await loadSceneAsync;
    }
    #endregion
}
