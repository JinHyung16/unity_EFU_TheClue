using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughGenerics;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using JetBrains.Annotations;
using static UnityEngine.Rendering.DebugUI;

public class UIManager : Singleton<UIManager>
{
    //Panel 단위로 UI를 컨트롤 할 때 담을 Dictionary와 Queue
    private Dictionary<string, GameObject> panelDictionary = new Dictionary<string, GameObject>();
    private Queue<GameObject> panelQueue = new Queue<GameObject>();

    //Canvas 단위로 UI를 컨트롤 할 때 담을 Dictionary와 Queue
    private Dictionary<string, Canvas> canvasDictionary = new Dictionary<string, Canvas>();
    private Queue<Canvas> canvasQueue = new Queue<Canvas>();

    #region UI Panel단위로 Control
    /// <summary>
    /// Scene이 바뀔때마다, 해당 Scene에서 사용할 Panel들을 넣어준다.
    /// Panel이름, 해당 Panel GameObject 형식으로 저장해둔다.
    /// </summary>

    public void AddPanelInDictionary(string panelName, GameObject panel)
    {
        if (panelDictionary == null)
        {
            panelDictionary = new Dictionary<string, GameObject>();
        }

        panelQueue.Clear();

        if (panelDictionary.TryGetValue(panelName, out GameObject temp))
        {
            panelDictionary.Clear();
            return;
        }
        else
        {
            panelDictionary.Add(panelName, panel);
            panel.SetActive(false);
        }
    }

    public void ShowPanel(string panelName)
    {
        if (panelDictionary.TryGetValue(panelName, out GameObject obj))
        {
            if (panelQueue.Contains(obj))
            {
                obj.SetActive(false);
                panelQueue.Clear();
            }
            else
            {
                if (0 < panelQueue.Count)
                {
                    var removeObj = panelQueue.Peek();
                    removeObj.SetActive(false);
                    panelQueue.Dequeue();
                }

                panelQueue.Enqueue(obj);
                obj.SetActive(true);
            }
        }
    }

    public void HidePanel()
    {
        if (panelQueue.Count > 0)
        {
            foreach (var panel in panelQueue)
            {
                panel.SetActive(false);
            }
        }
        panelQueue.Clear();
    }

    /// <summary>
    /// 각 Viewer에서 씬 전환전의 호출한다.
    /// </summary>
    public void ClearAllPanel()
    {
        panelDictionary.Clear();
        panelQueue.Clear();
    }
    #endregion

    #region UI Canvas단위로 Control
    /// <summary>
    /// Scene이 바뀔때마다, 해당 Scene에서 사용할 Canvas들을 넣어준다.
    /// Canvas이름, 해당 Canvas 형식으로 저장해둔다.
    /// </summary>

    public void AddCanvasInDictionary(string canvasName, Canvas canvas)
    {
        if (canvasDictionary == null)
        {
            canvasDictionary = new Dictionary<string, Canvas>();
        }
        canvasQueue.Clear();
        if (canvasDictionary.TryGetValue(canvasName, out Canvas obj))
        {
            canvasDictionary.Clear();
            return;
        }
        else
        {
            canvasDictionary.Add(canvasName, canvas);
            canvas.enabled = false;
        }
    }

    public void ShowCanvas(string canvasName)
    {
        if (canvasDictionary.TryGetValue(canvasName, out Canvas canvas))
        { 
            if (canvasQueue.Contains(canvas))
            {
                canvas.enabled = false;
                canvasQueue.Clear();
            }
            else
            {
                if (0 < canvasQueue.Count)
                {
                    var removeObj = canvasQueue.Peek();
                    removeObj.enabled = false;
                    canvasQueue.Dequeue();
                }
                canvasQueue.Enqueue(canvas);
                canvas.enabled = true;
            }
        }
    }

    public void HideCanvas()
    {
        if (canvasQueue.Count > 0)
        {
            foreach (var canvas in canvasQueue)
            {
                canvas.enabled = false;
            }
        }
        canvasQueue.Clear();
    }

    public void ClearAllCanvas()
    {
        canvasDictionary.Clear();
        canvasQueue.Clear();

        Debug.Log("UIManager에서 canvas 초기화함");
    }
    #endregion
}
