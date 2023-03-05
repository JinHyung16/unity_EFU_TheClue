using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughCanvas;
using HughUIType;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class LoadingCanvas : CanvasManager
{
    //실제 로딩 시간
    [SerializeField] private float realLoadTime = 4.0f;

    //로딩 시간 중 최솟값을 담을 변수
    private float minLoadRatio;

    //가짜 로딩시간과 비율
    private float fakeLoadTime;
    private float fakeLoadRatio;

    private void Start()
    {
        UIManager.GetInstance.AddCanvasInDictionary(CanvasType.FixedCanvas, this);

        SceneController.GetInstance.SetCurScene();
        LoadThemeScene().Forget();
    }

    private void OnDestroy()
    {
        UIManager.GetInstance.ClearAllCanvas();
    }

    /// <summary>
    /// Loading Scene에서만 호출하는 함수
    /// 항상 LoadingScene으로 이동한 뒤, LoadingScene에서 최종 이동할 씬을 불러 씬 전환의 최적화를 진행한다.
    /// </summary>
    /// <returns>비동기 처리</returns>
    private async UniTaskVoid LoadThemeScene()
    {
        AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(SceneController.GetInstance.LoadSceneName);
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

            if (minLoadRatio >= 1.0f)
            {
                break;
            }

            await UniTask.Yield();
        }
        loadSceneAsync.allowSceneActivation = true;
        GameManager.GetInstance.SpawnPlayer();
    }
}
