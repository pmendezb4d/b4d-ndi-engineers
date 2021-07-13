using UnityEngine;
using UnityEngine.UI;

namespace B4D.UI.Utils
{
    [RequireComponent(typeof(Button))]
    public class OpenExternalURLOnClick : MonoBehaviour
    {
        [SerializeField] string defaultUrl = "www.google.com";

        private Button button;
        private string url;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void Start()
        {
            url = defaultUrl;
            button.onClick.AddListener(OpenUrl);
        }

        private void OpenUrl() => Application.OpenURL(url);

        public void SetUrl(string url) => this.url = url;

    }
}