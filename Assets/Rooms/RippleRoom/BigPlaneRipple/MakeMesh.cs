using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]		// making sure that the gameobject has a MeshFilter component
[RequireComponent(typeof(MeshRenderer))]	// making sure that the gameobject has a MeshRenderer component
public class MakeMesh : MonoBehaviour
{

    //float waterLevelY;          // the water - surface's level (along the Y - axis)
    float surfaceActualWidth;   // surface - dimension along the X-axis
    float surfaceActualLength;  // surface - dimension along the Z-axis
    int surfaceWidthPoints;     // number of points on the X-axis
    int surfaceLengthPoints;    // number of points on the Z-axis
    // Start is called before the first frame update
    void OnEnable()
    {
        int width = 16;
        int height = 9;

        surfaceActualWidth = width;
        surfaceActualLength = height;
        surfaceWidthPoints = width * 10;
        surfaceLengthPoints = height * 10;

        CreateMesh();
        //Reset Collider after generation
        this.gameObject.AddComponent<MeshCollider>();
    }

    private void CreateMesh()
    {
        /* This function creates the mesh object - triangle by triangle - 
		 * and then applies it to the Mesh Filter's mesh. */
        Mesh newMesh = new Mesh
        {
            name = "Procedural Mesh"
        };
        List<Vector3> verticeList = new List<Vector3>();    // list that will hold the mesh vertices
        List<Vector2> uvList = new List<Vector2>();         // list that will hold the mesh UVs
        List<int> triList = new List<int>();                // list that will hold the mesh triangles
                                                            //mesh - data creation double loop
        for (int i = 0; i < surfaceWidthPoints; i++)
        {
            for (int j = 0; j < surfaceLengthPoints; j++)
            {
                float x = MapValue(i, 0.0f, surfaceWidthPoints, -surfaceActualWidth / 2.0f, surfaceActualWidth / 2.0f);
                float z = MapValue(j, 0.0f, surfaceLengthPoints, -surfaceActualLength / 2.0f, surfaceActualLength / 2.0f);
                verticeList.Add(new Vector3(x, 0f, z));
                uvList.Add(new Vector2(x, z));
                //Skip if a new square on the plane hasn't been formed
                if (i == 0 || j == 0)
                    continue;
                //Adds the index of the three vertices in order to make up each of the two tris
                triList.Add(surfaceLengthPoints * i + j); //Top right
                triList.Add(surfaceLengthPoints * i + j - 1); //Bottom right
                triList.Add(surfaceLengthPoints * (i - 1) + j - 1); //Bottom left - First triangle
                triList.Add(surfaceLengthPoints * (i - 1) + j - 1); //Bottom left 
                triList.Add(surfaceLengthPoints * (i - 1) + j); //Top left
                triList.Add(surfaceLengthPoints * i + j); //Top right - Second triangle
            }
        }
        //creating the mesh with the data generated above
        newMesh.vertices = verticeList.ToArray();   //pass vertices to mesh
        newMesh.uv = uvList.ToArray();              //pass uv list to mesh
        newMesh.triangles = triList.ToArray();      //pass triabgles to mesh
        newMesh.RecalculateNormals();               //recalculate mesh normals
        GetComponent<MeshFilter>().mesh = newMesh;  //pass the created mesh to the mesh filter
    }

    private float MapValue(float refValue, float refMin, float refMax, float targetMin, float targetMax)
    {
        /* This function converts the value of a variable (reference value) from one range (reference range) to another (target range)
		in this example it is used to convert the x and z value to the correct range, while creating the mesh, in the CreateMesh() function*/
        return targetMin + (refValue - refMin) * (targetMax - targetMin) / (refMax - refMin);
    }
}
