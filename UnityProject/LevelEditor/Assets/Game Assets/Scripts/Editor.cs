using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editor : MonoBehaviour
{
    #region SINGLETON_GAMEOBJECT
    public static Editor instance = null;
    private void Awake() {
        if (instance)
            Destroy(this);
        else
            instance = this;
    }
    #endregion

    public static bool placementMode = true;
    public static bool playMode = false;
    public static GameObject templateToCopy = null;
    public static Material ghostMatGreen = null;
    public static Material ghostMatRed = null;

    public GameObject objectGhost;

    public List<GameObject> objects;

    Stack<ICommand> m_undoStack;
    Stack<ICommand> m_redoStack;

    public bool canUndo = false;
    public bool canRedo = false;

    public void ClearAll() {
        foreach (GameObject go in objects) {
            Destroy(go);
        }
        objects.Clear();
        m_undoStack.Clear();
        m_redoStack.Clear();
    }

    public void StartPlayMode() {
        playMode = true;
        foreach (GameObject go in objects) {
            Placable p;
            if (go.TryGetComponent<Placable>(out p)) {
                p.OnPlay();
            }
        }
    }

    public void StopPlayMode() {
        playMode = false;
        foreach (GameObject go in objects) {
            Placable p;
            if (go.TryGetComponent<Placable>(out p)) {
                p.OnStop();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_undoStack = new Stack<ICommand>();
        m_redoStack = new Stack<ICommand>();
        objects = new List<GameObject>();
        templateToCopy = Resources.Load<GameObject>("Box");
        ghostMatGreen = Resources.Load<Material>("GhostMatGreen");
        ghostMatRed = Resources.Load<Material>("GhostMatRed");
    }

    public void SpawnObject() {
        ICommand cmd = new SpawnObjectCommand();
        cmd.execute(templateToCopy, objectGhost.transform.position, objectGhost.transform.rotation);
        m_undoStack.Push(cmd);
        m_redoStack.Clear();
    }

    public void UndoLastCommand() {
        if (m_undoStack.Count > 0) {
            m_undoStack.Peek().undo();
            m_redoStack.Push(m_undoStack.Peek());
            m_undoStack.Pop();
        }
    }

    public void RedoLastCommand() {
        if (m_redoStack.Count > 0) {
            m_redoStack.Peek().redo();
            m_undoStack.Push(m_redoStack.Peek());
            m_redoStack.Pop();
        }
    }


    // Update is called once per frame
    void Update()
    {
        canUndo = m_undoStack.Count > 0;
        canRedo = m_redoStack.Count > 0;
        if (playMode) {
            //Time.timeScale = 1.0f;
            foreach (GameObject go in objects) {
                foreach (Rigidbody rb in go.GetComponentsInChildren<Rigidbody>()) {
                    rb.isKinematic = false;
                }
            }
        } else
        if (placementMode) {
            foreach (GameObject go in objects) {
                foreach (Rigidbody rb in go.GetComponentsInChildren<Rigidbody>()) {
                    rb.isKinematic = true;
                }
            }
            //Time.timeScale = 0.0f;
            //Placement Mode
            if (templateToCopy!=null) {
                if (objectGhost!=null) {
                    Destroy(objectGhost);
                }
                objectGhost = Instantiate(templateToCopy);
                objectGhost.SendMessage("GhostMe", SendMessageOptions.DontRequireReceiver);
                MeshRenderer[] mrs = objectGhost.GetComponentsInChildren<MeshRenderer>();
                Material mat = ghostMatRed;
                if (Input.mousePosition.y < Screen.height - 120) {
                    mat = ghostMatGreen;
                }

                foreach (MeshRenderer mr in mrs) {
                    mr.material = mat;
                }

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                Vector3 offset = objectGhost.GetComponent<Placable>().offset;

                Physics.Raycast(ray, out hit);

                //If you actually hit something...
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("World") || hit.collider.gameObject.layer == LayerMask.NameToLayer("Placable")) {

                    objectGhost.transform.position = hit.point + offset;

                    Rigidbody[] rbs = objectGhost.GetComponentsInChildren<Rigidbody>();
                    foreach (Rigidbody rb in rbs) {
                        if (rb != null) {
                            Destroy(rb);
                        }
                    }
                    Collider[] colliders = objectGhost.GetComponentsInChildren<Collider>();
                    foreach (Collider c in colliders) {
                        c.isTrigger = true;
                        c.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                    }

                    //On left click
                    if (Input.GetMouseButtonDown(0)) {
                        //Dead zone over UI
                        if (Input.mousePosition.y < Screen.height - 120) {
                            //Place
                            SpawnObject();
                            //GameObject toPlace = Instantiate(templateToCopy);
                            //toPlace.layer = LayerMask.NameToLayer("Placable");
                            //toPlace.transform.position = objectGhost.transform.position;
                            //toPlace.transform.rotation = objectGhost.transform.rotation;
                            //toPlace.transform.localScale = objectGhost.transform.localScale;
                            //
                            //Collider[] pcolliders = toPlace.GetComponentsInChildren<Collider>();
                            //foreach (Collider c in pcolliders) {
                            //    c.gameObject.layer = LayerMask.NameToLayer("Placable");
                            //}
                            //
                            //objects.Add(toPlace);
                        }
                    }
                    //Remove with right click
                    if (Input.GetMouseButton(1)) {
                        Placable p;
                        if (hit.collider.gameObject.TryGetComponent<Placable>(out p)) {
                            objects.Remove(p.gameObject);
                            Destroy(p.gameObject);
                        }
                    }

                } else {
                    foreach(MeshRenderer mr in mrs) {
                        mr.material = ghostMatRed;
                    }
                }
            }
        }
    }
}
