using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public Material waveMaterial;
    public ComputeShader waveCompute;
    public RenderTexture NState, NM1State, Np1State;
    public Vector2Int resolution;

    public RenderTexture obstaclesTex;

    public Vector3 effect; // x cord, y coord, strenght
    // Start is called before the first frame update
    [SerializeField] public float dispersion = 0.98f;
    void Start()
    {
        InitilizeTexture(ref NState);
        InitilizeTexture(ref NM1State);
        InitilizeTexture(ref Np1State);
        
        obstaclesTex.enableRandomWrite = true;
        Debug.Assert(obstaclesTex.width == resolution.x && obstaclesTex.height == resolution.y);

        waveMaterial.mainTexture = NState;
    } 

    void InitilizeTexture(ref RenderTexture tex)
    {
        tex = new RenderTexture(resolution.x, resolution.y, 1, UnityEngine.Experimental.Rendering.GraphicsFormat.R16G16B16A16_SNorm);
        tex.enableRandomWrite = true;
        tex.Create();
    }

    // Update is called once per frame
    void Update()
    {
        Graphics.CopyTexture(NState, NM1State);
        Graphics.CopyTexture(Np1State, NState);

        waveCompute.SetTexture(0, "NState", NState);
        waveCompute.SetTexture(0, "NM1State", NM1State);
        waveCompute.SetTexture(0, "Np1State", Np1State);
        waveCompute.SetVector("effect", effect);
        waveCompute.SetVector("resolution", new Vector2(resolution.x, resolution.y));
        waveCompute.SetFloat("dispersion", dispersion);

        waveCompute.SetTexture(0, "obstaclesTex", obstaclesTex);

        waveCompute.Dispatch(0, resolution.x/8, resolution.y/8, 1);

    }
}
