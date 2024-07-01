using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class DragAndShoot : MonoBehaviour
{
    public GameObject SplatMark; // Add the SplatMark object

    private Vector3 mousePressDownPos;
    private Vector3 mouseReleasePos;

    private Rigidbody rb;
    private CollisionPainter collisionPainter;

    private bool isShoot;
    public float forceMultiplier = 25;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collisionPainter = GetComponent<CollisionPainter>(); // Get the CollisionPainter component

        // Disable collision initially
        GetComponent<Collider>().enabled = false;
        // Enable collision after a short delay
        StartCoroutine(EnableColliderAfterDelay(0.5f));
    }

    private void OnMouseDown()
    {
        mousePressDownPos = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        mouseReleasePos = Input.mousePosition;
        Shoot(mouseReleasePos - mousePressDownPos);
    }

    void Shoot(Vector3 Force)
    {
        if (isShoot)
            return;

        rb.AddForce(new Vector3(Force.x, Force.y, Force.y) * forceMultiplier);
        isShoot = true;

        if (collisionPainter != null)
        {
            collisionPainter.MarkAsThrown(); // Mark the object as thrown
        }

        Debug.Log("Object thrown with force: " + Force);
        Spawner.Instance.NewSpawnRequest();
    }

    IEnumerator EnableColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<Collider>().enabled = true;
    }
}
