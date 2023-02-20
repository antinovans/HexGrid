using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public event Action<Vector2Int> onHighlightEvent;
    public event Action onNormalizeEvent;
    public event Action<Vector2Int> onRevertEvent;
    public event Action onSwitchToConstructionEvent;
    public event Action<Vector2Int> onConstructEvent;
    private void Awake() {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    public void HighlightMaterial(Vector2Int gridId)
    {
        onHighlightEvent?.Invoke(gridId);
    }
    public void NormalizeMaterial()
    {
        onNormalizeEvent?.Invoke();
    }
    public void RevertMaterial(Vector2Int gridId)
    {
        onRevertEvent?.Invoke(gridId);
    }
    public void ShowAllAvailableGrids()
    {
        onSwitchToConstructionEvent?.Invoke();
    }
    public void BuildConstruction(Vector2Int gridId)
    {
        onConstructEvent?.Invoke(gridId);
    }
}
