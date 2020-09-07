using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]

public class LookAtPoint : MonoBehaviour
{
    public Vector3 lookAtPoint = new Vector3(0,1.6f,0);

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, 1.6f, transform.position.z);
    }

    // Update is called once per frame
    public void Update()
    {
        transform.LookAt(lookAtPoint);
    }
}
