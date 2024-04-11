using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Zenject;

public class LinesDrawer : MonoBehaviour
{
    public static LinesDrawer instance;

    public Tilemap tilemap;

    private LineRenderer lineCantDraw;
    private Camera cam;

    [Space (30f)]
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private Gradient lineColor;
    [SerializeField] private Color lineCantDrawColor;
    [SerializeField] private Material lineCantDrawMaterial;
    [SerializeField] private float linePointsMinDistance;
    [SerializeField] private float lineWidth;
    [SerializeField] private int maxLineCanDraw;
    [SerializeField] private Blade blade;

    [SerializeField] private LayerMask cantDrawOverLayer;
    int cantDrawOverLayerIndex;

    ProtectLine currentLine;
    public int currentNumLines;

    private Vector2 mousePos, mouseStart;
    private Vector3 previousMousePos;
    private bool canDraw;

    public event Action OnEndDraw;

    private Transform tf;
    public Transform TF {
        get {
            if (tf == null) {
                tf = transform;
            }
            return tf;
        }
    }
    
    private GameManager _gameManager;
    private LevelManager _levelManager;

    [Inject]
    private void Construct(GameManager gameManager, LevelManager levelManager)
    {
        _gameManager = gameManager;
        _levelManager = levelManager;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
            instance = this;
    }
    void Start()
    {
        InitLoad();
        cantDrawOverLayerIndex = LayerMask.NameToLayer("CantDrawOver");

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
    void Update()
    {
        if (_gameManager.IsState(GameState.GamePlay) && currentNumLines < maxLineCanDraw)
        {

            if (Input.GetMouseButtonDown(0) && !MouseOverLayerCantDraw() && !MouseOverTilemap())
            {
                canDraw = true;
                mouseStart = Input.mousePosition;
                blade.gameObject.SetActive(false);
                HideLineCantDraw();
                BeginDraw();
            }
            if (currentLine != null)
            {
                CalculateInkDraw();
                Draw();
            }
            else
            {
                HideLineCantDraw();
            }
            if (Input.GetMouseButton(0) && currentLine == null && !MouseOverTilemap())
            {
                canDraw = true;
                mouseStart = Input.mousePosition;
                blade.gameObject.SetActive(false);
                BeginDraw();
            }
            if (Input.GetMouseButtonUp(0))
            {
                mousePos = Input.mousePosition;
                EndDraw();
                HideLineCantDraw();
                if (currentLine != null)
                {
                    if (currentLine.pointsCount < 2)
                        return;
                }
                if (TF.childCount > 0 && canDraw /* && !EventSystem.current.IsPointerOverGameObject()*/)
                {
                    CameraShaker.Invoke();
                    OnEndDraw?.Invoke();
                    if (_levelManager.currentLevel.levelNumberInGame != 0) blade.gameObject.SetActive(true);
                    if (_levelManager.currentLevel.levelNumberInGame == 1)
                    {
                        if (DataController.Instance.currentGameData.levelDoneInGame[1] == 0)
                        {
                            Transform tutAttack = _levelManager.currentLevel.transform.Find("TutAttack");
                            tutAttack.gameObject.SetActive(true);
                        }
                    }
                    currentNumLines++;
                    canDraw = false;
                }
            }
            //if (Input.touchCount > 0)
            //{
            //    Touch touch = Input.GetTouch(0);

            //    if (touch.phase == TouchPhase.Began)
            //    {
            //        BeginDraw();
            //    }

            //    if (currentLine != null)
            //    {
            //        CalculateInkDraw();
            //        Draw();
            //    }

            //    if (touch.phase == TouchPhase.Ended)
            //    {
            //        EndDraw();
            //        if (transform.childCount > 0)
            //        {
            //            OnEndDraw?.Invoke();
            //            currentNumLines++;
            //        }
            //    }
            //}
        }
    }

    private void CalculateInkDraw()
    {
        if (Input.mousePosition != previousMousePos && !MouseOverLayerCantDraw())
        {
            float lengthMouseMoved = Mathf.Clamp((Input.mousePosition - previousMousePos).magnitude, 0f, 10f);
            UIManager.Instance.GetUI<UIGameplay>().CurrentInk -= UIManager.Instance.GetUI<UIGameplay>().InkLoseRate * lengthMouseMoved * Time.deltaTime;
            previousMousePos = Input.mousePosition;

            UIManager.Instance.GetUI<UIGameplay>().UpdateInkBar();
        }
        if (UIManager.Instance.GetUI<UIGameplay>().CurrentInk <= 0)
        {
            EndDraw();
        }
    }



    // Begin Draw --------------------------------------------
    void BeginDraw()
    {
        currentLine = Instantiate(linePrefab, TF).GetComponent<ProtectLine>();

        currentLine.UsePhysics(false);
        currentLine.SetLineColor(lineColor);
        currentLine.SetPointMinDistance(linePointsMinDistance);
        currentLine.SetLineWidth(lineWidth);
    }
    // Draw ---------------------------------------------------
    void Draw()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.CircleCast(mousePos, lineWidth, Vector2.zero, 0.2f, cantDrawOverLayer);

        bool isColliding = false;
        if (currentLine == null)
            return;
        if (currentLine.pointsCount >= 1)
        {
            isColliding = Physics2D.Linecast(currentLine.GetLastPoint(), mousePos, cantDrawOverLayer);
        }

        
        if (isColliding || hit)
        {
            //EndDraw();
            DrawRedLineCantDraw(mousePos);
        }
        else
        {
            currentLine.AddPoint(mousePos);
            HideLineCantDraw();
        }
    }
    // End Draw ---------------------------------------------
    void EndDraw()
    {
        if (currentLine != null)
        {
            if (currentLine.pointsCount < 2)
            {
                Destroy(currentLine.gameObject);
            }
            else
            {
                //currentLine.gameObject.layer = cantDrawOverLayerIndex;
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

        // Reset so luong line ve 0
        currentNumLines = 0;

        // Hide duong mau do
        HideLineCantDraw();
    }

    public Vector2[] GetArrayPointsOfLine()
    {
        currentLine = GetComponentInChildren<ProtectLine>();
        if (currentLine != null)
        {
            Vector2[] linePoints = currentLine.points.ToArray();
            return linePoints;
        }
        return null;
    }

    private void DrawRedLineCantDraw(Vector2 mousePos)
    {
        if (currentLine.pointsCount >= 1)
        {
            lineCantDraw.positionCount = 2;
            lineCantDraw.SetPosition(0, currentLine.GetLastPoint());
            lineCantDraw.SetPosition(1, mousePos);
        }
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
        /*RaycastHit2D hit*/ bool hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f, cantDrawOverLayer);
        return hit;
    }
    private bool MouseOverTilemap()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = tilemap.WorldToCell(mousePos);
        TileBase tile = tilemap.GetTile(cellPos);

        if (tile != null)
        {
            return true;
        }
        return false;
    }
}
