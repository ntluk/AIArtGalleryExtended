using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothSkeleton : MonoBehaviour
{
    [System.Serializable]
    public class TransformData
    {
        public GameObject trans;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public TransformData(GameObject trans, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.trans = trans;
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }
    }
    public float smooth;
    [SerializeField]
    public List<TransformData> bones;
    public List<TransformData> bonesThisFrame;
    public GameObject boneroot;
    public void SetupTransformData(GameObject g)
    {
        bones.Add(new TransformData(g, g.transform.position, g.transform.rotation, g.transform.localScale));
        for (int i = 0; i < g.transform.childCount; i++)
            SetupTransformData(g.transform.GetChild(i).gameObject);
    }
    public void Start()
    {
        SetupTransformData(boneroot);
    }
    public void LateUpdate()
    {
        foreach (TransformData td in bones)
        {
            td.trans.transform.position = Vector3.Lerp(td.position, td.trans.transform.position, smooth);
            td.trans.transform.rotation = Quaternion.Lerp(td.rotation, td.trans.transform.rotation, smooth);
            td.trans.transform.localScale = Vector3.Lerp(td.scale, td.trans.transform.localScale, smooth);
            td.position = td.trans.transform.position;
            td.rotation = td.trans.transform.rotation;
            td.scale = td.trans.transform.localScale;
        }
    }
}
