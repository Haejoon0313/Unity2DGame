using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public GameObject player;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = player.transform.position;
    }
}
