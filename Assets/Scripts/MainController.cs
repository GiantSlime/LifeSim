using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerController.transform.position = transform.position;
        playerController.transform.rotation = transform.rotation;
        playerController.transform.localScale = transform.localScale;
        playerController.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
