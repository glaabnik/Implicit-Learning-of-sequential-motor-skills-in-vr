using System.Collections;
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

    public static void SerializeSportGameData(string filename, List<float> time_left, List<float> precision_left, List<int> pointsEarnedLeft, 
        List<float> time_right, List<float> precision_right, List<int> pointsEarnedRight, int spawnedObjectCount, int countHitObjectsRightHand, int countHitObjectsLeftHand,
        List<int> round_generated_left, List<int> round_generated_right, List<Difficulty> difficulty_left, List<Difficulty> difficulty_right)
    {
        int index = 1;
        string filenameComplete = filename + index.ToString();
        while (File.Exists("Assets/xml/"+filenameComplete + ".xml"))
        {
            ++index;
            filenameComplete = filename + index.ToString();
            Debug.Log("The file alreday exists.");
        }
        string filenameNew = "Assets/xml/" + filenameComplete + ".xml";
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
}
