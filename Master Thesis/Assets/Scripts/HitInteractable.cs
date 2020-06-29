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
    private bool startColliderHit = false;
    private bool middleColliderHit = false;
    private bool endColliderHit = false;
    private int actAccuracyStart = 0;
    private int actAccuracyMiddle = 0;
    private Vector3 relevantPositionToAddForce;
    public int destroyVariant = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
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
            if(!middleColliderHit && !endColliderHit)
            startColliderHit = true;
            assignStartAccuracy(other.gameObject.name);
            //Debug.Log("Fith collider entered");
        }
        if (other.gameObject.CompareTag("directionSecond"))
        {
            if(startColliderHit && !endColliderHit)
            middleColliderHit = true;
            assignMiddleAccuracy(other.gameObject.name);
            //Debug.Log("Fith collider entered");
        }
        if (other.gameObject.CompareTag("directionThird"))
        {
            endColliderHit = true;
            //Debug.Log("Fith collider entered");
        }
        /*SpawnedInteractable si = other.gameObject.transform.parent.GetComponent<SpawnedInteractable>();
        si.addForceToRigidBody(gameObject.transform.position);*/
    }

    private void assignStartAccuracy(string colliderName)
    {
        assignConditionalAcc(colliderName, "One", ref actAccuracyStart, 1);
        assignConditionalAcc(colliderName, "Two", ref actAccuracyStart, 2);
        assignConditionalAcc(colliderName, "Three", ref actAccuracyStart, 3);
        assignConditionalAcc(colliderName, "Four", ref actAccuracyStart, 4);
        assignConditionalAcc(colliderName, "Five", ref actAccuracyStart, 5);
        assignConditionalAcc(colliderName, "Six", ref actAccuracyStart, 6);
        assignConditionalAcc(colliderName, "Seven", ref actAccuracyStart, 7);
        assignConditionalAcc(colliderName, "Eight", ref actAccuracyStart, 8);
        assignConditionalAcc(colliderName, "Nine", ref actAccuracyStart, 9);
        assignConditionalAcc(colliderName, "Ten", ref actAccuracyStart, 10);
    }

    private void assignMiddleAccuracy(string colliderName)
    {
        assignConditionalAcc(colliderName, "One", ref actAccuracyMiddle, 1);
        assignConditionalAcc(colliderName, "Two", ref actAccuracyMiddle, 2);
        assignConditionalAcc(colliderName, "Three", ref actAccuracyMiddle, 3);
        assignConditionalAcc(colliderName, "Four", ref actAccuracyMiddle, 4);
        assignConditionalAcc(colliderName, "Five", ref actAccuracyMiddle, 5);
        assignConditionalAcc(colliderName, "Six", ref actAccuracyMiddle, 6);
        assignConditionalAcc(colliderName, "Seven", ref actAccuracyMiddle, 7);
        assignConditionalAcc(colliderName, "Eight", ref actAccuracyMiddle, 8);
        assignConditionalAcc(colliderName, "Nine", ref actAccuracyMiddle, 9);
        assignConditionalAcc(colliderName, "Ten", ref actAccuracyMiddle, 10);
    }

    private void assignConditionalAcc(string colliderName, string ending, ref int accuracy, int val)
    {
        if (colliderName.EndsWith(ending)) assignIfGreaterThanActAccuracy(ref accuracy, val);
    }

    private void assignIfGreaterThanActAccuracy(ref int acc, int newVal)
    {
        if (newVal > acc) acc = newVal;
    }

    private void OnTriggerExit(Collider other)
    {
        int precision = 0;
        float precisionPercent = 0.0f;
        if (other.gameObject.CompareTag("precisionOne"))
        {
            precision = 1;
            precisionPercent = 0.2f;
        }
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
        }
        SpawnedInteractable si = other.gameObject.transform.parent.GetComponent<SpawnedInteractable>();
        bool pointsRewarded = si.getPointsRewarded();
        if (!pointsRewarded && startColliderHit && middleColliderHit)
        {
            precision = (actAccuracyStart + actAccuracyMiddle) / 2;
            precisionPercent = (actAccuracyStart + actAccuracyMiddle) / 2.0f * 0.1f;
            si.setPointsRewarded();
            float percentRemainingTime = si.getRemainingTimeInPercent();
            int pointsEarned = 0;
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


            Debug.Log("Earned Points: " + pointsEarned + " with precision: " + precisionPercent);
            highScore.updateHighscore(pointsEarned);
            destroyEffect(si);
            listTimeNeededToHitObject.Add(si.getNeededTimeToHitObject());
            listPrecisionWithThatObjectWasHit.Add(precisionPercent);
            listEarnedPoints.Add(pointsEarned);
            listBlockPairNumber.Add(si.roundGenerated);
            ++countObjectsHit;
            initialiseVariables();
        }


        if (precision == 1)
        {
            bool pr = si.getPointsRewarded();
            if(pr) Object.Destroy(other.gameObject.transform.parent.gameObject, 10f);
            //si.addForceToRigidBody(relevantPositionToAddForce);
        }
    }

    private void initialiseVariables()
    {
        interactionLocked = false;
        startColliderHit = false;
        middleColliderHit = false;
        endColliderHit = false;
        actAccuracyStart = 0;
        actAccuracyMiddle = 0;
    }

    private void destroyEffect(SpawnedInteractable si)
    {
        if(destroyVariant == 0) si.addForceToRigidBody(relevantPositionToAddForce);
        if(destroyVariant == 1) si.ExplodeIntoPieces(relevantPositionToAddForce);
    }
}
