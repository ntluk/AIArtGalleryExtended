using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMH
{
    public class Test : MonoBehaviour
    {
        
        //[SerializeField] Avatar avatar;
        GameObject newT;
        

        // Start is called before the first frame update
        void Start()
        {
            newT = new GameObject();

            GameObject LeftHand = GameObject.Find("Left_Wrist_Joint_01");
            if (LeftHand != null)
                newT.transform.position = LeftHand.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            GameObject LeftHand = GameObject.Find("Left_Wrist_Joint_01");
            if (LeftHand != null)
                newT.transform.position = LeftHand.transform.position;
            
                Debug.Log(newT.transform.position);



    }
    }
}
