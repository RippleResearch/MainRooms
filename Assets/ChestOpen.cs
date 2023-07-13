using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpen : MonoBehaviour {

    [SerializeField] GameObject PivotObject;
    bool active;
    bool open;

    void Start() {
        Debug.Assert(PivotObject != null);
        active = false;
        open = false;
    }

    // Update is called once per frame
    void Update() {
        /*for (int i = 0; i < Input.touchCount; i++) {
            Touch touch = Input.touches[i];
            if (touch.phase == TouchPhase.Began) {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.CompareTag("Moveable")) {
                    active = true;
                }
            }
        }*/

        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.CompareTag("Moveable")) {
                active = true;
                Debug.Log("active");
            }
        }

        if(active == true) {
            if (open == false && PivotObject.transform.rotation.x < 40) {
                PivotObject.transform.rotation = new Quaternion(PivotObject.transform.rotation.x + 0.01f,
                    PivotObject.transform.rotation.y, PivotObject.transform.rotation.z,
                    PivotObject.transform.rotation.w);
                Debug.Log("1");
                Debug.Log("Rotation: " + PivotObject.transform.rotation);
            }
            else if (open == true) {
                PivotObject.transform.rotation = new Quaternion(0, 0, 0, PivotObject.transform.rotation.w);
                Debug.Log("2");
            }
        }

        if(PivotObject.transform.rotation.x >= 40) {
            open = true;
            active = false;
            Debug.Log("3");
        }
        if(PivotObject.transform.rotation == new Quaternion(0, 0, 0, PivotObject.transform.rotation.w)) {
            open = false;
            Debug.Log("4");
        }

    }
}
