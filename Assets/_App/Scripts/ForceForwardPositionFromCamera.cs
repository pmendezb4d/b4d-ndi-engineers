using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceForwardPositionFromCamera : MonoBehaviour
{
    [SerializeField] Transform myParent = default;
    [SerializeField] bool parentToCamera = true;
    [SerializeField] float distance = 2f;

    public void SetPosition()
    {
        Vector3 position =  Camera.main.transform.forward* distance;

        transform.position = position;


        if(parentToCamera)
            myParent.SetParent(myParent);
        

    }
}
