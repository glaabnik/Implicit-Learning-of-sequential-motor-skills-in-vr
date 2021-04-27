using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Data;
using System.IO;

public class SerializeData : MonoBehaviour
{

    [XmlRootAttribute("SportGameData", Namespace = "http://www.cpandl.com",
        IsNullable = false)]
    public class SportGameData
    {
        public List<float> time_left;
        public List<float> time_right;
        public List<float> precision_left;
        public List<float> precision_right;
        public List<int> points_earned_left;
        public List<int> points_earned_right;
        public List<int> round_generated_left;
        public List<int> round_generated_right;
        public List<Difficulty> difficultyLeft;
        public List<Difficulty> difficultyRight;
        public int spawnedObjectCount;
        public int countObjectsHitRightHand;
        public int countObjectsHitLeftHand;
    }

    [XmlRootAttribute("AntizipationTestData", Namespace = "http://www.cpandl.com",
        IsNullable = true)]
    public class AntizipationTestData
    {
        public List<Vector3> positionCubeOriginalLeft;
        public List<Vector3> positionCubeOriginalRight;
        public List<Vector3> positionCubePointedLeft;
        public List<Vector3> positionCubePointedRight;

        public List<Vector3> rotationCubeOriginalLeft;
        public List<Vector3> rotationCubeOriginalRight;
        public List<Vector3> rotationCubePointedLeft;
        public List<Vector3> rotationCubePointedRight;

        public List<float> rotationZOriginalLeft;
        public List<float> rotationZOriginalRight;
        public List<float> rotationZChoosenLeft;
        public List<float> rotationZChoosenRight;
    }

    [Serializable]
    public class TransformData
    {
        public Vector3 Position = Vector3.zero;
        public Vector3 EulerRotation = Vector3.zero;

        // Unity requires a default constructor for serialization
        public TransformData() { }

        public TransformData(Transform transform)
        {
           Position = transform.position;
           EulerRotation = transform.localEulerAngles;
        }

        public void ApplyTo(Transform transform)
        {
            transform.position = Position;
            transform.localEulerAngles = EulerRotation;
        }
    }

    [XmlRootAttribute("BlockGameData", Namespace = "http://www.cpandl.com",
        IsNullable = true)]
    public class BlockGameData
    {
        public List<int> listCubePairCount;
        public List<int> listIterationCount;
        public List<int> listBestPointScoreAllIterations;
        public List<int> listAvgPointScoreAllIterations;
        public List<int> listPointScoreAllIterations;
    }

    public static void SerializeSportGameData(string filename, List<float> time_left, List<float> precision_left, List<int> pointsEarnedLeft, 
        List<float> time_right, List<float> precision_right, List<int> pointsEarnedRight, int spawnedObjectCount, int countHitObjectsRightHand, int countHitObjectsLeftHand,
        List<int> round_generated_left, List<int> round_generated_right, List<Difficulty> difficulty_left, List<Difficulty> difficulty_right)
    {
        int index = 1;
        string filenameComplete = filename + index.ToString();
        while (File.Exists("Assets/xml/SportGame/"+filenameComplete + ".xml"))
        {
            ++index;
            filenameComplete = filename + index.ToString();
            Debug.Log("The file alreday exists.");
        }
        string filenameNew = "Assets/xml/SportGame/" + filenameComplete + ".xml";
        XmlSerializer ser = new XmlSerializer(typeof(SportGameData));
        TextWriter writer = new StreamWriter(filenameNew);

        SportGameData sgd = new SportGameData();
        sgd.time_left = time_left;
        sgd.time_right = time_right;
        sgd.precision_left = precision_left;
        sgd.precision_right = precision_right;
        sgd.points_earned_left = pointsEarnedLeft;
        sgd.points_earned_right = pointsEarnedRight;
        sgd.spawnedObjectCount = spawnedObjectCount;
        sgd.countObjectsHitLeftHand = countHitObjectsLeftHand;
        sgd.countObjectsHitRightHand = countHitObjectsRightHand;
        sgd.round_generated_left = round_generated_left;
        sgd.round_generated_right = round_generated_right;
        sgd.difficultyLeft = difficulty_left;
        sgd.difficultyRight = difficulty_right;

        ser.Serialize(writer, sgd);
        writer.Close();
    }

