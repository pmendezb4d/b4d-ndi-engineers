using System.Collections;
using UnityEngine;
//using DG.Tweening;

namespace B4D.UI.Utils
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ContentLoader : MonoBehaviour
    {
        [SerializeField] bool hideOnAwake = false;

        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();

            if (hideOnAwake) Hide();
        }

        public void Show(bool inmediate = false)
        {
            canvasGroup.blocksRaycasts = true;
            if (inmediate)
            {
                canvasGroup.alpha = 1f;
            }
            else
            {
                canvasGroup.alpha = 0f;
                //canvasGroup.DOFade(1.0f, 0.25f);
            }
        }

        public void Hide(float delay) => StartCoroutine(HideDelayCoroutine(delay));

        private IEnumerator HideDelayCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            Hide();
        }

        public void Hide()
        {
            //canvasGroup.DOFade(0f, 0.25f);
            canvasGroup.blocksRaycasts = false;
        }

    }
}