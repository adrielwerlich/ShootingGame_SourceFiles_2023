using UnityEngine;
using Siccity.GLTFUtility;
using UnityEditor;

public class ImportGlbModel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //ImportGLTF("Assets/GlbModels/krishna-peacock-feather-v1.glb");
    }

    void ImportGLTF(string filepath)
    {
        GameObject result = Importer.LoadFromFile(filepath);

        if (result != null)
        {
            Debug.Log(" result " + EditorJsonUtility.ToJson(result, true));
            result.transform.position = new Vector3(346.06f, 0.13f, 132.28f);
            result.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            result.transform.Rotate(0, -147.41f, 0);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
