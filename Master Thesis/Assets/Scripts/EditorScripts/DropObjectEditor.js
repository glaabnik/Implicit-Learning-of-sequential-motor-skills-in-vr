class DropObjectEditor extends EditorWindow
{
    // add menu item
    @MenuItem ("Window/Drop Object")
   
    static function Init ()
    {
        // Get existing open window or if none, make a new one:
        var window : DropObjectEditor = EditorWindow.GetWindow(DropObjectEditor);
        window.Show ();
    }
   
    function OnGUI ()
    {
        GUILayout.Label ("Drop Using:", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
       
        if(GUILayout.Button("Bottom"))
        {
            DropObjects("Bottom");
        }
       
        if(GUILayout.Button("Origin"))
        {
            DropObjects("Origin");
        }
       
        if(GUILayout.Button("Center"))
        {
            DropObjects("Center");
        }
       
        GUILayout.EndHorizontal();
    }
   
    function DropObjects(method : String)
    {
        // drop multi-selected objects using the right method
        for(var i : int = 0; i < Selection.transforms.length; i++)
        {
            // get the game object
            var go : GameObject = Selection.transforms[i].gameObject;
           
            // don't think I need to check, but just to be sure...
            if(!go)
            {
                continue;
            }
           
            // get the bounds
            var bounds : Bounds = go.renderer.bounds;
            var hit : RaycastHit;
            var yOffset : float;
       
            // override layer so it doesn't hit itself
            var savedLayer : int = go.layer;
            go.layer = 2; // ignore raycast
            // see if this ray hit something
            if(Physics.Raycast(go.transform.position, -Vector3.up, hit))
            {
                // determine how the y will need to be adjusted
                switch(method)
                {
                    case "Bottom":
                        yOffset = go.transform.position.y - bounds.min.y;
                        break;
                    case "Origin":
                        yOffset = 0.0;
                        break;
                    case "Center":
                        yOffset = bounds.center.y - go.transform.position.y;
                        break;
                }
                go.transform.position = hit.point;
                go.transform.position.y += yOffset;
            }
            // restore layer
            go.layer = savedLayer;
        }
    }
}