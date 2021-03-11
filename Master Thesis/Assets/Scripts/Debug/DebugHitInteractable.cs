using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHitInteractable : MonoBehaviour
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
    private Vector3 positionInitialColliderEntered;
    private Vector3 positionInitialColliderLeft;
    private bool resetHasToBeDone = false;
    private DebugSpawnedInteractable siToReset;
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
        Debug.Log("On Collision Enter!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        Collider other = collision.collider;
        if(other.gameObject.CompareTag("precisionOne"))
        {
            Vector3 contact_point = collision.GetContact(0).point;
            Debug.Log("Collision Enter: Position Collider betreten: " + contact_point);
        }
    }

        private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        DebugSpawnedInteractable si = other.gameObject.transform.parent.GetComponent<DebugSpawnedInteractable>();
        if (other.gameObject.CompareTag("precisionOne"))
        {
            interactionLocked = true;
            si.resetColorOfColliderGroupHits();
            //relevantPositionToAddForce = gameObject.GetComponent<Collider>().ClosestPoint(si.getCenter());
            relevantPositionToAddForce = gameObject.transform.position;
            positionInitialColliderEntered = gameObject.GetComponent<Collider>().ClosestPoint(si.getCenter());
            GameObject sphere = Instantiate(sphereToSpawn);
            sphere.transform.position = positionInitialColliderEntered;
            sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            Object.Destroy(sphere, 10f);
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

        DebugSpawnedInteractable si = other.gameObject.transform.parent.GetComponent<DebugSpawnedInteractable>();
        positionInitialColliderLeft = gameObject.GetComponent<Collider>().ClosestPoint(si.getCenter());
        GameObject sphere = Instantiate(sphereToSpawn2);
        sphere.transform.position = positionInitialColliderLeft;
        sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        Object.Destroy(sphere, 10f);
        bool pointsRewarded = si.getPointsRewarded();
        if(!hitOnObjectWasIntended(positionInitialColliderLeft, si))
        {
            Debug.Log("Schlag fehlgeschlagen!");
            Debug.Log("War der Schlag gedacht?: " + hitOnObjectWasIntended(positionInitialColliderLeft, si));
            Debug.Log("Position Collider verlassen: " + positionInitialColliderLeft);
            Debug.Log("Position Collider betreten: " + positionInitialColliderEntered);
            Debug.Log("Länge des Vektors: " + (positionInitialColliderLeft - positionInitialColliderEntered).magnitude);
            Debug.Log("Scale des Würfels: " + si.gameObject.transform.localScale.x);
        }
       

        if (!hitOnObjectWasIntended(positionInitialColliderLeft, si))
        {
            SoundManager.Instance.PlayHitSound(12, 0.5f);
            //findContactPointsBetweenTwoColliders(si);
        }

        if (!pointsRewarded && hitOnObjectWasIntended(positionInitialColliderLeft, si))
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

            if (canOnlyHitMatchingCube && !other.gameObject.transform.parent.gameObject.CompareTag(tagCube))
            {
                SoundManager.Instance.PlayHitSound(11, 0.5f);
                return;
            }

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

    private  void findContactPointsBetweenTwoColliders(DebugSpawnedInteractable si)
    {
        Vector3[] edges = si.getVertices();
        Transform tr = si.gameObject.transform;

        for (int i = 0; i < edges.Length; ++i) // transform local space edges coordinates to world space
        {
            edges[i] = tr.TransformPoint(edges[i]);
        }
        
        foreach (Vector3 edge in edges)
        {
            Debug.Log("Edge: " + edge);
            Debug.Log("Closest Point to this edge: " + gameObject.GetComponent<Collider>().ClosestPoint(edge));
            Debug.Log("Schlag gedacht for this edge: " + hitOnObjectWasIntended(edge, si));
        }
        positionInitialColliderLeft = gameObject.GetComponent<Collider>().ClosestPoint(si.getCenter());
        Debug.Log("Closest Point to center of renderer: " + positionInitialColliderLeft);
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

    private bool hitOnObjectWasIntended(Vector3 positionCubeColliderLeft, DebugSpawnedInteractable si)
    {
        return (positionCubeColliderLeft - positionInitialColliderEntered).magnitude >= 0.7 * si.gameObject.transform.localScale.x;
    }

    private void destroyEffect(DebugSpawnedInteractable si, int pointsEarned)
    {
        /*if(pointsEarned == 0) si.fadeOutEffect();
        else if(destroyVariant == 0) si.addForceToRigidBody(relevantPositionToAddForce);
        else if(destroyVariant == 1) si.ExplodeIntoPieces(relevantPositionToAddForce, pointsEarned);*/
    }
}
