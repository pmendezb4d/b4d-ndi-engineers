using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCameraPortal : MonoBehaviour
{

    public void LookAtCamera()
    {
        Vector3 angle = transform.rotation.eulerAngles;
        transform.LookAt(Camera.main.transform);
        angle.y = transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(angle);

    }

}
