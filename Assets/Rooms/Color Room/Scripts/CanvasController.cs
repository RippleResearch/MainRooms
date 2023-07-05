using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public GameObject SideMenu;
    public void OnEnable() {
        SideMenu.GetComponent<RectTransform>().parent = gameObject.GetComponent<RectTransform>();
    }
    public void Reposition(int height, int width) {
        transform.position = new Vector3(height/2f, 1, width/2f /*- (camPos.z * .08f)*/);//1 to be above blocks

        RectTransform rt = gameObject.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(width * 2f, height * 2f);

        updateSideBar(width);
    }

    public void updateSideBar(float width) {
       SideMenu.GetComponent<RectTransform>().sizeDelta = new Vector2(width / 2f, gameObject.GetComponent<RectTransform>().sizeDelta.y);
    }
}
