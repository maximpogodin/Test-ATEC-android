using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongClick : MonoBehaviour
{
    ManageWindow mw;
    public bool pointerDown;
    public float pointerDownTimer;
    public float requiredHoldTime;
    public GameObject contextMenu;
    public GameObject panelLock;
    public bool isMenu;

    private void Start()
    {
        mw = GetComponent<ManageWindow>();
    }

    private void Update()
    {
        if (pointerDown)
        {
            pointerDownTimer += Time.deltaTime;
            if (pointerDownTimer >= requiredHoldTime)
            {
                isMenu = true;
                ShowMenu();
                Reset();
            }
        }
    }

    public void Reset()
    {
        pointerDown = false;
        pointerDownTimer = 0;
    }

    public void UpdateItem()//клик по меню Изменить
    {
        HideMenu();
        if (mw.headerWindow.GetComponent<Text>().text == "Выбор курса")
        {
            mw.OpenCreateCourseWindow(1);
            mw.CloseChooseCourseWindow();
        }
        else//иначе выбор пациента
        {
            mw.OpenAddPacientWindow(1);
            mw.CloseChoosePacientWindow();
        }
    }

    public void RemoveItem()//клие по меню Удалить
    {
        HideMenu();
    }

    public void HideMenu()
    {
        contextMenu.SetActive(false);
        contextMenu.transform.position = new Vector2(1000, -500);
        panelLock.SetActive(false);
    }

    public void ShowMenu()
    {
        contextMenu.SetActive(true);
        contextMenu.transform.position = new Vector2(Input.mousePosition.x + 300f, Input.mousePosition.y - 50f);
        panelLock.SetActive(true);
        mw.GetGameObject();
    }

    public void DoIt()
    {
        if (mw.headerWindow.GetComponent<Text>().text == "Выбор курса")
            mw.PointerDownCourse();
        else
            mw.PointerDownPacient();
    }

}