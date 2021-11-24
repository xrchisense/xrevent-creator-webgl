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
        public static GameObject CreateModelFromAddress(string modelUrl)
        {
            string result = DownloadModelFromAddress(modelUrl);

            return CreateGameObject(result);
        }
        
        private static string DownloadModelFromAddress(string modelUrl)
        {
            UnityWebRequest req = UnityWebRequest.Get(modelUrl);

            int fileSize = 0;
            req.downloadHandler = new DownloadHandlerFile(FileDownloader.GetFilePath(modelUrl));
            req.SendWebRequest();

            while (!req.isDone)
            {
                if (fileSize == 0)
                {
                    Int32.TryParse(req.GetResponseHeader("Content-Length"), out fileSize);
                }
            }

            return FileDownloader.GetFilePath(modelUrl);
        }


        [CanBeNull]
        private static GameObject CreateGameObject(string path)
        {
            GameObject createdObject = null;
            try

            {
                createdObject = Importer.LoadFromFile(path, new ImportSettings() {useLegacyClips = true});
            }
            catch (Exception e)
            {
                Debug.LogError("Cannot load model to create GameObject!!" + Environment.NewLine + e.Message);
            }

            return createdObject;
        }
    }
}