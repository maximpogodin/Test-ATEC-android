using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartApp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;//60 fps
        // Toggles the dimmed out state (where status/navigation content is darker)
        ApplicationChrome.dimmed = false;

        // Makes the status bar and navigation bar visible (default)
        ApplicationChrome.statusBarState = ApplicationChrome.navigationBarState = ApplicationChrome.States.Visible;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
