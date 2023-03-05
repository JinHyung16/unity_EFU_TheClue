using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughCanvas;
using HughUIType;
using HughGenerics;

public class UIManager : Singleton<UIManager>
{
    /// <summary>
    /// Canvas들은 CanvasManager를 상속받아 UIManager에게 본인의 CanvasType과 본인을 넣어준다.
    /// 그리고 UIManager에서 Control을 진행한다
    /// </summary>
    private Dictionary<CanvasType, CanvasManager> canvasDictionary = new Dictionary<CanvasType, CanvasManager>();
    private Queue<CanvasManager> canvasQueue = new Queue<CanvasManager>();

    public void AddCanvasInDictionary(CanvasType canvasType, CanvasManager canvasManager)
    {
        canvasDictionary.Add(canvasType, canvasManager);
        if (canvasType != CanvasType.FixedCanvas)
        {
            canvasManager.HideCanvas();
        }
    }

    public void ShowCanvas<T>(CanvasType canvasType) where T : CanvasManager
    {
        canvasDictionary[canvasType].ShowCanvas();

        if (canvasType == CanvasType.FixedCanvas)
        {
            if (canvasQueue.Count > 0)
            {
                foreach (var panel in canvasQueue)
                {
                    panel.gameObject.SetActive(false);
                }
                canvasQueue.Clear();
            }
        }

        if (canvasDictionary.TryGetValue(canvasType, out CanvasManager obj) && canvasType != CanvasType.FixedCanvas)
        {
            if (canvasQueue.Contains(obj))
            {
                obj.HideCanvas();
                canvasQueue.Clear();
            }
            else
            {
                if (canvasQueue.Count > 0)
                {
                    var removeObj = canvasQueue.Peek();
                    removeObj.HideCanvas();
                    canvasQueue.Dequeue();
                }

                canvasQueue.Enqueue(obj);
                obj.ShowCanvas();
            }
        }
    }

    /// <summary>
    /// CanvasManager를 상속받은 각 Canvas에서 OnDestroy()시 호출한다.
    /// </summary>
    public void ClearAllCanvas()
    {
        canvasDictionary.Clear();
        canvasQueue.Clear();
    }
}
