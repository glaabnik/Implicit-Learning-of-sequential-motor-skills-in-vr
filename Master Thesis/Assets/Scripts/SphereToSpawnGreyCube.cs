using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SphereToSpawnGreyCube : VRTK_InteractableObject
{
    public VRTK_Pointer pointer_left, pointer_right;
    public GameObject cubeGrey;
    public SpawnCubes spawnCubes;
    private bool canSpawnCube = false;
    private bool cubeSpawnedOne = false;
    private bool cubeSpawnedTwo = false;
    private int zRotationLeft, zRotationRight;

    void Start()
    {
        
    }

    public void activateSpawningAbility()
    {
        activatePointer(false);
        canSpawnCube = true;
    }

    public void setZRotationLeft(int n)
    {
        zRotationLeft = n;
    }

    public void setZRotationRight(int n)
    {
        zRotationRight = n;
    }

    public void activatePointer(bool leftHand)
    {
        if (leftHand) pointer_left.Toggle(true);
        else pointer_right.Toggle(true);
    }

    public void deactivatePointer(bool leftHand)
    {
        if (leftHand) pointer_left.Toggle(false);
        else pointer_right.Toggle(false);
    }

    public bool bothCubesSpawned()
    {
        return cubeSpawnedOne && cubeSpawnedTwo;
    }

    public void reset()
    {
        cubeSpawnedOne = false;
        cubeSpawnedTwo = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision occured!");
        Debug.Log("collision contact point: " + collision.GetContact(0).point);
        Debug.Log("IsActive One: " + pointer_left.IsPointerActive());
        Debug.Log("Selection Button pressed: " + pointer_left.IsSelectionButtonPressed());

        Debug.Log("IsActive One2: " + pointer_right.IsPointerActive());
        Debug.Log("Selection Button pressed2: " + pointer_right.IsSelectionButtonPressed());
        /*if (oneSelectionButtonPressed() && !cubeSpawnedOne)
        {
            spawnCubes.spawnGameObject(false, getRelevantSpawnPosition(), new Vector3(0, 0, zRotationRight), new Vector3(0.4f, 0.4f, 0.4f), cubeGrey);
            cubeSpawnedOne = true;
        }*/
    }

    private bool oneSelectionButtonPressed()
    {
        return (pointer_left.IsSelectionButtonPressed() && pointer_left.IsPointerActive()) || (pointer_right.IsSelectionButtonPressed() && pointer_right.IsPointerActive());
    }

    private Vector3 getRelevantSpawnPosition()
    {
        if(pointer_left.IsPointerActive())
        {
            VRTK_StraightPointerRenderer spr = pointer_left.pointerRenderer as VRTK_StraightPointerRenderer;
            return spr.positionRayHit;
        }
        if(pointer_right.IsPointerActive())
        {
            VRTK_StraightPointerRenderer spr = pointer_right.pointerRenderer as VRTK_StraightPointerRenderer;
            return spr.positionRayHit;
        }
        return new Vector3(0, 0, 0);
    }

    public override void StartUsing(VRTK_InteractUse currentUsingObject = null)
    {
        Debug.Log("Start Using called!!!");

        base.StartUsing(currentUsingObject);
        if (canSpawnCube && cubeSpawnedOne && !cubeSpawnedTwo)
        {
            spawnCubes.spawnGameObject(true, getRelevantSpawnPosition(), new Vector3(0, 0, zRotationLeft), new Vector3(0.4f, 0.4f, 0.4f), cubeGrey);
            cubeSpawnedTwo = true;
            canSpawnCube = false;
            deactivatePointer(true);
        }
        if (canSpawnCube && !cubeSpawnedOne)
        {
            spawnCubes.spawnGameObject(false, getRelevantSpawnPosition(), new Vector3(0, 0, zRotationRight), new Vector3(0.4f, 0.4f, 0.4f), cubeGrey);
            cubeSpawnedOne = true;
            deactivatePointer(false);
            activatePointer(true);
        }
    }

    public override void StopUsing(VRTK_InteractUse previousUsingObject = null, bool resetUsingObjectState = true)
    {
        base.StopUsing(previousUsingObject, resetUsingObjectState);
    }

    
}
