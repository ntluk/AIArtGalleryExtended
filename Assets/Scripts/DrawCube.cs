using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCube : MonoBehaviour
{

    [SerializeField] MovePicture movePicture;

    Vector3 movePosition = new Vector3(0f, 0f, 0f);
    Vector3 current;
    
    Vector3 tempVel;
    
    Vector3 previous;
    float horizontalInput;
    float verticalInput;
    Vector3 movement;

   




    float speed = 20f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*calcMovement();
        this.transform.position += new Vector3 (tempVel.x, tempVel.y, 0) * Time.deltaTime;*/

        Debug.Log(movePicture.rightHand.name);
        Debug.Log(movePicture.rightForearm.name);
        /*if(movePicture.rightHand != null & movePicture.rightForearm != null)
        {


            if (movePicture.rightHand.transform.position.y >= movePicture.rightForearm.transform.position.y)
            {*/


                if (movePicture.gameMode == MovePicture.GameMode.Canva)
                {

                    Debug.Log(movePicture.gameMode);
                    Debug.Log(movePicture.control);

                    calcMovement();
                    tempVel.Normalize();

                    // Move the object
                    this.transform.position -= new Vector3(tempVel.x, tempVel.y, 0) * speed * Time.deltaTime;
                    this.transform.position = new Vector3(transform.position.x, transform.position.y, -14.838f);


                    if (this.transform.position.x <= -2.6)
                        this.transform.position = new Vector3(-2.6f, this.transform.position.y, -14.838f);
                    if (this.transform.position.x >= 2.6)
                        this.transform.position = new Vector3(2.6f, this.transform.position.y, -14.838f);
                    if (this.transform.position.y <= -3.9)
                        this.transform.position = new Vector3(this.transform.position.x, -3.9f, -14.838f);
                    if (this.transform.position.y >= 3.9f)
                        this.transform.position = new Vector3(this.transform.position.x, 3.9f, -14.838f);

                    Debug.Log("bin drin");
                }
            //}
        //}


        

    }

    Vector3 calcMovement()
    {

        float horizontalInput =  movePicture.rightHand.transform.localPosition.x;
        float verticalInput = movePicture.rightHand.transform.localPosition.y;

        // Calculate the movement direction
        movement = new Vector3(horizontalInput, verticalInput, 0);

        Invoke("waitol", 0.05f);

        tempVel = new Vector3(current.x - movement.x, current.y - movement.y, 0) ;


        //float distance = (current.magnitude - movement.magnitude) / Time.deltaTime;
        //float distance = Vector3.Distance(current.normalized, movement.normalized) / Time.deltaTime;

        float distance = tempVel.magnitude;

        Debug.Log("distance : " + distance);

        /*if (distance < 1500)
            tempVel = new Vector3(0, 0, 0);
        */
        if ( distance <= 0.20)
            tempVel = new Vector3(0, 0, 0);



        //tempVel = tempVel.normalized;

        return tempVel;
    }

    Vector3 waitol()
    {

        current = movePicture.rightHand.transform.localPosition;
        

        return current; 
    }

    



}
