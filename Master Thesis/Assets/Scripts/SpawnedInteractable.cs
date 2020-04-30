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
    void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        rigidBody = gameObject.GetComponent<Rigidbody>();

    }

    public bool isMoving()
    {
        return rigidBody.velocity.magnitude > 0;
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
        remainingTime -= Time.deltaTime;
    }
}
