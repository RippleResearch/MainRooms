using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    public static ObjectPool SharedInstance;
    public List<GameObject> pooledObjects;
    public List<GameObject> starPool;
    public GameObject objectToPool;
    public GameObject starObjectToPool;
    public int amountToPool;


    void Awake(){
        SharedInstance = this;
    }
    // Start is called before the first frame update
    void Start(){
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for(int i=0;i<amountToPool;i++){
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
        starPool = new List<GameObject>();
        GameObject tmp2;
        for(int i=0;i<amountToPool;i++){
            tmp2 = Instantiate(starObjectToPool);
            tmp2.SetActive(false);
            starPool.Add(tmp2);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetPooledObject(List<GameObject> objectPool){
        for(int i=0;i<amountToPool;i++){
            if(!objectPool[i].activeInHierarchy){
                return objectPool[i];
            }
        }
        return null;
    }
    // public GameObject GetPooledObject(){
    //     for(int i=0;i<amountToPool;i++){
    //         if(!pooledObjects[i].activeInHierarchy){
    //             return pooledObjects[i];
    //         }
    //     }
    //     return null;
    // }
}
