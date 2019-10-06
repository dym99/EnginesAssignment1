using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    [SerializeField]
    private Button m_undoButton;
    [SerializeField]
    private Button m_redoButton;

    void Update() {
        m_undoButton.interactable = Editor.instance.canUndo;
        m_redoButton.interactable = Editor.instance.canRedo;
    }
}
