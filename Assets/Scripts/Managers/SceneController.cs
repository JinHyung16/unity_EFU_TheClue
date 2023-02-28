using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using HughGenerics;
using Cysharp.Threading.Tasks;

public class SceneController : Singleton<SceneController>
{
    [SerializeField] private Canvas loadingCanvas;


    //scene 전환 비동기 연산을 위한 제공되는 코루틴
    private AsyncOperation loadSceneAsync;

    //실제 로딩 시간
    [SerializeField] private float realLoadTime = 4.0f;

    //로딩 시간 중 최솟값을 담을 변수
    private float minLoadRatio;

    //가짜 로딩시간과 비율
    private float fakeLoadTime;
    private float fakeLoadRatio;

    private void Start()
    {
        loadingCanvas.enabled = false;
    }

    public async UniTaskVoid LoadSceneAsync(string sceneName)
    {
        loadingCanvas.enabled = true;

        //먼저 아무것도 없는 Scene을 올리고,
        loadSceneAsync = SceneManager.LoadSceneAsync("LoadingScene");
        if (loadSceneAsync.isDone)
        {
            loadSceneAsync.allowSceneActivation = true;
            loadSceneAsync = null;
        }
        
        loadSceneAsync = SceneManager.LoadSceneAsync(sceneName);
        loadSceneAsync.allowSceneActivation = false;

        while (!loadSceneAsync.isDone)
        {
            //fake 로딩 시간 계산하기
            fakeLoadTime += Time.deltaTime;
            fakeLoadRatio = fakeLoadTime / realLoadTime;

            //실제 로딩 시간과 fake 로딩 시간 중 최솟값으로 로딩률 지정하기
            minLoadRatio = Mathf.Min(loadSceneAsync.progress + 0.1f, fakeLoadRatio);

            //Loading UI의 text Update하기
            //loadingGaugeTxt.text = (minLoadRatio * 100).ToString("F0") + "%";

            if (minLoadRatio >= 1.0f)
            {
                //loadingCanvasGroup.DOFade(1, 1.0f);
                loadingCanvas.enabled = false;
                break;
            }

            await UniTask.Yield();
        }

        loadSceneAsync.allowSceneActivation = true;
    }
}
