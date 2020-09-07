using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Linq;

public class LevelEditorWindow : EditorWindow
{
    public Texture tex1;
    public Texture tex2;
    public Texture tex3;
    public Texture tex4;
    public Texture tex5;
    public Texture tex6;
    public Texture tex7;
    public Texture tex8;

    public Texture2D texBackgroundSelected;
    public Vector3 lookAtPoint = new Vector3(0, 1.6f, 0);

    private static GUIStyle ToggleButtonStyleNormal = null;
    private static GUIStyle ToggleButtonStyleToggled = null;

    bool groupEnabled;
    bool randomizedHeight = false;
    bool randomizedDirection = false;
    bool toggle1, toggle2, toggle3, toggle4;
    bool toggle5, toggle6, toggle7, toggle8;
    float radiusDesired = 3.7f;
    float heightToPlaceCubes = 1.5f;
    float minHeight = 0.5f;
    float maxHeight = 2.0f;

    float rotationZ;
    int buttonSizeDirections = 70;

    static int idCounterRed = 0;
    static int idCounterBlue = 0;

    // Add menu item named "Level Editor Window" to the Window menu
    [MenuItem("Window/Level Editor Window")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(LevelEditorWindow));
    }

    private void deleteAllCubesInLevel()
    {
        Debug.Log("Deletes all Cubes in Level");
        Scene level_editor_scene = SceneManager.GetActiveScene();
        if(level_editor_scene.name.CompareTo("LevelEditor") != 0)
        {
            Debug.Log("Level Editor scene isn`t active. First open the Level Editor scene");
            return;
        }
        foreach(GameObject go in level_editor_scene.GetRootGameObjects())
        {
            if(go.CompareTag("blue") || go.CompareTag("red")) GameObject.DestroyImmediate(go);
        }
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    private void setRotationZ(float n)
    {
        rotationZ = n;
        Debug.Log("Rotation" + n + " was successfully saved!");
    }

    private void applyChangeToRotation()
    {
        Scene level_editor_scene = SceneManager.GetActiveScene();
        if (level_editor_scene.name.CompareTo("LevelEditor") != 0)
        {
            Debug.Log("Level Editor scene isn`t active. First open the Level Editor scene");
            return;
        }
        for (int i = 0; i < Selection.transforms.Length; i++)
        {
            GameObject go = Selection.transforms[i].gameObject;
            if (go.CompareTag("blue") || go.CompareTag("red"))
            {
                go.transform.localEulerAngles = new Vector3(go.transform.localEulerAngles.x, go.transform.localEulerAngles.y, rotationZ);
            }
        }
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    private void applyChangesToSelection()
    {
        /*var transforms = Selection.gameObjects.Select(go => go.transform).ToArray();
        var so = new SerializedObject(transforms);
        // you can Shift+Right Click on property names in the Inspector to see their paths
        if (randomizedHeight)
        {
            so.FindProperty("m_LocalPosition").vector3Value =  new Vector3(go.transform.position.x, Random.Range(minHeight, maxHeight), go.transform.position.z);
        }
        so.FindProperty("m_LocalPosition").vector3Value = Vector3.zero;
        so.ApplyModifiedProperties();*/
        Scene level_editor_scene = SceneManager.GetActiveScene();
        if (level_editor_scene.name.CompareTo("LevelEditor") != 0)
        {
            Debug.Log("Level Editor scene isn`t active. First open the Level Editor scene");
            return;
        }
        for (int i = 0; i < Selection.transforms.Length; i++)
        {
            GameObject go = Selection.transforms[i].gameObject;
            if (go.CompareTag("blue") || go.CompareTag("red"))
            {
                if (randomizedHeight)
                {
                    go.transform.position = new Vector3(go.transform.position.x, Random.Range(minHeight, maxHeight), go.transform.position.z);
                }
                else go.transform.position = new Vector3(go.transform.position.x, heightToPlaceCubes, go.transform.position.z);

                if (randomizedDirection)
                {
                    int rZ = 0;
                    int z = (int)Random.Range(1, 8);
                    if (z == 1) rZ = 0;
                    if (z == 2) rZ = 90;
                    if (z == 3) rZ = 180;
                    if (z == 4) rZ = 270;
                    if (z == 5) rZ = 45;
                    if (z == 6) rZ = 135;
                    if (z == 7) rZ = 225;
                    if (z == 8) rZ = 315;
                    go.transform.localEulerAngles = new Vector3(go.transform.localEulerAngles.x, go.transform.localEulerAngles.y, rZ);
                }
                else go.transform.localEulerAngles = new Vector3(go.transform.localEulerAngles.x, go.transform.localEulerAngles.y, rotationZ);

                transformPositionToMatchRadius(go);
                lookAtCenter(go);
            }
        }
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    private void lookAtCenter(GameObject go)
    {
        float zRotation = go.transform.localEulerAngles.z;
        go.transform.LookAt(lookAtPoint);
        go.transform.localEulerAngles = new Vector3(go.transform.localEulerAngles.x, go.transform.localEulerAngles.y, zRotation);
    }

    private void forceRadiusOnAllCubes()
    {
        Scene level_editor_scene = SceneManager.GetActiveScene();
        if (level_editor_scene.name.CompareTo("LevelEditor") != 0)
        {
            Debug.Log("Level Editor scene isn`t active. First open the Level Editor scene");
            return;
        }
        for (int i = 0; i < Selection.transforms.Length; i++)
        {
            GameObject go = Selection.transforms[i].gameObject;
            if (go.CompareTag("blue") || go.CompareTag("red"))
            {
                transformPositionToMatchRadius(go);
                lookAtCenter(go);
            }
        }
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    private void transformPositionToMatchRadius(GameObject go)
    {
        float actRadius = (go.transform.position - new Vector3(0,0f,0)).magnitude;
        Vector2 res = cartesianToSphereCoordinate(go.transform.position);
        go.transform.position = sphereToCartesianCoordinate(res[0], res[1], actRadius);
    }

    private Vector2 cartesianToSphereCoordinate(Vector3 position)
    {
        Vector2 result = new Vector2();
        float radius = position.magnitude;
        result[0] = Mathf.Rad2Deg * Mathf.Atan2(position.z, position.x);
        result[1] = Mathf.Rad2Deg * Mathf.Acos((position.y - 1.6f) / radius);
        return result;
    }

    private Vector3 sphereToCartesianCoordinate(float phi, float theta, float radius)
    {
        float x = radiusDesired * Mathf.Sin(DegToRadians(theta)) * Mathf.Cos(DegToRadians(phi));
        float z = radiusDesired * Mathf.Sin(DegToRadians(theta)) * Mathf.Sin(DegToRadians(phi));
        float y = radius * Mathf.Cos(DegToRadians(theta)) + lookAtPoint.y;
        return new Vector3(x, y, z);
    }

    private float DegToRadians(float degree)
    {
        return degree * Mathf.PI / 180;
    }

    private void assignUniqueId()
    {
        Scene level_editor_scene = SceneManager.GetActiveScene();
        if (level_editor_scene.name.CompareTo("LevelEditor") != 0)
        {
            Debug.Log("Level Editor scene isn`t active. First open the Level Editor scene");
            return;
        }
        for (int i = 0; i < Selection.transforms.Length; i++)
        {
            GameObject go = Selection.transforms[i].gameObject;
            SpawnedInteractable si = go.GetComponent<SpawnedInteractable>();
            if (si != null)
            {
                SerializedObject so = new SerializedObject(si);
                so.Update();
                SerializedProperty sp = so.FindProperty("id");
                if (go.CompareTag("red")) si.setId(++idCounterRed);
                if (go.CompareTag("blue")) si.setId(++idCounterBlue);
                sp.intValue = si.getId();
                so.ApplyModifiedProperties();
            }
        }
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    private void setSelection(string tag)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        Selection.objects = gameObjects;
    }

    private void updateToggles(ref bool toggleC)
    {
        bool original = toggleC;
        toggle1 = false;
        toggle2 = false;
        toggle3 = false;
        toggle4 = false;
        toggle5 = false;
        toggle6 = false;
        toggle7 = false;
        toggle8 = false;
        toggleC = !original;
    }

    void OnGUI()
    {
        ToggleButtonStyleNormal = "Button";
        ToggleButtonStyleToggled = new GUIStyle(ToggleButtonStyleNormal);
        ToggleButtonStyleToggled.normal.background = texBackgroundSelected;

        heightToPlaceCubes = EditorGUILayout.FloatField("Height to place Cubes", heightToPlaceCubes);
        radiusDesired = EditorGUILayout.FloatField("Radius", radiusDesired);

        GUILayout.Label("Select rotation of Cube", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        if(GUILayout.Button(tex1, toggle1 ? ToggleButtonStyleToggled : ToggleButtonStyleNormal, GUILayout.Width(buttonSizeDirections), GUILayout.Height(buttonSizeDirections)))
        {
            updateToggles(ref toggle1);
            setRotationZ(0);
            applyChangeToRotation();
        }
        if (GUILayout.Button(tex2, toggle2 ? ToggleButtonStyleToggled : ToggleButtonStyleNormal, GUILayout.Width(buttonSizeDirections), GUILayout.Height(buttonSizeDirections)))
        {
            updateToggles(ref toggle2);
            setRotationZ(90);
            applyChangeToRotation();
        }
        if (GUILayout.Button(tex3, toggle3 ? ToggleButtonStyleToggled : ToggleButtonStyleNormal, GUILayout.Width(buttonSizeDirections), GUILayout.Height(buttonSizeDirections)))
        {
            updateToggles(ref toggle3);
            setRotationZ(180);
            applyChangeToRotation();
        }
        if (GUILayout.Button(tex4, toggle4 ? ToggleButtonStyleToggled : ToggleButtonStyleNormal, GUILayout.Width(buttonSizeDirections), GUILayout.Height(buttonSizeDirections)))
        {
            updateToggles(ref toggle4);
            setRotationZ(270);
            applyChangeToRotation();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button(tex5, toggle5 ? ToggleButtonStyleToggled : ToggleButtonStyleNormal, GUILayout.Width(buttonSizeDirections), GUILayout.Height(buttonSizeDirections)))
        {
            updateToggles(ref toggle5);
            setRotationZ(45);
            applyChangeToRotation();
        }
        if (GUILayout.Button(tex6, toggle6 ? ToggleButtonStyleToggled : ToggleButtonStyleNormal, GUILayout.Width(buttonSizeDirections), GUILayout.Height(buttonSizeDirections)))
        {
            updateToggles(ref toggle6);
            setRotationZ(135);
            applyChangeToRotation();
        }
        if (GUILayout.Button(tex7, toggle7 ? ToggleButtonStyleToggled : ToggleButtonStyleNormal, GUILayout.Width(buttonSizeDirections), GUILayout.Height(buttonSizeDirections)))
        {
            updateToggles(ref toggle7);
            setRotationZ(225);
            applyChangeToRotation();
        }
        if (GUILayout.Button(tex8, toggle8 ? ToggleButtonStyleToggled : ToggleButtonStyleNormal, GUILayout.Width(buttonSizeDirections), GUILayout.Height(buttonSizeDirections)))
        {
            updateToggles(ref toggle8);
            setRotationZ(315);
            applyChangeToRotation();
        }
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Select all Red Cubes in Level"))
        {
            setSelection("red");
        }

        if (GUILayout.Button("Select all Blue Cubes in Level"))
        {
            setSelection("blue");
        }

        if (GUILayout.Button("Apply changes to selection"))
        {
            applyChangesToSelection();
        }

        if(GUILayout.Button("Force Radius to cubes"))
        {
            forceRadiusOnAllCubes();
        }

        if(GUILayout.Button("Assign Unique Id"))
        {
            assignUniqueId();
        }

        if (GUILayout.Button("Clear Level"))
        {
            deleteAllCubesInLevel();
        }

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        float originalValue = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 280;
        randomizedHeight = EditorGUILayout.Toggle("Randomize Height for Cube Placement", randomizedHeight);
        EditorGUIUtility.labelWidth = originalValue;
        minHeight = EditorGUILayout.Slider("Minimum Height", minHeight, 0, 2);
        maxHeight = EditorGUILayout.Slider("Maximum Height", maxHeight, 0, 3);
        EditorGUIUtility.labelWidth = 280;
        randomizedDirection = EditorGUILayout.Toggle("Randomize Direction of Arrow", randomizedDirection);
        EditorGUIUtility.labelWidth = originalValue;
        EditorGUILayout.EndToggleGroup();
    }
}
