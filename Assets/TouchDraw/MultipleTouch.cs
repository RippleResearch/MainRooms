// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class MultipleTouch : MonoBehaviour
// {
//     public GameObject circle;
//     public List<touchLocation> touches = new List<touchLocation>();

//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         int i = 0;
//         while(i<Input.touchCount){
//             Touch t = Input.GetTouch(i);
//             if(t.phase==TouchPhase.Began){
//                 Debug.Log("touch began");
//                 touches.Add(new TouchLocation(t.fingerId, CreateCircle(t)));
//             }
//             else if(t.phase==TouchPhase.Ended){
//                 Debug.Log("touch ended");
//                 touchLocation thisTouch = touches.Find(touchLocation => touchLocation.touchId== fingerId);
//                 Destroy(thisTouch.circle);
//                 touches.RemoveAt(touches.IndexOf(thisTouch));
//             }
//             else if(t.phase==TouchPhase.Moved){
//                 Debug.Log("touch is moving");
//             }
//             i++;


//         }
//     }

//     Vector2 GetTouchPosition(Vector2 touchPosition){
//         return GetComponent<Camera>().ScreenToWorldPoint(new Vector3(touch.position.x,touch.position.y,0));//not sure what z should be
//     }

//     GameObject CreateCircle(Touch t){
//         GameObject c = Instantiate(circle) as GameObject;
//         c.name = "Touch" + t.fingerId;
//         c.transform.position = GetTouchPosition(t.position);
//         return c;
//     }

// }
