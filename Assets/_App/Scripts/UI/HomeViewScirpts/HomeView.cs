using App.Data;
using B4D.DataManagement;
using B4D.UI.Utils;
//using Doozy.Engine.UI;
using SimpleJSON;
using System;
using UnityEngine;
using UnityEngine.UI;
using DanielLochner.Assets.SimpleScrollSnap;
using System.Collections.Generic;

namespace App.UI.Home
{
    public class HomeView : MonoBehaviour
    {
        [SerializeField] Transform contentParent = default;
        [SerializeField] GameObject bannerPrefab = default;
        private const string ENDPOINT = "https://beprime-noc.s3.us-west-2.amazonaws.com/engineers.json";
        private int bannersToSpawn;
        private int currentBanners;
        public SimpleScrollSnap scrollRect;
        public Dictionary<string, string> certificates = new Dictionary<string, string>();

        public static HomeView Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private void Start()
        {
            DownloaderManager.Instance.GetText(ENDPOINT, CreateAllBanners, null);
            scrollRect.enabled = false;
        }

        private void CreateAllBanners(string json)
        {
            JSONNode node = JSON.Parse(json);

            JSONArray arrayCertificates = node["certificates"].AsArray;
            for (int i = 0; i < arrayCertificates.Count; i++)
            {
                certificates.Add(arrayCertificates[i]["code"], arrayCertificates[i]["imageURL"]);
            }

            DestroyAllChildren<EngineerBanner>(contentParent); //Clean
            JSONArray array = node["engineers"].AsArray;
            Debug.Log(array.Count);
            bannersToSpawn = array.Count;
            for (int i = 0; i < array.Count; i++)
            {
                CreateBanner(array[i], i);
            }

            //Loader.Instance.Hide();
        }

        private void CreateBanner(JSONNode json, int index)
        {
            GameObject bannerObject = Instantiate(bannerPrefab, contentParent);
            EngineerBanner banner = bannerObject.GetComponent<EngineerBanner>();
            banner.SetEngineer(Engineer.CreateEngineerFromJson(json), index, scrollRect);
            currentBanners++;
            if (currentBanners >= bannersToSpawn)
            {
                scrollRect.enabled = true;
            }
        }

        private void DestroyAllChildren<T>(Transform contentParent) where T : Component
        {
            var children = contentParent.GetComponentsInChildren<T>();
            for (int i = 0; i < children.Length; i++)
            {
                Destroy(children[i].gameObject);
            }
        }
    }
}