using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using HughGenerics;

public class SceneController : Singleton<SceneController>
{
    public string CurSceneName { get; set; } //현재 내가 머물고있는 Scene이름
    public string LoadSceneName { get; private set; } //내가 이동할 Scene이름

    //실제 로딩 시간
    [SerializeField] private float realLoadTime = 4.0f;

    //로딩 시간 중 최솟값을 담을 변수
    private float minLoadRatio = 0.0f;

    //가짜 로딩시간과 비율
    private float fakeLoadTime = 0.0f;
    private float fakeLoadRatio = 0.0f;

    public string GetLoadRatio { get; private set; }

    /// <summary>
    /// 기본적으로 호출하는 함수
    /// 이동할 Scene의 이름을 받아 저장해두고 LoadingScene으로 이동한 뒤, 미리 받아놓은 Scene으로 이동시킨다.
    /// </summary>
    /// <param name="loadSceneName"> 이동할 씬 이름을 받는다 </param>
    public void LoadScene(string loadSceneName)
    {
        this.CurSceneName = "Loading";
        this.LoadSceneName = loadSceneName;

        SceneManager.LoadScene("Loading");
    }

    /// <summary>
    /// Loading Scene에서만 호출하는 함수
    /// 항상 LoadingScene으로 이동한 뒤, LoadingScene에서 최종 이동할 씬을 불러 씬 전환의 최적화를 진행한다.
    /// </summary>
    /// <returns>비동기 처리</returns>
    public async UniTaskVoid LoadThemeScene()
    {
        AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(this.LoadSceneName);
        loadSceneAsync.allowSceneActivation = false;
        while (!loadSceneAsync.isDone)
        {
            //fake 로딩 시간 계산하기
            fakeLoadTime += Time.deltaTime;
            fakeLoadRatio = fakeLoadTime / realLoadTime;

            //실제 로딩 시간과 fake 로딩 시간 중 최솟값으로 로딩률 지정하기
            minLoadRatio = Mathf.Min(loadSceneAsync.progress + 0.1f, fakeLoadRatio);

            //Scene 로드 게이지 UI관련
            //loadingGaugeTxt.text = (minLoadRatio * 100).ToString("F0") + "%";
            GetLoadRatio = (minLoadRatio * 100).ToString("F0") + "%";
            if (minLoadRatio >= 1.0f)
            {
                break;
            }

            await UniTask.Yield();
        }
        loadSceneAsync.allowSceneActivation = true;
    }
}
