using HughEnumData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : InteractiveObject
{
    [SerializeField] private Transform tilePatternTransform;

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
    private Vector3 offset;

    private void Start()
    {
        offset = new Vector3(0.2f, 0.3f, 0);
    }

    public void InitialTileSetting(Transform transform, Color color)
    {
        this.tilePatternTransform = transform;
        this.tileColor = color;
    }

    #region InteractiveObject Override
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!IsSetDice)
            {
                InteractiveManager.GetInstance.IsInteractive = true;
                this.Interacitve();
            }
        }
    }

    protected override void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!IsSetDice)
            {
                InteractiveManager.GetInstance.IsInteractive = true;
                this.Interacitve();
            }
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!IsSetDice)
            {
                InteractiveManager.GetInstance.IsInteractive = false;
                this.NotInteractvie();
            }
        }
    }

    protected override void Interacitve()
    {
        GameManager.GetInstance.VisibleInteractiveCanvas(tilePatternTransform, offset, true);
        InteractiveManager.GetInstance.SetPuzzleInteractive(this, this.gameObject);
    }

    protected override void NotInteractvie()
    {
        GameManager.GetInstance.InvisibleInteractiveCanvas();
        InteractiveManager.GetInstance.SetPuzzleInteractive(null, null);
    }

    public override InteractiveType GetInteractiveType()
    {
        return InteractiveType.ThemeFirst_Tile_Pattern;
    }

    #endregion

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

    /// <summary>
    /// 1) 탈출키가 아닌 타일의 경우
    /// 2) 탈출키인데 현재 불빛의 색이 내 색과 다른경우
    /// 2가지 경우에 대해서 ThemeFristPresneter에서 호출한다.
    /// </summary>
    /// <param name="color"></param>
    public void ChangeEmissionColor(Color color)
    {
        if (!IsSetDice)
        {
            //Color finalColor = color * Mathf.LinearToGammaSpace(0.8f);
            if (!IsEscapeKey)
            {
                tileRender.material.SetColor("_EmissionColor", color);
            }
            else if (IsEscapeKey && this.tileColor != color)
            {
                tileRender.material.SetColor("_EmissionColor", color);
            }
            else
            {
                tileRender.material.SetColor("_EmissionColor", Color.black);
            }
        }
    }

    /// <summary>
    /// 탈출키인 타일 위에 최종적으로 주사위를 올려 놨을 때 TileManager에서 호출
    /// </summary>
    public void SetDiceDone()
    {
        tileRender.material.SetColor("_EmissionColor", Color.green);
    }
}
