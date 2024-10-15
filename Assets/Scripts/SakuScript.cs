using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SakuScript : MonoBehaviour, ICol
{
    public void Col(GameObject other)
    {
        Debug.Log("Saku");
        Destroy(other.gameObject);
        //Destroy(gameObject);
    }
    void Start()
    {
    }
    void Update()
    {

    }
}
