using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]		// making sure that the gameobject has a MeshFilter component
[RequireComponent(typeof(MeshRenderer))]	// making sure that the gameobject has a MeshRenderer component
public class MyMesh : MonoBehaviour
{
    Mesh myMesh;
    
    [SerializeField] Vector2 planeSize;
    [SerializeField] int planeResolution;

    List<Vector3> vertices;
    List<int> triangles;
    void OnEnable()
    {
        myMesh = new Mesh();
        GenerateMesh();
        GetComponent<MeshFilter>().mesh = myMesh;
        this.gameObject.AddComponent<MeshCollider>();
    }

    void GenerateMesh()
    {
        vertices = new List<Vector3>();
        float xPerStep = planeSize.x / planeResolution;
        float yPerStep = planeSize.y / planeResolution;

        for (int y = 0; y < planeResolution + 1; y++)
        {
            for(int x = 0; x < planeResolution+1; x++)
            {
                vertices.Add(new Vector3(x*xPerStep,0,y*yPerStep));
            }
        }

        triangles = new List<int>();
        for(int row = 0; row < planeResolution; row++)
        {
            for(int col = 0; col < planeResolution; col++)
            {
                int i = row*planeResolution + row +col;

                triangles.Add(i);
                triangles.Add(i + planeResolution + 1);
                triangles.Add((i+planeResolution)+2);

                triangles.Add(i);
                triangles.Add(i+planeResolution+2);
                triangles.Add(i + 1);
            }
        }

        myMesh.vertices = vertices.ToArray();
        myMesh.triangles = triangles.ToArray();

    }
}
