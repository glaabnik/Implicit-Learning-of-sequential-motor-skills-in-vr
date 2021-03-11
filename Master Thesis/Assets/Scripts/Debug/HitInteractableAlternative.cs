using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitInteractableAlternative : MonoBehaviour
{
    public HighScore highScore;
    public int countObjectsHit;
    public List<float> listTimeNeededToHitObject;
    public List<float> listPrecisionWithThatObjectWasHit;
    public List<int> listEarnedPoints;
    public List<int> listBlockPairNumber;
    public List<Difficulty> listDifficulty;
    public GameObject sphereToSpawn;
    public GameObject sphereToSpawn2;
    public bool oneTryForHittingCubesCorrectly = true;
    public bool canOnlyHitMatchingCube = false;
    public string tagCube;
    private bool interactionLocked = false;
    private Vector3 relevantPositionToAddForce;
    private Vector3 positionInitialColliderEnteredSphere;
    private Vector3 positionInitialColliderLeftSphere;
    private Vector3 positionInitialColliderEntered;
    private Vector3 positionInitialColliderLeft, positionInitialColliderLeft_Avg;
    private Vector3 positionColliderGroupStartEntered, positionColliderGroupStartLeft;
    private Vector3 positionColliderGroupMiddleEntered, positionColliderGroupMiddleLeft;
    private Vector3 positionColliderGroupEndEntered, positionColliderGroupEndLeft;
    private bool resetHasToBeDone = false;
    private SpawnedInteractableAlternative siToReset;
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

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("On Collision Enter!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! " + collision.collider.name);
        Collider other = collision.collider;
        if(other.gameObject.CompareTag("precisionOne"))
        {
            Vector3 contact_point = collision.GetContact(0).point;
            ContactPoint[] contacts = new ContactPoint[collision.contactCount];
            collision.GetContacts(contacts);
            Vector3 avg_contact_point = getAvgContactPoint(contacts);
            positionInitialColliderEntered = contact_point;
            Debug.Log("Collision Enter: Position Collider betreten: " + contact_point);
            Debug.Log("Collision Enter: Position Collider betreten (Avg): " + avg_contact_point);
        }
        if (other.gameObject.CompareTag("directionFirst"))
        {
            if (other.gameObject.name.EndsWith("One"))
            {
                positionColliderGroupStartEntered = collision.GetContact(0).point;
                Debug.Log("direction First Collider collision enter event");
                Debug.Log("Collision Enter: direction-First-Collider betreten: " + positionColliderGroupStartEntered);
            }
        }
        if (other.gameObject.CompareTag("directionSecond"))
        {
            if (other.gameObject.name.EndsWith("One"))
            {
                positionColliderGroupMiddleEntered = collision.GetContact(0).point;
                Debug.Log("direction Second Collider collision enter event");
                Debug.Log("Collision Enter: direction-Second-Collider betreten: " + positionColliderGroupMiddleEntered);
            }
        }
        if (other.gameObject.CompareTag("directionThird"))
        {
            if (other.gameObject.name.EndsWith("One"))
            {
                positionColliderGroupEndEntered = collision.GetContact(0).point;
                Debug.Log("direction Third Collider collision enter event");
                Debug.Log("Collision Enter: direction-Third-Collider betreten: " + positionColliderGroupEndEntered);
            }
        }
        if (other.gameObject.CompareTag("directionFourth"))
        {
            if (other.gameObject.name.EndsWith("One"))
            {
                Debug.Log("direction Fourth Collider collision enter event");
            }
        }
        if (other.gameObject.CompareTag("directionFith"))
        {
            if (other.gameObject.name.EndsWith("One"))
            {
                Debug.Log("direction Fith Collider collision enter event");
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Collider other = collision.collider;
        if (other.gameObject.CompareTag("precisionOne"))
        {
            Vector3 contact_point = collision.GetContact(0).point;
            ContactPoint[] contacts = new ContactPoint[collision.contactCount];
            collision.GetContacts(contacts);
            Vector3 avg_contact_point = getAvgContactPoint(contacts);
            positionInitialColliderLeft = contact_point;
            positionInitialColliderLeft_Avg = avg_contact_point;
            //positionInitialColliderEntered = contact_point;
        }
        if (other.gameObject.CompareTag("directionFirst"))
        {
            if (other.gameObject.name.EndsWith("One"))
            {
                positionColliderGroupStartLeft = collision.GetContact(0).point;
            }
        }
        if (other.gameObject.CompareTag("directionSecond"))
        {
            if (other.gameObject.name.EndsWith("One"))
            {
                positionColliderGroupMiddleLeft = collision.GetContact(0).point;
            }
        }
        if (other.gameObject.CompareTag("directionThird"))
        {
            if (other.gameObject.name.EndsWith("One"))
            {
                positionColliderGroupEndLeft = collision.GetContact(0).point;
            }
        }
        if (other.gameObject.CompareTag("directionFourth"))
        {
            if (other.gameObject.name.EndsWith("One"))
            {
                Debug.Log("direction Fourth Collider collision enter event");
            }
        }
        if (other.gameObject.CompareTag("directionFith"))
        {
            if (other.gameObject.name.EndsWith("One"))
            {
                Debug.Log("direction Fith Collider collision enter event");
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        //Debug.Log("On Collision Exit!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! " + collision.collider.name);
        Collider other = collision.collider;
        
        if (other.gameObject.CompareTag("precisionOne"))
        {
            SpawnedInteractableAlternative si = other.gameObject.transform.parent.GetComponent<SpawnedInteractableAlternative>();
            Debug.Log("Collision Exit: Position Haupt-Collider verlassen: " + positionInitialColliderLeft);
            Debug.Log("Collision Exit: Position Haupt-Collider verlassen (avg): " + positionInitialColliderLeft_Avg);
            //positionInitialColliderLeft = collision.GetContact(0).point;
            /*Debug.Log("Ideal Vector from Arrow Direction: " + si.getIdealVector());
            Debug.Log("Ideal Vector local from Arrow Direction: " + si.getIdealVectorLocal());
            Vector3 calc_Vector = positionInitialColliderLeft - positionInitialColliderEntered;
            Debug.Log("Calculated Vector from Collider contact points: " + calc_Vector);*/
            vectorHitDirectionEqualsIdealVector(si);

        }

            if (other.gameObject.CompareTag("directionFirst"))
        {
            if (other.gameObject.name.EndsWith("One"))
            {
                Debug.Log("direction First Collider collision exit event");
                Debug.Log("Collision Exit: direction-First-Collider verlassen: " + positionColliderGroupStartLeft);
            }
        }
        if (other.gameObject.CompareTag("directionSecond"))
        {
            if (other.gameObject.name.EndsWith("One"))
            {
                Debug.Log("direction Second Collider collision exit event");
                Debug.Log("Collision Exit: direction-Second-Collider verlassen: " + positionColliderGroupMiddleLeft);
            }
        }
        if (other.gameObject.CompareTag("directionThird"))
        {
            if (other.gameObject.name.EndsWith("One"))
            {
                Debug.Log("direction Third Collider collision exit event");
                Debug.Log("Collision Exit: direction-Third-Collider verlassen: " + positionColliderGroupEndLeft);
            }
        }
        if (other.gameObject.CompareTag("directionFourth"))
        {
            if (other.gameObject.name.EndsWith("One"))
            {
                Debug.Log("direction Fourth Collider collision exit event");
            }
        }
        if (other.gameObject.CompareTag("directionFith"))
        {
            if (other.gameObject.name.EndsWith("One"))
            {
                Debug.Log("direction Fith Collider collision exit event");
            }
        }
    }

    private Vector3 getAvgContactPoint(ContactPoint[] contact_points)
    {
        float x_sum = 0, y_sum = 0, z_sum = 0;  
        foreach(ContactPoint contact in contact_points)
        {
            x_sum += contact.point.x;
            y_sum += contact.point.y;
            z_sum += contact.point.z;
        }
        x_sum = x_sum / (float) contact_points.Length;
        y_sum = y_sum / (float) contact_points.Length;
        z_sum = z_sum / (float) contact_points.Length;
        return new Vector3(x_sum, y_sum, z_sum);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        SpawnedInteractableAlternative si = other.gameObject.transform.parent.GetComponent<SpawnedInteractableAlternative>();
        if (other.gameObject.CompareTag("precisionOne"))
        {
            interactionLocked = true;
            si.resetColorOfColliderGroupHits();
            //relevantPositionToAddForce = gameObject.GetComponent<Collider>().ClosestPoint(si.getCenter());
            relevantPositionToAddForce = gameObject.transform.position;
            positionInitialColliderEnteredSphere = gameObject.GetComponent<Collider>().ClosestPoint(si.getCenter());
            GameObject sphere = Instantiate(sphereToSpawn);
            sphere.transform.position = positionInitialColliderEntered;
            sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            Object.Destroy(sphere, 2f);
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
            if (other.gameObject.name.EndsWith("One"))
            {
                si.incrementColliderGroupsHit(other);
                si.changeColorColliderGroup(other);
            }
        }
        if (other.gameObject.CompareTag("directionSecond"))
        {
            si.middleColliderGroupHit(other.gameObject.name);
            if (other.gameObject.name.EndsWith("One"))
            {
                si.incrementColliderGroupsHit(other);
                si.changeColorColliderGroup(other);
            }
        }
        if (other.gameObject.CompareTag("directionThird"))
        {
            if (si.uses5ColliderGroups) si.middleColliderGroupHit2(other.gameObject.name);
            else si.endColliderGroupHit(other.gameObject.name);

            if (other.gameObject.name.EndsWith("One"))
            {
                si.incrementColliderGroupsHit(other);
                si.changeColorColliderGroup(other);
            }
        }
        if (other.gameObject.CompareTag("directionFourth"))
        {
            si.middleColliderGroupHit3(other.gameObject.name);
            if (other.gameObject.name.EndsWith("One"))
            {
                si.incrementColliderGroupsHit(other);
                si.changeColorColliderGroup(other);
            }
        }
        if (other.gameObject.CompareTag("directionFith"))
        {
            si.endColliderGroupHit(other.gameObject.name);
            if (other.gameObject.name.EndsWith("One"))
            {
                si.incrementColliderGroupsHit(other);
                si.changeColorColliderGroup(other);
            }
        }
        /*SpawnedInteractable si = other.gameObject.transform.parent.GetComponent<SpawnedInteractable>();
        si.addForceToRigidBody(gameObject.transform.position);*/
    }



    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("precisionOne")) return;

        SpawnedInteractableAlternative si = other.gameObject.transform.parent.GetComponent<SpawnedInteractableAlternative>();
        positionInitialColliderLeftSphere = gameObject.GetComponent<Collider>().ClosestPoint(si.getCenter());
        GameObject sphere = Instantiate(sphereToSpawn2);
        sphere.transform.position = positionInitialColliderLeft;
        sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        Object.Destroy(sphere, 2f);
        bool pointsRewarded = si.getPointsRewarded();

        if(!hitOnObjectWasIntended(positionInitialColliderLeftSphere, si))
        {
            //Debug.Log("Schlag fehlgeschlagen!");
            Debug.Log("War der Schlag gedacht?: " + hitOnObjectWasIntended(positionInitialColliderLeft, si));
            Debug.Log("Position Collider verlassen: " + positionInitialColliderLeft);
            Debug.Log("Position Collider betreten: " + positionInitialColliderEntered);
            Debug.Log("Länge des Vektors: " + (positionInitialColliderLeft - positionInitialColliderEntered).magnitude);
            Debug.Log("Scale des Würfels: " + si.gameObject.transform.localScale.x);
        }


        if (!hitOnObjectWasIntended(positionInitialColliderLeftSphere, si))
        {
            SoundManager.Instance.PlayHitSound(12, 0.5f);
        }

        if (!pointsRewarded && hitOnObjectWasIntended(positionInitialColliderLeft, si))
        {
            int precision = 0;
            float precisionPercent = 0.0f;
            float percentRemainingTime = si.getRemainingTimeInPercent();
            int pointsEarned = 0;
            bool b = vectorHitDirectionEqualsIdealVector(si);
            //si.wasHitInRightDirection() ||
            if (b)
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

            Debug.Log("Precision before sound: " + precision);
            SoundManager.Instance.PlayHitSound(precision, 0.5f);
            interactionLocked = false;
            siToReset = si;
            resetHasToBeDone = true;
            resetColliderGroups();

            if (precision == 0 && !oneTryForHittingCubesCorrectly) return;

            si.setPointsRewarded(pointsEarned);
            Debug.Log("Earned Points: " + pointsEarned + " with precision: " + precisionPercent);
            highScore.updateHighscore(pointsEarned, si.getColor(pointsEarned), tagCube);
            //destroyEffect(si, pointsEarned);
            listTimeNeededToHitObject.Add(si.getNeededTimeToHitObject());
            listPrecisionWithThatObjectWasHit.Add(precisionPercent);
            listEarnedPoints.Add(pointsEarned);
            listBlockPairNumber.Add(si.roundGenerated);
            listDifficulty.Add(DifficultyManager.Instance.difficulty);
            ++countObjectsHit;
            //Object.Destroy(other.gameObject.transform.parent.gameObject, 10f);
           
           
        }
    }

    private bool vectorHitDirectionEqualsIdealVector(SpawnedInteractableAlternative si)
    {
        Vector3 idealVector = si.getIdealVector();
        Vector3 calc_Vector = positionInitialColliderLeft - positionInitialColliderEntered;
        Vector3 diff = idealVector - calc_Vector;
        float magnitude = diff.magnitude;

        Vector3 idealVectorlocal = si.getIdealVectorLocal();
        Vector3 calc_Vector_local = si.gameObject.transform.InverseTransformPoint(positionInitialColliderLeft) - 
                                    si.gameObject.transform.InverseTransformPoint(positionInitialColliderEntered);
        Vector3 calc_Vector_local_avg = si.gameObject.transform.InverseTransformPoint(positionInitialColliderLeft_Avg) -
                                        si.gameObject.transform.InverseTransformPoint(positionInitialColliderEntered);
        Debug.Log("Ideal Vector local: " + idealVectorlocal);
        Debug.Log("Calc Vector local: " + calc_Vector_local);
        Debug.Log("Calc Vector local: " + calc_Vector_local_avg);

        Vector2 idealVectorlocal2D = new Vector2(idealVectorlocal.x, idealVectorlocal.y);
        Vector2 calc_Vector_local2D = new Vector2(calc_Vector_local.x, calc_Vector_local.y);
        Vector2 calc_Vector_Avg_local2D = new Vector2(calc_Vector_local_avg.x, calc_Vector_local_avg.y);
        float ideal_magnitude = calc_Vector_local2D.magnitude > 0.1 ? calc_Vector_local2D.magnitude : 1;
        float ideal_magnitude_avg = calc_Vector_Avg_local2D.magnitude > 0.1 ? calc_Vector_Avg_local2D.magnitude : 1;

        float dot_product_2D = idealVectorlocal2D.x * calc_Vector_local2D.x + idealVectorlocal2D.y * calc_Vector_local2D.y;
        float alpha_2D = Mathf.Acos(dot_product_2D / (idealVectorlocal2D.magnitude * calc_Vector_local2D.magnitude));
        Debug.Log("angle between two 2D local vectors: " + alpha_2D * Mathf.Rad2Deg);
        Debug.Log("cosine similiary for 2D vectors: " + dot_product_2D / (idealVectorlocal2D.magnitude * ideal_magnitude));

        float dot_product2_2D = idealVectorlocal2D.x * calc_Vector_Avg_local2D.x + idealVectorlocal2D.y * calc_Vector_Avg_local2D.y;
        float alpha2_2D = Mathf.Acos(dot_product_2D / (idealVectorlocal2D.magnitude * calc_Vector_Avg_local2D.magnitude));
        Debug.Log("angle between two 2D local vectors (Avg): " + alpha2_2D * Mathf.Rad2Deg);
        Debug.Log("cosine similiary for 2D vectors (Avg): " + dot_product2_2D / (idealVectorlocal2D.magnitude * ideal_magnitude_avg));

        Debug.Log("PositionInitialColliderEntered Local Space: " + si.gameObject.transform.InverseTransformPoint(positionInitialColliderLeft));
        Debug.Log("PositionInitialColliderLeft Local Space: " + si.gameObject.transform.InverseTransformPoint(positionInitialColliderEntered));

        // calculate angle between vectors
        float dot_product = idealVector.x * calc_Vector.x + idealVector.y * calc_Vector.y + idealVector.z * calc_Vector.z;
        float alpha = Mathf.Acos(dot_product / (idealVector.magnitude * calc_Vector.magnitude));
        Debug.Log("angle between two vectors: " + alpha * Mathf.Rad2Deg);

        return (alpha_2D * Mathf.Rad2Deg) <= 40;
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

    private bool hitOnObjectWasIntended(Vector3 positionCubeColliderLeft, SpawnedInteractableAlternative si)
    {
        return (positionCubeColliderLeft - positionInitialColliderEnteredSphere).magnitude >= 1.2 * si.gameObject.transform.localScale.x;
    }

    private void destroyEffect(SpawnedInteractableAlternative si, int pointsEarned)
    {
        /*if(pointsEarned == 0) si.fadeOutEffect();
        else if(destroyVariant == 0) si.addForceToRigidBody(relevantPositionToAddForce);
        else if(destroyVariant == 1) si.ExplodeIntoPieces(relevantPositionToAddForce, pointsEarned);*/
    }
}
