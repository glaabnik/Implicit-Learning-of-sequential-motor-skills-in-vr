using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSpawnedInteractable : MonoBehaviour
{
    public float remainingTime;
    public float timeToHitObjects;
    public int roundGenerated;
    private bool pointsRewarded = false;
    public Color colorAfterHit;
    public Color colorAfterHit2;
    public Color colorAfterHit3;

    public Color colorAfterHitGroup;
    public Color colorAfterHitGroup2;
    public Color colorAfterHitGroup3;
    public Color colorAfterHitGroup4;
    public Color colorAfterHitGroup5;
    public Color originalColor;

    public int id;
    public bool uses5ColliderGroups = false;
    private MeshRenderer meshRenderer;
    private Rigidbody rigidBody;
    private Vector3 movedOffset;
    private bool isAnimating = true;
    private float timeToAnimate = 0.0f;
    private Transform hmd_transform;

    private bool startColliderHit = false;
    private bool middleColliderHit = false;
    private bool middleColliderHit2 = false;
    private bool middleColliderHit3 = false;
    private bool endColliderHit = false;
    private int actAccuracyStart = 0;
    private int actAccuracyMiddle = 0;
    private int actAccuracyMiddle2 = 0;
    private int actAccuracyMiddle3 = 0;
    private int actAccuracyEnd = 0;


    public GameObject pieceToSpawn;
    public GameObject pieceToSpawn2;
    public GameObject pieceToSpawn3;
    public GameObject pieceToSpawn4;
    private float cubeSize = 0.2f;
    private int cubesInRow = 5;
    private float cubesPivotDistance;
    private Vector3 cubesPivot;
    public float explosionForce = 125f;
    public float explosionRadius = 2f;
    public float explosionUpward = 0.8f;
    private List<GameObject> spawnedPieces;
    private bool startFadingOut = false;
    private float fadeSpeed = 1.0f;
    private SpriteRenderer spriteRenderer;
    private new Renderer renderer;
    private int countColliderGroupsHit = 0;
    private List<MeshRenderer> listToReset;
    void Start()
    {
        listToReset = new List<MeshRenderer>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        rigidBody = gameObject.GetComponent<Rigidbody>();

        //calculate pivot distance
        cubesPivotDistance = cubeSize * cubesInRow / 2;
        //use this value to create pivot vector)
        cubesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);
        spawnedPieces = new List<GameObject>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        renderer = this.GetComponent<Renderer>();
    }

    public void changeColorColliderGroup(Collider other)
    {
        Transform child_transform = other.transform.GetChild(0);
        if (child_transform == null) return;
        GameObject child = child_transform.gameObject;
        MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
        if(meshRenderer != null) listToReset.Add(meshRenderer);
        //int countColliderGroupsHit = countColliderGroupsHitGroups();
        Debug.Log("Collider Group Number: " + countColliderGroupsHit + " Collider Group Tag: " + other.gameObject.tag);
        if (countColliderGroupsHit == 1) meshRenderer.material.color = colorAfterHitGroup;
        if (countColliderGroupsHit == 2) meshRenderer.material.color = colorAfterHitGroup2;
        if (countColliderGroupsHit == 3) meshRenderer.material.color = colorAfterHitGroup3;
        if (countColliderGroupsHit == 4) meshRenderer.material.color = colorAfterHitGroup4;
        if (countColliderGroupsHit == 5) meshRenderer.material.color = colorAfterHitGroup5;

    }

    public void incrementColliderGroupsHit(Collider other)
    {
        countColliderGroupsHit++;
    }

    public void resetColorOfColliderGroupHits()
    {
        foreach(MeshRenderer renderer in listToReset)
        {
            renderer.material.color = originalColor;
        }
        listToReset.Clear();
    }

    /*private int countColliderGroupsHitGroups()
    {
        int result = 0;
        if (startColliderHit) result += 1;
        if (middleColliderHit) result += 1;
        if (middleColliderHit2) result += 1;
        if (middleColliderHit3) result += 1;
        if (endColliderHit) result += 1;
        return result;
    }*/

    public void resetColliderGroupsHit()
    {
        countColliderGroupsHit = 0;
        startColliderHit = false;
        middleColliderHit = false;
        middleColliderHit2 = false;
        middleColliderHit3 = false;
        endColliderHit = false;
        actAccuracyStart = 0;
        actAccuracyMiddle = 0;
        actAccuracyMiddle2 = 0;
        actAccuracyMiddle3 = 0;
        actAccuracyEnd = 0;
    }

    public void startColliderGroupHit(string colliderName)
    {
        if (!middleColliderHit && !endColliderHit)
        {
            startColliderHit = true;
            assignAccuracyFor(colliderName, ref actAccuracyStart);
        }   
    }

    public void middleColliderGroupHit(string colliderName)
    {
        if (startColliderHit && !endColliderHit)
        {
            middleColliderHit = true;
            assignAccuracyFor(colliderName, ref actAccuracyMiddle);
        }
    }

    public void middleColliderGroupHit2(string colliderName)
    {
        if (startColliderHit && middleColliderHit &&  !endColliderHit)
        {
            middleColliderHit2 = true;
            assignAccuracyFor(colliderName, ref actAccuracyMiddle2);
        }
    }

    public void middleColliderGroupHit3(string colliderName)
    {
        if (startColliderHit && middleColliderHit && middleColliderHit2 && !endColliderHit)
        {
            middleColliderHit3 = true;
            assignAccuracyFor(colliderName, ref actAccuracyMiddle3);
        }
    }

    public void endColliderGroupHit(string colliderName)
    {
        if (( startColliderHit && middleColliderHit && !uses5ColliderGroups) || (startColliderHit && middleColliderHit && middleColliderHit2 && middleColliderHit3) )
        {
            endColliderHit = true;
            assignAccuracyFor(colliderName, ref actAccuracyEnd);
        }
            
    }

    public bool wasHitInRightDirection()
    {
        if(uses5ColliderGroups) return startColliderHit && middleColliderHit && middleColliderHit2 && middleColliderHit3 && endColliderHit;
        else                    return startColliderHit && middleColliderHit && endColliderHit;
    }

    public int getAvgAccuracy()
    {
        if (uses5ColliderGroups) return (actAccuracyStart + actAccuracyMiddle + actAccuracyMiddle2 + actAccuracyMiddle3 + actAccuracyEnd) / 5;
        else                     return (actAccuracyStart + actAccuracyMiddle + actAccuracyEnd) / 3;
    }

    public float getAvgAccuracyPercent()
    {
        if (uses5ColliderGroups) return (actAccuracyStart + actAccuracyMiddle + actAccuracyMiddle2 + actAccuracyMiddle3 + actAccuracyEnd) / 5.0f * 0.1f;
        else                     return (actAccuracyStart + actAccuracyMiddle + actAccuracyEnd) / 3.0f * 0.1f;
    }

    private void assignAccuracyFor(string colliderName, ref int accAttribute)
    {
        assignConditionalAcc(colliderName, "One", ref accAttribute, 1);
        assignConditionalAcc(colliderName, "Two", ref accAttribute, 2);
        assignConditionalAcc(colliderName, "Three", ref accAttribute, 3);
        assignConditionalAcc(colliderName, "Four", ref accAttribute, 4);
        assignConditionalAcc(colliderName, "Five", ref accAttribute, 5);
        assignConditionalAcc(colliderName, "Six", ref accAttribute, 6);
        assignConditionalAcc(colliderName, "Seven", ref accAttribute, 7);
        assignConditionalAcc(colliderName, "Eight", ref accAttribute, 8);
        assignConditionalAcc(colliderName, "Nine", ref accAttribute, 9);
        assignConditionalAcc(colliderName, "Ten", ref accAttribute, 10);
    }

    private void assignConditionalAcc(string colliderName, string ending, ref int accuracy, int val)
    {
        if (colliderName.EndsWith(ending)) assignIfGreaterThanActAccuracy(ref accuracy, val);
    }

    private void assignIfGreaterThanActAccuracy(ref int acc, int newVal)
    {
        if (newVal > acc) acc = newVal;
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

    public void setPointsRewarded(int pointsGained)
    {
        changeColor(pointsGained);
    }

    private void changeColor(int pointsGained)
    {
        /*if (pointsGained > 35 && pointsGained <= 70) meshRenderer.material.color = new Color(colorAfterHit.r, colorAfterHit.g, colorAfterHit.b, 0.25f);
        else if (pointsGained > 70 && pointsGained <= 100) meshRenderer.material.color = new Color(colorAfterHit2.r, colorAfterHit2.g, colorAfterHit2.b, 0.25f);*/
    }

    public Color getColor(int pointsGained)
    {
        if(pointsGained <= 35) return meshRenderer.material.color;
        if (pointsGained > 35 && pointsGained <= 70) return colorAfterHit;
        else if (pointsGained > 70 && pointsGained <= 100) return colorAfterHit2;
        else return colorAfterHit3;
    }

    public Vector3 getCenter()
    {
        return renderer.bounds.center;
    }

    public Vector3[] getVertices()
    {
        return GetComponent<MeshFilter>().mesh.vertices;
    }

    public void ExplodeIntoPieces(Vector3 positionVector, int pointsRewarded)
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
                    createPiece(x, y, z, pointsRewarded);
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
            Object.Destroy(piece, 5.0f);
        }

    }

    void createPiece(int x, int y, int z, int pointsRewarded)
    {

        //create piece
        GameObject piece;
        if (pointsRewarded <= 35) piece = Instantiate(pieceToSpawn);
        else if (pointsRewarded <= 70) piece = Instantiate(pieceToSpawn2);
        else if (pointsRewarded <= 100) piece = Instantiate(pieceToSpawn3);
        else piece = Instantiate(pieceToSpawn4);
        

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

    public void fadeOutEffect()
    {
        startFadingOut = true;
        Object.Destroy(gameObject, 5.0f);
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

    public void lookAt()
    {
        float zRotation = transform.localEulerAngles.z;
        transform.LookAt(hmd_transform);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, zRotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (DifficultyManager.Instance.gamePaused) return;

        if(isAnimating)
        {
            transform.position -= movedOffset * (Time.deltaTime / 2.0f);
            timeToAnimate += Time.deltaTime;
            if (timeToAnimate >= 2.0f) isAnimating = false;
            lookAt();
        }
        else remainingTime -= Time.deltaTime;
       
        if(startFadingOut)
        {
            Color objectColor = renderer.material.color;
            float fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            renderer.material.color = objectColor;

            Color spriteColor = spriteRenderer.material.color;
            spriteColor = new Color(spriteColor.r, spriteColor.g, spriteColor.b, fadeAmount);
            spriteRenderer.material.color = spriteColor;

            if (objectColor.a <= 0)
            {
                startFadingOut = false;
            }
        }
    }
}
