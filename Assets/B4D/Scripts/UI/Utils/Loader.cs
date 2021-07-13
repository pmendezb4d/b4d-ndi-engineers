using UnityEngine;
//using Doozy.Engine.UI;
using TMPro;

namespace B4D.UI.Utils
{
    public class Loader : MonoBehaviour
    {
        public static Loader Instance { get; private set; }

        //[SerializeField] UIView view = default;
        [SerializeField] TMP_Text messageLabel = default;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public void Show(string message = "")
        {
            SetMessage(message);
            //view.Show();
        }

        public void ShowInmediate(string message = "")
        {
            SetMessage(message);
            //view.Show(true);
        }

        //public void Hide() => view.Hide();
        //public void Hide(float delay) => view.Hide(delay);
        public void SetMessage(string s) => messageLabel.text = s;

    }
}