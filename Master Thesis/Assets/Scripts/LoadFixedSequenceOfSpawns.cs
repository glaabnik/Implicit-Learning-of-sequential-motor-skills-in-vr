using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadFixedSequenceOfSpawns : MonoBehaviour
{
    public string filename;
    private List<SphereCoordinates> list;
    void Start()
    {
        list = new List<SphereCoordinates>();
    }

    public static void loadSpawnSequence(ref List<SphereCoordinates> list, string filename)
    {
        bool firstLine = true;
        using (var reader = new StreamReader("Assets/csv/"+filename+".csv"))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(';');
                if(firstLine)
                {
                    firstLine = false;
                }
                else
                {
                    SphereCoordinates sc = new SphereCoordinates();
                    sc.phi = float.Parse(values[0]);
                    sc.theta = float.Parse(values[1]);
                    sc.phi2 = float.Parse(values[2]);
                    sc.theta2 = float.Parse(values[3]);
                    list.Add(sc);
                }
            }
        }
    }

}
