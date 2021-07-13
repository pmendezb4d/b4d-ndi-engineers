using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace B4D.DataManagement
{
    public class DownloaderManager : MonoBehaviour
    {
        public static DownloaderManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }


        public void GetAssetTexture2D(string url, Action<Texture2D> onDownloadComplete, Action<string> onDownloadError, bool forceUpdate)
        {
            StartCoroutine(GetAssetTextureRoutine(url, onDownloadComplete, onDownloadError, forceUpdate));
        }
        public void GetAsseBundle(string url, string name, Action<string> onDownloadComplete, Action<string> onDownloadError, Action<float> onProgressUpdate = null)
        {
            StartCoroutine(GetAssetBundleRoutine(url, name, onDownloadComplete, onDownloadError, onProgressUpdate));
        }
        public void GetAssetSpecial(string url, string name, Action<string> onDownloadComplete, Action<string> onDownloadError, Action<float> onProgressUpdate = null)
        {
            StartCoroutine(GetAssetSpecialRoutine(url, name, onDownloadComplete, onDownloadError, onProgressUpdate));
        }
        public void GetText(string url, Action<string> onDownloadComplete, Action<string> onDownloadError)
        {
            StartCoroutine(GetTextRoutine(url, onDownloadComplete, onDownloadError));
        }
        public void GetText(string url, string json,  Action<string> onDownloadComplete, Action<string> onDownloadError)
        {
            StartCoroutine(GetTextRoutine(url, json, onDownloadComplete, onDownloadError));
        }public void GetText(string url, string json,  Action<long ,string> onDownloadComplete, Action<string> onDownloadError)
        {
            StartCoroutine(GetTextRoutine(url, json, onDownloadComplete, onDownloadError));
        }
        public void GetFile(string url, Action<byte[]> onDownloadComplete, Action<string> onDownloadError)
        {
            StartCoroutine(GetFileRoutine(url, onDownloadComplete, onDownloadError));
        }

        private IEnumerator GetAssetTextureRoutine(string url, Action<Texture2D> onDownloadComplete, Action<string> onDownloadError, bool forceUpdate)
        {
            bool doesDownlowadIsRequired = true;
            if (!forceUpdate && Storage.Instance.DoesImageExists(url))
            {
                Texture2D texture = Storage.Instance.GetImage(url);

                if (texture == null)
                {
                    doesDownlowadIsRequired = true;
                }
                else
                {
                    doesDownlowadIsRequired = false;
                    onDownloadComplete(texture);
                }
            }

            if (doesDownlowadIsRequired)
            {
                using (UnityWebRequest www = UnityWebRequest.Get(url))
                {
                    www.downloadHandler = new DownloadHandlerTexture();

                    // Request and wait for the desired page.
                    yield return www.SendWebRequest();

                    if (www.isHttpError || www.isNetworkError)
                    {
                        if (onDownloadError != null)
                            onDownloadError(www.error);

                        //Check cache
                        if (Storage.Instance.DoesImageExists(url))
                        {
                            Texture2D texture = Storage.Instance.GetImage(url);

                            if (texture != null)
                                onDownloadComplete(texture);
                        }
                    }
                    else
                    {
                        //TODO: Verify memory
                        Texture2D t = DownloadHandlerTexture.GetContent(www);
                        onDownloadComplete(t);

                        Storage.Instance.SaveTexture(url, t);
                    }
                }
            }


        }

        private IEnumerator GetAssetBundleRoutine(string url, string name, Action<string> onDownloadComplete, Action<string> onDownloadError, Action<float> onProgressUpdate = null)
        {
            Debug.Log("Downloading asset: " + name);
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                //www.downloadHandler = new DownloadHandlerAssetBundle();

                // Request and wait for the desired page.
                
                var operation = www.SendWebRequest();
                while(!operation.isDone)
                {
                    float progress = www.downloadProgress;
                    onProgressUpdate?.Invoke(progress);
                    yield return null;
                }
                

                if (www.isHttpError || www.isNetworkError)
                {
                    //Check cache
                    bool fileExists = Storage.Instance.DoesFileExists(name);

                    if(fileExists)
                        onDownloadComplete?.Invoke(Storage.Instance.GetPath(name));

                    if (!fileExists)
                        onDownloadError?.Invoke(www.error);

                    //FirebaseManager.Instance.LogNetworkError(www.error);
                }
                else
                {
                    //Save
                    Storage.Instance.SaveToDisk(www.downloadHandler.data, name);
                    string path = Storage.Instance.GetPath(name);
                    onDownloadComplete?.Invoke(path);
                }
            }
        }

        private IEnumerator GetAssetSpecialRoutine(string url, string name, Action<string> onDownloadComplete, Action<string> onDownloadError, Action<float> onProgressUpdate = null)
        {
            Debug.Log("Downloading asset: " + name);
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                // Request and wait for the desired page.

                var operation = www.SendWebRequest();
                while (!operation.isDone)
                {
                    float progress = www.downloadProgress;
                    onProgressUpdate?.Invoke(progress);
                    yield return null;
                }


                if (www.isHttpError || www.isNetworkError)
                {
                    //Check cache
                    bool fileExists = Storage.Instance.DoesFileExists(name);

                    if (fileExists)
                        onDownloadComplete?.Invoke(Storage.Instance.GetPath(name));

                    if (!fileExists)
                        onDownloadError?.Invoke(www.error);

                    //FirebaseManager.Instance.LogNetworkError(www.error);
                }
                else
                {
                    //Save
                    Storage.Instance.SaveToDisk(www.downloadHandler.data, name);
                    string path = Storage.Instance.GetPath(name);
                    onDownloadComplete?.Invoke(path);
                }
            }
        }

        private IEnumerator GetTextRoutine(string url, Action<string> onDownloadComplete, Action<string> onDownloadError)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                // Request and wait for the desired page.
                yield return www.SendWebRequest();

                if (www.isHttpError || www.isNetworkError)
                {
                    if (onDownloadError != null)
                        onDownloadError(www.error);

                    //FirebaseManager.Instance.LogNetworkError(www.error);
                }
                else
                {
                    string s = www.downloadHandler.text;
                    onDownloadComplete(s);
                }
            }
        }

        private IEnumerator GetTextRoutine(string url, string json, Action<string> onDownloadComplete, Action<string> onDownloadError)
        {
            var www = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.isHttpError || www.isNetworkError)
            {
                onDownloadError?.Invoke(www.error);
                //FirebaseManager.Instance.LogNetworkError(www.error);
            }
            else
            {
                string s = www.downloadHandler.text;
                onDownloadComplete(s);
            }
        }
        private IEnumerator GetTextRoutine(string url, string json, Action<long, string> onDownloadComplete, Action<string> onDownloadError)
        {
            var www = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.isHttpError || www.isNetworkError)
            {
                onDownloadError?.Invoke(www.error);
                //FirebaseManager.Instance.LogNetworkError(www.error);
            }
            else
            {
                long code = www.responseCode;
                string s = www.downloadHandler.text;
                onDownloadComplete(code, s);
            }
        }

        private IEnumerator GetFileRoutine(string url, Action<byte[]> onDownloadComplete, Action<string> onDownloadError)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                // Request and wait for the desired page.
                yield return www.SendWebRequest();

                if (www.isHttpError || www.isNetworkError)
                {
                    if (onDownloadError != null)
                        onDownloadError(www.error);

                    Debug.Log("Error downloading: " + www.error);
                }
                else
                {
                    byte[] bytes = www.downloadHandler.data;
                    onDownloadComplete(bytes);
                }
            }
        }
    }
}