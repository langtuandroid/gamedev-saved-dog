using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UbiManager : MonoBehaviour
{
    private Dictionary<string, UnityEvent> eventDictionary;
    private List<UnityEvent> eventStack = new List<UnityEvent>();

    public static UbiManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            Instance.Init();
        }
        DontDestroyOnLoad(gameObject);

        Debug.Log("UbiManager start init firebase");
    }
    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }
    }

    public static void StartListening(string eventName, UnityAction listener)
    {
        if (Instance == null) return;
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

   
    private void Update()
    {
        if (Instance == null)
        {
            return;
        }
        while (Instance.eventStack.Count > 0)
        {
            UnityEvent thisEvent = Instance.eventStack[0];
            if (thisEvent != null)
            {
                thisEvent.Invoke();
            }
            Instance.eventStack.RemoveAt(0);
        }
    }

    public GameObject objectLoading;
    private GameObject currentLoading;
    public void ShowLoading(LoadType load)
    {
        if (currentLoading == null)
        {
            currentLoading = Instantiate(objectLoading);
            LoadingScreen ls = currentLoading.GetComponent<LoadingScreen>();
            ls.Show(load);
        }
    }
    public void HideLoading(LoadType load)
    {
        if (currentLoading)
        {
            switch (load)
            {
                case LoadType.Ads:
                    Destroy(currentLoading);
                    currentLoading = null;
                    break;
                case LoadType.Scene:
                    LoadingScreen ls = currentLoading.GetComponent<LoadingScreen>();
                    ls.Hide();
                    Destroy(currentLoading, 0.5f);
                    currentLoading = null;
                    break;
                default:
                    break;
            }
           
        }
    }

    public bool IsLoadingShowing()
    {
        return currentLoading != null;
    }
    private void OnApplicationPause(bool pause)
    {
        Debug.Log("OnApplicationPause " + pause);
    }
    private void OnApplicationFocus(bool focus)
    {
        Debug.Log("OnApplicationFocus " + focus);
    }
}
