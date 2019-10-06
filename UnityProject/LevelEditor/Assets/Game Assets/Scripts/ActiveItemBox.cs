using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveItemBox : MonoBehaviour
{
    Dropdown dd;
    public void Start() {
        dd = GetComponent<Dropdown>();
    }
    public void OnChange() {
        switch (dd.value) {
            case 0:
                Editor.templateToCopy = Resources.Load<GameObject>("Box");
                break;
            case 1:
                Editor.templateToCopy = Resources.Load<GameObject>("BrickWall");
                break;
            case 2:
                Editor.templateToCopy = Resources.Load<GameObject>("Player");
                break;
            case 3:
                Editor.templateToCopy = Resources.Load<GameObject>("Lamp");
                break;
        }
    }
}
