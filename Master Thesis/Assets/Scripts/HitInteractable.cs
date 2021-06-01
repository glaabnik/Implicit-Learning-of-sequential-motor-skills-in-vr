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
            //positionInitialPhysicsColliderEntered = contact_point;
            //relevantPositionToAddForce = gameObject.transform.position;
            relevantPositionToAddForce = contact_point;*/
        }
        if (other.gameObject.transform.parent != null)
        {
            SpawnedInteractable si = other.gameObject.transform.parent.GetComponent<SpawnedInteractable>();

            if (si == null || !si.isHittable()) return;

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

    private int getAngleVectorHitDirectionToIdealVector(SpawnedInteractable si, Vector3 idealVectorlocal, Vector3 positionEntered, Vector3 positionLeft)
    {
        Vector3 calc_Vector_local = si.gameObject.transform.InverseTransformPoint(positionLeft) -
                                   si.gameObject.transform.InverseTransformPoint(positionEntered);

        Vector2 idealVectorlocal2D = new Vector2(idealVectorlocal.x, idealVectorlocal.y);
        Vector2 calc_Vector_local2D = new Vector2(calc_Vector_local.x, calc_Vector_local.y);

        float dot_product_2D = idealVectorlocal2D.x * calc_Vector_local2D.x + idealVectorlocal2D.y * calc_Vector_local2D.y;
        float alpha_2D = Mathf.Acos(dot_product_2D / (idealVectorlocal2D.magnitude * calc_Vector_local2D.magnitude));

        Debug.Log("Angle calculated: " + alpha_2D * Mathf.Rad2Deg);
        angle = alpha_2D * Mathf.Rad2Deg;

        return (int) angle;
    }

    private bool vectorHitDirectionDeviatesIdealVector(SpawnedInteractable si, Vector3 idealVectorlocal, Vector3 positionEntered, Vector3 positionLeft)
    {
        Vector3 calc_Vector_local = si.gameObject.transform.InverseTransformPoint(positionLeft) -
                                    si.gameObject.transform.InverseTransformPoint(positionEntered);

        Vector2 idealVectorlocal2D = new Vector2(idealVectorlocal.x, idealVectorlocal.y);
        Vector2 calc_Vector_local2D = new Vector2(calc_Vector_local.x, calc_Vector_local.y);

        float dot_product_2D = idealVectorlocal2D.x * calc_Vector_local2D.x + idealVectorlocal2D.y * calc_Vector_local2D.y;
        float alpha_2D = Mathf.Acos(dot_product_2D / (idealVectorlocal2D.magnitude * calc_Vector_local2D.magnitude));

        Debug.Log("Angle calculated: " + alpha_2D * Mathf.Rad2Deg);
        angle = alpha_2D * Mathf.Rad2Deg;

        return (alpha_2D * Mathf.Rad2Deg) >= 60;
    }

    private bool vectorHitDirectionEqualsIdealVector(SpawnedInteractable si)
    {
        return vectorHitDirectionEqualsIdealVector(si, si.getIdealVectorLocal(), positionAngleMiddleColliderEntered, positionAngleMiddleColliderLeft) ||
               vectorHitDirectionEqualsIdealVector(si, si.getFrontIdealVectorLocal(), positionAngleFrontColliderEntered, positionAngleFrontColliderLeft);
    }

    private int getAnglePrecision(SpawnedInteractable si)
    {
        int angleOne = getAngleVectorHitDirectionToIdealVector(si, si.getIdealVectorLocal(), positionAngleMiddleColliderEntered, positionAngleMiddleColliderLeft);
        int angleTwo = getAngleVectorHitDirectionToIdealVector(si, si.getFrontIdealVectorLocal(), positionAngleFrontColliderEntered, positionAngleFrontColliderLeft);
        if (angleOne < 5 || angleTwo < 5) return 10;
        if (angleOne < 10 || angleTwo < 10) return 9;
        if (angleOne < 15 || angleTwo < 15) return 8;
        if (angleOne < 20|| angleTwo < 20) return 7;
        if (angleOne < 25 || angleTwo < 25) return 6;
        if (angleOne < 30 || angleTwo < 30) return 5;
        if (angleOne < 33 || angleTwo < 33) return 4;
        if (angleOne < 36 || angleTwo < 36) return 3;
        if (angleOne < 39 || angleTwo < 39) return 2;
        if (angleOne < 46 || angleTwo < 46) return 1;
        else return 0;
    }

    private bool vectorHitDirectionDeviatesTooStrong(SpawnedInteractable si)
    {
        return vectorHitDirectionDeviatesIdealVector(si, si.getIdealVectorLocal(), positionAngleMiddleColliderEntered, positionAngleMiddleColliderLeft) ||
               vectorHitDirectionDeviatesIdealVector(si, si.getFrontIdealVectorLocal(), positionAngleFrontColliderEntered, positionAngleFrontColliderLeft);
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
                si.setInnerAccuracy(1);
                relevantPositionToAddForce = gameObject.GetComponent<Collider>().ClosestPoint(si.getCenter());
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
                si.setInnerAccuracy(8);
            }
            if (other.gameObject.CompareTag("precisionFive"))
            {
                si.setInnerAccuracy(10);
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
            int anglePrecision = 0;
            float precisionPercent = 0.0f;
            float percentRemainingTime = si.getRemainingTimeInPercent();
            int pointsEarned = 0;
            int precisionPointsEarned = 0;
            if ( (si.wasHitInRightDirection() && !vectorHitDirectionDeviatesTooStrong(si)) || (vectorHitDirectionEqualsIdealVector(si) /*&& hitWasThroughWholeCube(positionInitialPhysicsColliderLeft, si)*/) )
            {
                precision = si.getAvgAccuracy();
                anglePrecision = getAnglePrecision(si);
                if (anglePrecision * si.getInnerAccuracy() / 10.0 > precision) precision = (int) (anglePrecision * si.getInnerAccuracy() / 10.0f);
                precisionPercent = si.getAvgAccuracyPercent();

                if (precision == 1) precisionPointsEarned = 3;
                if (precision == 2) precisionPointsEarned = 5;
                if (precision == 3) precisionPointsEarned = 10;
                if (precision == 4) precisionPointsEarned = 15;
                if (precision == 5) precisionPointsEarned = 20;

                if (precision == 6) precisionPointsEarned = 25;
                if (precision == 7) precisionPointsEarned = 35;
                if (precision == 8) precisionPointsEarned = 45;
                if (precision == 9) precisionPointsEarned = 50;
                if (precision == 10) precisionPointsEarned = 60;

                pointsEarned = (int)(percentRemainingTime * 60 + precisionPointsEarned);

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
