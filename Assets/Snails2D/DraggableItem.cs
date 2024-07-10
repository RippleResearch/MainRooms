using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{    
    public Image image;
    public GameObject snailPrefab;
    public Transform parentAfterDrag;

    public void OnBeginDrag(PointerEventData eventData){
        //Debug.Log("Begin Drag");
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData){
        //Debug.Log("Dragging");
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData){
        //Debug.Log("End Drag");
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;

        
        //GameObject snailPrefab = draggableItem.snailPrefab;
        //Debug.Log(snailPrefab.name);
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log("New Snail created");
        GameObject newSnail = Instantiate(snailPrefab,mousePosition,Quaternion.identity);

    }

}
