using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSwiper : MonoBehaviour

{
    public GameObject dummyhand;
    float lastPosX;

    // Start is called before the first frame update
    void Start()
    {
        lastPosX = dummyhand.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (dummyhand.transform.position.x < lastPosX)
            transform.Translate(Vector3.left * 10 * Time.deltaTime);
        else if (dummyhand.transform.position.x > lastPosX)
            transform.Translate(Vector3.right * 10 * Time.deltaTime);
        lastPosX = dummyhand.transform.position.x;
    }
}
