using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCube : MonoBehaviour
{
    public float speed = 1;
    public GameObject MoveCube;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(this.transform.position, MoveCube.transform.position, speed * Time.deltaTime);
    }
}
