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
    public GameObject cubeNormalBlue, cubeNormalRed;
    public GameObject cubeDiagonalBlue, cubeDiagonalRed;
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
    int customId = 0;
    float minHeight = 0.5f;
    float maxHeight = 2.0f;

    int rotationZ;
    int buttonSizeDirections = 70;
    int phiRed = -40, phiBlue = 40;
    float phiRedSlider = 0, phiBlueSlider = 50;

    private bool idInitialised = false;
    static int idCounterRed = 0;
    static int idCounterBlue = 0;

    private void initialiseIdCounters()
    {
        Scene level_editor_scene = SceneManager.GetActiveScene();
        if (level_editor_scene.name.CompareTo("LevelEditor") != 0)
        {
            Debug.Log("Level Editor scene isn`t active. First open the Level Editor scene");
            return;
        }
        GameObject[] redCubes = GameObject.FindGameObjectsWithTag("red");
        GameObject[] blueCubes = GameObject.FindGameObjectsWithTag("blue");
        int idMaxRed= 0, idMaxBlue=0;
        foreach(GameObject go in redCubes)
        {
            SpawnedInteractable si = go.GetComponent<SpawnedInteractable>();
            if (si.getId() > idMaxRed) idMaxRed = si.getId();
        }
        foreach (GameObject go in blueCubes)
        {
            SpawnedInteractable si = go.GetComponent<SpawnedInteractable>();
            if (si.getId() > idMaxBlue) idMaxBlue = si.getId();
        }
        idCounterRed = idMaxRed;
        idCounterBlue = idMaxBlue;
        Debug.Log("IdCounterRed: " + idCounterRed);
        Debug.Log("IdCounterBlue: " + idCounterBlue);
        idInitialised = true;
        Debug.Log("Id counters initialised!");
    }

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
        idCounterBlue = 1;
        idCounterRed = 1;
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    private void setRotationZ(int n)
    {
        rotationZ = n;
        Debug.Log("Rotation" + n + " was successfully saved!");
    }

    private void applyChangeToRotation()
    {
        applyChangeToRotation(rotationZ);
    }

    private void applyChangeToRotation(int rotationZ)
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
                SpawnedInteractable si = go.GetComponent<SpawnedInteractable>();
                GameObject diagonalReplacement = go.CompareTag("red") ? cubeDiagonalRed : cubeDiagonalBlue;
                GameObject normalReplacement = go.CompareTag("red") ? cubeNormalRed : cubeNormalBlue;
                if (si.uses5ColliderGroups && (rotationZ == 0 || rotationZ == 90 || rotationZ == 180 || rotationZ == 270))
                {
                    Vector3 old_pos = go.transform.position;
                    Vector3 old_scale = go.transform.localScale;
                    Vector3 old_rotation = go.transform.eulerAngles;
                    GameObject replacement = Instantiate(normalReplacement, old_pos, Quaternion.identity);
                    //Selection.activeObject = PrefabUtility.InstantiatePrefab(normalReplacement);
                    //GameObject replacementTest = Selection.activeGameObject;
                    StageUtility.PlaceGameObjectInCurrentStage(replacement);
                    replacement.transform.localScale = old_scale;
                    replacement.transform.localEulerAngles = new Vector3(old_rotation.x, old_rotation.y, rotationZ);
                    replacement.GetComponent<SpawnedInteractable>().setId(si.getId());
                    Selection.activeGameObject = replacement;
                    GameObject.DestroyImmediate(go);
                }
                else if (!si.uses5ColliderGroups && (rotationZ == 45 || rotationZ == 135 || rotationZ == 225 || rotationZ == 315))
                {
                    Vector3 old_pos = go.transform.position;
                    Vector3 old_scale = go.transform.localScale;
                    Vector3 old_rotation = go.transform.eulerAngles;
                    GameObject replacement = Instantiate(diagonalReplacement, old_pos, Quaternion.identity);
                    StageUtility.PlaceGameObjectInCurrentStage(replacement);
                    replacement.transform.localScale = old_scale;
                    replacement.transform.localEulerAngles = new Vector3(old_rotation.x, old_rotation.y, rotationZ -45);
                    replacement.GetComponent<SpawnedInteractable>().setId(si.getId());
                    Selection.activeGameObject = replacement;
                    Object.DestroyImmediate(go);
                }
                else
                {
                    int diagonalOffset = 0;
                    if (si.uses5ColliderGroups) diagonalOffset = 45;
                    go.transform.localEulerAngles = new Vector3(go.transform.localEulerAngles.x, go.transform.localEulerAngles.y, rotationZ - diagonalOffset);
                }
            }
        }
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    private Vector3 sphereToCartesianCoordinate(float phi, float radius)
    {
        float theta = Mathf.Acos((heightToPlaceCubes - 1.6f) / radius) * Mathf.Rad2Deg;
        float x = radius * Mathf.Sin(DegToRadians(theta)) * Mathf.Cos(DegToRadians(phi));
        float z = radius * Mathf.Sin(DegToRadians(theta)) * Mathf.Sin(DegToRadians(phi));
        float y = heightToPlaceCubes;
        //heightToPlaceCubes = radius * Mathf.Cos(DegToRadians(theta)) + 1.6f;
        return new Vector3(x, y, z);
    }


    private void spawnCube(string color)
    {
        GameObject toSpawn = null;
        Vector3 rotation = new Vector3(0, 0, rotationZ);
        Vector3 pos= new Vector3(0, heightToPlaceCubes, 0);
        if (color == "red")
        {
            pos = sphereToCartesianCoordinate(phiRedSlider, radiusDesired);
            if(rotationZ == 0 || rotationZ == 90 || rotationZ == 180 || rotationZ == 270)
            {
                toSpawn = cubeNormalRed;
            }
            if(rotationZ == 45 || rotationZ == 135 || rotationZ == 225 || rotationZ == 315)
            {
                toSpawn = cubeDiagonalRed;
                rotation = new Vector3(0, 0, rotationZ - 45);
            }
        }
        if(color == "blue")
        {
            pos = sphereToCartesianCoordinate(phiBlueSlider, radiusDesired);
            if (rotationZ == 0 || rotationZ == 90 || rotationZ == 180 || rotationZ == 270)
            {
                toSpawn = cubeNormalBlue;
            }
            if (rotationZ == 45 || rotationZ == 135 || rotationZ == 225 || rotationZ == 315)
            {
                toSpawn = cubeDiagonalBlue;
                rotation = new Vector3(0, 0, rotationZ - 45);
            }
        }
        GameObject newSpawn = Instantiate(toSpawn, pos, Quaternion.identity);
        newSpawn.transform.localEulerAngles = rotation;
        StageUtility.PlaceGameObjectInCurrentStage(newSpawn);
        Selection.activeGameObject = newSpawn;
        assignUniqueId(newSpawn);
        lookAtCenter(newSpawn);
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
                    applyChangeToRotation(rZ);
                }
                else if (oneToggleActive()) applyChangeToRotation();

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

    private void assignUniqueId(GameObject go)
    {
        if(go.CompareTag("red"))
        {
            if (!checkIsCustomIdAvailable(go, idCounterRed + 1)) idInitialised = false;
        }
        if(go.CompareTag("blue"))
        {
            if (!checkIsCustomIdAvailable(go, idCounterBlue + 1)) idInitialised = false;
        }
        assignUniqueId();
    }

    private void assignUniqueId()
    {
        Scene level_editor_scene = SceneManager.GetActiveScene();
        if (level_editor_scene.name.CompareTo("LevelEditor") != 0)
        {
            Debug.Log("Level Editor scene isn`t active. First open the Level Editor scene");
            return;
        }
        if(!idInitialised)
        {
            initialiseIdCounters();
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

    private bool checkIsCustomIdAvailable(GameObject gameObject, int newId)
    {
        Scene level_editor_scene = SceneManager.GetActiveScene();
        if (level_editor_scene.name.CompareTo("LevelEditor") != 0)
        {
            Debug.Log("Level Editor scene isn`t active. First open the Level Editor scene");
            return false;
        }
        GameObject[] matchingCubes = GameObject.FindGameObjectsWithTag(gameObject.tag);
        foreach (GameObject go in matchingCubes)
        {
            SpawnedInteractable si = go.GetComponent<SpawnedInteractable>();
            if (si.getId() == newId) return false;
        }
        return true;
    }

    private GameObject getGameObjectWithCustomId(GameObject gameObject, int customId)
    {
        Scene level_editor_scene = SceneManager.GetActiveScene();
        if (level_editor_scene.name.CompareTo("LevelEditor") != 0)
        {
            Debug.Log("Level Editor scene isn`t active. First open the Level Editor scene");
            return null;
        }
        GameObject[] matchingCubes = GameObject.FindGameObjectsWithTag(gameObject.tag);
        foreach (GameObject go in matchingCubes)
        {
            SpawnedInteractable si = go.GetComponent<SpawnedInteractable>();
            if (si.getId() == customId) return go;
        }
        return null;
    }

    private void selectOtherColoredMatchingCube()
    {
        Scene level_editor_scene = SceneManager.GetActiveScene();
        if (level_editor_scene.name.CompareTo("LevelEditor") != 0)
        {
            Debug.Log("Level Editor scene isn`t active. First open the Level Editor scene");
            return;
        }
        GameObject active = Selection.activeGameObject;
        string tag = active.tag;
        int id = active.GetComponent<SpawnedInteractable>().getId();
        string otherTag = tag.Equals("red") ? "blue" : "red";
        GameObject[] matchingCubes = GameObject.FindGameObjectsWithTag(otherTag);
        GameObject match = null;
        foreach (GameObject go in matchingCubes)
        {
            SpawnedInteractable si = go.GetComponent<SpawnedInteractable>();
            if (si.getId() == id) match = go;
        }
        if(match != null)
        {
            GameObject[] arr = new GameObject[2];
            arr[0] = active;
            arr[1] = match;
            Selection.objects = arr;
        }
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

    private bool oneToggleActive()
    {
        return toggle1 || toggle2 || toggle3 || toggle4 || toggle5 || toggle6 || toggle7 || toggle8;
    }

    void OnGUI()
    {
        ToggleButtonStyleNormal = "Button";
        ToggleButtonStyleToggled = new GUIStyle(ToggleButtonStyleNormal);
        ToggleButtonStyleToggled.normal.background = texBackgroundSelected;
        //phiRed = EditorGUILayout.IntField("Angle to spawn cube: ", phiRed);
        phiRedSlider = EditorGUILayout.Slider("Angle to spawn red cube", phiRedSlider, 0, 360);
        if (GUILayout.Button("Spawn Red Cube"))
        {
            spawnCube("red");
        }
        //phiBlue = EditorGUILayout.IntField("Angle to spawn cube: ", phiBlue);
        phiBlueSlider = EditorGUILayout.Slider("Angle to spawn blue cube", phiBlueSlider, 0, 360);
        if (GUILayout.Button("Spawn Blue Cube"))
        {
            spawnCube("blue");
        }

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

        if(GUILayout.Button("Select matching other colored Cube with Same Id"))
        {
            selectOtherColoredMatchingCube();
        }

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

        GUILayout.BeginHorizontal();

        customId = EditorGUILayout.IntField("Enter Custom Id: ", customId);
        if(GUILayout.Button("Assign Custom Id"))
        {
            if(Selection.gameObjects.Length == 1)
            {
                if(checkIsCustomIdAvailable(Selection.activeGameObject,customId))
                {
                    Selection.activeGameObject.GetComponent<SpawnedInteractable>().setId(customId);
                }
                else
                {
                    bool result = EditorUtility.DisplayDialog("Warning", "Your Custom Id is already used by another cube", "Assign this id anyway (With Id Swap)", "Cancel");
                    if(result)
                    {
                        int oldId = Selection.activeGameObject.GetComponent<SpawnedInteractable>().getId();
                        GameObject go = getGameObjectWithCustomId(Selection.activeGameObject, customId);
                        go.GetComponent<SpawnedInteractable>().setId(oldId);
                        Selection.activeGameObject.GetComponent<SpawnedInteractable>().setId(customId);
                    }
                }
            }
        }

        GUILayout.EndHorizontal();

        if (GUILayout.Button("Assign Generated Id"))
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