    public static void SerializeAntizipationTestData(string filename, List<TransformData> originalLeft, List<TransformData> originalRight, List<TransformData> pointedLeft, List<TransformData> pointedRight,
       List<float> rotationZOriginalLeft, List<float> rotationZOriginalRight, List<float> rotationZChoosenLeft, List<float> rotationZChoosenRight)
    {
        int index = 1;
        string filenameComplete = filename + index.ToString();
        while (File.Exists("Assets/xml/AntizipationTest/" + filenameComplete + ".xml"))
        {
            ++index;
            filenameComplete = filename + index.ToString();
            Debug.Log("The file alreday exists.");
        }
        string filenameNew = "Assets/xml/AntizipationTest/" + filenameComplete + ".xml";
        XmlSerializer ser = new XmlSerializer(typeof(AntizipationTestData));
        TextWriter writer = new StreamWriter(filenameNew);

        AntizipationTestData atd = new AntizipationTestData();
        atd.positionCubeOriginalLeft = originalLeft.ConvertAll(new Converter<TransformData, Vector3>(transformToPosition));
        atd.positionCubeOriginalRight = originalRight.ConvertAll(new Converter<TransformData, Vector3>(transformToPosition));
        atd.positionCubePointedLeft = pointedLeft.ConvertAll(new Converter<TransformData, Vector3>(transformToPosition));
        atd.positionCubePointedRight = pointedRight.ConvertAll(new Converter<TransformData, Vector3>(transformToPosition));

        atd.rotationCubeOriginalLeft = originalLeft.ConvertAll(new Converter<TransformData, Vector3>(transformToRotation));
        atd.rotationCubeOriginalRight = originalRight.ConvertAll(new Converter<TransformData, Vector3>(transformToRotation));
        atd.rotationCubePointedLeft = pointedLeft.ConvertAll(new Converter<TransformData, Vector3>(transformToRotation));
        atd.rotationCubePointedRight = pointedRight.ConvertAll(new Converter<TransformData, Vector3>(transformToRotation));


        atd.rotationZOriginalLeft = rotationZOriginalLeft;
        atd.rotationZOriginalRight = rotationZOriginalRight;
        atd.rotationZChoosenLeft = rotationZChoosenLeft;
        atd.rotationZChoosenRight = rotationZChoosenRight;

        ser.Serialize(writer, atd);
        writer.Close();
    }

    public static Vector3 transformToPosition(TransformData t)
    {
        return t.Position;
    }

    public static Vector3 transformToRotation(TransformData t)
    {
        return t.EulerRotation;
    }

    public static void SerializeBlocks(string filename, BlockSequence [] blockSequences, List<int> listCubePairCount, List<int> listIterationCount,
        List<int> listBestPointScore, List<int> listAvgPointScore, List<int> listPointScoreAllIterations)
    {
        int index = 1;
        string filenameComplete = filename + index.ToString();
        while (File.Exists("Assets/xml/BlockData/" + filenameComplete + ".xml")) 
        {
            ++index;
            filenameComplete = filename + index.ToString();
            Debug.Log("The file alreday exists.");
        }
        string filenameNew = "Assets/xml/BlockData/" + filenameComplete + ".xml";
        XmlSerializer ser = new XmlSerializer(typeof(BlockGameData));
        TextWriter writer = new StreamWriter(filenameNew);

        BlockGameData bgd = new BlockGameData();
        bgd.listCubePairCount = listCubePairCount;
        bgd.listIterationCount = listIterationCount;
        bgd.listPointScoreAllIterations = listPointScoreAllIterations;
        bgd.listBestPointScoreAllIterations = listBestPointScore;
        bgd.listAvgPointScoreAllIterations = listAvgPointScore;

        ser.Serialize(writer, bgd);
        writer.Close();
    }
}
