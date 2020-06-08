using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DropObjectEditor : EditorWindow
{
    bool AlignNormals;
    Vector3 UpVector = new Vector3(0, 90, 0);
    // add menu item
    [MenuItem("Window/Drop Object")]
    // Start is called before the first frame update
    static void Awake()
    {
        EditorWindow.GetWindow<DropObjectEditor>().Show();                         // Get existing open window or if none, make a new one
    }

    /*void Start()
    {
        DropObjectEditor window = (DropObjectEditor) EditorWindow.GetWindow(typeof(DropObjectEditor), true, "Drop Object Editor Window");
        window.Show();
    }*/

    void OnGUI()
    {
        GUILayout.Label("Drop Using:", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Bottom"))
        {
            DropObjects("Bottom");
        }

        if (GUILayout.Button("Origin"))
        {
            DropObjects("Origin");
        }

        if (GUILayout.Button("Center"))
        {
            DropObjects("Center");
        }

        GUILayout.EndHorizontal();

        AlignNormals = EditorGUILayout.ToggleLeft("Align Normals", AlignNormals);  // toggle to align the object with the normal direction of the surface
        if (AlignNormals)
        {
            EditorGUILayout.BeginHorizontal();
            UpVector = EditorGUILayout.Vector3Field("Up Vector", UpVector);          // Vector3 helping to specify the Up vector of the object
                                                                                     // default has 90° on the Y axis, this is because by default
                                                                                     // the objects I import have a rotation.
                                                                                     // If anyone has a better way to do this I'd be happy
                                                                                     // to see a better solution!
            GUILayout.EndHorizontal();
        }
    }

    void DropObjects(string method)
    {
        // drop multi-selected objects using the right method
        for (int i =  0; i < Selection.transforms.Length; i++)
        {
            // get the game object
            GameObject go = Selection.transforms[i].gameObject;

            // don't think I need to check, but just to be sure...
            if (!go)
            {
                continue;
            }

            // get the bounds
            Renderer r = go.GetComponent<Renderer>();
            Bounds bounds = r.bounds;
            RaycastHit hit;
            float yOffset = 0;

            // override layer so it doesn't hit itself
            int savedLayer = go.layer;
            go.layer = 2; // ignore raycast
            // see if this ray hit something
            if (Physics.Raycast(go.transform.position, -Vector3.up, out hit, 30.0f))
            {
                // determine how the y will need to be adjusted
                switch (method)
                {
                    case "Bottom":
                        yOffset = go.transform.position.y - bounds.min.y;
                        break;
                    case "Origin":
                        yOffset = 0.0f;
                        break;
                    case "Center":
                        yOffset = bounds.center.y - go.transform.position.y;
                        break;
                }
                if (AlignNormals)                                                   // if "Align Normals" is checked, set the gameobject's rotation
                                                                                    // to match the raycast's hit position's normal, and add the specified offset.
                {
                    go.transform.up = hit.normal + UpVector;
                }
                go.transform.position = new Vector3(hit.point.x, hit.point.y + yOffset, hit.point.z);
        }
            // restore layer
            go.layer = savedLayer;
        }
    }
}
