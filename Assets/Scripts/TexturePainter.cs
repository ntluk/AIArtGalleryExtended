using UnityEngine;
using System.Collections;
using System.IO;
using System;
using UnityEngine.UI;

public class TexturePainter : MonoBehaviour
{
    public GameObject brushCursor, brushContainer; //The cursor that overlaps the model and our container for the brushes painted
    public Camera sceneCamera, canvasCam;  //The camera that looks at the model, and the camera that looks at the canvas.
    public Sprite cursorPaint; // Cursor for the differen functions 
    public RenderTexture canvasTexture; // Render Texture that looks at our Base Texture and the painted brushes
    public Material baseMaterial; // The material of our base texture (Were we will save the painted texture

    //Textures and models for painting
    public Texture2D tex;
    public Texture2D tex2;
    public GameObject statue;


    //Save textures
    public Material saveMat;
    public GameObject rendTex;

    public MovePicture movePicture;

    public Sprite CanvSprite;

    public GameObject drawCube;

    [SerializeField] MovePicture controlMode;


    //public GameObject Image;



    Color brushColor; //The selected color
    public float brushSize = 0.6f; //The size of our brush
    int brushCounter = 0, MAX_BRUSH_COUNT = 20000; //To avoid having millions of brushes
    bool saving = false; //Flag to check if we are saving the texture






    void Start()
    {




        brushColor = Color.black;
        brushCursor.GetComponent<SpriteRenderer>().sprite = cursorPaint;
    }

    void Update()
    {



        if (Input.GetMouseButton(0) && movePicture.gameMode == MovePicture.GameMode.Canva)
        {
            DoAction();
        }

        if (movePicture.gameMode != MovePicture.GameMode.Canva)
        {
            brushCursor.SetActive(false);
            restoreMaterial();
        }


        




        UpdateBrushCursor();





        //save texture

        /*SaveTexture2D();
         statue.GetComponent<MeshRenderer>().material.mainTexture = canvasTexture;
     */



        //reset texture
        /*
            restoreMaterial();
            rendTex.GetComponent<MeshRenderer>().material.mainTexture = tex;                        
            statue.GetComponent<MeshRenderer>().material.mainTexture = canvasTexture;
            grabColor.resetPaint = false;
			*/

        //load texture

        /*
        
            restoreMaterial();                                                                      
            rendTex.GetComponent<MeshRenderer>().material.mainTexture = tex2;
            statue.GetComponent<MeshRenderer>().material.mainTexture = canvasTexture;
            grabColor.loadSave = false;
        
*/


    }

    public void SaveTexture2D()
    {
        if (statue == null)
        {
            throw new FileNotFoundException("Keine Statue gefunden");
        }
        //brushCursor.SetActive(false);

        Texture2D myTexture = toTexture2D(canvasTexture);           //brushstrokes werden auf textur übertragen

        //speichern in assets
        saveMat.mainTexture = tex2;
        rendTex.GetComponent<MeshRenderer>().material.mainTexture = tex2;           //statue bekommt neue textur mit gespeicherten strokes
        statue.GetComponent<MeshRenderer>().material.mainTexture = canvasTexture;
        restoreMaterial();                                                          //stroke objekte werden gelöscht.
                                                                                    //StartCoroutine(SaveTextureToFile(tex2));
                                                                                    //brushCursor.SetActive(true);
        statue.GetComponent<MeshRenderer>().material.mainTexture = tex2;
    }

