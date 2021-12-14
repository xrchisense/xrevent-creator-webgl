using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using JetBrains.Annotations;
using Siccity.GLTFUtility;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace Xrchitecture.Creator.Common.Data
{
    public class GLTFLoader
    {
        public static void CreateModelFromAddress(string modelUrl, Action<GameObject> onSuccess)
        {
            
            HelperBehaviour.Instance.StartCoroutine(CreationRoutine());

            IEnumerator CreationRoutine()
            {
                
                string path = FileDownloader.GetFilePath(modelUrl);

                /*
                if (!SkipCache && File.Exists(path))
                {
                    onSuccess(path);
                    yield return null;
                }
                */
                
                UnityWebRequest req = UnityWebRequest.Get(modelUrl);

                int fileSize = 0;
                req.downloadHandler = new DownloadHandlerFile(FileDownloader.GetFilePath(modelUrl));
                req.SendWebRequest();

                while (!req.isDone)
                {
                    if (fileSize == 0)
                    {
                        Int32.TryParse(req.GetResponseHeader("Content-Length"),
                            out fileSize);
                    }

                    yield return new WaitForSeconds(.1f);
                }
                
                string result = FileDownloader.GetFilePath(modelUrl);


                GameObject createdObject = null;

                try

                {
                    createdObject = Importer.LoadFromFile(result,
                        new ImportSettings() {useLegacyClips = true});
                }
                catch (Exception e)
                {
                    Debug.LogError("Cannot load model to create GameObject!!" +
                                   Environment.NewLine + e.Message);
                }

                onSuccess(createdObject);
            }
        }
    }
}