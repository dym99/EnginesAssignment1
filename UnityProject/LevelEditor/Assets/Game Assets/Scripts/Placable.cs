using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectID
{
    BOX,
    BRICKS,
    PLAYER,
    LAMP
}


public class Placable : MonoBehaviour
{
    [SerializeField]
    public Vector3 offset;

    [SerializeField]
    public ObjectID objectID;

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
