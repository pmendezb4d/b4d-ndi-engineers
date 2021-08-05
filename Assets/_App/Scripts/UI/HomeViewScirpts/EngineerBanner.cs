using App.Data;
using B4D.UI.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DanielLochner.Assets.SimpleScrollSnap;

namespace App.UI.Home
{
    public class EngineerBanner : MonoBehaviour
    {
        [SerializeField] DownloadableImage bannerImage = default;
        [SerializeField] TMP_Text nameLabel = default;
        [SerializeField] TMP_Text codeLabel = default;

        [SerializeField] Image profileBackgound = default;
        public Sprite fortineBackgroundSprite;

        public Animator cardAnimator;
        private RectTransform rt;

        private int index;
        public Engineer Engineer { get; private set; }

        private Button button;

        public GameObject certificatePrefab;
        [SerializeField] Transform ciscoHolder = default;
        [SerializeField] Transform fortinetHolder = default;
        private SimpleScrollSnap scroller;

        private void Awake()
        {
            //button = GetComponentInChildren<Button>();
            //button.onClick.AddListener(ShowEngineer);
            //cardAnimator = GetComponent<Animator>();
            //cardAnimator = GetComponent<Animator>();
            rt = GetComponent<RectTransform>();
        }

        public void SetEngineer(Engineer engineer, int i, SimpleScrollSnap scroll)
        {
            scroller = scroll;
            Engineer = engineer;
            index = i;
            bannerImage.SetUrl(Engineer.ImageUrl);
            nameLabel.text = Engineer.Name;
            codeLabel.text = Engineer.Code;
            for (int j = 0; j < Engineer.Cisco.Length; j++)
            {
                string key = Engineer.Cisco[j];
                if (HomeView.Instance.certificates.ContainsKey(key))
                {
                    string url = HomeView.Instance.certificates[key];
                    if (url != "")
                    {
                        GameObject certificate = Instantiate(certificatePrefab, ciscoHolder);
                        DownloadableImage dwlImage = certificate.GetComponent<DownloadableImage>();
                        dwlImage.SetUrl(url);
                    }
                }
            }
            for (int j = 0; j < Engineer.Fortinet.Length; j++)
            {
                string key = Engineer.Fortinet[j];
                if (HomeView.Instance.certificates.ContainsKey(key))
                {
                    string url = HomeView.Instance.certificates[key];
                    if (url != "")
                    {
                        GameObject certificate = Instantiate(certificatePrefab, fortinetHolder);
                        DownloadableImage dwlImage = certificate.GetComponent<DownloadableImage>();
                        dwlImage.SetUrl(url);
                    }
                }
            }

            if(Engineer.Cisco.Length < Engineer.Fortinet.Length)
            {
                profileBackgound.sprite = fortineBackgroundSprite;
            }
        }

        private void ShowEngineer()
        {
            //WS_Client.Instance.SendEngineerToWebSocket(index.ToString());
        }

        private void Update()
        {
            if (scroller.CurrentPanel == index)
            {
                cardAnimator.SetBool("isOpen", true);
            }
            else
            {
                cardAnimator.SetBool("isOpen", false);
            }
            //if (rt.localPosition.z < 250)
            //{
            //    cardAnimator.SetBool("isOpen", true);
            //}
            //else
            //{
            //    cardAnimator.SetBool("isOpen", false);
            //}
        }
    }
}