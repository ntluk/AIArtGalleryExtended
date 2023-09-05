using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Tobii.XR;
using TMPro;
using System.Linq;


// GalleryManager 
public class GalleryManager : MonoBehaviour
{
    public static GalleryManager instance;

    public List<GameObject> pictureFrames = new List<GameObject>();     //contains all picture frames
    public List<GameObject> picturePool = new List<GameObject>();       //contains all picture prefabs
    public List<GameObject> pictureSelection = new List<GameObject>();  //contains the selected picture prefabs for this run
    //private List<String> flickeringLights = new List<String>();
    public List<String> focusedBefore = new List<String>();            //contains the names of previously focused image segments
    public List<String> styles = new List<String>();                   //contains the style descriptions of previously focused images

    //picturesfront wall 
    public GameObject pictureF_A1;
    public GameObject pictureF_A2;
    public GameObject pictureF_A3;
    public GameObject pictureF_B1;
    public GameObject pictureF_B2;
    public GameObject pictureF_B3;
    public GameObject pictureF_C1;
    public GameObject pictureF_C2;
    public GameObject pictureF_C3;
    public GameObject pictureF_D1;
    public GameObject pictureF_D2;
    public GameObject pictureF_D3;
    public GameObject pictureF_E1;
    public GameObject pictureF_E2;
    public GameObject pictureF_E3;
    public GameObject pictureF_F1;
    public GameObject pictureF_F2;
    public GameObject pictureF_F3;

    //pictures left wall 
    public GameObject pictureL_A1;
    public GameObject pictureL_A2;
    public GameObject pictureL_A3;
    public GameObject pictureL_B1;
    public GameObject pictureL_B2;
    public GameObject pictureL_B3;
    public GameObject pictureL_C1;
    public GameObject pictureL_C2;
    public GameObject pictureL_C3;
    public GameObject pictureL_D1;
    public GameObject pictureL_D2;
    public GameObject pictureL_D3;
    public GameObject pictureL_E1;
    public GameObject pictureL_E2;
    public GameObject pictureL_E3;
    public GameObject pictureL_F1;
    public GameObject pictureL_F2;
    public GameObject pictureL_F3;

    //final picture
    public GameObject personalized;
    public GameObject personalPrefab;
    
    //lights for light control
    public GameObject spotlight1;
    public GameObject spotlight2;

    private GameObject focused;             // currently focused object
    private string focusedPictureFrame;     // frame containing the currently focused object
    private String finalPrompt = "";        // contains the prompt for img gen
    private String focusedLast = "";        // contains the name of the previously focused object
    private String changedLast = "";        // contains the name of the previously picture swapped frame
    private int swapCount = 0;              // counts the total amount of picture swaps
    private int focusCount = 0;             // counts the amount of already focused objects 
    //private int numerOfKeywords = 0;
    //private bool isFlickering = false;
    private bool sent = false;              // control var ; prompt sent or not ?
    private bool userIsOriented = false;    // control var ; is orientation phase over ?
    private bool removeFrames = false;      // control var ; start removing picture frames ?
    private bool cooldown = false;          // control var ; picture swap on cooldown ?
    private bool styleAdded = false;        // control var ; has a style alread been added to the prompt ?

    private System.Random rnd;              // RNG
    //private float timeDelay;
    //private int focusTime = 0;               
    private int remainingPictureFrames = 14;// amount of remaing picture frames in the scene

    //UDP Sender for Prompt
    private UDPSend sender = new UDPSend();

   

    // Start is called before the first frame update
    void Start()
    {
        //start UDP Sender
        sender.Start();
        //init RNG
        rnd = new System.Random();
        
        //scene setup
        SetupRoom();
        SelectFromPool();
    }

    // Update is called once per frame
    void Update()
    {   
        //program flow
        if (Input.GetKeyDown("space"))
            ManageInput();
        
        if (userIsOriented == true)
            CheckEyeFocus();

        if (removeFrames && !cooldown)
            StartCoroutine(RemovePictureFrames());
        
        if(pictureFrames.Count == 0)
        {
            gameObject.SetActive(false);
            personalized.SetActive(true);
            
            //set the first two prompt term as picture title
            string[] t = finalPrompt.ToString().Split(new[] { ',' }, 2);
            string title = t[0].ToUpper();
            personalized.GetComponentInChildren<TextMeshPro>().text = title;
        }
    }
    
