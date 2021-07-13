using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace B4D.UI.Utils
{
    [RequireComponent(typeof(RawImage), typeof(AspectRatioFitter))]
    public class PreserveAspectRatio : MonoBehaviour
    {
        [SerializeField] bool updateOnAwake = false;

        private RawImage image;

        private void Awake()
        {
            image = GetComponent<RawImage>();

            if (updateOnAwake)
                UpdateAspectRatio();
        }

        public void UpdateAspectRatio()
        {
            float ratio = (float)image.texture.width / (float)image.texture.height;

            AspectRatioFitter aspectRatioFitter = GetComponent<AspectRatioFitter>();
            aspectRatioFitter.aspectRatio = ratio;
        }

    }
}