using TMPro;
using UnityEngine;

namespace B4D.UI.Utils
{
    [RequireComponent(typeof(TMP_Text))]
    public class TMPVersion : MonoBehaviour
    {
        private TMP_Text versionLabel;

        private void Awake()
        {
            versionLabel = GetComponent<TMP_Text>();
            versionLabel.text = "v" + Application.version;
        }

    }
}