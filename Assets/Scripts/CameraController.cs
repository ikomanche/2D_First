﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    /*private void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y, player.position.z);
    }*/
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y, player.position.z);
    }
    
}
