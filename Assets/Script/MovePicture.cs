using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePicture : MonoBehaviour
{

    Vector3 test = new Vector3(0.01f,0,0);
    Vector3 test2 = new Vector3(0,0,0.05f);

    public GameObject BackgroundImage;
    public GameObject Image;
    public GameObject DislikeImage;
    public GameObject LikeImage;

    public int randomNumber;

    public List<Sprite> spriteList;
    List<Sprite> tempList;
    List<Sprite> dislikeList;

    string promptText;

    public Sprite BackgroundImg;
    public Sprite FrontImg;

    public GameObject DislikeAnim;
    public GameObject LikeAnim;
    // Start is called before the first frame update
    void Start()
    {
       
        tempList = new List<Sprite>();
        dislikeList = new List<Sprite>();
        
        for (int i = 0; i < spriteList.Count; i++)
        {
            tempList.Add(spriteList[i]);
        }


        Debug.Log(tempList.Count);
        Debug.Log(spriteList.Count);

        ChooseImage();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey("a") && this.transform.position.x > -2.5)
        {
            Dislike();
        }

        if (Input.GetKey("d") && this.transform.position.x < 2.5)
        {
            Like();
        }

    }


    void Dislike()
    {
        this.transform.position -= test;
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

            promptText += FrontImg.name;

            ChooseImage();

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
                Debug.Log(tempList.Count);

                dislikeList.Clear();


                Debug.Log(dislikeList.Count);
                Debug.Log(tempList.Count);
            }

            
        }
    }

}
