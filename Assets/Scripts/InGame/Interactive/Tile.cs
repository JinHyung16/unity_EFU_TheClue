using HughEnumData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Tile : InteractiveObject
{
    [SerializeField] private Transform tilePatternTransform;

    private Vector3 offset = Vector3.zero;

    [SerializeField] private MeshRenderer tileRender;

    [Header("Tile의 Sprite")]
    [SerializeField] Sprite tileSprte; //본인의 타일 문양
    [SerializeField] private string tilePatternName; //본인의 문양 이름

    private Color tileColor = Color.white; //본인의 타일 색깔, 기본은 흰색

    public bool IsEscapeKey { get; set; } = false; //본인이 탈출 열쇠인지 아닌지
    public bool IsSetDice { get; set; } = false; //완벽하게 주사위가 배치된 타일인지

    public int IsColorChangeNum { get; set; } = 1;

    public Sprite TilePatternSprite { get { return this.tileSprte; } }
    public Color TileColor { get { return this.tileColor; } }
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

    public void SetActiveTile(bool active)
    {
        if (active)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
    public void InitialTileSetting(Transform transform, Color color)
    {
        this.tilePatternTransform = transform;
        this.tileColor = color;
    }

    /// <summary>
    /// EscapeKey가 아닌 tile의 경우 실행된다.
    /// </summary>
    /// <param name="transform"> 변경할 위치를 받는다 </param>
    public void ChangeTilePosition(Transform transform)
    {
        if (!IsEscapeKey)
        {
            this.tilePatternTransform = transform;
        }
    }

    public void ChangeEmissionColor(Color color)
    {
        if (!IsEscapeKey)
        {
            tileRender.material.SetColor("_EmissionColor", color * Mathf.LinearToGammaSpace(2.0f));
        }
        else if (IsEscapeKey && this.tileColor != color)
        {
            tileRender.material.SetColor("_EmissionColor", color * Mathf.LinearToGammaSpace(2.0f));
        }
    }
}
