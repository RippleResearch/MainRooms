using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectMaker : MonoBehaviour
{
    [SerializeField]
    GameObject objToMake, parentObj;
    bool active;
    GameObject newObj;

    private void Start() {
        Debug.Assert(objToMake != null);
        Debug.Assert(parentObj != null);
        active = false;
    }

    private void Update() {
        for (int i = 0; i < Input.touchCount; i++) {
            Touch touch = Input.touches[i];
            if (touch.phase == TouchPhase.Began) {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out hit) && hit.transform.gameObject == parentObj) {
                    Vector3 pos = objToMake.transform.position;
                    pos.y += 10f;
                    newObj = Instantiate(objToMake, pos, new Quaternion(0, 0, 0, 1));
                    //newObj.SetActive(false);
                    newObj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    newObj.GetComponent<Rigidbody>().useGravity = false;
                    newObj.GetComponent<Collider>().enabled = true;
                    active = true;
                }
            }
            if ((touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && active == true) {
                newObj.transform.localScale = Vector3.one;
                newObj.SetActive(true);
                newObj.transform.localScale *= 2f;
            }
            if (touch.phase == TouchPhase.Ended && active == true) {
                newObj.GetComponent<Rigidbody>().useGravity = true;
                Ray ray = Camera.main.ScreenPointToRay(newObj.transform.position);
                newObj.GetComponent<Rigidbody>().AddForce(-ray.direction * 500f);
                newObj = null;
                active = false;
            }
        }
    }
}
