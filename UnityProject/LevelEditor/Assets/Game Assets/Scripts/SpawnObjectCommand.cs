using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
class SpawnObjectCommand : ICommand {
    private object[] m_execArgs = null;
    private GameObject m_objectPlaced = null;
    
    public void execute(params object[] args) {
        m_execArgs = args;
        if (args.Count<object>() != 3) {
            Debug.LogError("Incorrect number of arguments for SpawnObjectCommand!");
            return;
        }

        if (args[0].GetType() == typeof(GameObject) && args[1].GetType() == typeof(Vector3) && args[2].GetType() == typeof(Quaternion)) {
        
            GameObject toPlace = GameObject.Instantiate((GameObject)args[0]);
            toPlace.layer = LayerMask.NameToLayer("Placable");
            toPlace.transform.position = (Vector3)args[1];//Editor.instance.objectGhost.transform.position;
            toPlace.transform.rotation = (Quaternion)args[2];//Editor.instance.objectGhost.transform.rotation;

            Collider[] pcolliders = toPlace.GetComponentsInChildren<Collider>();
            foreach (Collider c in pcolliders) {
                c.gameObject.layer = LayerMask.NameToLayer("Placable");
            }

            Editor.instance.objects.Add(toPlace);
            m_objectPlaced = toPlace;

        } else {
            Debug.LogError("SpawnObjectCommand requires a correct types as parameters!");
        }
    }

    public void redo() {
        execute(m_execArgs);
    }

    public void undo() {
        GameObject.Destroy(m_objectPlaced);
        Editor.instance.objects.Remove(m_objectPlaced);
    }
}