    //The main action, instantiates a brush or decal entity at the clicked position on the UV map
    void DoAction()
    {
        if (saving)
        {
            return;
        }
        Vector3 uvWorldPosition = Vector3.zero;

        if (HitTestUVPosition(ref uvWorldPosition))
        {
            GameObject brushObj;
            brushObj = (GameObject)Instantiate(Resources.Load("TexturePainter-Instances/BrushEntity"));
            brushObj.GetComponent<SpriteRenderer>().color = brushColor; //Set the brush color



            brushColor.a = 100; // Brushes have alpha to have a merging effect when painted over.
            brushObj.transform.parent = brushContainer.transform; //Add the brush to our container to be wiped later
            brushObj.transform.localPosition = uvWorldPosition; //The position of the brush (in the UVMap)
            brushObj.transform.localScale = Vector3.one * brushSize;//The size of the brush

            //Debug.Log(brushObj.transform.localEulerAngles);

        }
        brushCounter++; //Add to the max brushes
        if (brushCounter >= MAX_BRUSH_COUNT)
        {
            //If we reach the max brushes available, flatten the texture and clear the brushes
            brushCursor.SetActive(false);
            saving = true;
            //Invoke("SaveTexture",0.1f);
        }
    }
    //To update at realtime the painting cursor on the mesh
    void UpdateBrushCursor()
    {
        Vector3 uvWorldPosition = Vector3.zero;
        if (HitTestUVPosition(ref uvWorldPosition) && !saving && movePicture.gameMode == MovePicture.GameMode.Canva)
        {
            brushCursor.SetActive(true);
            brushCursor.transform.position = uvWorldPosition + brushContainer.transform.position;
        }
        else
        {
            brushCursor.SetActive(false);
        }
        brushCursor.transform.localScale = Vector3.one * brushSize;
    }
    //Returns the position on the texuremap according to a hit in the mesh collider
    public bool HitTestUVPosition(ref Vector3 uvWorldPosition)
    {
        RaycastHit hit;
        if (controlMode.control == MovePicture.ControlMode.Keyboard)

        {
            Vector3 cursorPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);

            Ray cursorRay = sceneCamera.ScreenPointToRay(cursorPos);

            if (Physics.Raycast(cursorRay, out hit, 500))
            {
                if (hit.collider.gameObject == statue)
                {
                    MeshCollider meshCollider = hit.collider as MeshCollider;
                    if (meshCollider == null || meshCollider.sharedMesh == null)
                        return false;
                    Vector2 pixelUV = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
                    uvWorldPosition.x = pixelUV.x - canvasCam.orthographicSize;//To center the UV on X
                    uvWorldPosition.y = pixelUV.y - canvasCam.orthographicSize;//To center the UV on Y
                    uvWorldPosition.z = 0.0f;

                    return true;
                }
                else
                    return false;
            }
            else
            {
                return false;
            }
        }     
        else
        {
           

            if (Physics.Raycast(drawCube.transform.position, drawCube.transform.forward, out hit, 1000))
            {
                MeshCollider meshCollider = hit.collider as MeshCollider;
                if (meshCollider == null || meshCollider.sharedMesh == null)
                    return false;
                Vector2 pixelUV = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
                uvWorldPosition.x = pixelUV.x - canvasCam.orthographicSize;//To center the UV on X
                uvWorldPosition.y = pixelUV.y - canvasCam.orthographicSize;//To center the UV on Y
                uvWorldPosition.z = 0.0f;
                return true;
            }
            else
            {
                return false;
            }
        }


        //With VR

        /*
        RaycastHit hit;

        if (Physics.Raycast(GunPaint.transform.position, GunPaint.transform.forward, out hit, 200))
        {
            MeshCollider meshCollider = hit.collider as MeshCollider;
            if (meshCollider == null || meshCollider.sharedMesh == null)
                return false;
            Vector2 pixelUV = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
            uvWorldPosition.x = pixelUV.x - canvasCam.orthographicSize;//To center the UV on X
            uvWorldPosition.y = pixelUV.y - canvasCam.orthographicSize;//To center the UV on Y
            uvWorldPosition.z = 0.0f;
            return true;
        }
        else
        {
            return false;
        }
		*/
    }



    //Changes the Cursor depending on burshmode



    public void SetBrushSize()
    { //Sets the size of the cursor brush or decal
      //brushSize = BrushSize.value;
        brushCursor.transform.localScale = Vector3.one * brushSize;
    }

    /*public void SetColor(Color newColor)
    {
		brushColor = newColor;
    }*/



    //Savetexture local
