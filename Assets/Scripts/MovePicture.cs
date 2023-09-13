using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
        Canva,
        TinderEnde,
        CanvaEnde
    }

    public ControlMode control;
    public GameMode gameMode;

    // Vector3 test = new Vector3(0.01f,0,0);
    Vector3 test;

    Vector3 test2 = new Vector3(0, 0, 0.5f);

    Vector3 VelVec;
    float VelVecFloat;

    public GameObject BackgroundImage;
    public GameObject Image;
    public GameObject DislikeImage;
    public GameObject LikeImage;
    public GameObject Background;
    public GameObject Description;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject leftForearm;
    public GameObject rightForearm;
    public GameObject hips;
    public GameObject rightShoulder;

    public int randomNumber;
    

    Dictionary<Sprite, Text> spriteDatabase = new Dictionary<Sprite, Text>();

    [System.Serializable]
    public struct DescPairs
    {
        public Sprite img;
        public Text desc;
    }


    public List<Sprite> PlaceList;
    public List<Sprite> ObjectList;
    public List<Sprite> ArtistList;

    public List<Sprite> ColorList;

    public List<Sprite> EmotionList;

    public List<Sprite> AtmosphereList;

    public DescPairs[] spriteDescriptionPairs;

    public List<Sprite> tempList;
    public List<Sprite> dislikeList;



    string TempString = "";
    string promptText = "";

    string strObject = "_Object";

    string strPlace = "_Place";
    string strColor = "_Color";
    string strArtist = "_Artist";
    string strAtmos = "_Atmosphere";
    string strEmotion = "_Emotions";

    string testString = "";

    public Sprite BackgroundImg;
    public Sprite FrontImg;
    Sprite PrepareImg;
    public Sprite MenuImg;

    public Sprite CanvaImg;

    Sprite DisPrepareImg;

    public GameObject DislikeAnim;
    public GameObject LikeAnim;
    float current;
    Vector3 vel;

    bool sleep = false;
    bool isSwipe;
    bool leftHandUp;
    bool rightHandUp;
    bool sent;
    bool stuckPic;
    public bool isDrawing;
    bool onlyDraw;

    int likeCounter = 0;

    Sprite tinderErgebnis;


 


    public GameObject CanvaQuad;

    private UDPSend sender = new UDPSend();

  

    public TexturePainter texPaint;
    

    [SerializeField] LoadImage Imageload;
    // Start is called before the first frame update
    void Start()
    {

        gameMode = GameMode.Menu;

        for (int i = 0; i < spriteDescriptionPairs.Length; i++)
        {
            spriteDatabase.Add(spriteDescriptionPairs[i].img, spriteDescriptionPairs[i].desc);
        }


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

        GameObject LeftHand = GameObject.Find("Left_Middle_Finger_Joint_01c");
        GameObject RightHand = GameObject.Find("Right_Wrist_Joint_01");
        GameObject LeftForearm = GameObject.Find("Left_Forearm_Joint_01");
        GameObject RightForearm = GameObject.Find("Right_Forearm_Joint_01");
        GameObject RightShoulder = GameObject.Find("Right_Shoulder_Joint_01");
        GameObject Hips = GameObject.Find("Hip");

        if (likeCounter == 6)
        {

            StartCoroutine(Imageload.loadImage());


            Invoke("prepareTinderImg", 0.5f);

            likeCounter++;
            sleep = true;
            Invoke("SleepNow", 1f);

        }
        

        if (likeCounter == 8)
        {
   
            Image.GetComponent<Image>().sprite = MenuImg;
            FrontImg = MenuImg;
            gameMode = GameMode.Menu;
            prepareTinder();
            likeCounter = 0;
            Background.SetActive(false);
            Description.SetActive(false);

            sleep = true;
            Invoke("SleepNow", 1f);
        }



        
        





        if (stuckPic && !isSwipe)
        {
            /*Vector3 test3 = new Vector3(0.001f, 0, 0);
            if (this.transform.position.x <= 0)
            {
                this.transform.position += test3;
                this.transform.eulerAngles += test2;
            }
            else
            {
                this.transform.position -= test3;
                this.transform.eulerAngles -= test2;
            }*/

            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, new Vector3(0, 0, -15), 3 * Time.deltaTime);
            

            if (gameMode != GameMode.Canva)
            {
                Image.transform.position = new Vector3(this.transform.position.x, Image.transform.position.y, Image.transform.position.z);
                
                float angle = Mathf.MoveTowardsAngle(Image.transform.eulerAngles.z, 0, 10f * Time.deltaTime);
                Image.transform.eulerAngles = new Vector3(0, 0, angle);
                //Debug.Log(angle);
                angle = 0;
                
            }
            else
            {
                CanvaQuad.transform.position = new Vector3(this.transform.position.x, CanvaQuad.transform.position.y, CanvaQuad.transform.position.z);
                
                float angle = Mathf.MoveTowardsAngle(CanvaQuad.transform.eulerAngles.z, 0, 10f * Time.deltaTime);
                CanvaQuad.transform.eulerAngles = new Vector3(0, 0, angle);
                //Debug.Log(angle);
                angle = 0;

            }

            


        }

        if (gameMode == GameMode.Menu || gameMode == GameMode.Tinder || gameMode == GameMode.TinderEnde)
        {
            if (gameMode == GameMode.Tinder)
            {
                Background.SetActive(true);
                Description.SetActive(true);

                if(likeCounter < 6)
                Description.GetComponent<Text>().text = spriteDatabase[Image.GetComponent<Image>().sprite].text;
            }

                if (control == ControlMode.Kinect)
            {
                
                

                //Debug.Log(LeftHand);
                //Debug.Log(RightHand);
                //Debug.Log(LeftForearm);
                //Debug.Log(RightForearm);

                //if (LeftHand != null & LeftForearm != null & RightHand != null & RightForearm != null)
                //{


                    if (LeftHand.transform.position.y > LeftForearm.transform.position.y || RightHand.transform.position.y > RightForearm.transform.position.y)
                    {
                        isSwipe = true;
                    }
                    else
                    {
                        isSwipe = false;
                    }

                    if (LeftHand.transform.position.y > LeftForearm.transform.position.y)
                    {
                        leftHandUp = true;
                        rightHandUp = false;
                    }
                    else
                        leftHandUp = false;


                    if (RightHand.transform.position.y > RightForearm.transform.position.y)
                    {
                        rightHandUp = true;
                        leftHandUp = false;
                    }
                    else
                        rightHandUp = false;

                    if (LeftHand != null)
                        leftHand.transform.position = LeftHand.transform.position;

                    if (RightHand != null)
                        rightHand.transform.position = RightHand.transform.position;


                    if (leftHandUp)
                        calcVelocity(leftHand);
                    else if (rightHandUp)
                        calcVelocity(rightHand);

                //}

                VelVec.x = -VelVec.x / 1000f;

                if (VelVec.x < 0 && this.transform.position.x >= -2.5 && sleep == false && isSwipe)
                {
                    VelVec.x -= vel.magnitude / 1.8f;
                    test = new Vector3(VelVec.x, 0, 0);
                    stuckPic = false;
                    Dislike();
                }
                else if (VelVec.x > 0 && this.transform.position.x < 2.5 && sleep == false && isSwipe)
                {
                    VelVec.x += vel.magnitude / 1.8f;
                    test = new Vector3(VelVec.x, 0, 0);
                    stuckPic = false;
                    Like();
                }
                else
                {
                    stuckPic = true;
                }

            }

            if (control == ControlMode.Keyboard)
            {
                if (Input.GetKey("a") && this.transform.position.x > -2.5)
                {
                    test = new Vector3(-0.05f, 0, 0);
                    stuckPic = false;
                    Dislike();
                }
                else if (Input.GetKey("d") && this.transform.position.x < 2.5)
                {
                    test = new Vector3(+0.05f, 0, 0);
                    stuckPic = false;
                    Like();
                }
                else
                {
                    stuckPic = true;
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
                    stuckPic = false;
                    Dislike();
                }
                else if (Input.GetKey("e") && this.transform.position.x < 2.5)
                {
                    test = new Vector3(+0.05f, 0, 0);
                    stuckPic = false;
                    Like();
                }
                else
                {
                    stuckPic = true;
                }

            }

            if (control == ControlMode.Kinect)
            {

                //float DistanceArmHand = RightHand.transform.position.z - RightShoulder.transform.position.z;
                float DistanceArmHand = Vector3.Distance(RightHand.transform.position, RightShoulder.transform.position);
                //Debug.Log("Hallo " + DistanceArmHand);

                //if (DistanceArmHand > 7 && DistanceArmHand < 10.5f)
                //    isDrawing = true;
                //else
                //    isDrawing = false;

                if (LeftHand.transform.position.y > LeftForearm.transform.position.y && DistanceArmHand < 10 )
                {
                    isDrawing = true;
                    onlyDraw = true;
                }
                else
                    isDrawing = false;

                if (LeftHand.transform.position.y < LeftForearm.transform.position.y && RightHand.transform.position.y < RightForearm.transform.position.y)
                    onlyDraw = false;


                if (LeftHand.transform.position.y > LeftForearm.transform.position.y && RightHand.transform.position.y < Hips.transform.position.y && !onlyDraw)
                {
                    isSwipe = true;
                }
                else
                {
                    isSwipe = false;
                }
                if (LeftHand.transform.position.y > LeftForearm.transform.position.y)
                {
                    leftHandUp = true;
                }
                else
                    leftHandUp = false;


                if (LeftHand != null)
                    leftHand.transform.position = LeftHand.transform.position;

                if (RightHand != null)
                    rightHand.transform.position = RightHand.transform.position;

                if (leftHandUp)
                    calcVelocity(leftHand);



                VelVec.x = -VelVec.x / 1000f;

                if (VelVec.x < 0 && this.transform.position.x >= -2.5 && sleep == false && isSwipe)
                {
                    VelVec.x -= vel.magnitude / 1.8f;
                    test = new Vector3(VelVec.x, 0, 0);
                    stuckPic = false;
                    Dislike();
                }
                else if (VelVec.x > 0 && this.transform.position.x < 2.5 && sleep == false && isSwipe)
                {
                    VelVec.x += vel.magnitude / 1.8f;
                    test = new Vector3(VelVec.x, 0, 0);
                    stuckPic = false;
                    Like();
                }
                else
                {
                    stuckPic = true;
                }




            }
        }

    }


    void KinectJoints()
    {
        leftHand = new GameObject();
        rightHand = new GameObject();
        leftForearm = new GameObject();
        rightForearm = new GameObject();
        hips = new GameObject();

        GameObject LeftHand = GameObject.Find("Left_Middle_Finger_Joint_01c");
        GameObject RightHand = GameObject.Find("Right_Wrist_Joint_01");
        GameObject LeftForearm = GameObject.Find("Left_Forearm_Joint_01");
        GameObject RightForearm = GameObject.Find("Right_Forearm_Joint_01");
        GameObject Hips = GameObject.Find("Hip");
        GameObject RightShoulder = GameObject.Find("Right_Shoulder_Joint_01");
        

        if (LeftHand != null)
            leftHand.transform.position = LeftHand.transform.position;
        if (RightHand != null)
            rightHand.transform.position = RightHand.transform.position;
        if (LeftForearm != null)
            leftForearm.transform.position = LeftForearm.transform.position;
        if (RightForearm != null)
            rightForearm.transform.position = RightForearm.transform.position;
        if (Hips != null)
            hips.transform.position = Hips.transform.position;
        if (RightShoulder != null)
            rightShoulder.transform.position = RightShoulder.transform.position;

    }


    void Dislike()
    {
        
        this.transform.position += test;
        this.transform.eulerAngles -= test2;

        if (gameMode != GameMode.Canva)
        {
            Image.transform.position = new Vector3(this.transform.position.x, Image.transform.position.y, Image.transform.position.z);
            Image.transform.eulerAngles = new Vector3(Image.transform.eulerAngles.x, Image.transform.eulerAngles.y, -this.transform.eulerAngles.z);
        }
        else
        {
            CanvaQuad.transform.position = new Vector3(this.transform.position.x, CanvaQuad.transform.position.y, CanvaQuad.transform.position.z);
            CanvaQuad.transform.eulerAngles = new Vector3(CanvaQuad.transform.eulerAngles.x, CanvaQuad.transform.eulerAngles.y, -this.transform.eulerAngles.z);
        }



        if (gameMode == GameMode.Menu)
        {

            if (this.transform.position.x < 0)
            {
                BackgroundImage.GetComponent<Image>().sprite = CanvaImg;
            }


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
                Image.GetComponent<Image>().sprite = CanvaImg;


                DislikeAnim.GetComponent<Image>().enabled = true;
                DislikeAnim.GetComponent<Animator>().Play("Dislike");
                DislikeAnim.GetComponent<Image>().sprite = FrontImg;

                FrontImg = CanvaImg;
                CanvaQuad.transform.position = new Vector3(0, 0, -0.01f) ;


                sleep = true;
                Invoke("SleepNow", 1f);

            }


        }


        if (gameMode == GameMode.Canva)
        {


            if (this.transform.position.x < 0)
            {
                Image.GetComponent<Image>().sprite = CanvaImg;
                BackgroundImage.GetComponent<Image>().sprite = CanvaImg;

                ;
            }

            if (this.transform.position.x < -2.5)
            {
                this.transform.position = new Vector3(-2.5f, this.transform.position.y, this.transform.position.z);
                this.transform.eulerAngles = new Vector3(0, 0, -12.5f);
                CanvaQuad.transform.position = new Vector3(this.transform.position.x, CanvaQuad.transform.position.y, CanvaQuad.transform.position.z);
                CanvaQuad.transform.eulerAngles = new Vector3(CanvaQuad.transform.eulerAngles.x, CanvaQuad.transform.eulerAngles.y, -this.transform.eulerAngles.z);



                this.transform.position = new Vector3(0, this.transform.position.y, this.transform.position.z);
                this.transform.eulerAngles = new Vector3(0, 0, 0);
                CanvaQuad.transform.position = new Vector3(this.transform.position.x, CanvaQuad.transform.position.y, CanvaQuad.transform.position.z);
                CanvaQuad.transform.eulerAngles = new Vector3(CanvaQuad.transform.eulerAngles.x, CanvaQuad.transform.eulerAngles.y, -this.transform.eulerAngles.z);

                texPaint.SpriteCreate();

                gameMode = GameMode.Canva;
                Image.GetComponent<Image>().sprite = CanvaImg;



                DislikeAnim.GetComponent<Image>().enabled = true;
                DislikeAnim.GetComponent<Animator>().Play("Dislike");
                DislikeAnim.GetComponent<Image>().sprite = texPaint.CanvSprite; ;

                FrontImg = CanvaImg;

                texPaint.restoreMaterial();



                sleep = true;
                Invoke("SleepNow", 1f);
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
        if (gameMode == GameMode.TinderEnde)
        {

            if (this.transform.position.x < 0)
            {
                BackgroundImage.GetComponent<Image>().sprite = MenuImg;
            }


            if (this.transform.position.x < -2.5)
            {

                likeCounter++;
                this.transform.position = new Vector3(-2.5f, this.transform.position.y, this.transform.position.z);
                this.transform.eulerAngles = new Vector3(0, 0, -12.5f);
                Image.transform.position = new Vector3(this.transform.position.x, Image.transform.position.y, Image.transform.position.z);
                Image.transform.eulerAngles = new Vector3(Image.transform.eulerAngles.x, Image.transform.eulerAngles.y, -this.transform.eulerAngles.z);

                this.transform.position = new Vector3(0, this.transform.position.y, this.transform.position.z);
                this.transform.eulerAngles = new Vector3(0, 0, 0);
                Image.transform.position = new Vector3(this.transform.position.x, Image.transform.position.y, Image.transform.position.z);
                Image.transform.eulerAngles = new Vector3(Image.transform.eulerAngles.x, Image.transform.eulerAngles.y, -this.transform.eulerAngles.z);


                

                //FrontImg = Imageload.loadImage()
                
                DislikeAnim.GetComponent<Image>().enabled = true;
                DislikeAnim.GetComponent<Animator>().Play("Dislike");
                DislikeAnim.GetComponent<Image>().sprite = FrontImg;

                           

            }


        }
    }
    void Like()
    {
        
        this.transform.position += test;
        this.transform.eulerAngles += test2;

        if(gameMode != GameMode.Canva)
        {
        Image.transform.position = new Vector3(this.transform.position.x, Image.transform.position.y, Image.transform.position.z);
        Image.transform.eulerAngles = new Vector3(Image.transform.eulerAngles.x, Image.transform.eulerAngles.y, -this.transform.eulerAngles.z);
        }
        else
        {
             CanvaQuad.transform.position = new Vector3(this.transform.position.x, CanvaQuad.transform.position.y, CanvaQuad.transform.position.z);
        CanvaQuad.transform.eulerAngles = new Vector3(CanvaQuad.transform.eulerAngles.x, CanvaQuad.transform.eulerAngles.y, -this.transform.eulerAngles.z);
        }
        if (gameMode == GameMode.Menu)
        {

            if (this.transform.position.x > 0)
            {
                BackgroundImage.GetComponent<Image>().sprite = PrepareImg;
            }


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


                sleep = true;
                Invoke("SleepNow", 1f);
            }



        }


        if (gameMode == GameMode.Canva)
        {

            if (this.transform.position.x > 0)
            {
                Image.GetComponent<Image>().sprite = CanvaImg;
                BackgroundImage.GetComponent<Image>().sprite = CanvaImg;
                
            }


            if (this.transform.position.x > 2.5)
            {

                this.transform.position = new Vector3(2.5f, this.transform.position.y, this.transform.position.z);
                this.transform.eulerAngles = new Vector3(0, 0, 12.5f);
                CanvaQuad.transform.position = new Vector3(this.transform.position.x, CanvaQuad.transform.position.y, CanvaQuad.transform.position.z);
                CanvaQuad.transform.eulerAngles = new Vector3(CanvaQuad.transform.eulerAngles.x, CanvaQuad.transform.eulerAngles.y, -this.transform.eulerAngles.z);


                this.transform.position = new Vector3(0, this.transform.position.y, this.transform.position.z);
                this.transform.eulerAngles = new Vector3(0, 0, 0);
                CanvaQuad.transform.position = new Vector3(this.transform.position.x, CanvaQuad.transform.position.y, CanvaQuad.transform.position.z);
                CanvaQuad.transform.eulerAngles = new Vector3(CanvaQuad.transform.eulerAngles.x, CanvaQuad.transform.eulerAngles.y, -this.transform.eulerAngles.z);

                texPaint.SpriteCreate();
                
                

                gameMode = GameMode.Menu;
                Image.GetComponent<Image>().sprite = MenuImg;


                LikeAnim.GetComponent<Image>().enabled = true;
                LikeAnim.GetComponent<Animator>().Play("Like");

                LikeAnim.GetComponent<Image>().sprite = texPaint.CanvSprite;

                FrontImg = MenuImg;
                CanvaQuad.transform.position = new Vector3(0, 0, +0.01f);

                sleep = true;
                Invoke("SleepNow", 1f);
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


                    else if (FrontImg.name.IndexOf("_Emotions") > -1)
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

                    SavePrompt();

                    NoDuplicates(BackgroundImg);
                    
                if(likeCounter < 6)
                    ChooseImageLike();
                else
                {
                    DislikeAnim.GetComponent<Image>().sprite = FrontImg;
                    LikeAnim.GetComponent<Image>().sprite = FrontImg;

                    FrontImg = BackgroundImg;
                    Image.GetComponent<Image>().sprite = FrontImg;
                }

                    sleep = true;
                    Invoke("SleepNow", 1f);
                
            }
        }

        if (gameMode == GameMode.TinderEnde)
        {

            if (this.transform.position.x > 0)
            {
                BackgroundImage.GetComponent<Image>().sprite = MenuImg;
            }


            if (this.transform.position.x > 2.5)
            {
                likeCounter++;

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

                LikeAnim.GetComponent<Image>().sprite = FrontImg;

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

    float calcVelocity(GameObject hand)
    {
        float previous = hand.transform.position.x;

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
        if (leftHandUp)
            current = leftHand.transform.position.x;
        else if (rightHandUp)
            current = rightHand.transform.position.x;

        return current;
    }

    void SleepNow()
    {
        sleep = false;
    }

    private void SavePrompt()
    {
        string path = "C:/Users/Mirevi/source/repos/AIArtGalleryExtended/Prompt/prompt.txt";

        StreamWriter writer = new StreamWriter(path);
        writer.WriteLine(promptText);
        writer.Close();
    }

    private void prepareTinderImg()
    {
        tinderErgebnis = Imageload.Ergebnis;
        FrontImg = tinderErgebnis;

        gameMode = GameMode.TinderEnde;
        BackgroundImage.GetComponent<Image>().sprite = MenuImg;

        Background.SetActive(false);
        Description.SetActive(false);
    }

      
    

}

