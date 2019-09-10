using UnityEngine;
using System.Collections;

public class StatusBarHidden : ScriptableObject
{
	enum Style
	{
		Default = 0,
		Translucent,
		Opaque,
	}

	[SerializeField]
	bool HideStatusBar = false;

	[SerializeField]
	Style style = Style.Default;


	#if UNITY_ANDROID
	void OnEnable ()
	{
		if (HideStatusBar == false)
        {
			switch (style) 
			{
			case Style.Default:
			case Style.Opaque:
				ApplicationChrome.statusBarState = ApplicationChrome.States.Visible;
				break;
			case Style.Translucent:
				ApplicationChrome.statusBarState = ApplicationChrome.States.TranslucentOverContent;
				break;
			}
		} else {
			ApplicationChrome.statusBarState = ApplicationChrome.States.Hidden;
		}
	}
	#endif
}