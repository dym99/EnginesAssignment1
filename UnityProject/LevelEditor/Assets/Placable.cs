using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placable : MonoBehaviour
{
    [SerializeField]
    public Vector3 offset;

    private Vector3 position;
    private Quaternion rotation;

    public void OnStop() {
        transform.position = position;
        transform.rotation = rotation;
    }

    public void OnPlay() {
        position = transform.position;
        rotation = transform.rotation;
    }

    private void Update() {
        if (!Editor.playMode) {
        }
    }
}
