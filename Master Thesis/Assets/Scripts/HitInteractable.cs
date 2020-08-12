using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitInteractable : MonoBehaviour
{
    public HighScore highScore;
    public int countObjectsHit;
    public List<float> listTimeNeededToHitObject;
    public List<float> listPrecisionWithThatObjectWasHit;
    public List<int> listEarnedPoints;
    public List<int> listBlockPairNumber;
    public bool oneTryForHittingCubesCorrectly = true;
    private bool interactionLocked = false;
    private Vector3 relevantPositionToAddForce;
    private Vector3 positionInitialColliderEntered;
    private Vector3 positionInitialColliderLeft;
    private bool resetHasToBeDone = false;
    private SpawnedInteractable siToReset;
    public int destroyVariant = 0;

    void Start()
    {
        listTimeNeededToHitObject = new List<float>();
        listPrecisionWithThatObjectWasHit = new List<float>();
        listEarnedPoints = new List<int>();
        listBlockPairNumber = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        resetColliderGroups();
    }

    private Vector3 GetPointOfContact()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            return hit.point;
        }
        return new Vector3(0, 0, 0);
    }

    /*void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contacts = new ContactPoint[20];
        collision.GetContacts(contacts);
        foreach (ContactPoint contact in contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
        if (collision.collider.gameObject.CompareTag("precisionOne"))
        {
            positionInitialColliderEntered = collision.GetContact(0).point;
            relevantPositionToAddForce = positionInitialColliderEntered;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("precisionOne")) positionInitialColliderLeft = collision.GetContact(0).point;
    }*/

    private void OnTriggerEnter(Collider other)
    {
        SpawnedInteractable si = other.gameObject.transform.parent.GetComponent<SpawnedInteractable>();
        if (other.gameObject.CompareTag("precisionOne"))
        {
            interactionLocked = true;
            relevantPositionToAddForce = gameObject.GetComponent<Collider>().ClosestPoint(si.getCenter());
            positionInitialColliderEntered = gameObject.GetComponent<Collider>().ClosestPoint(si.getCenter());
            //Debug.Log("First collider entered");
        }
        if (other.gameObject.CompareTag("precisionTwo"))
        {
            //Debug.Log("Second collider entered");
        }
        if (other.gameObject.CompareTag("precisionThree"))
        {
            //Debug.Log("Third collider entered");
        }
        if (other.gameObject.CompareTag("precisionFour"))
        {
            //Debug.Log("Fourth collider entered");
        }
        if (other.gameObject.CompareTag("precisionFive"))
        {
            //Debug.Log("Fith collider entered");
        }

        if (other.gameObject.CompareTag("directionFirst"))
        {
            si.startColliderGroupHit(other.gameObject.name);
        }
        if (other.gameObject.CompareTag("directionSecond"))
        {
            si.middleColliderGroupHit(other.gameObject.name);
        }
        if (other.gameObject.CompareTag("directionThird"))
        {
            si.endColliderGroupHit(other.gameObject.name);
        }
        /*SpawnedInteractable si = other.gameObject.transform.parent.GetComponent<SpawnedInteractable>();
        si.addForceToRigidBody(gameObject.transform.position);*/
    }

   

    private void OnTriggerExit(Collider other)
    {

        if (!other.gameObject.CompareTag("precisionOne")) return;

        SpawnedInteractable si = other.gameObject.transform.parent.GetComponent<SpawnedInteractable>();
        positionInitialColliderLeft = gameObject.GetComponent<Collider>().ClosestPoint(si.getCenter());
        bool pointsRewarded = si.getPointsRewarded();
        Debug.Log("War der Schlag gedacht?: " + hitOnObjectWasIntended(si));
        Debug.Log("Position Collider verlassen: " + positionInitialColliderLeft);
        Debug.Log("Position Collider betreten: " + positionInitialColliderEntered);
        Debug.Log("Länge des Vektors: " + (positionInitialColliderLeft - positionInitialColliderEntered).magnitude);
        Debug.Log("Scale des Würfels: " + si.gameObject.transform.localScale.x);
        if (!pointsRewarded && hitOnObjectWasIntended(si))
        {
            int precision = 0;
            float precisionPercent = 0.0f;
            float percentRemainingTime = si.getRemainingTimeInPercent();
            int pointsEarned = 0;
            if (si.wasHitInRightDirection())
            {
                precision = si.getAvgAccuracy();
                precisionPercent = si.getAvgAccuracyPercent();

                if (precision == 1) pointsEarned = (int)(percentRemainingTime * 10);
                if (precision == 2) pointsEarned = (int)(percentRemainingTime * 15);
                if (precision == 3) pointsEarned = (int)(percentRemainingTime * 20);
                if (precision == 4) pointsEarned = (int)(percentRemainingTime * 30);
                if (precision == 5) pointsEarned = (int)(percentRemainingTime * 35);

                if (precision == 6) pointsEarned = (int)(percentRemainingTime * 50);
                if (precision == 7) pointsEarned = (int)(percentRemainingTime * 60);
                if (precision == 8) pointsEarned = (int)(percentRemainingTime * 75);
                if (precision == 9) pointsEarned = (int)(percentRemainingTime * 90);
                if (precision == 10) pointsEarned = (int)(percentRemainingTime * 100);

                pointsEarned = (int) (pointsEarned * DifficultyManager.Instance.getPointModifier());
            }
            else
            {
                precision = 0;
                precisionPercent = 0.0f;
            }

            SoundManager.Instance.PlayHitSound(precision, 0.5f);
            interactionLocked = false;
            siToReset = si;
            resetHasToBeDone = true;
            if (precision == 0 && !oneTryForHittingCubesCorrectly) return;

            si.setPointsRewarded(pointsEarned);
            Debug.Log("Earned Points: " + pointsEarned + " with precision: " + precisionPercent);
            highScore.updateHighscore(pointsEarned, si.getColor(pointsEarned));
            destroyEffect(si, pointsEarned);
            listTimeNeededToHitObject.Add(si.getNeededTimeToHitObject());
            listPrecisionWithThatObjectWasHit.Add(precisionPercent);
            listEarnedPoints.Add(pointsEarned);
            listBlockPairNumber.Add(si.roundGenerated);
            ++countObjectsHit;
            Object.Destroy(other.gameObject.transform.parent.gameObject, 10f);
           
           
        }
    }

    private void resetColliderGroups()
    {
        if (resetHasToBeDone && !interactionLocked)
        {
            if(siToReset != null) siToReset.resetColliderGroupsHit();
            Debug.Log("Collider Groups resetted!!");
            resetHasToBeDone = false;
        }
    }

    private bool hitOnObjectWasIntended(SpawnedInteractable si)
    {
        return (positionInitialColliderLeft - positionInitialColliderEntered).magnitude >= 0.9 * si.gameObject.transform.localScale.x;
    }

    private void destroyEffect(SpawnedInteractable si, int pointsEarned)
    {
        if(pointsEarned == 0) si.fadeOutEffect();
        else if(destroyVariant == 0) si.addForceToRigidBody(relevantPositionToAddForce);
        else if(destroyVariant == 1) si.ExplodeIntoPieces(relevantPositionToAddForce, pointsEarned);
    }
}
