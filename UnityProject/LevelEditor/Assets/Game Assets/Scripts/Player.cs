using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody m_rBody;

    private void Start() {
        m_rBody = GetComponent<Rigidbody>();
    }

    public void GhostMe() {
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        //Only update in play mode
        if (!Editor.playMode) {
            return;
        } else if (m_rBody == null) {
            throw new UnityException("Where the player's rigidbody at?");
        } else {
            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            input.Normalize();
            input *= 25.0f * Time.deltaTime;
            m_rBody.AddForce(input, ForceMode.VelocityChange);

        }
    }
}