    /// <summary>
    /// Selects pictures for the current run from the picture pool.  
    /// </summary>
    private void SelectFromPool()
    {
        for (int i = 0; i < pictureSelection.Count; i++)
        {
            int pos = rnd.Next(0, picturePool.Count);
            pictureSelection[i] = picturePool[pos];
            picturePool.Remove(picturePool[pos]);
        }
    }

    /// <summary>
    /// Places picture frames in scene and populates them with images from the picture pool.
    /// </summary>
    private void SetupRoom()
    {
        for (int i = 0; i < pictureFrames.Count; i++)
        {
            //get a random number between 0 and the size of the picture pool as index
            int pos = rnd.Next(0, picturePool.Count);
            //Instantiate the prefab at the index in the pool at the position of the corresponding frame
            Instantiate(picturePool[pos], pictureFrames[i].transform);
            //Destroy the initial picture object seen in the scene preview
            Destroy(pictureFrames[i].transform.GetChild(0).gameObject);
            //Remove the used picture from the pool
            picturePool.Remove(picturePool[pos]);
        }
    }
    
    /// <summary>
    /// Checks the user's eye focus and handles the picture swapping feature.
    /// </summary>
    private void CheckEyeFocus()
    {
        // Check for focused objects.
        if (TobiiXR.FocusedObjects.Count > 0)//&& pictureSelection.Count > 0
        {
            
            focused = TobiiXR.FocusedObjects[0].GameObject;
            //Debug.Log(focused.transform.parent.parent.parent.name);
            //if (focusedLast.Contains(focused.name))
                //focusTime++;
            focusedLast += focused.name;
            
            int pos = rnd.Next(0, pictureSelection.Count);

            // if the focused object has a frame as parent
            if (focused.transform.parent.parent.parent != null)
                //save its name in this variable
                focusedPictureFrame = focused.transform.parent.parent.parent.name;

            // one case per picture frame in scene
            switch (focusedPictureFrame)
            {
                case "PictureF_A1":
                    ResolveCase("pictureF_A1", pictureF_D3, pos);
                    break;

                case "PictureF_A2":
                    ResolveCase("pictureF_A2", pictureF_D2, pos);
                    break;

                case "PictureF_A3":
                    ResolveCase("pictureF_A3", pictureF_D1, pos);
                    break;

                case "PictureF_B1":
                    ResolveCase("pictureF_B1", pictureF_E3, pos);
                    break;

                case "PictureF_B2":
                    ResolveCase("pictureF_B2", pictureF_E2, pos);
                    break;

                case "PictureF_B3":
                    ResolveCase("pictureF_B3", pictureF_E1, pos);
                    break;

                case "PictureF_C1":
                    ResolveCase("pictureF_C1", pictureF_F3, pos);
                    break;

                case "PictureF_C2":
                    ResolveCase("pictureF_C2", pictureF_F2, pos);
                    break;

                case "PictureF_C3":
                    ResolveCase("pictureF_C3", pictureF_F1, pos);
                    break;

                case "PictureF_D1":
                    ResolveCase("pictureF_D1", pictureF_A3, pos);
                    break;

                case "PictureF_D2":
                    ResolveCase("pictureF_D2", pictureF_A2, pos);
                    break;

                case "PictureF_D3":
                    ResolveCase("pictureF_D3", pictureF_A1, pos);
                    break;

                case "PictureF_E1":
                    ResolveCase("pictureF_E1", pictureF_B3, pos);
                    break;

                case "PictureF_E2":
                    ResolveCase("pictureF_E2", pictureF_B2, pos);
                    break;

                case "PictureF_E3":
                    ResolveCase("pictureF_E3", pictureF_B1, pos);
                    break;

                case "PictureF_F1":
                    ResolveCase("pictureF_F1", pictureF_C3, pos);
                    break;

                case "PictureF_F2":
                    ResolveCase("pictureF_F2", pictureF_C2, pos);
                    break;

                case "PictureF_F3":
                    ResolveCase("pictureF_F3", pictureF_C1, pos);
                    break;

                case "PictureL_A1":
                    ResolveCase("pictureL_A1", pictureL_D3, pos);
                    break;

                case "PictureL_A2":
                    ResolveCase("pictureL_A2", pictureL_D2, pos);
                    break;

                case "PictureL_A3":
                    ResolveCase("pictureL_A3", pictureL_D1, pos);
                    break;

                case "PictureL_B1":
                    ResolveCase("pictureL_B1", pictureL_E3, pos);
                    break;

                case "PictureL_B2":
                    ResolveCase("pictureL_B2", pictureL_E2, pos);
                    break;

                case "PictureL_B3":
                    ResolveCase("pictureL_B3", pictureL_E1, pos);
                    break;

                case "PictureL_C1":
                    ResolveCase("pictureL_C1", pictureL_F3, pos);
                    break;

                case "PictureL_C2":
                    ResolveCase("pictureL_C2", pictureL_F2, pos);
                    break;

                case "PictureL_C3":
                    ResolveCase("pictureL_C3", pictureL_F1, pos);
                    break;

                case "PictureL_D1":
                    ResolveCase("pictureL_D1", pictureL_A3, pos);
                    break;

                case "PictureL_D2":
                    ResolveCase("pictureL_D2", pictureL_A2, pos);
                    break;

                case "PictureL_D3":
                    ResolveCase("pictureL_D3", pictureL_A1, pos);
                    break;

                case "PictureL_E1":
                    ResolveCase("pictureL_E1", pictureL_B3, pos);
                    break;

                case "PictureL_E2":
                    ResolveCase("pictureL_E2", pictureL_B2, pos);
                    break;

                case "PictureL_E3":
                    ResolveCase("pictureL_E3", pictureL_B1, pos);
                    break;

                case "PictureL_F1":
                    ResolveCase("pictureL_F1", pictureL_C3, pos);
                    break;

                case "PictureL_F2":
                    ResolveCase("pictureL_F2", pictureL_C2, pos);
                    break;

                case "PictureL_F3":
                    ResolveCase("pictureL_F3", pictureL_C1, pos);
                    break;
            }

            //if (focusedBefore.Contains(focused.name) && IsStillInFocus() && !sent)
            //{
                
                    
            //    //if (!finalPrompt.Contains(focused.name) && !focused.name.Contains("(Clone)"))
            //    //    finalPrompt += focused.name +", ";
            //    //if (!finalPrompt.Contains(focused.GetComponent<Text>().text))
            //    //    finalPrompt += focused.GetComponent<Text>().text + ", ";
            //    Debug.Log(finalPrompt);
            //}

            // add the name of the currently focused object to the list of previously focused objects
            focusedBefore.Add(focused.name);
            // if the focused object has a text component
            if(focused.GetComponent<Text>() != null)
                // add its content to the styles list
                styles.Add(focused.GetComponent<Text>().text);
        }
    }

