using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager _instance;
    [SerializeField]
    private GameObject fedTorp;
    [SerializeField]
    private GameObject kliTorp;
    [SerializeField]
    private GameObject oriTorp;
    [SerializeField]
    private GameObject romTorp;
    [SerializeField]
    private List<GameObject> fedTorpPool;
    [SerializeField]
    private List<GameObject> kliTorpPool;
    [SerializeField]
    private List<GameObject> oriTorpPool;
    [SerializeField]
    private List<GameObject> romTorpPool;
    [SerializeField]
    private GameObject fedTorpsGameObject;
    [SerializeField]
    private GameObject kliTorpsGameObject;
    [SerializeField]
    private GameObject oriTorpsGameObject;
    [SerializeField]
    private GameObject romTorpsGameObject;

    public static PoolManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.Log("PoolManager is null");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        GenerateTorps(40);
    }

   void GenerateTorps(int ammount)
   {
        for(int i = 0; i < ammount; i++)
        {
            GameObject torp = Instantiate(fedTorp);
            torp.transform.parent = fedTorpsGameObject.transform;
            torp.SetActive(false);
            fedTorpPool.Add(torp);
        }
        for (int i = 0; i < ammount; i++)
        {
            GameObject torp = Instantiate(kliTorp);
            torp.transform.parent = kliTorpsGameObject.transform;
            torp.SetActive(false);
            kliTorpPool.Add(torp);
        }
        for (int i = 0; i < ammount; i++)
        {
            GameObject torp = Instantiate(oriTorp);
            torp.transform.parent = oriTorpsGameObject.transform;
            torp.SetActive(false);
            oriTorpPool.Add(torp);
        }
        for (int i = 0; i < ammount; i++)
        {
            GameObject torp = Instantiate(romTorp);
            torp.transform.parent = romTorpsGameObject.transform;
            torp.SetActive(false);
            romTorpPool.Add(torp);
        }
   }

    public GameObject fedRequestTorp()
    {

        foreach(GameObject torp in fedTorpPool)
        {
            if(torp.activeInHierarchy == false)
            {
                return torp;
            }
        }
        GameObject newtorp = Instantiate(fedTorp);
        newtorp.transform.parent = fedTorpsGameObject.transform;
        newtorp.SetActive(false);
        fedTorpPool.Add(newtorp);
        return newtorp;
    }
    public GameObject kliRequestTorp()
    {

        foreach (GameObject torp in kliTorpPool)
        {
            if (torp.activeInHierarchy == false)
            {
                return torp;
            }
        }
        GameObject newtorp = Instantiate(kliTorp);
        newtorp.transform.parent = kliTorpsGameObject.transform;
        newtorp.SetActive(false);
        kliTorpPool.Add(newtorp);
        return newtorp;
    }
    public GameObject oriRequestTorp()
    {

        foreach (GameObject torp in oriTorpPool)
        {
            if (torp.activeInHierarchy == false)
            {
                return torp;
            }
        }
        GameObject newtorp = Instantiate(oriTorp);
        newtorp.transform.parent = oriTorpsGameObject.transform;
        newtorp.SetActive(false);
        oriTorpPool.Add(newtorp);
        return newtorp;
    }
    public GameObject romRequestTorp()
    {

        foreach (GameObject torp in romTorpPool)
        {
            if (torp.activeInHierarchy == false)
            {
                return torp;
            }
        }
        GameObject newtorp = Instantiate(romTorp);
        newtorp.transform.parent = romTorpsGameObject.transform;
        newtorp.SetActive(false);
        romTorpPool.Add(newtorp);
        return newtorp;
    }
}
