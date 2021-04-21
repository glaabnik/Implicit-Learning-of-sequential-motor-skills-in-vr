using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BlockSequence : MonoBehaviour
{
    public abstract bool hasNextSphereCoordinates();
    public abstract SphereCoordinates nextSphereCoordinates();
    public abstract SphereCoordinates[] twoRandomSphereCoordinatesPairsForWholeSequence();

    public int pointScoreActIteration;
    public int maxPointScoreOneIteration;
    public List<int> pointScoreAllIterations;

    public virtual void addToPointScore(int val)
    {
        pointScoreActIteration += val;
    }

    public virtual float getAvgPointScoreOneIteration()
    {
        if (pointScoreAllIterations.Count == 0) return 0;
        int sum = 0;
        foreach(int i in pointScoreAllIterations)
        {
            sum += i;
        }
        return sum / (float) pointScoreAllIterations.Count;
    }

    public virtual void Awake()
    {
        pointScoreAllIterations = new List<int>();
    }

    public virtual void Update()
    {
        
    }

}
