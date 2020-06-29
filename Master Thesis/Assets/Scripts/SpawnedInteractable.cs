using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedInteractable : MonoBehaviour
{
    public float remainingTime;
    public float timeToHitObjects;
    public int roundGenerated;
    private bool pointsRewarded = false;
    public Color colorAfterHit;
    private MeshRenderer meshRenderer;
    private Rigidbody rigidBody;
    private Vector3 movedOffset;
    private bool isAnimating = true;
    private float timeToAnimate = 0.0f;
    private int id;
    private Transform hmd_transform;


    public GameObject pieceToSpawn;
    private float cubeSize = 0.2f;
    private int cubesInRow = 5;
    private float cubesPivotDistance;
    private Vector3 cubesPivot;
    public float explosionForce = 125f;
    public float explosionRadius = 2f;
    public float explosionUpward = 0.8f;
    private List<GameObject> spawnedPieces;

    void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        rigidBody = gameObject.GetComponent<Rigidbody>();

        //calculate pivot distance
        cubesPivotDistance = cubeSize * cubesInRow / 2;
        //use this value to create pivot vector)
        cubesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);
        spawnedPieces = new List<GameObject>();
    }

    public void setMovedOffset(Vector3 offset)
    {
        movedOffset = offset;
        if (movedOffset.x == 0 && movedOffset.z == 0) isAnimating = false;
    }

    public void setHmd_transform(Transform t)
    {
        hmd_transform = t;
    }

    public bool isMoving()
    {
        return rigidBody.velocity.magnitude > 0;
    }

    public void setId(int id)
    {
        this.id = id;
    }

    public int getId()
    {
        return id;
    }

    public void setTimeToHitObjects(float timeToHitGameObjects)
    {
        timeToHitObjects = remainingTime = timeToHitGameObjects;
    }

    public void setPointsRewarded()
    {
        pointsRewarded = true;
        changeColor();
    }

    private void changeColor()
    {
        meshRenderer.material.color = colorAfterHit;
    }

    public void ExplodeIntoPieces(Vector3 positionVector)
    {
        //make object disappear
        gameObject.SetActive(false);

        //loop 3 times to create 5x5x5 pieces in x,y,z coordinates
        for (int x = 0; x < cubesInRow; x++)
        {
            for (int y = 0; y < cubesInRow; y++)
            {
                for (int z = 0; z < cubesInRow; z++)
                {
                    createPiece(x, y, z);
                }
            }
        }

        foreach (GameObject piece in spawnedPieces)
        {
            Rigidbody rb = piece.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpward);
                //Vector3 direction = rb.transform.position - positionVector;
                //rb.AddForceAtPosition(direction * 50.0f, piece.transform.position);
            }
            Object.Destroy(piece, 7.0f);
        }

    }

    void createPiece(int x, int y, int z)
    {

        //create piece
        GameObject piece = Instantiate(pieceToSpawn);
        

        //set piece position and scale
        piece.transform.position = transform.position + new Vector3(cubeSize * x, cubeSize * y, cubeSize * z) - cubesPivot;
        piece.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

        spawnedPieces.Add(piece);
    }


    public void addForceToRigidBody(Vector3 positionVector)
    {
        Vector3 direction = rigidBody.transform.position - positionVector;
        rigidBody.AddForceAtPosition(direction * 200.0f, transform.position);
        rigidBody.useGravity = true;
    }

    public bool getPointsRewarded()
    {
        return pointsRewarded;
    }

    public float getRemainingTimeInPercent()
    {
        if (remainingTime > 0) return remainingTime / timeToHitObjects;
        else return 0.5f;
    }

    public float getNeededTimeToHitObject()
    {
        return timeToHitObjects - remainingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(isAnimating)
        {
            transform.position -= movedOffset * (Time.deltaTime / 2.0f);
            timeToAnimate += Time.deltaTime;
            if (timeToAnimate >= 2.0f) isAnimating = false;
        }
        else remainingTime -= Time.deltaTime;
        float zRotation = transform.localEulerAngles.z;
        transform.LookAt(hmd_transform);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, zRotation);
    }
}
