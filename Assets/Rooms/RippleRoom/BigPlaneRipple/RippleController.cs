using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[RequireComponent(typeof(MeshFilter))]		// making sure that the gameobject has a MeshFilter component
[RequireComponent(typeof(MeshRenderer))]	// making sure that the gameobject has a MeshRenderer component
public class RippleController : MonoBehaviour
{
    /////////////WAVE FIELDS//////////////
    [Range(0, 10)]
    public float scale;
    public float speed;
    [Range(.5f, 1f)]
    public float dispersion;
    [Range(0, 1.5f)]
    public float waveSpreadSpeed;
    public float zoom;
    /////////////WAVE FIELDS//////////////

    [Range(0f, 3f)]
    public float defaultAmp;
    [Range(0f, 1f)]
    public float ampModifier;


    private Mesh waveMesh;

    List<WaveInfo> Waves;
    /*int coreCount;*/
    public class WaveInfo
    {
        //Impact location
        public Vector3 Impact { get; set; }
        //Distace from object (center)
        public Vector2 Dir { get; set; }
        public float RenderDist { get; set; }
        //distance / mesh bounds?
        public Vector3 OffSet { get; set; }
        //strenght of wave
        public float WaveAmp { get; set; }

        public WaveInfo(Vector3 Impact, Vector2 Dir, float RenderDist, float WaveAmp, Vector3 OffSet)
        {
            this.Impact = Impact;
            this.Dir = Dir;     
            this.RenderDist = RenderDist;
            this.WaveAmp = WaveAmp;
            this.OffSet = OffSet;
        }
    }
    void Start()
    {
        waveMesh = GetComponent<MeshFilter>().mesh;
        Waves = new List<WaveInfo>();
        //Defualts:
        scale = .09f;
        speed = .2f;
        dispersion = .93f;
        waveSpreadSpeed = .08f;
        zoom = 2f;
        defaultAmp = 1f;
        ampModifier = .5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckTouches();
        Vector3[] verts = waveMesh.vertices;
        float time = Time.time;

         Parallel.For(0, verts.Length, i =>
        {
            float cirFunc = zoom * (Mathf.Pow(verts[i].x, 2) + Mathf.Pow(verts[i].z, 2));
            float y = 0;
            //Loop through Waves and add offsets if they are within the correct distance
            for (int j = 0; j < Waves.Count; j++)
            {
                if (Vector3.Distance(verts[i], Waves[j].Impact) < Waves[j].RenderDist)
                {
                    y += scale * MathF.Sin(time * (-speed) + cirFunc + (verts[i].z * Waves[j].OffSet).z * zoom + (verts[i].x * Waves[j].OffSet.x * zoom)) * Waves[j].WaveAmp;
                }
            }
            y += scale * Mathf.Sin(2*time * (-speed) + 3*(verts[i].x + verts[i].z)); //Diaganol Wave When Idle
            verts[i] = new Vector3(verts[i].x, y, verts[i].z);
       });
        changeAmps();
        waveMesh.vertices = verts;
        waveMesh.RecalculateNormals();
    }

    public Vector3[] ChangeToWorld(Vector3[] verts)
    {
        Vector3[] worldVerts = new Vector3[verts.Length];
        for(int i = 0; i < verts.Length; i++)
        {
            worldVerts[i] = transform.TransformVector(verts[i]);
        }
        return worldVerts;
    }

    public void changeAmps()
    {
        for (int i = 0; i < Waves.Count; i++)
        {
            if (Waves[i].WaveAmp > 0.00390625f)//1/256 (so small to remove what looks like stuttering)
            {
                Waves[i].RenderDist += waveSpreadSpeed;
                Waves[i].WaveAmp *= dispersion;
            }

            if (Waves[i].WaveAmp <= 0.00390625f) //1/256 (so small to remove what looks like stuttering)
                Waves.RemoveAt(i);
        }
    }

    void CheckTouches()
    {
        if (Input.touchCount > 0)
            for (int i = 0; i < Input.touchCount; i++)
                NewTouchWave(Input.GetTouch(i)); 

        // If this line is used and multiple touch points are registered 
        // a new wave will be created at the average location between thte touch points
        /*if (Input.GetMouseButton(0))
                NewMouseWave();*/
    }
    private void NewTouchWave(Touch touch)
    {
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        if (Physics.Raycast(ray, out var hitInfo)) //Still need to check if it is in bounds
        {
            if (touch.phase == TouchPhase.Began)
                defaultAmp = Mathf.Clamp(Math.Abs(touch.radius), 0, 5) * ampModifier;

            Vector2 dir = new Vector2(this.transform.position.x - hitInfo.point.x, this.transform.position.z - hitInfo.point.z);
            WaveInfo wave = new WaveInfo(new Vector3(hitInfo.point.x, 0, hitInfo.point.z), dir, 0, defaultAmp,
                new Vector3(dir.x - hitInfo.point.x, 0, dir.y - hitInfo.point.z));
            Waves.Add(wave);
        }
    }

    private void NewMouseWave()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo)) //Still need to check if it is in bounds
        {
            Vector2 dir = new Vector2(this.transform.position.x - hitInfo.point.x, this.transform.position.z - hitInfo.point.z);
            WaveInfo wave = new WaveInfo( new Vector3(hitInfo.point.x, 0, hitInfo.point.z), dir, 0, defaultAmp,
                new Vector3(dir.x - hitInfo.point.x, 0,dir.y - hitInfo.point.z));
            Waves.Add(wave);
        }
    }
}

