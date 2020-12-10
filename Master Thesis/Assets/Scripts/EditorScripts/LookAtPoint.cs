using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteInEditMode]

public class LookAtPoint : MonoBehaviour
{
    public Vector3 lookAtPoint = new Vector3(0,1.6f,0);

    private void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {
        bool containsThisGameObject = false;
        for (int i = 0; i < Selection.transforms.Length; ++i)
        {
            if (Selection.transforms[i].gameObject == this.gameObject) containsThisGameObject = true;
        }

       if (!containsThisGameObject)
       {
           float zRotation = transform.localEulerAngles.z;
           transform.LookAt(lookAtPoint);
           transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, zRotation);
       }
    }
}
