using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCubes : MonoBehaviour
{

    public Transform hmd_transform;
    public HitInteractable leftHand;
    public HitInteractable rightHand;
    public GameObject rightHandGameObject;
    public GameObject leftHandGameObject;
    public float sphereRadius;
    public int timeToHitGameObjects;
    public int timeToWaitBetween;
    public float scaleSpawnedGameObjects = 1.0f;
    public bool randomizeElevation = false;

    // Debug Variables
    public Vector3 forwardVectorTest;

    private float timeCounter;
    private float instantiateTimeCounter;
    private bool instantiated;
    private SpawnedInteractable lastLeftHandTarget;
    private SpawnedInteractable lastRightHandTarget;
    private int objectsSpawned = 0;
    private int roundGenerated = 0;
    void Start()
    {
        timeCounter = 0;
        instantiateTimeCounter = timeToWaitBetween + 1;
        forwardVectorTest = hmd_transform.forward;
        spawnCubesForBothHandsRandomizedInSightOfCameraDirection();
        instantiated = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(lastLeftHandTarget.getPointsRewarded() && lastRightHandTarget.getPointsRewarded() && instantiated)
        {
            timeCounter = timeToHitGameObjects;
        }
        forwardVectorTest = hmd_transform.forward;
        timeCounter += Time.deltaTime;
        instantiateTimeCounter += Time.deltaTime;
        if(instantiateTimeCounter >= timeToWaitBetween && !instantiated)
        {
            spawnCubesForBothHandsRandomizedInSightOfCameraDirection();
            instantiated = true;
        }
        if(timeCounter >= timeToHitGameObjects)
        {
            float destroyTimeLeft = 0;
            float destroyTimeRight = 0;
            if (lastLeftHandTarget.getPointsRewarded()) destroyTimeLeft = 5.0f;
            if (lastRightHandTarget.getPointsRewarded()) destroyTimeRight = 5.0f;

            Object.Destroy(lastLeftHandTarget.gameObject, destroyTimeLeft);
            Object.Destroy(lastRightHandTarget.gameObject, destroyTimeRight);
            timeCounter = -timeToWaitBetween;
            instantiateTimeCounter = 0;
            instantiated = false;
        }
        bool inputS = Input.GetKey(KeyCode.L);
        if(inputS)
        {
            Debug.Log("L pressed");
            SerializeData.SerializeSportGameData("sportGameData", leftHand.listTimeNeededToHitObject, leftHand.listPrecisionWithThatObjectWasHit, leftHand.listEarnedPoints,
                rightHand.listTimeNeededToHitObject, rightHand.listPrecisionWithThatObjectWasHit, rightHand.listEarnedPoints, objectsSpawned, leftHand.countObjectsHit, rightHand.countObjectsHit);
        }
    }

    private void spawnCubesForBothHandsRandomizedInSightOfCameraDirection()
    {
        int phiOffset = calculatePhiOffset();
        Debug.Log(phiOffset);
        int degLeftPhi = Random.Range(15, 40);
        int degRightPhi = Random.Range(-40, -15);
        degLeftPhi += phiOffset;
        degRightPhi += phiOffset;
        int degLeftTheta = 90;
        int degRightTheta = 90;
        if( randomizeElevation)
        {
            degLeftTheta = 70 + Random.Range(0, 40);
            degRightTheta = 70 + Random.Range(0, 40);
        }
        ++roundGenerated;
        spawnGameObject(true, sphereToCartesianCoordinate(degLeftTheta, degLeftPhi), leftHandGameObject);
        spawnGameObject(false, sphereToCartesianCoordinate(degRightTheta, degRightPhi), rightHandGameObject);
        objectsSpawned += 2;
    }

    private int calculatePhiOffset() // Interpolation of Phioffset for current Camera Direction
    {
        Vector3 forward = hmd_transform.forward;
        if (forward.x >= 0.99) return 0;
        if (forward.z >= 0.99) return 90;
        if (forward.x <= -0.99) return 180;
        if (forward.z <= -0.99) return 270;
        if (forward.x > 0 && forward.z > 0)
        {
            if(forward.z < 0.7) return (int) (forward.z * 45/ 0.7);
            else                return (int) (45 + ((forward.z - 0.7) * 45 / 0.3));
        }
        if (forward.x < 0 && forward.z > 0)
        {
            if(forward.x > -0.7) return (int) (90 + (forward.x * -45 / 0.7));
            else                 return (int) (135 + ((forward.x + 0.7) * -45 / 0.3));
        }
        if (forward.x < 0 && forward.z < 0)
        {
            if(forward.z > -0.7) return (int)(180 + (forward.z * -45 / 0.7));
            else                 return (int)(225 + ((forward.z + 0.7) * -45 / 0.3));
        }
        if (forward.x > 0 && forward.z < 0)
        {
            if(forward.x < 0.7) return (int)(270 + (forward.x * 45 / 0.7));
            else                return (int)(305 + ((forward.x - 0.7) * 45 / 0.3));
        }
        else return 0;
    }

    private Vector3 sphereToCartesianCoordinate(float theta, float phi)
    {
        float x = sphereRadius * Mathf.Sin(DegToRadians(theta)) * Mathf.Cos(DegToRadians(phi)) + hmd_transform.position.x;
        float z = sphereRadius * Mathf.Sin(DegToRadians(theta)) * Mathf.Sin(DegToRadians(phi)) + hmd_transform.position.z;
        float y = sphereRadius * Mathf.Cos(DegToRadians(theta)) + hmd_transform.position.y;
        return new Vector3(x, y, z);
    }

    private float DegToRadians(float degree)
    {
        return degree * Mathf.PI / 180;
    }

    private void spawnGameObject(bool leftHand, Vector3 position, GameObject objectToSpawn)
    {
        GameObject gameObject = Instantiate(objectToSpawn, position, Quaternion.identity);
        gameObject.transform.localScale = new Vector3(scaleSpawnedGameObjects, scaleSpawnedGameObjects, scaleSpawnedGameObjects);
        SpawnedInteractable si = gameObject.GetComponent<SpawnedInteractable>();
        si.setTimeToHitObjects(timeToHitGameObjects);
        si.roundGenerated = roundGenerated;
        if (leftHand) lastLeftHandTarget = si;
        else lastRightHandTarget = si;
    }
}
