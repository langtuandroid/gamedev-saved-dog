using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class LinesDrawer : MonoBehaviour
{
    public event Action OnEndDraw;
    
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private Gradient lineColor;
    [SerializeField] private Color lineCantDrawColor;
    [SerializeField] private Material lineCantDrawMaterial;
    [SerializeField] private float linePointsMinDistance;
    [SerializeField] private float lineWidth;
    [SerializeField] private int maxLineCanDraw;
    [SerializeField] private Blade blade;

    [SerializeField] private LayerMask cantDrawOverLayer;


    public Tilemap tilemap;
    private LineRenderer lineCantDraw;
    private Camera cam;
    private ProtectLine currentLine;
    private Vector2 mousePos;
    private Vector3 previousMousePos;
    private int currentNumLines;
    private bool canDraw;


    private Transform tf;
    private Transform TF {
        get
        {
            if (tf == null)
            {
                tf = transform;
            }
            return tf;
        }
    }
    
    private GameManager _gameManager;
    private LevelManager _levelManager;
    private DataController _dataController;
    private UIManager _uiManager;

    [Inject]
    private void Construct(GameManager gameManager, LevelManager levelManager, DataController dataController, UIManager uiManager)
    {
        _gameManager = gameManager;
        _levelManager = levelManager;
        _dataController = dataController;
        _uiManager = uiManager;
    }

    private void Start()
    {
        InitLoad();
        LayerMask.NameToLayer("CantDrawOver");

        previousMousePos = Input.mousePosition;

        InitLineCantDraw();
    }
    
    private void InitLoad()
    {
        cam = Camera.main;
    }
    
    private void InitLineCantDraw()
    {
        lineCantDraw = gameObject.AddComponent<LineRenderer>();
        lineCantDraw.startColor = lineCantDrawColor;
        lineCantDraw.endColor = lineCantDrawColor;
        lineCantDraw.startWidth = 0.1f;
        lineCantDraw.endWidth = 0.1f;
        lineCantDraw.sortingLayerName = "Line";
        lineCantDraw.material = lineCantDrawMaterial;
    }

    private void Update()
    {
        if (!_gameManager.IsState(GameState.GamePlay) || currentNumLines >= maxLineCanDraw)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && !MouseOverLayerCantDraw() && !MouseOverTilemap())
        {
            canDraw = true;
            blade.gameObject.SetActive(false);
            HideLineCantDraw();
            BeginDraw();
        }
        if (currentLine != null)
        {
            CalculateInkDraw();
            Draw();
        } else
        {
            HideLineCantDraw();
        }
        if (Input.GetMouseButton(0) && currentLine == null && !MouseOverTilemap())
        {
            canDraw = true;
            blade.gameObject.SetActive(false);
            BeginDraw();
        }

        if (!Input.GetMouseButtonUp(0))
        {
            return;
        }

        mousePos = Input.mousePosition;
        EndDraw();
        HideLineCantDraw();
        if (currentLine != null)
        {
            if (currentLine.PointsCount < 2)
                return;
        }

        if (TF.childCount <= 0 || !canDraw)
        {
            return;
        }

        CameraShaker.Invoke();
        OnEndDraw?.Invoke();
        if (_levelManager.currentLevel.LevelNumberInGame != 0) blade.gameObject.SetActive(true);
        if (_levelManager.currentLevel.LevelNumberInGame == 1)
        {
            if (_dataController.currentGameData.levelDoneInGame[1] == 0)
            {
                Transform tutAttack = _levelManager.currentLevel.transform.Find("TutAttack");
                tutAttack.gameObject.SetActive(true);
            }
        }
        currentNumLines++;
        canDraw = false;
    }

    private void CalculateInkDraw()
    {
        if (Input.mousePosition != previousMousePos && !MouseOverLayerCantDraw())
        {
            float lengthMouseMoved = Mathf.Clamp((Input.mousePosition - previousMousePos).magnitude, 0f, 10f);
            _uiManager.GetUI<UIGameplay>().CurrentInk -= _uiManager.GetUI<UIGameplay>().InkLoseRate * lengthMouseMoved * Time.deltaTime;
            previousMousePos = Input.mousePosition;

            _uiManager.GetUI<UIGameplay>().UpdateInkBar();
        }
        if (_uiManager.GetUI<UIGameplay>().CurrentInk <= 0)
        {
            EndDraw();
        }
    }

    private void BeginDraw()
    {
        currentLine = Instantiate(linePrefab, TF).GetComponent<ProtectLine>();

        currentLine.UsePhysics(false);
        currentLine.SetLineColor(lineColor);
        currentLine.SetPointMinDistance(linePointsMinDistance);
        currentLine.SetLineWidth(lineWidth);
    }
    
    private void Draw()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.CircleCast(mousePos, lineWidth, Vector2.zero, 0.2f, cantDrawOverLayer);

        bool isColliding = false;
        if (currentLine == null)
            return;
        
        if (currentLine.PointsCount >= 1)
        {
            isColliding = Physics2D.Linecast(currentLine.GetLastPoint(), mousePos, cantDrawOverLayer);
        }

        
        if (isColliding || hit)
        {
            DrawRedLineCantDraw(mousePos);
        } else
        {
            currentLine.AddPoint(mousePos);
            HideLineCantDraw();
        }
    }
    
    private void EndDraw()
    {
        if (currentLine != null)
        {
            if (currentLine.PointsCount < 2)
            {
                Destroy(currentLine.gameObject);
            } else
            {
                currentLine.UsePhysics(true);
                currentLine = null;
            }
        }
        HideLineCantDraw();
    }

    public void OnLoadNewLevelOrUI()
    {
        foreach(Transform tfChild in TF)
        {
            Destroy(tfChild.gameObject);
        }
        
        currentNumLines = 0;

        HideLineCantDraw();
    }

    public Vector2[] GetArrayPointsOfLine()
    {
        currentLine = GetComponentInChildren<ProtectLine>();

        if (currentLine == null)
        {
            return null;
        }

        Vector2[] linePoints = currentLine.points.ToArray();
        return linePoints;
    }

    private void DrawRedLineCantDraw(Vector2 mousePos)
    {
        if (currentLine.PointsCount < 1)
        {
            return;
        }

        lineCantDraw.positionCount = 2;
        lineCantDraw.SetPosition(0, currentLine.GetLastPoint());
        lineCantDraw.SetPosition(1, mousePos);
    }
    
    public void HideLineCantDraw()
    {
        if (lineCantDraw == null)
            return;
        lineCantDraw.positionCount = 0;
    }
    
    private bool MouseOverLayerCantDraw()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition); 
        bool hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f, cantDrawOverLayer);
        return hit;
    }
    
    private bool MouseOverTilemap()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = tilemap.WorldToCell(mousePos);
        TileBase tile = tilemap.GetTile(cellPos);

        return tile != null;
    }
}
