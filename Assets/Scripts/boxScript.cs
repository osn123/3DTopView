using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxScript : MonoBehaviour,ICol
{
    public void Col(GameObject other)
    {
        Debug.Log("box");
        //Destroy(other.gameObject);
        Destroy(gameObject);
    }

    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
