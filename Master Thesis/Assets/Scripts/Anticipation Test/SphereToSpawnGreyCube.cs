using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;


public class SphereToSpawnGreyCube : VRTK_InteractableObject
{
    public VRTK_Pointer pointer_left, pointer_right;
    public GameObject cubeGrey;
    public SpawnCubes spawnCubes;
    public RectTransform menuToSpawnTransform;
    [SerializeField]
    public Canvas notificationToSpawn;
    [SerializeField]
    public Text notification;
    public string testString;
    private bool canSpawnCube = false;
    private bool cubeSpawnedOne = false;
    private bool cubeSpawnedTwo = false;
    private bool pointerLeftUsedForSpawning = false;
    private bool pointerRightUsedForSpawning = false;
    private int zRotationLeft, zRotationRight;
    private GameObject cubeNeutralLeft, cubeNeutralRight;

    void Start()
    {
        notificationToSpawn.enabled = false;
    }

    public void colorRightSpawnedCube(GameObject toCompare, float distanceAllowed)
    {
        float dist = (cubeNeutralRight.transform.position - toCompare.transform.position).magnitude;
        MeshRenderer mr = cubeNeutralRight.GetComponent<MeshRenderer>();
        if (dist <= distanceAllowed)
        {
            mr.material.color = Color.green;
        }
        else
        {
            mr.material.color = new Color(120 /255.0f, 81 / 255.0f, 169 / 255.0f);
        }
    }

    public void colorLeftSpawnedCube(GameObject toCompare, float distanceAllowed)
    {
        float dist = (cubeNeutralLeft.transform.position - toCompare.transform.position).magnitude;
        MeshRenderer mr = cubeNeutralLeft.GetComponent<MeshRenderer>();
        if (dist <= distanceAllowed)
        {
            mr.material.color = Color.green;
        }
        else
        {
            mr.material.color = new Color(120 / 255.0f, 81 / 255.0f, 169 / 255.0f);
        }
    }

    public void activateSpawningAbility()
    {
        Vector3 local = menuToSpawnTransform.localPosition;
        //menuToSpawnTransform.localPosition = local - new Vector3(0, 0, 1.5f);
        notification.text = "Focus the activated pointer at ui window and press trigger button in order to spawn the right cube";
        notificationToSpawn.enabled = true;
        activatePointer(false);
        canSpawnCube = true;
    }

    public void deactivateSpawningAbility()
    {
        Vector3 local = menuToSpawnTransform.localPosition;
        canSpawnCube = false;
        //menuToSpawnTransform.localPosition = local + new Vector3(0, 0, 1.5f);
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
        pointerLeftUsedForSpawning = false;
        pointerRightUsedForSpawning = false;
    }

    protected override void Update()
    {
        base.Update();
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
        if (canSpawnCube && cubeSpawnedOne && !cubeSpawnedTwo && activePointerNotUsedForSpawning())
        {
            assignActivePointerUsedForSpawning();
            Vector3 res = spawnCubes.cartesianToSphereCoordinate(getRelevantSpawnPosition());
            Vector3 adjustedPosition = spawnCubes.sphereToCartesianCoordinate(res[1], res[0], res[2]);
            spawnCubes.spawnGameObject(true, adjustedPosition, new Vector3(0, 0, zRotationLeft), new Vector3(0.4f, 0.4f, 0.4f), cubeGrey);
            cubeNeutralLeft = spawnCubes.getLastLeftHandTarget().gameObject;
            cubeSpawnedTwo = true;
            canSpawnCube = false;
            deactivatePointer(true);
            notificationToSpawn.enabled = false;
        }
        if (canSpawnCube && !cubeSpawnedOne && activePointerNotUsedForSpawning())
        {
            assignActivePointerUsedForSpawning();
            Vector3 res = spawnCubes.cartesianToSphereCoordinate(getRelevantSpawnPosition());
            Vector3 adjustedPosition = spawnCubes.sphereToCartesianCoordinate(res[1], res[0], res[2]);
            spawnCubes.spawnGameObject(false, adjustedPosition, new Vector3(0, 0, zRotationRight), new Vector3(0.4f, 0.4f, 0.4f), cubeGrey);
            cubeNeutralRight = spawnCubes.getLastRightHandTarget().gameObject;
            cubeSpawnedOne = true;
            deactivatePointer(false);
            activatePointer(true);
            notification.text = "Focus the activated pointer at ui window and press trigger button in order to spawn the right cube";
        }
    }

    private bool activePointerNotUsedForSpawning()
    {
        if (pointer_left.IsPointerActive() && pointer_right.IsPointerActive()) return !pointerLeftUsedForSpawning && !pointerRightUsedForSpawning;
        if (pointer_left.IsPointerActive()) return !pointerLeftUsedForSpawning;
        if (pointer_right.IsPointerActive()) return !pointerRightUsedForSpawning;
        return false;
    }

    private void assignActivePointerUsedForSpawning()
    {
        VRTK_StraightPointerRenderer spl = pointer_left.pointerRenderer as VRTK_StraightPointerRenderer;
        VRTK_StraightPointerRenderer spr = pointer_right.pointerRenderer as VRTK_StraightPointerRenderer;
        if (pointer_left.IsPointerActive() && spl.positionRayHit != Vector3.zero) pointerLeftUsedForSpawning = true;
        if (pointer_right.IsPointerActive() && spr.positionRayHit != Vector3.zero) pointerRightUsedForSpawning = true;
    } 

    public override void StopUsing(VRTK_InteractUse previousUsingObject = null, bool resetUsingObjectState = true)
    {
        base.StopUsing(previousUsingObject, resetUsingObjectState);
    }

    
}