    /// <summary>
    /// Prevents the picture inside of the same frame from being swapped twice in a row.
    /// </summary>
    /// <param name="inFocus">object in focus</param> 
    /// <param name="swapTarget">the picture frame that is supposed to swap pictures</param> 
    /// <param name="picturePosInSelection">index position of the new picture inside of the picture selection</param> 
    private void ResolveCase(String inFocus, GameObject swapTarget, int picturePosInSelection)
    {
        if (!changedLast.Equals(inFocus) && pictureSelection.Count != 0)
            SwapPicture(inFocus, swapTarget, picturePosInSelection);
    }

    /// <summary>
    /// Swaps the picture being displayed in a given frame.
    /// </summary>
    /// <param name="inFocus">object in focus</param> 
    /// <param name="swapTarget">the picture frame that is supposed to swap pictures</param> 
    /// <param name="picturePosInSelection">index position of the new picture inside of the picture selection</param> 
    private void SwapPicture(String inFocus, GameObject swapTarget, int picturePosInSelection)
    {
        if(swapTarget != null)
        {
            Debug.Log("swapping picture");

            GameObject pictureToSet = pictureSelection[picturePosInSelection];
            Instantiate(pictureToSet, swapTarget.transform);
            Destroy(swapTarget.transform.GetChild(0).gameObject);
            pictureSelection.Remove(pictureToSet);

            changedLast = inFocus;
        }
       
    }

    //private bool IsStillInFocus()
    //{
    //    if (focusTime > 5 * 90)
    //        return true;
    //    else
    //        return false;
    //}

