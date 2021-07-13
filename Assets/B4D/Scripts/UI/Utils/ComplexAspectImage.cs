using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace B4D.UI.Utils
{
    [RequireComponent(typeof(Image), typeof(AspectRatioFitter))]
    public class ComplexAspectImage : MonoBehaviour
    {
        protected Image image;
        protected RectTransform rectTransform;

        private void Awake()
        {
            image = GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            StartCoroutine(CalculateNewSize());
        }

        private IEnumerator CalculateNewSize()
        {
            Vector2 imageSize = new Vector2(image.sprite.rect.width, image.sprite.rect.height);
            yield return null;

            RectTransform parent = transform.parent.GetComponent<RectTransform>();
            float targetHeight = parent.rect.height;
            float targetWidth = parent.rect.width;

            Vector2 sizeDelta = rectTransform.sizeDelta;

            bool isWidthBigger = imageSize.x > imageSize.y;
            AspectRatioFitter aspectRatioFitter = GetComponent<AspectRatioFitter>();
            float ratio = imageSize.x / imageSize.y;

            if (isWidthBigger)
            {
                aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
                aspectRatioFitter.aspectRatio = ratio;

                sizeDelta.y = targetHeight;
                rectTransform.sizeDelta = sizeDelta;
            }
            else
            {
                aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
                aspectRatioFitter.aspectRatio = ratio;

                sizeDelta.x = targetWidth;
                rectTransform.sizeDelta = sizeDelta;
            }


            yield return null;
            //Check again  but other side

            if (isWidthBigger)
            {
                sizeDelta = rectTransform.sizeDelta;
                if (sizeDelta.x < targetWidth) //Adjust widht
                {
                    aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
                    aspectRatioFitter.aspectRatio = ratio;
                    sizeDelta.x = targetWidth;
                    rectTransform.sizeDelta = sizeDelta;
                }
            }
            else
            {
                sizeDelta = rectTransform.sizeDelta;
                if (sizeDelta.y < targetHeight) //Adjust height
                {
                    aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
                    aspectRatioFitter.aspectRatio = ratio;
                    sizeDelta.y = targetHeight;
                    rectTransform.sizeDelta = sizeDelta;
                }
            }
        }

    }
}