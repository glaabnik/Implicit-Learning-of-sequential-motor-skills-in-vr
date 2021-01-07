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
    public List<Difficulty> listDifficulty;
    public bool oneTryForHittingCubesCorrectly = true;
    public bool canOnlyHitMatchingCube = false;
    public string tagCube;
    private bool interactionLocked = false;
    private Vector3 relevantPositionToAddForce;
    private Vector3 positionInitialColliderEnteredSphere;
    private Vector3 positionInitialColliderLeftSphere;
    private Vector3 positionInitialColliderEntered;
    private Vector3 positionInitialColliderLeft;
    private Vector3 positionInitialColliderLeft_Avg;
    private bool resetHasToBeDone = false;
    private SpawnedInteractable siToReset;
    public int destroyVariant = 0;

    void Start()
    {
        listTimeNeededToHitObject = new List<float>();
        listPrecisionWithThatObjectWasHit = new List<float>();
        listEarnedPoints = new List<int>();
        listBlockPairNumber = new List<int>();
        listDifficulty = new List<Difficulty>();
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

    void OnCollisionEnter(Collision collision)
    {
        Collider other = collision.collider;
        if (other.gameObject.CompareTag("precisionOne"))
        {
            Debug.Log("On Collision Enter working!");
            Vector3 contact_point = collision.GetContact(0).point;
            ContactPoint[] contacts = new ContactPoint[collision.contactCount];
            collision.GetContacts(contacts);
            Vector3 avg_contact_point = getAvgContactPoint(contacts);
            positionInitialColliderEntered = contact_point;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Collider other = collision.collider;
        if (other.gameObject.CompareTag("precisionOne"))
        {
            Debug.Log("On Collision Stay working!");
            Vector3 contact_point = collision.GetContact(0).point;
            ContactPoint[] contacts = new ContactPoint[collision.contactCount];
            collision.GetContacts(contacts);
            Vector3 avg_contact_point = getAvgContactPoint(contacts);
            positionInitialColliderLeft = contact_point;
            positionInitialColliderLeft_Avg = avg_contact_point;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        Collider other = collision.collider;

        if (other.gameObject.CompareTag("precisionOne"))
        {
            Debug.Log("On Collision Exit working!");
            SpawnedInteractable si = other.gameObject.transform.parent.GetComponent<SpawnedInteractable>();
            vectorHitDirectionEqualsIdealVector(si);
        }
    }

    private Vector3 getAvgContactPoint(ContactPoint[] contact_points)
    {
        float x_sum = 0, y_sum = 0, z_sum = 0;
        foreach (ContactPoint contact in contact_points)
        {
            x_sum += contact.point.x;
            y_sum += contact.point.y;
            z_sum += contact.point.z;
        }
        x_sum = x_sum / (float)contact_points.Length;
        y_sum = y_sum / (float)contact_points.Length;
        z_sum = z_sum / (float)contact_points.Length;
        return new Vector3(x_sum, y_sum, z_sum);
    }

    private bool vectorHitDirectionEqualsIdealVector(SpawnedInteractable si)
    {
        Vector3 idealVectorlocal = si.getIdealVectorLocal();
        Vector3 calc_Vector_local = si.gameObject.transform.InverseTransformPoint(positionInitialColliderLeft) -
                                    si.gameObject.transform.InverseTransformPoint(positionInitialColliderEntered);

        Vector2 idealVectorlocal2D = new Vector2(idealVectorlocal.x, idealVectorlocal.y);
        Vector2 calc_Vector_local2D = new Vector2(calc_Vector_local.x, calc_Vector_local.y);

        float dot_product_2D = idealVectorlocal2D.x * calc_Vector_local2D.x + idealVectorlocal2D.y * calc_Vector_local2D.y;
        float alpha_2D = Mathf.Acos(dot_product_2D / (idealVectorlocal2D.magnitude * calc_Vector_local2D.magnitude));

        Debug.Log("Angle calculated: " + alpha_2D * Mathf.Rad2Deg);

        return (alpha_2D * Mathf.Rad2Deg) <= 45;
    }

    private void OnTriggerEnter(Collider other)
    {
        SpawnedInteractable si = other.gameObject.transform.parent.GetComponent<SpawnedInteractable>();
        if (other.gameObject.CompareTag("precisionOne"))
        {
            interactionLocked = true;
            //relevantPositionToAddForce = gameObject.GetComponent<Collider>().ClosestPoint(si.getCenter());
            relevantPositionToAddForce = gameObject.transform.position;
            positionInitialColliderEnteredSphere = gameObject.GetComponent<Collider>().ClosestPoint(si.getCenter());
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
            if (si.uses5ColliderGroups) si.middleColliderGroupHit2(other.gameObject.name);
            else si.endColliderGroupHit(other.gameObject.name);
        }
        if (other.gameObject.CompareTag("directionFourth"))
        {
            si.middleColliderGroupHit3(other.gameObject.name);
        }
        if (other.gameObject.CompareTag("directionFith"))
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
        positionInitialColliderLeftSphere = gameObject.GetComponent<Collider>().ClosestPoint(si.getCenter());
        bool pointsRewarded = si.getPointsRewarded();

        if (!hitOnObjectWasIntended(positionInitialColliderLeftSphere, si))
        {
            SoundManager.Instance.PlayHitSound(12, 0.5f);
        }

        if (!pointsRewarded && hitOnObjectWasIntended(positionInitialColliderLeftSphere, si))
        {
            int precision = 0;
            float precisionPercent = 0.0f;
            float percentRemainingTime = si.getRemainingTimeInPercent();
            int pointsEarned = 0;
            if (si.wasHitInRightDirection() || (vectorHitDirectionEqualsIdealVector(si) /*&& hitWasThroughWholeCube(positionInitialColliderLeftSphere, si)*/) )
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

            if (canOnlyHitMatchingCube && !other.gameObject.transform.parent.gameObject.CompareTag(tagCube))
            {
                SoundManager.Instance.PlayHitSound(11, 0.5f);
                return;
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
            listDifficulty.Add(DifficultyManager.Instance.difficulty);
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

    private bool hitOnObjectWasIntended(Vector3 positionCubeColliderLeft, SpawnedInteractable si)
    {
        return (positionCubeColliderLeft - positionInitialColliderEnteredSphere).magnitude >= 0.7 * si.gameObject.transform.localScale.x;
    }

    private bool hitWasThroughWholeCube(Vector3 positionCubeColliderLeft, SpawnedInteractable si)
    {
        return (positionCubeColliderLeft - positionInitialColliderEnteredSphere).magnitude >= 1.2 * si.gameObject.transform.localScale.x;
    }

    private void destroyEffect(SpawnedInteractable si, int pointsEarned)
    {
        if(pointsEarned == 0) si.fadeOutEffect();
        else if(destroyVariant == 0) si.addForceToRigidBody(relevantPositionToAddForce, (int) (pointsEarned / 10.0f));
        else if(destroyVariant == 1) si.ExplodeIntoPieces(relevantPositionToAddForce, pointsEarned);
    }
}
