using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public event Action<Vector2Int> matHighlightEvent;
    public event Action<Vector2Int> matNormalizeEvent;
    private void Awake() {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    public void HighlightMaterial(Vector2Int gridId)
    {
        matHighlightEvent?.Invoke(gridId);
    }
    public void NormalizeMaterial(Vector2Int gridId)
    {
        matNormalizeEvent?.Invoke(gridId);
    }
}
