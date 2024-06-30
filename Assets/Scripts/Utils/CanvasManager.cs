using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UIStateGameObjectPair<T> where T : Enum {
    public T state;
    public GameObject gameObject;
}

public abstract class CanvasManager<T> : MonoBehaviour where T : Enum {
    [SerializeField] private List<UIStateGameObjectPair<T>> uiStateList;

    private Dictionary<T, GameObject> uiStates;
    private T currentState;

    protected virtual void Start() {
        InitializeDictionary();

        currentState = GetDefaultState();
        if (uiStates.ContainsKey(currentState)) {
            uiStates[currentState].SetActive(true);
        }
    }

    public void SetUIState(T newState) {
        if (uiStates.ContainsKey(currentState)) {
            uiStates[currentState].SetActive(false);
        }
        if (uiStates.ContainsKey(newState)) {
            uiStates[newState].SetActive(true);
        }
        currentState = newState;
    }

    public T GetCurrentState() {
        return currentState;
    }

    private void InitializeDictionary() {
        uiStates = new Dictionary<T, GameObject>();
        foreach (var pair in uiStateList) {
            if (!uiStates.ContainsKey(pair.state)) {
                uiStates.Add(pair.state, pair.gameObject);
            }
        }
    }

    private T GetDefaultState() {
        return (T)Enum.GetValues(typeof(T)).GetValue(0);
    }
}
