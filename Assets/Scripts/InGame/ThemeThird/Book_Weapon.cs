using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public class Book_Weapon : MonoBehaviour
{
    [SerializeField] private GradStudent gradStudent;

    [SerializeField] private Volume volume;
    private ChromaticAberration chromaticAberration;

    private CancellationTokenSource tokenSource;

    private int hitCount = 0;

    private void Start()
    {
        if (tokenSource != null)
        {
            tokenSource.Dispose();
        }
        tokenSource = new CancellationTokenSource();

        volume.profile.TryGet(out this.chromaticAberration);
        chromaticAberration.intensity.Override(0.0f);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gradStudent.IsAttackTime)
        {
            if (tokenSource != null)
            {
                tokenSource.Cancel();
                tokenSource.Dispose();
            }
            tokenSource = new CancellationTokenSource();

            hitCount += 1;
            ThemeThirdPresenter.GetInstance.EnemyHitToPlayer();
            if (3 < hitCount)
            {
                ThemeThirdPresenter.GetInstance.GameClear(false);
            }

            Debug.Log("책과 player충돌");
            volume.profile.TryGet(out this.chromaticAberration);
            chromaticAberration.intensity.Override(1.0f);

            ChromaticEffect().Forget();
        }
    }


    private async UniTaskVoid ChromaticEffect()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(2.5f), cancellationToken: tokenSource.Token);
        volume.profile.TryGet(out this.chromaticAberration);
        chromaticAberration.intensity.Override(0.0f);
        tokenSource.Cancel();
    }

}
