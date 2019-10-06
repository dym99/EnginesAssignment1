using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Editor : MonoBehaviour
{
    public static bool placementMode = true;
    public static bool playMode = false;
    public static GameObject templateToCopy = null;
    public static Material ghostMatGreen = null;
    public static Material ghostMatRed = null;

    GameObject m_objectGhost;

    List<GameObject> m_objects;

    public void StartPlayMode() {
        playMode = true;
        foreach (GameObject go in m_objects) {
            Placable p;
            if (go.TryGetComponent<Placable>(out p)) {
                p.OnPlay();
            }
        }
    }

    public void StopPlayMode() {
        playMode = false;
        foreach (GameObject go in m_objects) {
            Placable p;
            if (go.TryGetComponent<Placable>(out p)) {
                p.OnStop();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_objects = new List<GameObject>();
        templateToCopy = Resources.Load<GameObject>("Box");
        ghostMatGreen = Resources.Load<Material>("GhostMatGreen");
        ghostMatRed = Resources.Load<Material>("GhostMatRed");
    }

    // Update is called once per frame
    void Update()
    {
        if (playMode) {
            //Time.timeScale = 1.0f;
            foreach (GameObject go in m_objects) {
                foreach (Rigidbody rb in go.GetComponentsInChildren<Rigidbody>()) {
                    rb.isKinematic = false;
                }
            }
        } else
        if (placementMode) {
            foreach (GameObject go in m_objects) {
                foreach (Rigidbody rb in go.GetComponentsInChildren<Rigidbody>()) {
                    rb.isKinematic = true;
                }
            }
            //Time.timeScale = 0.0f;
            //Placement Mode
            if (templateToCopy!=null) {
                if (m_objectGhost!=null) {
                    Destroy(m_objectGhost);
                }
                m_objectGhost = Instantiate(templateToCopy);
                
                MeshRenderer[] mrs = m_objectGhost.GetComponentsInChildren<MeshRenderer>();
                Material mat = ghostMatRed;
                if (Input.mousePosition.y < Screen.height - 60) {
                    mat = ghostMatGreen;
                }

                foreach (MeshRenderer mr in mrs) {
                    mr.material = mat;
                }

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                Vector3 offset = m_objectGhost.GetComponent<Placable>().offset;

                Physics.Raycast(ray, out hit);
                
                m_objectGhost.transform.position = hit.point+offset;

                Rigidbody[] rbs = m_objectGhost.GetComponentsInChildren<Rigidbody>();
                foreach (Rigidbody rb in rbs) {
                    if (rb != null) {
                        Destroy(rb);
                    }
                }
                Collider[] colliders = m_objectGhost.GetComponentsInChildren<Collider>();
                foreach (Collider c in colliders) {
                    c.isTrigger = true;
                    c.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                }

                //On left click
                if (Input.GetMouseButtonDown(0)) {
                    //Dead zone over UI
                    if (Input.mousePosition.y < Screen.height - 60) {
                        //Place
                        GameObject toPlace = Instantiate(templateToCopy);
                        toPlace.layer = LayerMask.NameToLayer("Default");
                        toPlace.transform.position = m_objectGhost.transform.position;
                        toPlace.transform.rotation = m_objectGhost.transform.rotation;
                        toPlace.transform.localScale = m_objectGhost.transform.localScale;

                        Collider[] pcolliders = toPlace.GetComponentsInChildren<Collider>();
                        foreach (Collider c in pcolliders) {
                            c.gameObject.layer = LayerMask.NameToLayer("Default");
                        }

                        m_objects.Add(toPlace);
                    }
                }
                if (Input.GetMouseButton(1)) {

                }
            }
        }
    }
}
