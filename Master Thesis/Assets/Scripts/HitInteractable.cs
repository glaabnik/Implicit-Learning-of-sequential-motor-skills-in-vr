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
    private bool interactionLocked = false;
    private Vector3 relevantPositionToAddForce;
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
        
    }

    private void OnTriggerEnter(Collider other)
    {
        SpawnedInteractable si = other.gameObject.transform.parent.GetComponent<SpawnedInteractable>();
        if (other.gameObject.CompareTag("precisionOne"))
        {
            interactionLocked = true;
            relevantPositionToAddForce = gameObject.transform.position;
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
        /*if (other.gameObject.CompareTag("precisionOne"))
        {
            SpawnedInteractable block = other.gameObject.transform.parent.GetComponent<SpawnedInteractable>();
            block.resetColliderGroupsHit();
            Debug.Log("Collider Groups resettet");
            Debug.Log("Endcollider was hit!!!!!!!");
        }*/
        /*

        if (other.gameObject.CompareTag("precisionTwo"))
        {
            precision = 2;
            precisionPercent = 0.4f;
        }
        if (other.gameObject.CompareTag("precisionThree"))
        {
            precision = 3;
            precisionPercent = 0.6f;
        }
        if (other.gameObject.CompareTag("precisionFour"))
        {
            precision = 4;
            precisionPercent = 0.8f;
        }
        if (other.gameObject.CompareTag("precisionFive"))
        {
            precision = 5;
            precisionPercent = 1.0f;
        }*/
        if (!other.gameObject.CompareTag("precisionOne")) return;
    
        SpawnedInteractable si = other.gameObject.transform.parent.GetComponent<SpawnedInteractable>();
        bool pointsRewarded = si.getPointsRewarded();

        if (!pointsRewarded )
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
            }
            else
            {
                precision = 0;
                precisionPercent = 0.0f;
            }
                si.setPointsRewarded(pointsEarned);
                Debug.Log("Earned Points: " + pointsEarned + " with precision: " + precisionPercent);
                highScore.updateHighscore(pointsEarned);
                SoundManager.Instance.PlayHitSound(precision, 0.5f);
                destroyEffect(si, pointsEarned);
                listTimeNeededToHitObject.Add(si.getNeededTimeToHitObject());
                listPrecisionWithThatObjectWasHit.Add(precisionPercent);
                listEarnedPoints.Add(pointsEarned);
                listBlockPairNumber.Add(si.roundGenerated);
                ++countObjectsHit;
                interactionLocked = false;
                Object.Destroy(other.gameObject.transform.parent.gameObject, 10f);
           
           
        }
    }

    private void destroyEffect(SpawnedInteractable si, int pointsEarned)
    {
        if(pointsEarned == 0) si.fadeOutEffect();
        else if(destroyVariant == 0) si.addForceToRigidBody(relevantPositionToAddForce);
        else if(destroyVariant == 1) si.ExplodeIntoPieces(relevantPositionToAddForce, pointsEarned);
    }
}
