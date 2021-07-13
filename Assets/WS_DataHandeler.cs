using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WS_DataHandeler : MonoBehaviour
{
    private WS_Client wsClientScript;

    private void OnEnable()
    {
        SetInitialReferences();
        wsClientScript.GetData += GetDataFromServer;
    }

    private void OnDisable()
    {
        wsClientScript.GetData -= GetDataFromServer;
    }

    void SetInitialReferences()
    {
        wsClientScript = GetComponent<WS_Client>();
    }

    void GetDataFromServer(string data)
    {
        string[] splitArray = data.Split(char.Parse(":"));
        switch (splitArray[0])
        {
            case "btn":
                Debug.Log("Button: " + splitArray[1]);
                break;
            case "axis":
                Debug.Log("Axis: " + splitArray[1]);
                break;
        }
    }
}
