using App.Data;
using B4D.UI.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.UI.Home
{
    public class EngineerBanner : MonoBehaviour
    {
        [SerializeField] DownloadableImage bannerImage = default;
        [SerializeField] TMP_Text nameLabel = default;
        private Animator cardAnimator;
        private RectTransform rt;

        private int index;
        public Engineer Engineer { get; private set; }

        private Button button;

        private void Awake()
        {
            //button = GetComponentInChildren<Button>();
            //button.onClick.AddListener(ShowEngineer);
            cardAnimator = GetComponent<Animator>();
            rt = GetComponent<RectTransform>();
        }

        public void SetEngineer(Engineer engineer, int i)
        {
            Engineer = engineer;
            index = i;
            bannerImage.SetUrl(Engineer.ImageUrl);
            nameLabel.text = Engineer.Name;
        }

        private void ShowEngineer()
        {
            //WS_Client.Instance.SendEngineerToWebSocket(index.ToString());
        }

        private void Update()
        {
            if (rt.localPosition.z < 250)
            {
                cardAnimator.SetBool("isOpen", true);
            }
            else
            {
                cardAnimator.SetBool("isOpen", false);
            }
        }
    }
}