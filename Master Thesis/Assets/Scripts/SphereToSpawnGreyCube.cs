using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SphereToSpawnGreyCube : VRTK_InteractableObject
{
    public VRTK_Pointer pointer_one, pointer_two;
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

    public void activatePointer()
    {
        pointer_one.Toggle(true);
    }

    public void deactivatePointer()
    {
        pointer_one.Toggle(false);
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
        Debug.Log("IsActive One: " + pointer_one.IsPointerActive());
        Debug.Log("Selection Button pressed: " + pointer_one.IsSelectionButtonPressed());

        Debug.Log("IsActive One2: " + pointer_two.IsPointerActive());
        Debug.Log("Selection Button pressed2: " + pointer_two.IsSelectionButtonPressed());
        if (oneSelectionButtonPressed() && !cubeSpawnedOne)
        {
            spawnCubes.spawnGameObject(false, getRelevantSpawnPosition(), new Vector3(0, 0, zRotationRight), new Vector3(0.4f, 0.4f, 0.4f), cubeGrey);
            cubeSpawnedOne = true;
        }
    }

    private bool oneSelectionButtonPressed()
    {
        return (pointer_one.IsSelectionButtonPressed() && pointer_one.IsPointerActive()) || (pointer_two.IsSelectionButtonPressed() && pointer_two.IsPointerActive());
    }

    private Vector3 getRelevantSpawnPosition()
    {
        if(pointer_one.IsPointerActive())
        {
            VRTK_StraightPointerRenderer spr = pointer_one.pointerRenderer as VRTK_StraightPointerRenderer;
            return spr.positionRayHit;
        }
        if(pointer_two.IsPointerActive())
        {
            VRTK_StraightPointerRenderer spr = pointer_two.pointerRenderer as VRTK_StraightPointerRenderer;
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
        }
        if (canSpawnCube && !cubeSpawnedOne)
        {
            spawnCubes.spawnGameObject(false, getRelevantSpawnPosition(), new Vector3(0, 0, zRotationRight), new Vector3(0.4f, 0.4f, 0.4f), cubeGrey);
            cubeSpawnedOne = true;
           
        }
    }

    public override void StopUsing(VRTK_InteractUse previousUsingObject = null, bool resetUsingObjectState = true)
    {
        base.StopUsing(previousUsingObject, resetUsingObjectState);
    }

    
}
