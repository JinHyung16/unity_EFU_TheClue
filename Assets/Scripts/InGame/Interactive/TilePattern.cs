using HughEnumData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePattern : InteractiveObject
{
    [SerializeField] private Transform tilePatternTransform;

    private Vector3 offset = Vector3.zero;

    [SerializeField] private Sprite tileSprte; //본인의 타일 문양
    [SerializeField] private Color tileColor; //본인의 타일 색깔
    [SerializeField] private string tilePatternName; //본인의 문양 이름
    public bool IsEscapeKey { get; set; } = false; //본인이 탈출 열쇠인지 아닌지
    public bool IsDone { get; set; } = false; //완벽하게 주사위가 배치된 타일인지

    public Sprite TilePatternSprite { get { return this.tileSprte; } }
    public Color TilePatternColor { get { return this.tileColor; } }
    public string TilePatternName { get { return this.tilePatternName; } }

    private void Start()
    {
        offset = new Vector3(0, 0.7f, 0);
    }
    private void OnDestroy()
    {
        GameManager.GetInstance.InvisibleInteractiveCanvas();
        InteracitveOrNot(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.GetInstance.VisibleInteractiveCanvas(tilePatternTransform, offset);
            InteracitveOrNot(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.GetInstance.InvisibleInteractiveCanvas();
            InteracitveOrNot(false);
        }
    }

    public void ChangeEmissionColor()
    {
        if (IsDone || IsEscapeKey)
        {
            return;
        }
        

    }
    public override void InteracitveOrNot(bool interactive)
    {
        if (interactive)
        {
            InteractiveManager.GetInstance.SetInteractiveObject(this, true);
            InteractiveManager.GetInstance.SetTilePatternObject(this.gameObject);
        }
        else
        {
            InteractiveManager.GetInstance.SetInteractiveObject(this, false);
            InteractiveManager.GetInstance.SetTilePatternObject(null);
        }
    }

    public override InteractiveType GetInteractiveType()
    {
        this.myInteractiveType = InteractiveType.ThemeFirst_PatternTile;
        return this.myInteractiveType;
    }
}
