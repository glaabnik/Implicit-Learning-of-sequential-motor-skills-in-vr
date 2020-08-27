using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BlockSequence : MonoBehaviour
{
    public abstract bool hasNextSphereCoordinates();
    public abstract SphereCoordinates nextSphereCoordinates();

    public virtual void Start()
    {
        
    }

    public virtual void Update()
    {
        
    }

}