#if !UNITY_WEBPLAYER
    IEnumerator SaveTextureToFile(Texture2D savedTexture)
    {
        brushCounter = 0;
        string fullPath=System.IO.Directory.GetCurrentDirectory()+"\\Assets\\SavedTextures\\";
        //string fullPath = "C:\\Users\\soren\\OneDrive\\Desktop\\SavedTextures\\";
        System.DateTime date = System.DateTime.Now;

        string fileName = "_CanvasTexture.png";

        if (!System.IO.Directory.Exists(fullPath))
            System.IO.Directory.CreateDirectory(fullPath);
        var bytes = savedTexture.EncodeToJPG();
        System.IO.File.WriteAllBytes(fullPath + fileName, bytes);
        Debug.Log("<color=orange>Saved Successfully!</color>" + fullPath + fileName);
        yield return null;
    }
#endif


    //Sets the base material with a our canvas texture, then removes all our brushes
    /*void SaveTexture(){		
		brushCounter=0;
		System.DateTime date = System.DateTime.Now;
		RenderTexture.active = canvasTexture;
		Texture2D tex = new Texture2D(canvasTexture.width, canvasTexture.height, TextureFormat.RGB24, false);		
		tex.ReadPixels (new Rect (0, 0, canvasTexture.width, canvasTexture.height), 0, 0);
		tex.Apply ();
		RenderTexture.active = null;
		baseMaterial.mainTexture =tex;  //Put the painted texture as the base
		StartCoroutine(SaveTextureToFile(tex));
		foreach (Transform child in brushContainer.transform) {//Clear brushes
			Destroy(child.gameObject);
		}
		//Do you want to save the texture? This is your method!
		Invoke ("ShowCursor", 0.1f);
	}
	//Show again the user cursor (To avoid saving it to the texture)
	void ShowCursor(){	
		saving = false;
	}*/
    //restore texture
    public void restoreMaterial()
    {
        foreach (Transform child in brushContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }
    //Save texture
    Texture2D toTexture2D(RenderTexture rTex)
    {
        RenderTexture.active = rTex;
        tex2 = new Texture2D(2048, 2048, TextureFormat.RGBA32, false);
        // ReadPixels looks at the active RenderTexture.
        tex2.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex2.Apply();
        saveMat.mainTexture = tex2;

        return tex2;
    }
    public void MaterialToStandard()
    {
        Material currentMaterial = statue.GetComponent<MeshRenderer>().material; // Aktuelles Material des Renderers

        // Erstelle ein neues Material mit dem gewünschten Shader
        Material newMaterial = new Material(Shader.Find("Standard"));
        Debug.Log("Standard");
        // Kopiere die Eigenschaften des aktuellen Materials in das neue Material
        newMaterial.CopyPropertiesFromMaterial(currentMaterial);

        // Weise das neue Material dem Renderer zu
        statue.GetComponent<MeshRenderer>().material = newMaterial;
    }
    public void MaterialToUnlit()
    {
        Material currentMaterial = statue.GetComponent<MeshRenderer>().material; // Aktuelles Material des Renderers

        // Erstelle ein neues Material mit dem gewünschten Shader
        Material newMaterial = new Material(Shader.Find("Unlit/Texture"));

        // Kopiere die Eigenschaften des aktuellen Materials in das neue Material
        newMaterial.CopyPropertiesFromMaterial(currentMaterial);

        // Weise das neue Material dem Renderer zu
        statue.GetComponent<MeshRenderer>().material = newMaterial;
    }

    public static Sprite TextureToSprite(Texture2D texture)
    {
        Sprite CanvSprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 50f, 0, SpriteMeshType.FullRect);

        return CanvSprite;
    }

public void SpriteCreate()
{
   
            brushCursor.SetActive(false);
            Texture2D myTexture = toTexture2D(canvasTexture); 
            CanvSprite = TextureToSprite(myTexture);
            //Image.GetComponent<Image>().sprite = CanvSprite;
            StartCoroutine(SaveTextureToFile(myTexture));
            brushCursor.SetActive(true);
}


}
