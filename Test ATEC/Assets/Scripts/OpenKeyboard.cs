using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenKeyboard : MonoBehaviour
{
    public GameObject tbUser;
    public GameObject tbPass;
    public GameObject tbPacientName;
    public GameObject tbBirthdate;
    public GameObject tbDiagnos;
    public GameObject tbCourseName;
    public GameObject tbCourseDate;

    public void OpenTbUser()
    {
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }

    public void OpenTbPass()
    {
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, true);
    }

    public void OpenTbBirthdate()
    {
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.NumberPad);
    }

}
