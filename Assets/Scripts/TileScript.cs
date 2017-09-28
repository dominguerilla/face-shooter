using UnityEngine;
using System.Collections;

//https://gamedev.stackexchange.com/questions/111060/unity-tiling-of-a-material-independen-of-its-size
[ExecuteInEditMode]
public class TileScript : MonoBehaviour
{

    public float scaleFactor = 5.0f;
    Material mat;
    // Use this for initialization
    void Start()
    {
        GetComponent<Renderer>().sharedMaterial.mainTextureScale = new Vector2(transform.localScale.x / scaleFactor, transform.localScale.z / scaleFactor);
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.hasChanged && Application.isEditor && !Application.isPlaying)
        {
            Debug.Log("The transform has changed!");
            GetComponent<Renderer>().sharedMaterial.mainTextureScale = new Vector2(transform.localScale.x / scaleFactor, transform.localScale.z / scaleFactor);
            transform.hasChanged = false;
        }

    }
}