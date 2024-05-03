using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesController : MonoBehaviour
{
    public PlayerController Controller;

    // Start is called before the first frame update
    void Start()
    {
        if (Controller == null) 
            throw new System.Exception("No player controller attached to interactables controller");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
