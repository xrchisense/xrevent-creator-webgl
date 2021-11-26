using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Siccity.GLTFUtility;
using UnityEngine.Networking;
using System;


public class gltfImporter : MonoBehaviour
{
    GameObject loadedObject;
    string filePath;

    private void Start()
    {
        Debug.Log("D1");
        filePath = $"{Application.persistentDataPath}/Files/test.gltf";
        loadedObject = new GameObject
        {
            name = "Model"
        };
        Debug.Log("D2");
        
    }

    public void importGltfFromServer(string url)
    {
        Debug.Log("D4");
        DownloadFile(url); //http://fischerkinder.de/upload/low-poly-fox-by-pixelmannen.gltf
        Debug.Log("D5");
    }
    void ImportGLTF(string filepath)
    {
        Debug.Log("D12");
        loadedObject = Importer.LoadFromFile(filepath);
        loadedObject.layer = 9; // set to "Spawned Objects" layer to finde them later and save their properties to json build plan
        loadedObject.AddComponent<MeshCollider>();
    }

    public void DownloadFile(string url)
    {
        Debug.Log("D6");
        Debug.Log(url);
        StartCoroutine(GetFileRequest(url, (UnityWebRequest req) =>
        {
            Debug.Log("D7");
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.Log("D20");
                // Log any errors that may happen
                Debug.Log($"{req.error} : {req.downloadHandler.text}");
                Debug.Log("D23");
            }
            else
            {
                Debug.Log("D22");
                ResetObject();
                ImportGLTF(filePath);
            }
        }));
        Debug.Log("D8");
    }

    IEnumerator GetFileRequest(string url, Action<UnityWebRequest> callback)
    {
        Debug.Log("D25");
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            Debug.Log("D26");
            req.downloadHandler = new DownloadHandlerFile(filePath);
            yield return req.SendWebRequest();
            callback(req);
        }
        Debug.Log("D27");
    }


    void ResetObject()
    {
        Debug.Log("D10");
        if (loadedObject != null)
        {
            foreach (Transform trans in loadedObject.transform)
            {
                Destroy(trans.gameObject);
            }
        }
    }

}
