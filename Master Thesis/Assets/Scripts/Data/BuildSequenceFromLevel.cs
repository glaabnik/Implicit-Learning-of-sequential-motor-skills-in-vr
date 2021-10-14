using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildSequenceFromLevel : MonoBehaviour
{
    public string filename = "nameSavedLevel";
    public bool sortCubesById = false;
    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> rootObjects = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(rootObjects);
        List<GameObject> redCubes = rootObjects.Where(x => x.name.Contains("Red")).ToList();
        List<GameObject> blueCubes = rootObjects.Where(x => x.name.Contains("Blue")).ToList();
        rootObjects.Sort( (obj1, obj2)=>obj1.name.CompareTo(obj2.name));

        if (!sortCubesById)
        {
            foreach (GameObject o in redCubes)
            {
                SpawnedInteractable si = o.GetComponent<SpawnedInteractable>();
                string s = o.name;
                if (s.EndsWith(")"))
                {
                    int index = s.IndexOf("(");
                    string end = s.Substring(index + 1);
                    int index2 = end.IndexOf(")");
                    string number = end.Remove(index2);
                    si.setId(int.Parse(number));
                }
                else si.setId(0);
            }

            foreach (GameObject o in blueCubes)
            {
                SpawnedInteractable si = o.GetComponent<SpawnedInteractable>();
                string s = o.name;
                if (s.EndsWith(")"))
                {
                    int index = s.IndexOf("(");
                    string end = s.Substring(index + 1);
                    int index2 = end.IndexOf(")");
                    string number = end.Remove(index2);
                    si.setId(int.Parse(number));
                }
                else si.setId(0);
            }
        }

        // Sorting of red and blue cubes by their defined ID in SpawnedInteractable
        redCubes.Sort((obj1, obj2) => obj1.GetComponent<SpawnedInteractable>().getId().CompareTo(obj2.GetComponent<SpawnedInteractable>().getId()));
        blueCubes.Sort((obj1, obj2) => obj1.GetComponent<SpawnedInteractable>().getId().CompareTo(obj2.GetComponent<SpawnedInteractable>().getId()));

        List<SphereCoordinates> list = new List<SphereCoordinates>();
        // build list of sphereCoordinates to write in csv-file
        for (int i = 0; i < redCubes.Count; ++i)
        {
            Vector2 res = cartesianToSphereCoordinate(redCubes[i].transform.position);
            Vector2 res2 = cartesianToSphereCoordinate(blueCubes[i].transform.position);
            SphereCoordinates sc = new SphereCoordinates(res[0],res[1],res2[0],res2[1]);
            sc.radius = (redCubes[i].transform.position - new Vector3(0, 0, 0)).magnitude;
            sc.radius2 = (blueCubes[i].transform.position - new Vector3(0, 0f, 0)).magnitude;
            list.Add(sc);
            Debug.Log(redCubes[i].name);
            Debug.Log(blueCubes[i].name);
        }
        LoadFixedSequenceOfSpawns.writeCsvFromCubeLists(ref list, ref redCubes, ref blueCubes, filename);
    }

    private Vector2 cartesianToSphereCoordinate(Vector3 position)
    {
        Vector2 result = new Vector2();
        float radius = position.magnitude;
        /*Debug.Log("Radius: " + radius);
        if(position.x == 0)
        {
            result[0] = Mathf.Rad2Deg * Mathf.Atan(position.z / Mathf.Epsilon);
        }
        else result[0] = Mathf.Atan(position.z / position.x);
        if(position.x < 0)
        {
            result[0] += Mathf.PI;
        }
       result[0] = Mathf.Rad2Deg * result[0];
       result[1] = Mathf.Rad2Deg * Mathf.Asin(position.y - 1.6f / radius);*/
        // working implementation

        result[0] = Mathf.Rad2Deg * Mathf.Atan2(position.z, position.x);
        result[1] = Mathf.Rad2Deg * Mathf.Acos( (position.y -1.6f) / radius);
        return result;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
