using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public Transform followTrans;
    public static Transform rigTrans { get; private set; }
    public static Transform pivotTrans { get; private set; }
    public static Transform cameraTrans { get; private set; }

    private void Awake()
    {
        rigTrans = transform;
        pivotTrans = transform.Find("Pivot");
        cameraTrans = Camera.main.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rigTrans.position = followTrans.position;
        rigTrans.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X"), 0);
        pivotTrans.rotation *= Quaternion.Euler(-Input.GetAxis("Mouse Y"), 0, 0);
    }
}
