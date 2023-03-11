using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughGenerics;
using UnityEngine.EventSystems;

public class UIManager : Singleton<UIManager>
{
    /// <summary>
    /// Scene이 바뀔때마다, 해당 Scene에서 사용할 Panel들을 넣어준다.
    /// Panel이름, 해당 Panel GameObject 형식으로 저장해둔다.
    /// </summary>
    private Dictionary<string, GameObject> panelDictionary = new Dictionary<string, GameObject>();
    private Queue<GameObject> panelQueue = new Queue<GameObject>();

    public void AddPanelInDictionary(string panelName, GameObject panel)
    {
        panelDictionary.Add(panelName, panel);
        panel.SetActive(false);
    }

    /// <summary>
    /// Panel을 열 때 호출
    /// 해당 panel을 stack에 넣어 관리한다.
    /// </summary>
    /// <param name="panelName">열고싶은 Panel 이름</param>
    public void ShowPanel(string panelName)
    {
        if (panelDictionary.TryGetValue(panelName, out GameObject obj))
        {
            if (panelQueue.Contains(obj))
            {
                var removeObj = panelQueue.Peek();
                obj.SetActive(false);
                panelQueue.Clear();
            }
            else
            {
                if (panelQueue.Count > 0)
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

    /// <summary>
    /// Panel을 닫을 때 호출된다.
    /// 해당 panel이 존재하는 stack까지 그 위에 쌓인것들도 다 닫아준다.
    /// </summary>
    /// <param name="panelName"> 닫을 panel의 이름 </param>
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
    /// CanvasManager를 상속받은 각 Canvas에서 OnDestroy()시 호출한다.
    /// </summary>
    public void ClearAll()
    {
        panelDictionary.Clear();
        panelQueue.Clear();
    }
}
