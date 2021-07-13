using System.IO;
using System.Text;
using UnityEngine;

namespace B4D.DataManagement
{
    public class Storage : MonoBehaviour
    {
        private const string CACHE_FOLDER = "Cache";

        public static Storage Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private void Start()
        {
            CreateDirectory();
        }

        public bool SaveToDisk(string data, string name)
        {
            string path = Path.Combine(Application.persistentDataPath, CACHE_FOLDER, name);
            if (File.Exists(path))
            {
                string actualString = File.ReadAllText(path);

                string actualHash = Md5Sum(Encoding.ASCII.GetBytes(actualString));
                string newhash = Md5Sum(Encoding.ASCII.GetBytes(data));

                //Same file
                if (actualHash.Equals(newhash))
                {
                    //Do nothing
                    return false;
                }
                else
                {
                    //Save new
                    File.WriteAllText(path, data);
                    return true;
                }
            }
            else
            {
                File.WriteAllText(path, data);
                Debug.Log("Saving to: " + path);
                return true;
            }
        }

        public bool DeleteFile(string name)
        {
            string path = Path.Combine(Application.persistentDataPath, CACHE_FOLDER, name);
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            else
                return false;
        }

        public bool SaveToDisk(byte[] data, string name)
        {
            string path = Path.Combine(Application.persistentDataPath, CACHE_FOLDER, name);
            if (File.Exists(path))
            {
                byte[] actualBytes = File.ReadAllBytes(path);

                string actualHash = Md5Sum(actualBytes);
                string newhash = Md5Sum(data);

                //Same file
                if (actualHash.Equals(newhash))
                {
                    //Do nothing
                    return false;
                }
                else
                {
                    //Save new
                    File.WriteAllBytes(path, data);
                    return true;
                }
            }
            else
            {
                File.WriteAllBytes(path, data);
                Debug.Log("Saving to: " + path);
                return true;
            }
        }

        public bool DoesDataIsNew(string data, string name)
        {
            string path = Path.Combine(Application.persistentDataPath, CACHE_FOLDER, name);
            if (File.Exists(path))
            {
                string actualString = File.ReadAllText(path);

                string actualHash = Md5Sum(Encoding.ASCII.GetBytes(actualString));
                string newhash = Md5Sum(Encoding.ASCII.GetBytes(data));

                //Same file
                if (actualHash.Equals(newhash))
                {
                    //It's the same
                    return false;
                }
                else
                {
                    //It's different
                    return true;
                }
            }
            else
            {
                //File does not exists yet
                return true;
            }
        }

        public void SaveTexture(string url, Texture2D texture)
        {
            string md5 = Md5Sum(Encoding.ASCII.GetBytes(url));
            byte[] data = texture.EncodeToPNG();

            string path = Path.Combine(Application.persistentDataPath, CACHE_FOLDER, md5 + ".png");
            File.WriteAllBytes(path, data);
        }

        public Texture2D GetImage(string url)
        {
            string md5 = Md5Sum(Encoding.ASCII.GetBytes(url));

            string path = Path.Combine(Application.persistentDataPath, CACHE_FOLDER, md5 + ".png");
            byte[] data = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(data))
                return texture;

            return null;
        }

        public Texture2D GetTexture(string name)
        {
            string path = Path.Combine(Application.persistentDataPath, CACHE_FOLDER, name);

            if (!DoesFileExists(name)) return null;

            byte[] data = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(data))
                return texture;

            return null;
        }

        public string GetPath()
        {
            string path = Path.Combine(Application.persistentDataPath, CACHE_FOLDER);
            return path;
        }
        public string GetPath(string filename)
        {
            string path = Path.Combine(Application.persistentDataPath, CACHE_FOLDER, filename);
            return path;
        }

        public bool DoesImageExists(string url)
        {
            //Debug.Log("url: " + url);
            string md5 = Md5Sum(Encoding.ASCII.GetBytes(url));
            string path = Path.Combine(Application.persistentDataPath, CACHE_FOLDER, md5 + ".png");

            return DoesFileExists(path);
        }

        public string GetFileText(string filename)
        {
            string text = "";

            string path = Path.Combine(Application.persistentDataPath, CACHE_FOLDER, filename);
            if (File.Exists(path))
            {
                text = File.ReadAllText(path);
            }
            return text;
        }

        public bool DoesFileExists(string name)
        {
            string path = Path.Combine(Application.persistentDataPath, CACHE_FOLDER, name);
            return File.Exists(path);
        }

        public void Clean(string filename)
        {
            string path = Path.Combine(Application.persistentDataPath, CACHE_FOLDER, filename);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private void CreateDirectory()
        {
            string path = Path.Combine(Application.persistentDataPath, CACHE_FOLDER);

            if (Directory.Exists(path))
                return;
            else
                Directory.CreateDirectory(path);
        }

        public static string Md5Sum(byte[] bytesToEncrypt)
        {
            System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
            byte[] bytes = bytesToEncrypt;

            // encrypt bytes
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(bytes);

            // Convert the encrypted bytes back to a string (base 16)
            string hashString = "";

            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }

            return hashString.PadLeft(32, '0');
        }

    }
}