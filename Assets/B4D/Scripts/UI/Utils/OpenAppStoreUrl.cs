using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenAppStoreUrl : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(OpenUrl);
    }

    private void OpenUrl()
    {
#if UNITY_ANDROID
        Application.OpenURL("market://details?id="+Application.identifier);
#elif UNITY_IPHONE
        Application.OpenURL("itms-apps://itunes.apple.com/app/"+Application.identifier);
#endif
    }

}
