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
    private Vector3 positionInitialPhysicsColliderEntered;
    private Vector3 positionInitialPhysicsColliderLeft;
    private Vector3 positionAngleMiddleColliderEntered;
    private Vector3 positionAngleMiddleColliderLeft;
    private Vector3 positionAngleFrontColliderEntered;
    private Vector3 positionAngleFrontColliderLeft;
    private Vector3 positionInitialColliderLeft_Avg;
    private bool resetHasToBeDone = false;
    private SpawnedInteractable siToReset;
    private float angle;
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
            /*Vector3 contact_point = collision.GetContact(0).point;
            positionInitialPhysicsColliderEntered = contact_point;
            //relevantPositionToAddForce = gameObject.transform.position;
            relevantPositionToAddForce = positionInitialPhysicsColliderEntered;*/
        }
        if (other.gameObject.CompareTag("angleCollider"))
        {
            Vector3 contact_point = collision.GetContact(0).point;
            ContactPoint[] contacts = new ContactPoint[collision.contactCount];
            collision.GetContacts(contacts);
            Vector3 avg_contact_point = getAvgContactPoint(contacts);
            positionAngleFrontColliderEntered = contact_point;
        }
        if (other.gameObject.CompareTag("angleColliderMiddle"))
        {
            Vector3 contact_point = collision.GetContact(0).point;
            ContactPoint[] contacts = new ContactPoint[collision.contactCount];
            collision.GetContacts(contacts);
            Vector3 avg_contact_point = getAvgContactPoint(contacts);
            positionAngleMiddleColliderEntered = contact_point;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Collider other = collision.collider;
        if (other.gameObject.CompareTag("precisionOne"))
        {
            /*Vector3 contact_point = collision.GetContact(0).point;
            ContactPoint[] contacts = new ContactPoint[collision.contactCount];
            collision.GetContacts(contacts);
            Vector3 avg_contact_point = getAvgContactPoint(contacts);
            positionInitialPhysicsColliderLeft = contact_point;
            positionInitialColliderLeft_Avg = avg_contact_point;*/
        }
        if (other.gameObject.CompareTag("angleCollider"))
        {
            Vector3 contact_point = collision.GetContact(0).point;
            positionAngleFrontColliderLeft = contact_point;
        }
        if (other.gameObject.CompareTag("angleColliderMiddle"))
        {
            Vector3 contact_point = collision.GetContact(0).point;
            positionAngleMiddleColliderLeft = contact_point;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        Collider other = collision.collider;

        if (other.gameObject.CompareTag("precisionOne"))
        {
            Debug.Log("On Collision Exit working!");
            SpawnedInteractable si = other.gameObject.transform.parent.GetComponent<SpawnedInteractable>();
            //Debug.Log("Angle is ok: "+ vectorHitDirectionEqualsIdealVector(si));
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

    private bool vectorHitDirectionEqualsIdealVector(SpawnedInteractable si, Vector3 idealVectorlocal, Vector3 positionEntered, Vector3 positionLeft)
    {
        Vector3 calc_Vector_local = si.gameObject.transform.InverseTransformPoint(positionLeft) -
                                    si.gameObject.transform.InverseTransformPoint(positionEntered);

        Vector2 idealVectorlocal2D = new Vector2(idealVectorlocal.x, idealVectorlocal.y);
        Vector2 calc_Vector_local2D = new Vector2(calc_Vector_local.x, calc_Vector_local.y);

        float dot_product_2D = idealVectorlocal2D.x * calc_Vector_local2D.x + idealVectorlocal2D.y * calc_Vector_local2D.y;
        float alpha_2D = Mathf.Acos(dot_product_2D / (idealVectorlocal2D.magnitude * calc_Vector_local2D.magnitude));

        Debug.Log("Angle calculated: " + alpha_2D * Mathf.Rad2Deg);
        angle = alpha_2D * Mathf.Rad2Deg;

        return (alpha_2D * Mathf.Rad2Deg) <= 45;
    }

    private bool vectorHitDirectionEqualsIdealVector(SpawnedInteractable si)
    {
        return vectorHitDirectionEqualsIdealVector(si, si.getIdealVectorLocal(), positionAngleMiddleColliderEntered, positionAngleMiddleColliderLeft) ||
               vectorHitDirectionEqualsIdealVector(si, si.getFrontIdealVectorLocal(), positionAngleFrontColliderEntered, positionAngleFrontColliderLeft);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent != null)
        {

            SpawnedInteractable si = other.gameObject.transform.parent.GetComponent<SpawnedInteractable>();
            if (si == null || !si.isHittable()) return;

            if (other.gameObject.CompareTag("precisionOne"))
            {
                interactionLocked = true;
                //relevantPositionToAddForce = gameObject.GetComponent<Collider>().ClosestPoint(si.getCenter());
                positionInitialPhysicsColliderEntered = gameObject.GetComponent<Collider>().ClosestPoint(si.getCenter());
            }
        
            if (other.gameObject.CompareTag("precisionTwo"))
            {
                si.setInnerAccuracy(3);
            }
            if (other.gameObject.CompareTag("precisionThree"))
            {
                si.setInnerAccuracy(5);
            }
            if (other.gameObject.CompareTag("precisionFour"))
            {
                si.setInnerAccuracy(7);
            }
            if (other.gameObject.CompareTag("precisionFive"))
            {
                si.setInnerAccuracy(10);
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("precisionOne")) return;

        SpawnedInteractable si = other.gameObject.transform.parent.GetComponent<SpawnedInteractable>();

        if (!si.isHittable()) return;

        positionInitialPhysicsColliderLeft = gameObject.GetComponent<Collider>().ClosestPoint(si.getCenter());
        bool pointsRewarded = si.getPointsRewarded();

        if (!hitOnObjectWasIntended(positionInitialPhysicsColliderLeft, si))
        {
            SoundManager.Instance.PlayHitSound(12, 0.5f);
        }

        if (!pointsRewarded && hitOnObjectWasIntended(positionInitialPhysicsColliderLeft, si))
        {
            int precision = 0;
            float precisionPercent = 0.0f;
            float percentRemainingTime = si.getRemainingTimeInPercent();
            int pointsEarned = 0;
            if (si.wasHitInRightDirection() || (vectorHitDirectionEqualsIdealVector(si) /*&& hitWasThroughWholeCube(positionInitialPhysicsColliderLeft, si)*/) )
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
            highScore.updateHighscore(pointsEarned, si.getColor(pointsEarned), tagCube);
            destroyEffect(si, pointsEarned, precision);
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
        return (positionCubeColliderLeft - positionInitialPhysicsColliderEntered).magnitude >= 0.7 * si.gameObject.transform.localScale.x;
    }

    private bool hitWasThroughWholeCube(Vector3 positionCubeColliderLeft, SpawnedInteractable si)
    {
        return (positionCubeColliderLeft - positionInitialPhysicsColliderEntered).magnitude >= 0.7 * si.gameObject.transform.localScale.x;
    }

    private void destroyEffect(SpawnedInteractable si, int pointsEarned, int precision)
    {
        if(pointsEarned == 0) si.fadeOutEffect();
        else if(destroyVariant == 0) si.addForceToRigidBody(relevantPositionToAddForce, precision);
        else if(destroyVariant == 1) si.ExplodeIntoPieces(relevantPositionToAddForce, pointsEarned);
    }
}
