using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class UIManager : MonoBehaviour
{
    private readonly List<UICanvas> backCanvas = new List<UICanvas>();
    
    [SerializeField] private Transform parentCanvas;
    
    private UICanvas[] uiResources;
    private Dictionary<System.Type, UICanvas> canvasPrefab = new Dictionary<System.Type, UICanvas>();
    private Dictionary<System.Type, UICanvas> canvas = new Dictionary<System.Type, UICanvas>();
    private Dictionary<UICanvas, UnityAction> backActionEvent = new Dictionary<UICanvas, UnityAction>();

    [Inject] private DiContainer _diContainer;
   
    public void OpenUI<T>() where T : UICanvas
    {
        UICanvas canvas = GetUI<T>();

        canvas.Setup();
        canvas.Open();
    }
    
    private UICanvas BackTopUI
    {
        get
        {
            UICanvas canvas = null;
            if (backCanvas.Count > 0)
            {
                canvas = backCanvas[backCanvas.Count - 1];
            }

            return canvas;
        }
    }

    public void CloseUI<T>() where T : UICanvas
    {
        if (IsOpened<T>())
        {
            GetUI<T>().CloseImmediately();
        }
    }

    public bool IsOpened<T>() where T : UICanvas
    {
        return IsLoaded<T>() && canvas[typeof(T)].gameObject.activeInHierarchy;
    }
    
    public bool IsLoaded<T>() where T : UICanvas
    {
        System.Type type = typeof(T);
        return canvas.ContainsKey(type) && canvas[type] != null;
    }
    
    public T GetUI<T>() where T : UICanvas
    {
        if (IsLoaded<T>())
        {
            return this.canvas[typeof(T)] as T;
        }

        UICanvas canvas = _diContainer.InstantiatePrefabForComponent<UICanvas>(GetUIPrefab<T>(), parentCanvas);
        this.canvas[typeof(T)] = canvas;

        return this.canvas[typeof(T)] as T;
    }

    private T GetUIPrefab<T>() where T : UICanvas
    {
        if (canvasPrefab.ContainsKey(typeof(T)))
        {
            return canvasPrefab[typeof(T)] as T;
        }

        uiResources ??= Resources.LoadAll<UICanvas>("UI/");

        foreach (UICanvas t in uiResources)
        {
            if (t is not T)
            {
                continue;
            }

            canvasPrefab[typeof(T)] = t;
            break;
        }

        return canvasPrefab[typeof(T)] as T;
    }

    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Escape) && BackTopUI != null)
        {
            backActionEvent[BackTopUI]?.Invoke();
        }
    }

    public void PushBackAction(UICanvas canvas, UnityAction action)
    {
        backActionEvent.TryAdd(canvas, action);

    }

    public void AddBackUI(UICanvas canvas)
    {
        if (!backCanvas.Contains(canvas))
        {
            backCanvas.Add(canvas);
        }
    }

    public void RemoveBackUI(UICanvas canvas)
    {
        backCanvas.Remove(canvas);
    }
}
