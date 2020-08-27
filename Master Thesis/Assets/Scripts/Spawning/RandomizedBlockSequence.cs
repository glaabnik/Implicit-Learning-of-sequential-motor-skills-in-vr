using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedBlockSequence : BlockSequence
{
    public int countSphereCoordinatesOneIteration;
    public int iterations;
    public bool randomizeElevation = false;
    public int defaultElevation = 90;
    public int offsetElevationToBothSides = 30;
    public int offsetLeftSideRangeMin = 15;
    public int offsetLeftSideRangeMax = 40;
    public int offsetRightSideRangeMin = -40;
    public int offsetRightSideRangeMax = -15;
    public float scaleSpawnedGameObjects;
    public float sphereRadius;

    private List<SphereCoordinates> sequenceOfSpawns;
    private int actIndexInList;

    public override void Start()
    {
        sequenceOfSpawns = new List<SphereCoordinates>();
        generateRandomizedSphereCoordinates();
    }

    public override bool hasNextSphereCoordinates()
    {
        return actIndexInList < iterations * sequenceOfSpawns.Count;
    }

    public override SphereCoordinates nextSphereCoordinates()
    {
        return sequenceOfSpawns[actIndexInList++ % sequenceOfSpawns.Count];
    }

    private void generateRandomizedSphereCoordinates()
    {
        float degLeftPhi, degRightPhi, degLeftTheta, degRightTheta;
        float radiusLeft, radiusRight;
        float rotationZLeft, rotationZRight;
        Vector3 scaleLeft, scaleRight;
        degLeftPhi = Random.Range(offsetLeftSideRangeMin, offsetLeftSideRangeMax);
        degRightPhi = Random.Range(offsetRightSideRangeMin, offsetRightSideRangeMax);
        degLeftTheta = defaultElevation;
        degRightTheta = defaultElevation;
        if (randomizeElevation)
        {
            degLeftTheta = defaultElevation - offsetElevationToBothSides + Random.Range(0, offsetElevationToBothSides * 2);
            degRightTheta = defaultElevation - offsetElevationToBothSides + Random.Range(0, offsetElevationToBothSides * 2);
        }
        radiusLeft = sphereRadius;
        radiusRight = sphereRadius;
        rotationZLeft = getRandomRotationZ();
        rotationZRight = getRandomRotationZ();
        scaleLeft = new Vector3(scaleSpawnedGameObjects, scaleSpawnedGameObjects, scaleSpawnedGameObjects);
        scaleRight = new Vector3(scaleSpawnedGameObjects, scaleSpawnedGameObjects, scaleSpawnedGameObjects);
        SphereCoordinates sc = new SphereCoordinates();
        sc.phi = degLeftPhi;
        sc.phi2 = degRightPhi;
        sc.theta = degLeftTheta;
        sc.theta2 = degRightTheta;
        sc.radius = radiusLeft;
        sc.radius2 = radiusRight;
        sc.scale = scaleLeft;
        sc.scale2 = scaleRight;
        sc.rotationZ = rotationZLeft;
        sc.rotationZ2 = rotationZRight;
        sequenceOfSpawns.Add(sc);
    }

    private int getRandomRotationZ()
    {
        int z = Random.Range(0, 7);
        return z * 45;
    }
}
