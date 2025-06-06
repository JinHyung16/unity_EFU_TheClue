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

        hitCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gradStudent.IsAttackDone)
        {
            if (tokenSource != null)
            {
                tokenSource.Cancel();
                tokenSource.Dispose();
            }
            tokenSource = new CancellationTokenSource();

            hitCount += 1;

            ThemeThirdPresenter.GetInstance.EnemyHitToPlayer(hitCount);

            volume.profile.TryGet(out this.chromaticAberration);
            chromaticAberration.intensity.Override(1.0f);

            if (3 < hitCount)
            {
                ThemeThirdPresenter.GetInstance.GameClear(false);
            }

            gradStudent.IsAttackDone = false;
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
