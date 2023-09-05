using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;

public class PointerHandler : MonoBehaviour
{
    public SteamVR_LaserPointer leftLaserPointer;
    public SteamVR_LaserPointer rightLaserPointer;
    public Material transparentMaterial;
    public Material highlightMaterial;
    public Material selectionMaterial;

    void Awake()
    {
        leftLaserPointer.PointerIn += PointerInside;
        leftLaserPointer.PointerOut += PointerOutside;
        leftLaserPointer.PointerClick += PointerClick;

        rightLaserPointer.PointerIn += PointerInside;
        rightLaserPointer.PointerOut += PointerOutside;
        rightLaserPointer.PointerClick += PointerClick;
    }

    public void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.GetComponent<MeshRenderer>() != null)
        {
            GalleryManager.instance.focusedBefore.Add(e.target.name);
            Debug.Log(GalleryManager.instance.focusedBefore);
            if (e.target.GetComponent<Text>() != null)
                GalleryManager.instance.styles.Add(e.target.GetComponent<Text>().text);
            Debug.Log(GalleryManager.instance.styles);

            if (e.target.GetComponent<MeshRenderer>().material == selectionMaterial)
                e.target.GetComponent<MeshRenderer>().material = highlightMaterial;
            e.target.GetComponent<MeshRenderer>().material = selectionMaterial;
            Debug.Log("Quad was clicked");
        }
    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target.GetComponent<MeshRenderer>() != null && e.target.GetComponent<MeshRenderer>().material != selectionMaterial)
        {
            e.target.GetComponent<MeshRenderer>().material = highlightMaterial;
            Debug.Log("Quad was entered");
        }
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        if (e.target.GetComponent<MeshRenderer>() != null && e.target.GetComponent<MeshRenderer>().material != selectionMaterial)
        {
            e.target.GetComponent<MeshRenderer>().material = transparentMaterial;
            Debug.Log("Quad was exited");
        }

    }
}
