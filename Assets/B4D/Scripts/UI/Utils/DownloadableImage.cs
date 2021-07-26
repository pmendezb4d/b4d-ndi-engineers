using B4D.DataManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace B4D.UI.Utils
{
    [RequireComponent(typeof(RawImage), typeof(AspectRatioFitter))]
    public class DownloadableImage : MonoBehaviour
    {
        [SerializeField] bool preserveAspectRatio = true;
        [SerializeField] ContentLoader loader = default;

        protected RawImage image;
        protected RectTransform rectTransform;

        private void Awake()
        {
            image = GetComponent<RawImage>();
            rectTransform = GetComponent<RectTransform>();
        }

        public void SetUrl(string url)
        {
            Debug.Log(url);
            //loader.Show(true);
            DownloaderManager.Instance.GetAssetTexture2D(url, SetImage, null, true);
        }

        private void SetImage(Texture2D texture)
        {
            image.texture = texture;
            if (preserveAspectRatio) StartCoroutine(CalculateNewSize());
            //loader.Hide(0.5f);
        }

        protected virtual IEnumerator CalculateNewSize()
        {
            yield return null;

            float ratio = (float)image.texture.width / (float)image.texture.height;

            AspectRatioFitter aspectRatioFitter = GetComponent<AspectRatioFitter>();
            aspectRatioFitter.aspectRatio = ratio;
        }
    }
}