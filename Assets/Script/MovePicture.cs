using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePicture : MonoBehaviour
{

    public enum ControlMode
    {
        Keyboard,
        Kinect
    }

    public enum GameMode
    {
        Menu,
        Tinder,
        Canva
    }

    public ControlMode control;
    GameMode gameMode;

    // Vector3 test = new Vector3(0.01f,0,0);
    Vector3 test;

    Vector3 test2 = new Vector3(0, 0, 0.5f);

    Vector3 VelVec;
    float VelVecFloat;

    public GameObject BackgroundImage;
    public GameObject Image;
    public GameObject DislikeImage;
    public GameObject LikeImage;
    public GameObject leftHand;

    public int randomNumber;
    public int randomNumber2;


    public List<Sprite> PlaceList;
    public List<Sprite> ObjectList;
    public List<Sprite> ArtistList;

    public List<Sprite> ColorList;

    public List<Sprite> EmotionList;

    public List<Sprite> AtmosphereList;
    public List<Sprite> tempList;
    public List<Sprite> dislikeList;


    string TempString = "";
    string promptText = "";

    string strObject = "_Object";

    string strPlace = "_Place";
    string strColor = "_Color";
    string strArtist = "_Artist";
    string strAtmos = "_Atmosphere";
    string strEmotion = "_Emotion";

    string testString = "";

    public Sprite BackgroundImg;
    public Sprite FrontImg;
    Sprite PrepareImg;
    public Sprite MenuImg;

    Sprite DisPrepareImg;

    public GameObject DislikeAnim;
    public GameObject LikeAnim;
    float current;
    Vector3 vel;

    bool sleep = false;
    bool isSwipe;
    bool sent;

    int likeCounter = 0;
    private Rigidbody l;

    bool sameName;

    private UDPSend sender = new UDPSend();


    // Start is called before the first frame update
    void Start()
    {

        gameMode = GameMode.Menu;


        Image.GetComponent<Image>().sprite = MenuImg;
        FrontImg = MenuImg;

        prepareTinder();
        Debug.Log(tempList.Count);


        //ChooseImage();

        if (control == ControlMode.Kinect)
        {
            KinectJoints();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (likeCounter == 6)
        {
            Image.GetComponent<Image>().sprite = MenuImg;
            FrontImg = MenuImg;
            gameMode = GameMode.Menu;
            prepareTinder();
            likeCounter = 0;
        }

        if (gameMode == GameMode.Menu || gameMode == GameMode.Tinder)
        {



            if (control == ControlMode.Kinect)
            {
                GameObject LeftHand = GameObject.Find("Left_Middle_Finger_Joint_01c");
                GameObject Ribs = GameObject.Find("Left_Forearm_Joint_01");

                if (LeftHand.transform.position.y > Ribs.transform.position.y)
                {
                    isSwipe = true;
                }
                else
                {
                    isSwipe = false;
                }

                if (LeftHand != null)
                    leftHand.transform.position = LeftHand.transform.position;



                calcVelocity();

                VelVec.x = -VelVec.x / 1000f;

                if (VelVec.x < 0 && this.transform.position.x >= -2.5 && sleep == false && isSwipe)
                {
                    VelVec.x -= vel.magnitude / 1.8f;
                    test = new Vector3(VelVec.x, 0, 0);
                    Dislike();
                }


                if (VelVec.x > 0 && this.transform.position.x < 2.5 && sleep == false && isSwipe)
                {
                    VelVec.x += vel.magnitude / 1.8f;
                    test = new Vector3(VelVec.x, 0, 0);
                    Like();
                }

            }

            if (control == ControlMode.Keyboard)
            {
                if (Input.GetKey("a") && this.transform.position.x > -2.5)
                {
                    test = new Vector3(-0.05f, 0, 0);
                    Dislike();
                }


                if (Input.GetKey("d") && this.transform.position.x < 2.5)
                {
                    test = new Vector3(+0.05f, 0, 0);
                    Like();
                }

            }

        }


        if (gameMode == GameMode.Canva)
        {
            if (control == ControlMode.Keyboard)
            {
                if (Input.GetKey("q") && this.transform.position.x > -2.5)
                {
                    test = new Vector3(-0.05f, 0, 0);
                    Dislike();
                }


                if (Input.GetKey("e") && this.transform.position.x < 2.5)
                {
                    test = new Vector3(+0.05f, 0, 0);
                    Like();
                }

            }
        }

    }


    void KinectJoints()
    {
        leftHand = new GameObject();

        GameObject LeftHand = GameObject.Find("Left_Middle_Finger_Joint_01c");
        GameObject Ribs = GameObject.Find("Left_Forearm_Joint_01");
        if (LeftHand != null)
            leftHand.transform.position = LeftHand.transform.position;




    }


    void Dislike()
    {
        sameName = true;
        this.transform.position += test;
        this.transform.eulerAngles -= test2;
        Image.transform.position = new Vector3(this.transform.position.x, Image.transform.position.y, Image.transform.position.z);
        Image.transform.eulerAngles = new Vector3(Image.transform.eulerAngles.x, Image.transform.eulerAngles.y, -this.transform.eulerAngles.z);

        if (gameMode == GameMode.Menu)
        {
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


                gameMode = GameMode.Canva;
                Image.GetComponent<Image>().sprite = ObjectList[0];


                DislikeAnim.GetComponent<Image>().enabled = true;
                DislikeAnim.GetComponent<Animator>().Play("Dislike");
                DislikeAnim.GetComponent<Image>().sprite = FrontImg;

                FrontImg = ObjectList[0];




            }


        }


        if (gameMode == GameMode.Canva)
        {
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

                gameMode = GameMode.Menu;
                Image.GetComponent<Image>().sprite = MenuImg;



                DislikeAnim.GetComponent<Image>().enabled = true;
                DislikeAnim.GetComponent<Animator>().Play("Dislike");
                DislikeAnim.GetComponent<Image>().sprite = FrontImg;

                FrontImg = MenuImg;
            }
        }

        if (gameMode == GameMode.Tinder)
        {
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


                NoDuplicates(BackgroundImg);

                ChooseImageDislike();



                sleep = true;

                Invoke("SleepNow", 1f);

            }
        }
    }
    void Like()
    {
        sameName = true;
        this.transform.position += test;
        this.transform.eulerAngles += test2;
        Image.transform.position = new Vector3(this.transform.position.x, Image.transform.position.y, Image.transform.position.z);
        Image.transform.eulerAngles = new Vector3(Image.transform.eulerAngles.x, Image.transform.eulerAngles.y, -this.transform.eulerAngles.z);

        if (gameMode == GameMode.Menu)
        {
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

                gameMode = GameMode.Tinder;
                Image.GetComponent<Image>().sprite = PrepareImg;


                LikeAnim.GetComponent<Image>().enabled = true;
                LikeAnim.GetComponent<Animator>().Play("Like");

                LikeAnim.GetComponent<Image>().sprite = FrontImg;

                FrontImg = PrepareImg;

                Image.GetComponent<Image>().sprite = FrontImg;
                //tempList.RemoveAt(randomNumber);


                randomNumber = Random.Range(0, tempList.Count - 1);

                int counterI = 0;
                while (FrontImg.name.Contains(testString) && tempList[randomNumber].name.Contains(testString) && counterI < 100)
                {


                    randomNumber = Random.Range(0, tempList.Count - 1);

                    counterI++;
                }
                Debug.Log(counterI);


                BackgroundImg = tempList[randomNumber];
                BackgroundImage.GetComponent<Image>().sprite = BackgroundImg;
                tempList.RemoveAt(randomNumber);



            }



        }


        if (gameMode == GameMode.Canva)
        {
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

                gameMode = GameMode.Menu;
                Image.GetComponent<Image>().sprite = MenuImg;


                LikeAnim.GetComponent<Image>().enabled = true;
                LikeAnim.GetComponent<Animator>().Play("Like");

                LikeAnim.GetComponent<Image>().sprite = FrontImg;

                FrontImg = MenuImg;
            }
        }

        if (gameMode == GameMode.Tinder)
        {

            if (this.transform.position.x > 2.5)
            {



                likeCounter++;



                Debug.Log(likeCounter + "likes");
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



                if (FrontImg.name.IndexOf("_Object") > -1)
                {

                    TempString = FrontImg.name.Replace(strObject, "");

                    testString = strObject;
                    Debug.Log(testString);
                    Debug.Log("Its an Object");

                    tempList.RemoveAll(item => ObjectList.Contains(item));
                    dislikeList.RemoveAll(item => ObjectList.Contains(item));

                }

                else if (FrontImg.name.IndexOf("_Place") > -1)
                {
                    TempString = FrontImg.name.Replace(strPlace, "");
                    testString = strPlace;
                    Debug.Log(testString);
                    Debug.Log("Its a Place");
                    tempList.RemoveAll(item => PlaceList.Contains(item));
                    dislikeList.RemoveAll(item => PlaceList.Contains(item));
                }


                else if (FrontImg.name.IndexOf("_Emotion") > -1)
                {
                    TempString = FrontImg.name.Replace(strEmotion, "");
                    testString = strEmotion;
                    Debug.Log(testString);
                    Debug.Log("Its an Emotion");

                    tempList.RemoveAll(item => EmotionList.Contains(item));
                    dislikeList.RemoveAll(item => EmotionList.Contains(item));
                }


                else if (FrontImg.name.IndexOf("_Color") > -1)
                {
                    TempString = FrontImg.name.Replace(strColor, "");
                    testString = strColor;
                    Debug.Log(testString);
                    Debug.Log("Its a Color");


                    tempList.RemoveAll(item => ColorList.Contains(item));
                    dislikeList.RemoveAll(item => ColorList.Contains(item));
                }


                else if (FrontImg.name.IndexOf("_Artist") > -1)
                {
                    TempString = FrontImg.name.Replace(strArtist, "");
                    testString = strArtist;
                    Debug.Log(testString);
                    Debug.Log("Its an Artist");

                    tempList.RemoveAll(item => ArtistList.Contains(item));
                    dislikeList.RemoveAll(item => ArtistList.Contains(item));
                }


                else if (FrontImg.name.IndexOf("_Atmosphere") > -1)
                {
                    TempString = FrontImg.name.Replace(strAtmos, "");
                    testString = strAtmos;
                    Debug.Log(testString);
                    Debug.Log("Its an Atmosphere");

                    tempList.RemoveAll(item => AtmosphereList.Contains(item));
                    dislikeList.RemoveAll(item => AtmosphereList.Contains(item));
                }

                promptText += TempString + ", ";




                SendPrompt();

                NoDuplicates(BackgroundImg);


                ChooseImageLike();



                sleep = true;
                Invoke("SleepNow", 1f);

            }
        }
    }

    void prepareTinder()
    {
        tempList = new List<Sprite>();
        dislikeList = new List<Sprite>();
        BackgroundImg = null;
        sender.Start();

        for (int i = 0; i < ColorList.Count; i++)
        {
            tempList.Add(ColorList[i]);
        }
        for (int i = 0; i < ObjectList.Count; i++)
        {
            tempList.Add(ObjectList[i]);
        }
        for (int i = 0; i < ArtistList.Count; i++)
        {
            tempList.Add(ArtistList[i]);
        }
        for (int i = 0; i < AtmosphereList.Count; i++)
        {
            tempList.Add(AtmosphereList[i]);
        }
        for (int i = 0; i < EmotionList.Count; i++)
        {
            tempList.Add(EmotionList[i]);
        }
        for (int i = 0; i < PlaceList.Count; i++)
        {
            tempList.Add(PlaceList[i]);
        }

        randomNumber = Random.Range(0, tempList.Count - 1);
        PrepareImg = tempList[randomNumber];
        tempList.RemoveAt(randomNumber);

        BackgroundImage.GetComponent<Image>().sprite = PrepareImg;

        NoDuplicates(PrepareImg);
    }


    void NoDuplicates(Sprite sprite)
    {
        randomNumber = Random.Range(0, tempList.Count - 1);
        int counterI = 0;

        if (sprite.name.IndexOf("_Object") > -1)
        {
            testString = strObject;
            Debug.Log(testString);
            if (sprite != PrepareImg)
            {
            while (BackgroundImage.GetComponent<Image>().sprite.name.IndexOf("_Object") > -1 && likeCounter < 6 && tempList.Count >= 1 && counterI < 20)
            {
                BackgroundImage.GetComponent<Image>().sprite = tempList[randomNumber];
                counterI++;
            }
            }
        }
        else if (sprite.name.IndexOf("_Place") > -1)
        {
            testString = strPlace;
            Debug.Log(testString);
            if (sprite != PrepareImg)
            {
            while (BackgroundImage.GetComponent<Image>().sprite.name.IndexOf("_Place") > -1 && likeCounter < 6 && tempList.Count >= 1 && counterI < 20)
            {
                BackgroundImage.GetComponent<Image>().sprite = tempList[randomNumber];
                counterI++;
            }
            }
        }
        else if (sprite.name.IndexOf("_Emotion") > -1)
        {
            testString = strEmotion;
            Debug.Log(testString);
            if (sprite != PrepareImg)
            {
            while (BackgroundImage.GetComponent<Image>().sprite.name.IndexOf("_Emotion") > -1 && likeCounter < 6 && tempList.Count >= 1 && counterI < 20)
            {
                BackgroundImage.GetComponent<Image>().sprite = tempList[randomNumber];
                counterI++;
            }
            }
        }
        else if (sprite.name.IndexOf("_Color") > -1)
        {
            testString = strColor;
            Debug.Log(testString);
            if (sprite != PrepareImg)
            {
            while (BackgroundImage.GetComponent<Image>().sprite.name.IndexOf("_Color") > -1 && likeCounter < 6 && tempList.Count >= 1 && counterI < 20)
            {
                BackgroundImage.GetComponent<Image>().sprite = tempList[randomNumber];
                counterI++;
            }
            }
        }
        else if (sprite.name.IndexOf("_Artist") > -1)
        {
            testString = strArtist;
            Debug.Log(testString);
            if (sprite != PrepareImg)
            {
            while (BackgroundImage.GetComponent<Image>().sprite.name.IndexOf("_Artist") > -1 && likeCounter < 6 && tempList.Count >= 1 && counterI < 20)
            {
                BackgroundImage.GetComponent<Image>().sprite = tempList[randomNumber];
                counterI++;
            }
            }
        }
        else
        {
            testString = strAtmos;
            Debug.Log(testString);
            if (sprite != PrepareImg)
            {
            while (BackgroundImage.GetComponent<Image>().sprite.name.IndexOf("_Atmosphere") > -1 && likeCounter < 6 && tempList.Count >= 1 && counterI < 20)
            {
                BackgroundImage.GetComponent<Image>().sprite = tempList[randomNumber];
                counterI++;
            }
            }
        }
    }

    void ChooseImageDislike()
    {

        if (tempList.Count < 1)
        {
            addDislikeList();
        }

        DislikeAnim.GetComponent<Image>().sprite = FrontImg;
        LikeAnim.GetComponent<Image>().sprite = FrontImg;

        FrontImg = BackgroundImg;

        Image.GetComponent<Image>().sprite = FrontImg;


        randomNumber = Random.Range(0, tempList.Count - 1);

        int counterI = 0;
        while (FrontImg.name.Contains(testString) && tempList[randomNumber].name.Contains(testString) && counterI < 200)
        {
            randomNumber = Random.Range(0, tempList.Count - 1);

            counterI++;
        }
        Debug.Log(counterI);


        BackgroundImg = tempList[randomNumber];
        BackgroundImage.GetComponent<Image>().sprite = BackgroundImg;
        tempList.RemoveAt(randomNumber);
    }

    void ChooseImageLike()
    {

        if (tempList.Count < 1)
        {
            addDislikeList();
        }

        DislikeAnim.GetComponent<Image>().sprite = FrontImg;
        LikeAnim.GetComponent<Image>().sprite = FrontImg;

        FrontImg = BackgroundImg;
        Image.GetComponent<Image>().sprite = FrontImg;

        randomNumber = Random.Range(0, tempList.Count - 1);

        int counterI = 0;
        while (FrontImg.name.Contains(testString) && tempList[randomNumber].name.Contains(testString) && counterI < 200)
        {
            randomNumber = Random.Range(0, tempList.Count - 1);

            counterI++;
        }
        Debug.Log(counterI);


        BackgroundImg = tempList[randomNumber];
        BackgroundImage.GetComponent<Image>().sprite = BackgroundImg;
        //tempList.RemoveAt(randomNumber);
    }

    void addDislikeList()
    {
        tempList = new List<Sprite>();

        for (int i = 0; i < dislikeList.Count; i++)
        {

            tempList.Add(dislikeList[i]);

        }

        dislikeList.Clear();

    }

    float calcVelocity()
    {
        float previous = leftHand.transform.position.x;

        Invoke("waitol", 0.000000001f);


        float tempVel = current - previous;

        if (tempVel > 0.15f && tempVel < 4 || tempVel < -0.15f && tempVel > -4)
        {
            vel = new Vector3(current - previous, 0, 0);
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
        sender.sendString(promptText);
    }
}