    /// <summary>
    /// Responds to space bar input by the app operator according to the current program state.
    /// </summary>
    private void ManageInput()
    {   
        //  if the final picture is being displayed
        if (personalized.activeSelf)
        {
            // reload scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // if the prompt has been sent
        else if (sent)
        {
            //gameObject.SetActive(false);
            personalized.SetActive(true);
            
            string[] t = finalPrompt.ToString().Split(new[] { ',' }, 2);
            string title = t[0];
            title.Replace("(", "").Replace(")", "");
            //title = title.Remove(0);
            //title = title.Remove(title.Length);
            personalized.GetComponentInChildren<TextMeshPro>().text = title.ToUpper(); 
        }

        // if the prompt has not been sent yet and the orientation pahes is over
        else if (!sent && userIsOriented)
        {
            ConstructPrompt();
            SendPrompt();
            removeFrames = true;
            spotlight1.SetActive(false);
            spotlight2.SetActive(false);
        }

        else
            // end orientation phase
            userIsOriented = true;
    }

    /// <summary>
    /// Constructs the prompt meant for image generation.
    /// </summary>
    private void ConstructPrompt()
    {
        // sort image sections by count of occurrences in focused before string using Linq(like SQL)
        var sortedFeatures = focusedBefore.GroupBy(x => x)
                    .Select(sortedFeatures => new { Value = 
                    sortedFeatures.Key, Count = sortedFeatures.Count()})
                    .OrderByDescending(x => x.Count);

        // sort style elements by count of occurrences in styles string using Linq(like SQL)
        var sortedStyles = styles.GroupBy(x => x)
                    .Select(sortedStyles => new { Value = 
                    sortedStyles.Key, Count = sortedStyles.Count()})
                    .OrderByDescending(x => x.Count);

        // adds the first element in sorted image features to the prompt + formatting
        if (!finalPrompt.Contains(sortedFeatures.ElementAt(0).Value) 
            && !sortedFeatures.ElementAt(0).Value.Contains("(Clone)"))
            finalPrompt += "(" + sortedFeatures.ElementAt(0).Value + ")" + ", ";

        // adds the second element in sorted image features to the prompt + formatting
        if (!finalPrompt.Contains(sortedFeatures.ElementAt(1).Value) 
            && !sortedFeatures.ElementAt(1).Value.Contains("(Clone)"))
            finalPrompt += sortedFeatures.ElementAt(1).Value + ", ";

        // adds the first element in sorted styles to the prompt + formatting
        if (!finalPrompt.Contains(sortedStyles.ElementAt(0).Value))
            finalPrompt += sortedStyles.ElementAt(0).Value + ", ";         
    }

    /// <summary>
    /// Sends the finished prompt to the second pc system for image generation.
    /// </summary>
    private void SendPrompt()
    {
        if (!sent)
        {    
            sender.sendString(finalPrompt);
            sent = true;
        }
    }

    /// <summary>
    /// Coroutine removes a random picture frame every second.
    /// (A coroutine allows you to spread tasks across several frames. 
    /// In Unity, a coroutine is a method that can pause execution 
    /// and return control to Unity but then continue where it left off on the following frame.)
    /// </summary>
    /// <returns>suspend execution for one second</returns>
    IEnumerator RemovePictureFrames()
    {
        cooldown = true;
        int pos = rnd.Next(0, pictureFrames.Count);

        pictureFrames[pos].SetActive(false);
        pictureFrames.Remove(pictureFrames[pos]);

        yield return new WaitForSeconds(1f);
        cooldown = false;
    }

    //IEnumerator FlickerLight(GameObject swapTarget)
    //{
    //    isFlickering = true;
    //    flickeringLights.Add(swapTarget.name);

    //    swapTarget.GetComponentInChildren<Light>().enabled = false;
    //    timeDelay = rnd.Next(1, 3);
    //    yield return new WaitForSeconds(timeDelay);

    //    swapTarget.GetComponentInChildren<Light>().enabled = true;
    //    timeDelay = rnd.Next(1, 3);
    //    yield return new WaitForSeconds(timeDelay);
    //}

    //private void ChangeLable(GameObject swapTarget, GameObject pictureToSet)
    //{
    //    swapTarget.GetComponentInChildren<TextMeshPro>().text = pictureToSet.name;
    //}
}

