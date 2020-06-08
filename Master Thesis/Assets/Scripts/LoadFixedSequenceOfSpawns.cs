using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

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
                    sc.radius = float.Parse(values[2]);
                    sc.phi2 = float.Parse(values[3]);
                    sc.theta2 = float.Parse(values[4]);
                    sc.radius2 = float.Parse(values[5]);
                    list.Add(sc);
                }
            }
        }
    }

    public static void writeCsvFromCubeLists(ref List<SphereCoordinates> list, ref List<GameObject> listCubeRed, ref List<GameObject> listCubeBlue, string filename)
    {
        using (var w = new StreamWriter("Assets/csv/"+filename+".csv"))
        {
            var firstLine = "Phi;Theta;Radius;Phi2;Theta2;Radius2;PositionX;PositionY;PositionZ;PositionX2;PositionY2;PositionZ2;" + 
                            "RotationX;RotationY;RotationZ;RotationX2;RotationY2;RotationZ2;" +
                            "ScaleX;ScaleY;ScaleZ;ScaleX2;ScaleY2;ScaleZ2";
            w.WriteLine(firstLine);
            w.Flush();
            for (int i= 0; i < listCubeRed.Count && i < listCubeBlue.Count; ++i)
            {
                float positionX = listCubeRed[i].transform.position.x;
                float positionY = listCubeRed[i].transform.position.y;
                float positionZ = listCubeRed[i].transform.position.z;

                float positionX2 = listCubeBlue[i].transform.position.x;
                float positionY2 = listCubeBlue[i].transform.position.y;
                float positionZ2 = listCubeBlue[i].transform.position.z;

                float rotationX = listCubeRed[i].transform.rotation.x;
                float rotationY = listCubeRed[i].transform.rotation.y;
                float rotationZ = listCubeRed[i].transform.rotation.z;

                float rotationX2 = listCubeBlue[i].transform.rotation.x;
                float rotationY2 = listCubeBlue[i].transform.rotation.y;
                float rotationZ2 = listCubeBlue[i].transform.rotation.z;

                float scaleX = listCubeRed[i].transform.localScale.x;
                float scaleY = listCubeRed[i].transform.localScale.y;
                float scaleZ = listCubeRed[i].transform.localScale.z;

                float scaleX2 = listCubeBlue[i].transform.localScale.x;
                float scaleY2 = listCubeBlue[i].transform.localScale.y;
                float scaleZ2 = listCubeBlue[i].transform.localScale.z;

                float phi = list[i].phi;
                float theta = list[i].theta;
                float phi2 = list[i].phi2;
                float theta2 = list[i].theta2;
                float radius = list[i].radius;
                float radius2 = list[i].radius2;

                Debug.Log("Phi is: " + phi);
                Debug.Log("Phi2 is: " + phi2);
                Debug.Log("Theta is: " + theta);
                Debug.Log("Theta2 is: " + theta2);

                var line = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19};{20};{21};{22};{23}",phi, theta, radius, phi2, theta2, radius2,
                    positionX, positionY, positionZ, positionX2, positionY2, positionZ2,
                    rotationX, rotationY, rotationZ, rotationX2, rotationY2, rotationZ2,  scaleX, scaleY, scaleZ, scaleX2, scaleY2, scaleZ2);
                w.WriteLine(line);
                w.Flush();
            }
        }
    }

}
