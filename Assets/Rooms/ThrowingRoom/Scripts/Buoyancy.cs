using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy : MonoBehaviour {
    [SerializeField]
    float buoyancy_force;
    float oldDrag;
    // Start is called before the first frame update
    void Start() {
        
    }

    private void OnTriggerEnter(Collider other) {
        oldDrag = other.attachedRigidbody.drag;
        other.attachedRigidbody.drag *= 1.25f;
    }

    private void OnTriggerStay(Collider other) {
        float y_pos = other.transform.position.y;
        if (y_pos < this.transform.position.y) {
            /*if(y_pos > this.transform.position.y - 0.25f && other.attachedRigidbody.velocity.magnitude < 0.25f) {
                other.transform.position.Set(other.transform.position.x, this.transform.position.y, other.transform.position.z);
            }
            else {*/
                other.attachedRigidbody.AddForce(transform.up * buoyancy_force);
            //}
        }
    }

    private void OnTriggerExit(Collider other) {
        other.attachedRigidbody.drag = oldDrag;
        oldDrag = 0;
    }
}