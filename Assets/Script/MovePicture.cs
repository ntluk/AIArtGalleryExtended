using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePicture : MonoBehaviour
{

    // Vector3 test = new Vector3(0.01f,0,0);
    Vector3 test;
    
    Vector3 test2 = new Vector3(0,0,0.5f);

    Vector3 VelVec;
    float VelVecFloat;

    private GameObject BackgroundImage;
    public GameObject Image;
    public GameObject DislikeImage;
    public GameObject LikeImage;
    private GameObject leftHand;

    public int randomNumber;

    public List<Sprite> spriteList;
    List<Sprite> tempList;
    List<Sprite> dislikeList;

    string promptText;

    public Sprite BackgroundImg;
    public Sprite FrontImg;

    public GameObject DislikeAnim;
    public GameObject LikeAnim;
    float current;
    Vector3 vel;

    private bool sleep = false;
    private bool isSwipe;
    private bool sent = false;

    private UDPSend sender = new UDPSend();


    private Rigidbody l;
    // Start is called before the first frame update
    void Start()
    {
       
        tempList = new List<Sprite>();
        dislikeList = new List<Sprite>();
        
        for (int i = 0; i < spriteList.Count; i++)
        {
            tempList.Add(spriteList[i]);
        }


        //Debug.Log(tempList.Count);
        //Debug.Log(spriteList.Count);

        ChooseImage();

        leftHand = new GameObject();

        GameObject LeftHand = GameObject.Find("Left_Middle_Finger_Joint_01c");
        GameObject Ribs = GameObject.Find("Left_Forearm_Joint_01");
        if (LeftHand != null)
            leftHand.transform.position = LeftHand.transform.position;
        //leftHand.AddComponent<Rigidbody>();
        //l = leftHand.GetComponent<Rigidbody>();
        //l.isKinematic = true;
        //l.useGravity = true;
        //l.mass = 0;
       
       
    }

    // Update is called once per frame
    void Update()
    {
        GameObject LeftHand = GameObject.Find("Left_Middle_Finger_Joint_01c");
        GameObject Ribs = GameObject.Find("Left_Forearm_Joint_01");

        if(LeftHand.transform.position.y > Ribs.transform.position.y)
        {
             isSwipe = true;
        }
        else
        {
             isSwipe = false;
        }

        if (LeftHand != null)
            leftHand.transform.position = LeftHand.transform.position;
        //Debug.Log(leftHand.transform.position.x);
        //Debug.Log("---------------------------");
        //Debug.Log(calcVelocity());

        calcVelocity();

        //VelVec = new Vector3 (vel, 0, 0);
        //VelVecFloat = VelVec.magnitude;

        VelVec.x = -VelVec.x / 1000f;

        Debug.Log("LeftHand = " + leftHand.transform.localPosition.y);
        Debug.Log("Ribs = " + Ribs.transform.localPosition.y);
        Debug.Log("VelVec.x" + VelVec.x);
        
       

        //if (Input.GetKey("a") && this.transform.position.x > -2.5)
        //if (leftHand.transform.position.x  && this.transform.position.x > -2.5)
        if (VelVec.x < 0  && this.transform.position.x >= -2.5 && sleep == false && isSwipe)
        {
            VelVec.x -= vel.magnitude / 1.8f;
            test = new Vector3(VelVec.x, 0, 0);
            Dislike();
            Debug.Log("Dislike");
        }

        //if (Input.GetKey("d") && this.transform.position.x < 2.5)
        if (VelVec.x > 0 && this.transform.position.x < 2.5 && sleep == false && isSwipe)
        {
            VelVec.x += vel.magnitude / 1.8f;
            test = new Vector3(VelVec.x, 0, 0);
            Like();
            Debug.Log("Like");
        }

        if (sent)
            SendPrompt();

    }


    void Dislike()
    {
        this.transform.position += test;
        this.transform.eulerAngles -= test2;
        Image.transform.position = new Vector3(this.transform.position.x, Image.transform.position.y, Image.transform.position.z);
        Image.transform.eulerAngles = new Vector3(Image.transform.eulerAngles.x, Image.transform.eulerAngles.y, -this.transform.eulerAngles.z);

        if (this.transform.position.x < -2.5)
        {
            this.transform.position = new Vector3(-2.5f, this.transform.position.y, this.transform.position.z);
            this.transform.eulerAngles = new Vector3(0, 0, -12.5f);
            Image.transform.position = new Vector3(this.transform.position.x, Image.transform.position.y, Image.transform.position.z);
            Image.transform.eulerAngles = new Vector3(Image.transform.eulerAngles.x, Image.transform.eulerAngles.y, -this.transform.eulerAngles.z);

            this.transform.position = new Vector3(0, this.transform.position.y, this.transform.position.z);
            this.transform.eulerAngles = new Vector3(0, 0, 0);
            Image.transform.position = new Vector3(this.transform.position.x, Image.transform.position.y, Image.transform.position.z);
            Image.transform.eulerAngles = new Vector3(Image.transform.eulerAngles.x, Image.transform.eulerAngles.y, -this.transform.eulerAngles.z);


            DislikeAnim.GetComponent<Image>().enabled = true;       
            DislikeAnim.GetComponent<Animator>().Play("Dislike");

            dislikeList.Add(FrontImg);

            ChooseImage();

            sleep = true;

            Invoke("SleepNow", 1f);
            Debug.Log(promptText);
        }
    }


    void Like()
    {
        this.transform.position += test;
        this.transform.eulerAngles += test2;
        Image.transform.position = new Vector3(this.transform.position.x, Image.transform.position.y, Image.transform.position.z);
        Image.transform.eulerAngles = new Vector3(Image.transform.eulerAngles.x, Image.transform.eulerAngles.y, -this.transform.eulerAngles.z);

        if (this.transform.position.x > 2.5)
        {
            this.transform.position = new Vector3(2.5f, this.transform.position.y, this.transform.position.z);
            this.transform.eulerAngles = new Vector3(0, 0, 12.5f);
            Image.transform.position = new Vector3(this.transform.position.x, Image.transform.position.y, Image.transform.position.z);
            Image.transform.eulerAngles = new Vector3(Image.transform.eulerAngles.x, Image.transform.eulerAngles.y, -this.transform.eulerAngles.z);

            this.transform.position = new Vector3(0, this.transform.position.y, this.transform.position.z);
            this.transform.eulerAngles = new Vector3(0, 0, 0);
            Image.transform.position = new Vector3(this.transform.position.x, Image.transform.position.y, Image.transform.position.z);
            Image.transform.eulerAngles = new Vector3(Image.transform.eulerAngles.x, Image.transform.eulerAngles.y, -this.transform.eulerAngles.z);


            LikeAnim.GetComponent<Image>().enabled = true;            
            LikeAnim.GetComponent<Animator>().Play("Like");

            promptText += FrontImg.name + ", ";
 

            ChooseImage();
            sleep = true;
            Invoke("SleepNow", 1f);
            Debug.Log(promptText);
        }
    }
    

    void ChooseImage()
    {
        if (tempList.Count == spriteList.Count)
        {
            Debug.Log(tempList.Count);
            Debug.Log(spriteList.Count);

            randomNumber = Random.Range(0, tempList.Count - 1);
            FrontImg = tempList[randomNumber];
            Image.GetComponent<Image>().sprite = FrontImg;
            tempList.RemoveAt(randomNumber);
            randomNumber = Random.Range(0, tempList.Count - 1);
            BackgroundImg = tempList[randomNumber];
            BackgroundImage.GetComponent<Image>().sprite = BackgroundImg;
            tempList.RemoveAt(randomNumber);
            Debug.Log(tempList.Count);
            Debug.Log(spriteList.Count);

            DislikeAnim.GetComponent<Image>().sprite = FrontImg;
            LikeAnim.GetComponent<Image>().sprite = FrontImg;
        }
        else
        {
            DislikeAnim.GetComponent<Image>().sprite = FrontImg;
            LikeAnim.GetComponent<Image>().sprite = FrontImg;

            FrontImg = BackgroundImg;
            Image.GetComponent<Image>().sprite = FrontImg;
            

            randomNumber = Random.Range(0, tempList.Count - 1);
            BackgroundImg = tempList[randomNumber];
            BackgroundImage.GetComponent<Image>().sprite = BackgroundImg;
            tempList.RemoveAt(randomNumber);

            if (tempList.Count <= 0)
            {
                for (int i = 0; i < dislikeList.Count; i++)
                {
                    tempList.Add(dislikeList[i]);
                    
                }

                //Debug.Log(dislikeList.Count);
                //Debug.Log(tempList.Count);

                dislikeList.Clear();


                Debug.Log(dislikeList.Count);
                Debug.Log(tempList.Count);
            }
        }
    }

    float calcVelocity()
    {
        float previous = leftHand.transform.position.x;
        //StartCoroutine("waitol", 1);
        Invoke("waitol", 0.000000001f);
        //Debug.Log("previous: " + previous);
        //Debug.Log("current: " + current);

        float tempVel = current - previous;
        //Debug.Log(tempVel);
        if (tempVel > 0.15f && tempVel < 4 || tempVel < -0.15f  && tempVel > -4)
        {
            vel = new Vector3(current - previous, 0, 0);
            Debug.Log("VelMag = " + vel.magnitude);
            VelVec = vel.normalized;
        }
        else
        {
            VelVec.x = 0;
        }
        
        return VelVec.x;
    }

    float waitol()
    {
        current = leftHand.transform.position.x;
        return current;
    }

    void SleepNow()
    {
        sleep = false;
    }

    private void SendPrompt()
    {
        if (!sent)
        {
            sender.sendString(promptText);
            sent = true;
        }
    }

}


