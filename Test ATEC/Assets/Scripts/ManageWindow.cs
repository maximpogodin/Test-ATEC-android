using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System;
using System.Globalization;

public class ManageWindow : MonoBehaviour
{
    public GameObject[] buttonsDelete;
    public Dropdown prefabDrop;
    public List<int> pointsList = new List<int>();
    public int maxPoints;
    public WindowGraph wg;
    public Histogram h;
    public GameObject graph;
    public GameObject headerWindow;
    string nameWindow;
    public GameObject mainWindow;
    public GameObject chooseCourseWindow;
    public GameObject choosePacientWindow;
    public GameObject chooseLessonWindow;
    public GameObject addPacientWindow;
    public GameObject appBar;
    public GameObject createCourseWindow;
    public GameObject testWindow;
    public GameObject resultWindow;
    public GameObject settingsWindow;
    public GameObject statWindow;
    public GameObject errorWindow;
    public Text errorText;
    public List<GameObject> queueWindow = new List<GameObject>();
    bool isBack = true;
    public GameObject panelStatLesson;
    public GameObject panelStatPacient;
    public GameObject panelStatCourse;
    public GameObject courseItem;
    public GameObject pacientItem;
    public GameObject lessonItem;
    public GameObject listCourses;
    public GameObject listPacients;
    public GameObject listLessons;
    public GameObject panelYesNo;

    public InputField nameCourse;
    public Text dateCourse;

    public InputField namePacient;
    public Text birthdatePacient;
    public InputField diagnosPacient;
    public Text infoPacient;
    int countLessons;
    public string userName;
    string pacientName;
    string courseName;
    string lessonName;

    public GameObject text1;
    public GameObject text2;
    public GameObject text3;

    public Toggle[] sectionButtons;

    public GameObject clickedGameObject;

    SqliteDataReader reader;
    LoginRegistration lr;

    public int idUser; public int idCourse; public int idPacient; public int idLesson; public int idTest;
    public int[] section = new int[4];
    public int s1 = 0, s2 = 0, s3 = 0, s4 = 0;

    public List<string> listQuestion = new List<string>();
    public List<int> listIds = new List<int>();
    public List<int> idsAnswerVariants = new List<int>();
    public List<string> listAnswerVariants = new List<string>();
    public List<int> listPoints = new List<int>();
    public bool isTest;
    public List<int> idsQuestion = new List<int>();

    public List<int> resultQuestion = new List<int>();//список для данных вопросов
    public List<int> resultAnswers = new List<int>();//список для данных ответов
    public List<int> resultPoints = new List<int>();//список для полученных баллов

    public Dropdown[] dropBirthdate;
    Test t;
    public bool YesNoIsShow;
    public Animator barAnim;

    //первый раздел 14 вопросов, максимум за вопрос - 2 балла, всего 28 баллов.
    //второй раздел 20 вопросов, максимум за вопрос - 2 балла, всего 40 баллов.
    //третий раздел 18 вопросов, максимум за вопрос - 2 балла, всего 36 баллов.
    //четвертый раздел 25 вопросов, максимум за вопрос - 3 балла, всего 75 баллов.

    void Start()
    {
        Screen.fullScreen = false;
        YesNoIsShow = false;
        t = GetComponent<Test>();
        isTest = false;
        queueWindow.Add(mainWindow);
        lr = GetComponent<LoginRegistration>();
        DropdownYearLoad();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isTest)//если мы не в тесте
            {
                Back();
            }
        }
    }

    public void Back()//вернуться назад
    {
        if (!YesNoIsShow)
        {
            if (queueWindow.Count <= 2)
                Exit();
            queueWindow[queueWindow.Count - 1].GetComponent<Animator>().SetInteger("Open", 1);//закрыть текущее окно
            queueWindow[queueWindow.Count - 2].GetComponent<Animator>().SetInteger("Open", 2);//открыть предыдущее окно
            nameWindow = queueWindow[queueWindow.Count - 2].name;
            Debug.Log("Окно: " + nameWindow);
            queueWindow.RemoveAt(queueWindow.Count - 1);//удаляем из списка настоящее открытое окно
            OpenWindow(nameWindow);//открываем окно с именем
            if (statWindow.GetComponent<Animator>().GetInteger("Open") == 1)
                wg.CloseGraph();
        }
    }
    public void OpenWindow(string name)
    {
        switch (name)
        {
            case "ChooseCourseWindow":
                HideButtonDelete();
                headerWindow.GetComponent<Text>().text = "Выбор курса";
                BarTextAnimation();
                appBar.transform.Find("Bar").transform.Find("PanelLayer").transform.Find("ArrowBack").gameObject.SetActive(false);
                appBar.transform.Find("Bar").transform.Find("PanelLayer").transform.Find("Spinny Arrow Button").gameObject.SetActive(true);
                break;
            case "ChoosePacientWindow":
                HideButtonDelete();
                headerWindow.GetComponent<Text>().text = "Выбор пациента";
                BarTextAnimation();
                break;
            case "ChooseLessonWindow":
                HideButtonDelete();
                headerWindow.GetComponent<Text>().text = "Выбор тестирования";
                BarTextAnimation();
                if (panelStatCourse.transform.Find("panelTexts").transform.childCount > 3)
                {
                    for (int i = 3; i < panelStatCourse.transform.Find("panelTexts").transform.childCount; i++)//удаляем все предыдущие drop листы
                    {
                        Destroy(panelStatLesson.transform.Find("panelTexts").transform.GetChild(i).gameObject);
                    }
                }
                break; 
        }
    }//установка заголовка по системной кнопке назад

    public void BarTextAnimation()//анимация текста в баре
    {
        barAnim.Play("BarTextAnimationanim", -1, 0f);
    }


    public void CloseMainWindow()//закрыть окно авторизации
    {
        mainWindow.GetComponent<Animator>().SetInteger("Open", 1);
        OpenAppBar();
    }

    public void OpenMainWindow()//открыть окно авторизации
    {
        mainWindow.GetComponent<Animator>().SetInteger("Open", 2);
        CloseAppBar();
    }

    public void CloseChooseCourseWindow()//закрыть окно выбора курсов
    {
        chooseCourseWindow.GetComponent<Animator>().SetInteger("Open", 1);
        appBar.transform.Find("Bar").transform.Find("PanelLayer").transform.Find("ArrowBack").gameObject.SetActive(true);
        appBar.transform.Find("Bar").transform.Find("PanelLayer").transform.Find("Spinny Arrow Button").gameObject.SetActive(false);
    }

    public void OpenChooseCourseWindow()//открыть окно выбора курсов
    {
        headerWindow.GetComponent<Text>().text = "Выбор курса";
        BarTextAnimation();
        queueWindow.Add(chooseCourseWindow);
        chooseCourseWindow.GetComponent<Animator>().SetInteger("Open", 2);

        //if (panelStatCourse.transform.Find("panelTexts").transform.childCount > 5)
        //{
        //    for (int i = 5; i < panelStatCourse.transform.Find("panelTexts").transform.childCount; i++)//удаляем все предыдущие drop листы
        //    {
        //        Destroy(panelStatCourse.transform.Find("panelTexts").transform.GetChild(i).gameObject);
        //    }
        //}
        SelectCourses();
    }

    public void CloseChoosePacientWindow()//закрыть окно выбора пациентов
    {
        choosePacientWindow.GetComponent<Animator>().SetInteger("Open", 1);
    }

    public void OpenChoosePacientWindow()//открыть окно выбора пациентов
    {
        SelectPacients();
        headerWindow.GetComponent<Text>().text = "Выбор пациента";
        BarTextAnimation();
        queueWindow.Add(choosePacientWindow);
        choosePacientWindow.GetComponent<Animator>().SetInteger("Open", 2);
    }

    public void CloseChooseLessonWindow()//закрыть окно выбора занятий
    {
        chooseLessonWindow.GetComponent<Animator>().SetInteger("Open", 1);
    }

    public void OpenChooseLessonWindow()//открыть окно выбора занятий
    {
        headerWindow.GetComponent<Text>().text = "Выбор тестирования";
        BarTextAnimation();
        if (!isTest)
            queueWindow.Add(chooseLessonWindow);
        chooseLessonWindow.GetComponent<Animator>().SetInteger("Open", 2);
        SelectLessons();
    }

    public void CloseAddPacientWindow()//закрыть окно добавления пациента
    {
        addPacientWindow.GetComponent<Animator>().SetInteger("Open", 1);
    }
    public void OpenAddPacientWindow(int isEdit)//открыть окно добавления пациента
    {
        if (isEdit == 0)
        {
            headerWindow.GetComponent<Text>().text = "Новый пациент";
            BarTextAnimation();
            queueWindow.Add(addPacientWindow);
            addPacientWindow.GetComponent<Animator>().SetInteger("Open", 2);
            addPacientWindow.transform.Find("panelInfoAboutPacient").transform.Find("btnCreatePacient").transform.Find("Button Layer").transform.Find("Button Text").GetComponent<Text>().text = "Добавить";
            addPacientWindow.transform.Find("panelInfoAboutPacient").transform.Find("tbNamePacient").GetComponent<InputField>().text = "";
            dropBirthdate[0].value = 0;
            dropBirthdate[1].value = 0;
            dropBirthdate[2].value = 81;
            addPacientWindow.transform.Find("panelInfoAboutPacient").transform.Find("tbDiagnos").GetComponent<InputField>().text = "";
        }
        else//если режим редактирования
        {
            headerWindow.GetComponent<Text>().text = "Изменение пациента";
            BarTextAnimation();
            queueWindow.Add(addPacientWindow);
            addPacientWindow.GetComponent<Animator>().SetInteger("Open", 2);
            addPacientWindow.transform.Find("panelInfoAboutPacient").transform.Find("btnCreatePacient").transform.Find("Button Layer").transform.Find("Button Text").GetComponent<Text>().text = "Изменить";
            //получаем инфу о пациенте для изменения
            Database db = GetComponent<Database>();
            GetPacientName();
            idPacient = db.GetId("SELECT pacients.idPacient FROM pacients WHERE pacients.fullname = '" + pacientName + "';");
            db.OpenDataBase("testDB.db");
            string query = "SELECT " +
                "pacients.fullname, " +
                "pacients.birthdate, " +
                "pacients.diagnos " +
                "FROM pacients WHERE pacients.idPacient = '" + idPacient.ToString() + "';";
            reader = db.SelectQuery(query);
            while (reader.Read())
            {
                addPacientWindow.transform.Find("panelInfoAboutPacient").transform.Find("tbNamePacient").GetComponent<InputField>().text = reader[0].ToString();
                dropBirthdate[0].captionText.text = reader[1].ToString().Substring(0, 2);
                dropBirthdate[1].captionText.text = reader[1].ToString().Substring(3, 2);
                dropBirthdate[2].captionText.text = reader[1].ToString().Substring(6, 4);
                addPacientWindow.transform.Find("panelInfoAboutPacient").transform.Find("tbDiagnos").GetComponent<InputField>().text = reader[2].ToString();
            }
            db.CloseDataBase();
            reader.Close();
            reader = null;
        }
    }

    public void CloseCreateCourseWindow()//закрыть окно создания курса
    {
        createCourseWindow.GetComponent<Animator>().SetInteger("Open", 1);
    }
    public void OpenCreateCourseWindow(int isEdit)//открыть окно создания курс
    {
        if (isEdit == 0)//если режим добавления курса
        {
            headerWindow.GetComponent<Text>().text = "Новый курс";
            BarTextAnimation();
            queueWindow.Add(createCourseWindow);
            createCourseWindow.GetComponent<Animator>().SetInteger("Open", 2);
            createCourseWindow.transform.Find("panelChoose").transform.Find("panelChecks").gameObject.SetActive(true);
            createCourseWindow.transform.Find("panelChoose").transform.Find("textInfo1").gameObject.SetActive(true);
            createCourseWindow.transform.Find("panelChoose").transform.Find("btnCreateCourse").transform.Find("Button Layer").transform.Find("Button Text").GetComponent<Text>().text = "Создать";
            createCourseWindow.transform.Find("panelChoose").transform.Find("tbNameCourse").GetComponent<InputField>().text = "";
            for (int i = 0; i < sectionButtons.Length; i++)
            {
                sectionButtons[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
                sectionButtons[i].GetComponent<Toggle>().isOn = false;
            }
        }
        else//иначе если режим изменения курса
        {
            headerWindow.GetComponent<Text>().text = "Изменение курса";
            BarTextAnimation();
            queueWindow.Add(createCourseWindow);
            createCourseWindow.GetComponent<Animator>().SetInteger("Open", 2);
            createCourseWindow.transform.Find("panelChoose").transform.Find("panelChecks").gameObject.SetActive(false);
            createCourseWindow.transform.Find("panelChoose").transform.Find("textInfo1").gameObject.SetActive(false);
            createCourseWindow.transform.Find("panelChoose").transform.Find("btnCreateCourse").transform.Find("Button Layer").transform.Find("Button Text").GetComponent<Text>().text = "Изменить";

            //получим название выбранного курса для изменения
            Database db = GetComponent<Database>();

            idCourse = db.GetId("SELECT courses.idCourse FROM courses WHERE " +
                    "courses.time = '" + clickedGameObject.transform.Find("Time").GetComponent<Text>().text + "' " +
                    "AND courses.date = '" + clickedGameObject.transform.Find("Date").GetComponent<Text>().text + "';");

            string query = "SELECT courses.name FROM courses WHERE courses.idCourse = '" + idCourse.ToString() + "'";
            db.OpenDataBase("testDB.db");
            reader = db.SelectQuery(query);
            while (reader.Read())
            {
                createCourseWindow.transform.Find("panelChoose").transform.Find("tbNameCourse").GetComponent<InputField>().text = reader[0].ToString();
            }
            db.CloseDataBase();
            reader.Close();
            reader = null;

        }
    }

    public void CloseTestWindow()//закрыть окно теста
    {
        testWindow.GetComponent<Animator>().SetInteger("Open", 1);
        if (isTest)
            queueWindow.RemoveAt(queueWindow.Count - 1);
    }

    public void CheckIfTestDone()
    {
        Database db = GetComponent<Database>();

        idLesson = db.GetId("SELECT idLesson FROM lessons WHERE idPacient = '" + idPacient.ToString() + "' AND name = '" + lessonName + "';");
        Debug.Log("id занятия = " + idLesson);
        db.OpenDataBase("testDB.db");
        string query = "SELECT idTest FROM tests WHERE idLesson = '" + idLesson.ToString() + "';";
        reader = db.SelectQuery(query);
        if (!reader.HasRows)
        {
            db.CloseDataBase();
            reader.Close();
            reader = null;
            OpenTestWindow();
        }
        else
        {
            db.CloseDataBase();
            reader.Close();
            reader = null;
            //если тест уже был пройден то переходим в результаты теста
            OpenStatLessonWindow();
            CloseChooseLessonWindow();
        }

    }
    public void OpenTestWindow()//открыть окно теста
    {
        CloseChooseLessonWindow();
        headerWindow.GetComponent<Text>().text = "Тест";
        BarTextAnimation();
        queueWindow.Add(testWindow);
        testWindow.GetComponent<Animator>().SetInteger("Open", 2);
        BuildListQuestions();//открываем тест и строим вопросы
    }

    public void BuildListQuestions()
    {
        
        List<int> idsQuestionBackUp = new List<int>();
        idsQuestionBackUp.AddRange(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14,
                                               1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                                               1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18,
                                               1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25});
        listQuestion.Clear();
        listIds.Clear();
        idsAnswerVariants.Clear();
        listAnswerVariants.Clear();
        listPoints.Clear();
        idsQuestion.Clear();
        idsQuestion = idsQuestionBackUp;//запомним id вопросов для каждого раздела

        string query = "SELECT * FROM questions";
        Database db = GetComponent<Database>();
        db.OpenDataBase("testDB.db");
        reader = db.SelectQuery(query);
        while (reader.Read())
        {
            listQuestion.Add(reader[1].ToString());
            listIds.Add(reader.GetInt32(0));
        }
        db.CloseDataBase();
        reader.Close();
        reader = null;

        if (section[3] == 0)
        {
            listQuestion.RemoveRange(52, 25);
            listIds.RemoveRange(52, 25);
            idsQuestion.RemoveRange(52, 25);
        }
        if (section[2] == 0)
        {
            listQuestion.RemoveRange(34, 18);
            listIds.RemoveRange(34, 18);
            idsQuestion.RemoveRange(34, 18);
        }
        if (section[1] == 0)
        {
            listQuestion.RemoveRange(14, 20);
            listIds.RemoveRange(14, 20);
            idsQuestion.RemoveRange(14, 20);
        }
        if (section[0] == 0)
        {
            listQuestion.RemoveRange(0, 14);
            listIds.RemoveRange(0, 14);
            idsQuestion.RemoveRange(0, 14);
        }

        db.OpenDataBase("testDB.db");
        query = "SELECT * FROM answervariants";
        reader = db.SelectQuery(query);
        while (reader.Read())
        {
            idsAnswerVariants.Add(reader.GetInt32(0));
            listAnswerVariants.Add(reader[1].ToString());
            listPoints.Add(reader.GetInt32(2));
        }//заполнили списки вариантов ответа, баллы и их id
        db.CloseDataBase();
        reader.Close();
        reader = null;

        t.i = 0;
        t.idsChousenAnswer.Clear();
        t.idsChousenQuestion.Clear();
        t.ShowQuestionAndAnswer();
        isTest = true;
    }

    public void CloseResultWindow()//закрыть окно результатов
    {
        resultWindow.GetComponent<Animator>().SetInteger("Open", 1);
    }
    public void OpenResultWindow()//открыть окно результатов
    {
        headerWindow.GetComponent<Text>().text = "Результаты тестирования";
        BarTextAnimation();
        resultWindow.GetComponent<Animator>().SetInteger("Open", 2);
        appBar.transform.Find("Bar").transform.Find("PanelLayer").transform.Find("ArrowBack").gameObject.SetActive(false);
        appBar.transform.Find("Bar").transform.Find("PanelLayer").transform.Find("Spinny Arrow Button").gameObject.SetActive(true);
    }

    public void CloseAppBar()//закрыть бар
    {
        appBar.GetComponent<Animator>().SetInteger("Open", 1);
    }
    public void OpenAppBar()//открыть бар
    {
        appBar.GetComponent<Animator>().SetInteger("Open", 2);
    }

    public void CloseSettingsWindow()//закрыть окно настроек
    {
        settingsWindow.GetComponent<Animator>().SetInteger("Open", 1);
    }
    public void OpenSettingsWindow()//открыть окно настроек
    {
        headerWindow.GetComponent<Text>().text = "Настройки";
        BarTextAnimation();
        queueWindow.Add(settingsWindow);
        settingsWindow.GetComponent<Animator>().SetInteger("Open", 2);
    }

    public void CloseStatWindow()//закрыть окно статистики
    {
        statWindow.GetComponent<Animator>().SetInteger("Open", 1);
    }
    public void OpenStatLessonWindow()//открыть окно статистики занятия
    {
        int idLessonBefore = 0;
        headerWindow.GetComponent<Text>().text = "Статистика тестирования";
        BarTextAnimation();
        queueWindow.Add(statWindow);
        string name = "";
        panelStatLesson.SetActive(true);
        panelStatPacient.SetActive(false);
        panelStatCourse.SetActive(false);
        statWindow.GetComponent<Animator>().SetInteger("Open", 2);
        name = clickedGameObject.transform.Find("Name").GetComponent<Text>().text;
        Database db = GetComponent<Database>();
        idLesson = db.GetId("SELECT " +
            "lessons.IdLesson, " +
            "pacients.IdPacient, " +
            "lessons.Name " +
            "FROM pacients INNER JOIN lessons ON pacients.IdPacient = lessons.IdPacient " +
            "WHERE pacients.IdPacient = '" + idPacient.ToString() + "' AND lessons.Name = '" + name + "';");
        Debug.Log(name + ", " + idLesson.ToString());
        idTest = db.GetId("SELECT idTest FROM tests WHERE idLesson = '" + idLesson.ToString() + "';");//получили id пройденного теста
        Debug.Log("idTest = " + idTest.ToString());

        #region считаем стастистику теста After
        resultQuestion.Clear();
        resultAnswers.Clear();
        resultPoints.Clear();
        //получаем статистику по тестированию
        //получаем список вопросов
        string query = "SELECT idQuestion FROM answers WHERE idTest = '" + idTest.ToString() + "';";
        db.OpenDataBase("testDB.db");
        reader = db.SelectQuery(query);
        while (reader.Read())
        {
            resultQuestion.Add(reader.GetInt32(0));
        }
        db.CloseDataBase();
        reader.Close();
        reader = null;

        //получаем список ответов и баллов
        query = "SELECT " +
            "answers.idAnswer, " +
            "answervariants.points " +
            "FROM answervariants INNER JOIN answers ON answervariants.idAnswer = answers.idAnswer " +
            "WHERE answers.idTest = '" + idTest.ToString() + "';";
        db.OpenDataBase("testDB.db");
        reader = db.SelectQuery(query);
        while (reader.Read())
        {
            resultAnswers.Add(reader.GetInt32(0));
            resultPoints.Add(reader.GetInt32(1));
        }
        db.CloseDataBase();
        reader.Close();
        reader = null;

        string after = "";
        int[] sumPoints = new int[4];

        if (resultQuestion.Count != 0)
        {
            //считаем баллы
            for (int i = resultPoints.Count - 1; i >= 0; i--)//считаем баллы начиная с конца, то есть 4 раздел
            {
                if (resultQuestion[i] >= 52 && resultQuestion[i] <= 77)
                    sumPoints[3] += resultPoints[i];
                else if (resultQuestion[i] >= 35 && resultQuestion[i] <= 51)
                    sumPoints[2] += resultPoints[i];
                else if (resultQuestion[i] >= 15 && resultQuestion[i] <= 34)
                    sumPoints[1] += resultPoints[i];
                else
                    sumPoints[0] += resultPoints[i];
            }

            if (section[0] == 1)
                after += "Речь/Язык/Коммуникативные навыки: " + sumPoints[0].ToString() + "\n";
            if (section[1] == 1)
                after += "Социализация: " + sumPoints[1].ToString() + "\n";
            if (section[2] == 1)
                after += "Сенсорные навыки/Познавательные способности: " + sumPoints[2].ToString() + "\n";
            if (section[3] == 1)
                after += "Здоровье/Физическое развитие/Поведение: " + sumPoints[3].ToString() + "\n";
            panelStatLesson.transform.Find("panelTexts").transform.Find("textAfter").GetComponent<Text>().text = "Результат после:\n" + after;
        }
        else
        {
            panelStatLesson.transform.Find("panelTexts").transform.Find("textAfter").GetComponent<Text>().text = "Результат после:\nТест еще не пройден\n";
            panelStatLesson.transform.Find("panelTexts").transform.Find("testDynamic").GetComponent<Text>().text = "Динамика:\nНет информации";
        }
        #endregion

        #region считаем статистику теста Before
        //выберем теперь статистику предыдущего тестирования
        query = "SELECT idLesson FROM lessons WHERE idPacient = '" + idPacient.ToString() + "' AND idLesson < '" + idLesson.ToString() + "' ORDER BY idLesson DESC LIMIT 1;";
        db.OpenDataBase("testDB.db");
        reader = db.SelectQuery(query);
        if (!reader.HasRows)
        {
            panelStatLesson.transform.Find("panelTexts").transform.Find("textBefore").GetComponent<Text>().text = "Результат до:\nЭто первый пройденный тест\n";
            panelStatLesson.transform.Find("panelTexts").transform.Find("testDynamic").GetComponent<Text>().text = "Динамика:\nНет информации";
        }
        else
        {
            while (reader.Read())
            {
                idLessonBefore = reader.GetInt32(0);
            }
            db.CloseDataBase();
            reader.Close();
            reader = null;
            idTest = db.GetId("SELECT idTest FROM tests WHERE idLesson = '" + idLessonBefore.ToString() + "';");//получили id предыдущего теста
            Debug.Log("id предыдушего теста = " + idTest.ToString());
            List<int> resultQuestionBefore = new List<int>();//список для данных вопросов до
            List<int> resultAnswersBefore = new List<int>();//список для данных ответов до
            List<int> resultPointsBefore = new List<int>();//список для полученных баллов до
            query = "SELECT idQuestion FROM answers WHERE idTest = '" + idTest.ToString() + "';";
            db.OpenDataBase("testDB.db");
            reader = db.SelectQuery(query);
            while (reader.Read())
            {
                resultQuestionBefore.Add(reader.GetInt32(0));
            }
            db.CloseDataBase();
            reader.Close();
            reader = null;

            //получаем список ответов и баллов
            query = "SELECT " +
                "answers.idAnswer, " +
                "answervariants.points " +
                "FROM answervariants INNER JOIN answers ON answervariants.idAnswer = answers.idAnswer " +
                "WHERE answers.idTest = '" + idTest.ToString() + "';";
            db.OpenDataBase("testDB.db");
            reader = db.SelectQuery(query);
            while (reader.Read())
            {
                resultAnswersBefore.Add(reader.GetInt32(0));
                resultPointsBefore.Add(reader.GetInt32(1));
            }
            db.CloseDataBase();
            reader.Close();
            reader = null;

            string before = "";
            string dynamic = "";//и динамика тут же
            int[] sumPointsBefore = new int[4];
            bool[] isSectionBefore = new bool[] { false, false, false, false };

            for (int i = 0; i < resultQuestionBefore.Count; i++)
            {
                if (resultQuestionBefore[i] >= 1 && resultQuestionBefore[i] <= 14)//если 1 раздел
                {
                    isSectionBefore[0] = true;
                }
                else if (resultQuestionBefore[i] >= 15 && resultQuestionBefore[i] <= 34)//если 2 раздел
                {
                    isSectionBefore[1] = true;
                }
                else if (resultQuestionBefore[i] >= 35 && resultQuestionBefore[i] <= 51)//если 3 раздел
                {
                    isSectionBefore[2] = true;
                }
                else                                                       //если 4 раздел
                {
                    isSectionBefore[3] = true;
                    break;
                }
            }
            //считаем баллы до
            for (int i = resultPointsBefore.Count - 1; i >= 0; i--)//считаем баллы начиная с конца, то есть 4 раздел
            {
                if (resultQuestionBefore[i] >= 52 && resultQuestionBefore[i] <= 77)
                    sumPointsBefore[3] += resultPointsBefore[i];
                else if (resultQuestionBefore[i] >= 35 && resultQuestionBefore[i] <= 51)
                    sumPointsBefore[2] += resultPointsBefore[i];
                else if (resultQuestionBefore[i] >= 15 && resultQuestionBefore[i] <= 34)
                    sumPointsBefore[1] += resultPointsBefore[i];
                else
                    sumPointsBefore[0] += resultPointsBefore[i];
            }

            if (section[0] == 1)
            {
                before += "Речь/Язык/Коммуникативные навыки: " + sumPointsBefore[0].ToString() + "\n";
                if (sumPointsBefore[0] > sumPoints[0])
                    dynamic += "Речь/Язык/Коммуникативные навыки: улучшение результата на " + (Math.Abs(sumPoints[0] - sumPointsBefore[0])).ToString() + "\n";
                else if (sumPointsBefore[0] < sumPoints[0])
                    dynamic += "Речь/Язык/Коммуникативные навыки: ухудшение результата на " + (Math.Abs(sumPoints[0] - sumPointsBefore[0])).ToString() + "\n";
                else
                    dynamic += "Речь/Язык/Коммуникативные навыки: результат не изменился: " + (Math.Abs(sumPoints[0] - sumPointsBefore[0])).ToString() + "\n";

            }
            if (section[1] == 1)
            {
                before += "Социализация: " + sumPointsBefore[1].ToString() + "\n";
                if (sumPointsBefore[1] > sumPoints[1])
                    dynamic += "Социализация: улучшение результата на " + (Math.Abs(sumPoints[1] - sumPointsBefore[1])).ToString() + "\n";
                else if (sumPointsBefore[1] < sumPoints[1])
                    dynamic += "Социализация: ухудшение результата на " + (Math.Abs(sumPoints[1] - sumPointsBefore[1])).ToString() + "\n";
                else
                    dynamic += "Социализация: результат не изменился: " + (Math.Abs(sumPoints[1] - sumPointsBefore[1])).ToString() + "\n";
            }
            if (section[2] == 1)
            {
                before += "Сенсорные навыки/Познавательные способности: " + sumPointsBefore[2].ToString() + "\n";
                if (sumPointsBefore[2] > sumPoints[2])
                    dynamic += "Сенсорные навыки/Познавательные способности: улучшение результата на " + (Math.Abs(sumPoints[2] - sumPointsBefore[2])).ToString() + "\n";
                else if (sumPointsBefore[2] < sumPoints[2])
                    dynamic += "Сенсорные навыки/Познавательные способности: ухудшение результата на " + (Math.Abs(sumPoints[2] - sumPointsBefore[2])).ToString() + "\n";
                else
                    dynamic += "Сенсорные навыки/Познавательные способности: результат не изменился: " + (Math.Abs(sumPoints[2] - sumPointsBefore[2])).ToString() + "\n";
            }
            if (section[3] == 1)
            {
                before += "Здоровье/Физическое развитие/Поведение: " + sumPointsBefore[3].ToString() + "\n";
                if (sumPointsBefore[3] > sumPoints[3])
                    dynamic += "Здоровье/Физическое развитие/Поведение: улучшение результата на " + (Math.Abs(sumPoints[3] - sumPointsBefore[3])).ToString() + "\n";
                else if (sumPointsBefore[3] < sumPoints[3])
                    dynamic += "Здоровье/Физическое развитие/Поведение: ухудшение результата на " + (Math.Abs(sumPoints[3] - sumPointsBefore[3])).ToString() + "\n";
                else
                    dynamic += "Здоровье/Физическое развитие/Поведение: результат не изменился: " + (Math.Abs(sumPoints[3] - sumPointsBefore[3])).ToString() + "\n";
            }
            panelStatLesson.transform.Find("panelTexts").transform.Find("textBefore").GetComponent<Text>().text = "Результат до:\n" + before;
            panelStatLesson.transform.Find("panelTexts").transform.Find("testDynamic").GetComponent<Text>().text = "Динамика:\n" + dynamic;
        }
        #endregion

        #region создаем dropdown для ответов в тестировании
        //создадим dropdown для статистики тестирования
        Dropdown clone = Instantiate(prefabDrop, panelStatLesson.transform.Find("panelTexts"), false);//создали выпадающий список
        clone.ClearOptions();
        clone.transform.Find("Header").GetComponent<Text>().text = "Просмотреть ответы";//задали заголовок теста для выпадающего списка

        List<string> testStatList = new List<string>();//drop для статистики по ответам
        string stat = "";

        query = "select " +
            "lessons.name, " +
            "questions.name, " +
            "answervariants.answer " +
            "from answers " +
            "inner join tests on tests.idTest = answers.idTest " +
            "inner join lessons on lessons.idLesson = tests.idLesson " +
            "inner join questions on questions.idQuestion = answers.idQuestion " +
            "inner join answervariants on answervariants.idAnswer = answers.idAnswer " +
            "group by answers.idTest, questions.idQuestion " +
            "having answers.idTest = '" + idTest.ToString() + "' " +
            "order by questions.idQuestion";
        db.OpenDataBase("testDB.db");
        reader = db.SelectQuery(query);
        if (reader.HasRows)
            while (reader.Read())
            {
                testStatList.Clear();
                stat = "";
                stat +=
                    "На вопрос:\n\b" + reader[1].ToString() + "\b" +
                    "\nдал(а) ответ:\n" + reader[2].ToString();
                testStatList.Add(stat);
                panelStatLesson.transform.Find("panelTexts").transform.Find("Dropdown(Clone)").GetComponent<Dropdown>().AddOptions(testStatList);
            }
        db.CloseDataBase();
        reader.Close();
        reader = null;
        #endregion
    }

    public void OpenStatPacientWindow()//открыть окно статистики пациента
    {
        headerWindow.GetComponent<Text>().text = "Статистика пациента";
        BarTextAnimation();
        queueWindow.Add(statWindow);
        string name = "";
        panelStatLesson.SetActive(false);
        panelStatPacient.SetActive(true);
        panelStatCourse.SetActive(false);
        statWindow.GetComponent<Animator>().SetInteger("Open", 2);
        name = clickedGameObject.transform.parent.transform.Find("Name").GetComponent<Text>().text;
        Database db = GetComponent<Database>();
        idPacient = db.GetId("SELECT pacients.idPacient FROM pacients WHERE pacients.fullname = '" + name.ToString() + "';");
        Debug.Log(name + ", " + idPacient.ToString());

        string query = "SELECT " +
            "pacients.fullname, " +
            "pacients.birthdate, " +
            "pacients.diagnos " +
            "FROM pacients WHERE pacients.idPacient = '" + idPacient.ToString() + "';";
        db.OpenDataBase("testDB.db");
        reader = db.SelectQuery(query);
        while (reader.Read())
        {
            panelStatPacient.transform.Find("textInfo").GetComponent<Text>().text = reader[0].ToString() + "\n" +
                                                                                reader[1].ToString() + "\n" +
                                                                                reader[2].ToString() + "\n";
        }
        db.CloseDataBase();
        reader.Close();
        reader = null;

        query = "SELECT " +
            "Count(tests.idTest), " +
            "pacients.idPacient " +
            "FROM (pacients INNER JOIN lessons ON pacients.idPacient = lessons.idPacient) " +
            "INNER JOIN tests ON lessons.idLesson = tests.idLesson " +
            "GROUP BY pacients.idPacient " +
            "HAVING(((pacients.idPacient) = '" + idPacient.ToString() + "'));";
        db.OpenDataBase("testDB.db");
        reader = db.SelectQuery(query);
        int count = 0;
        if (!reader.HasRows)
            count = 0;
        else
        {
            while (reader.Read())
                count = reader.GetInt32(0);
        }
        panelStatPacient.transform.Find("textInfo").GetComponent<Text>().text += "Пройдено тестирований: " + count.ToString();
        if (count > 0)//видимость графа
            graph.SetActive(true);
        else
            graph.SetActive(false);

        db.CloseDataBase();
        reader.Close();
        reader = null;

        //получаем статистику пациента по пройденным занятиям
        query = "select lessons.name, sum(answervariants.points) " +
            "from lessons " +
            "inner join tests on tests.idLesson = lessons.idLesson " +
            "inner join answers on answers.idTest = tests.idTest " +
            "inner join answervariants on answervariants.idAnswer = answers.idAnswer " +
            "group by lessons.idLesson " +
            "having lessons.idPacient = '" + idPacient.ToString() + "' " +
            "order by lessons.name";
        db.OpenDataBase("testDB.db");
        reader = db.SelectQuery(query);
        pointsList.Clear();//очистим список баллов
        while (reader.Read())
        {
            pointsList.Add(reader.GetInt32(1));
        }
        db.CloseDataBase();
        reader.Close();
        reader = null;

        maxPoints = 0;
        //выведем в граф макс. количество баллов
        if (section[0] == 1)
            maxPoints += 28;
        if (section[1] == 1)
            maxPoints += 40;
        if (section[2] == 1)
            maxPoints += 36;
        if (section[3] == 1)
            maxPoints += 75;
        wg.ShowGraph(pointsList, maxPoints + 10);//рисуем граф
    }
    public void OpenStatCourseWindow()//открыть окно статистики курса
    {
        h.Clear();

        headerWindow.GetComponent<Text>().text = "Статистика курса";
        BarTextAnimation();
        queueWindow.Add(statWindow);
        string name = "";
        panelStatLesson.SetActive(false);
        panelStatPacient.SetActive(false);
        panelStatCourse.SetActive(true);
        statWindow.GetComponent<Animator>().SetInteger("Open", 2);
        name = clickedGameObject.transform.parent.transform.Find("Name").GetComponent<Text>().text;
        panelStatCourse.transform.Find("panelTexts").transform.Find("Name").GetComponent<Text>().text = name;
        Database db = GetComponent<Database>();
        Debug.Log(clickedGameObject.name);
        idCourse = db.GetId("SELECT idCourse " +
            "FROM courses " +
            "WHERE date = '" + clickedGameObject.transform.parent.transform.Find("Date").GetComponent<Text>().text + "' " +
            "AND time = '" + clickedGameObject.transform.parent.transform.Find("Time").GetComponent<Text>().text + "';");

        string query = "SELECT " +
            "Count(idPacient) " +
            "FROM pacients WHERE idCourse = '" + idCourse.ToString() + "';";
        db.OpenDataBase("testDB.db");
        reader = db.SelectQuery(query);
        int count = 0;
        while (reader.Read())
        {
            if (!reader.HasRows)
                count = 0;
            else
            {
                count = reader.GetInt32(0);//получаем количество пациентов
            }
        }
        Debug.Log("id курса в статистике: " + idCourse.ToString());
        db.CloseDataBase();
        reader.Close();
        reader = null;
 
        query = "SELECT " +
            "courses.section1, " +
            "courses.section2, " +
            "courses.section3, " +
            "courses.section4, " +
            "courses.name " +
            "FROM courses " +
            "GROUP BY courses.date, courses.time " +
            "HAVING courses.date = '" + clickedGameObject.transform.parent.transform.Find("Date").GetComponent<Text>().text + "' " +
            "AND courses.time = '" + clickedGameObject.transform.parent.transform.Find("Time").GetComponent<Text>().text + "';";//выборка по статистике курса
        db.OpenDataBase("testDB.db");
        reader = db.SelectQuery(query);
        string section1, section2, section3, section4;
        while (reader.Read())
        {
            if (reader.GetInt32(0) == 1)
                section1 = "Речь/Язык/Коммуникативные навыки\n\n";
            else
                section1 = "";
            if (reader.GetInt32(1) == 1)
                section2 = "Социализация\n\n";
            else
                section2 = "";
            if (reader.GetInt32(2) == 1)
                section3 = "Сенсорные навыки/Познавательные способности\n\n";
            else
                section3 = "";
            if (reader.GetInt32(3) == 1)
                section4 = "Здоровье/Физическое развитие/Поведение\n\n";
            else
                section4 = "";
            panelStatCourse.transform.Find("panelTexts").transform.Find("Count").GetComponent<Text>().text = "Количество респондентов: " + count.ToString();
            panelStatCourse.transform.Find("panelTexts").transform.Find("Sections").GetComponent<Text>().text = "\nВыбранные разделы:\n\n" +
                                                                                   section1 +
                                                                                   section2 +
                                                                                   section3 +
                                                                                   section4;
        }
        reader.Close();
        reader = null;
        db.CloseDataBase();


        //выведем гистограмму с баллами
        query = "select " +
            "pacients.fullname, " +
            "sum(answervariants.points) " +
            "from courses " +
            "inner join pacients on pacients.idCourse = courses.idCourse " +
            "inner join lessons on lessons.idPacient = pacients.idPacient " +
            "inner join tests on tests.idLesson = lessons.idLesson " +
            "inner join answers on answers.idTest = tests.idTest " +
            "inner join answervariants on answervariants.idAnswer = answers.idAnswer " +
            "group by pacients.idPacient " +
            "having courses.idCourse = '" + idCourse.ToString() + "'";
        db.OpenDataBase("testDB.db");
        List<string> pacientsList = new List<string>();//заполняем список имен пациентов
        List<int> listPoints = new List<int>();//заполняем список полученных ими баллов
        reader = db.SelectQuery(query);
        if (reader.HasRows)
        {
            h.gameObject.SetActive(true);
            while (reader.Read())
            {
                pacientsList.Add(reader[0].ToString());
                listPoints.Add(reader.GetInt32(1));
            }
        }
        else
        {
            h.gameObject.SetActive(false);
        }
        reader.Close();
        reader = null;
        db.CloseDataBase();

        int maxPoints = 0;
        for (int i = 0; i < listPoints.Count; i++)
            maxPoints += listPoints[i];

        h.ShowChart(listPoints, maxPoints + 10);
    }

    public void Exit()//выход из приложения
    {
        Application.Quit();
    }

    public void CloseAll()
    {
        CloseAddPacientWindow();
        CloseCreateCourseWindow();
        CloseChooseCourseWindow();
        CloseChooseLessonWindow();
        CloseChoosePacientWindow();
        CloseSettingsWindow();
        CloseResultWindow();
        CloseStatWindow();
        CloseTestWindow();
        queueWindow.RemoveRange(1, queueWindow.Count - 1);
        appBar.transform.Find("Bar").transform.Find("PanelLayer").transform.Find("ArrowBack").gameObject.SetActive(false);
        appBar.transform.Find("Bar").transform.Find("PanelLayer").transform.Find("Spinny Arrow Button").gameObject.SetActive(true);
        isTest = false;
    }

    public void ShowError(string error)//Показать окно ошибки
    {
        errorWindow.GetComponent<Animator>().SetInteger("Error", 2);
        errorText.text = error;
    }

    public void CloseError()//скрыть окно ошибки
    {
        errorWindow.GetComponent<Animator>().SetInteger("Error", 1);
    }

    public void AddCourse()//добавление курса по кнопке, либо его изменение
    {
        if (headerWindow.GetComponent<Text>().text == "Изменение курса")
        {
            //изменяем курс
            if (nameCourse.textComponent.text.Length == 0)
                ShowError("Укажите имя курса");
            else
            {
                Database db = GetComponent<Database>();
                db.OpenDataBase("testDB.db");
                db.UpdateQuery("UPDATE courses SET name = '" + nameCourse.textComponent.text + "' WHERE idCourse = '" + idCourse.ToString() + "';");
                Debug.Log("Курс изменен");
                db.CloseDataBase();
                SelectCourses();
                queueWindow.RemoveAt(queueWindow.Count - 1);
                queueWindow.RemoveAt(queueWindow.Count - 1);
                CloseCreateCourseWindow();
                OpenChooseCourseWindow();
            }
        }
        else
        {
            if (nameCourse.textComponent.text.Length == 0)
                ShowError("Укажите имя курса");
            else if (section[0] == 0 && section[1] == 0 && section[2] == 0 && section[3] == 0)
                ShowError("Выберите раздел(ы)");
            else
            {
                text1.SetActive(false);
                InsertCourse();
                SelectCourses();
                nameCourse.textComponent.text = "";
                foreach (var item in sectionButtons)
                    item.GetComponent<Image>().color = Color.white;//ворачиваем кнопкам цвет
                queueWindow.RemoveAt(queueWindow.Count - 1);
                queueWindow.RemoveAt(queueWindow.Count - 1);
                CloseCreateCourseWindow();
                OpenChooseCourseWindow();
            }
        }
    }

    public void AddPacient()//добавление пациента по кнопке
    {
        if (headerWindow.GetComponent<Text>().text == "Изменение пациента")
        {
            if (namePacient.textComponent.text.Length == 0)
                ShowError("Укажите имя пациента");
            else
            {
                Database db = GetComponent<Database>();
                db.OpenDataBase("testDB.db");
                db.UpdateQuery("UPDATE pacients SET fullname = '" + namePacient.textComponent.text + "', birthdate = '" + dropBirthdate[0].transform.Find("Label").GetComponent<Text>().text + "." + dropBirthdate[1].transform.Find("Label").GetComponent<Text>().text + "." + dropBirthdate[2].transform.Find("Label").GetComponent<Text>().text + "', diagnos = '" + diagnosPacient.textComponent.text + "' WHERE idPacient = '" + idPacient.ToString() + "';");
                Debug.Log("Пациент изменен");
                db.CloseDataBase();
                queueWindow.RemoveAt(queueWindow.Count - 1);
                queueWindow.RemoveAt(queueWindow.Count - 1);
                CloseAddPacientWindow();
                OpenChoosePacientWindow();
            }
        }
        else
        {
            if (namePacient.textComponent.text.Length == 0)
                ShowError("Укажите имя пациента");
            else
            {
                text2.SetActive(false);
                InsertPacient();
                addPacientWindow.transform.Find("panelInfoAboutPacient").transform.Find("tbNamePacient").GetComponent<InputField>().text = "";
                //addPacientWindow.transform.Find("panelInfoAboutPacient").transform.Find("tbBirthDate").GetComponent<InputField>().text = "";
                addPacientWindow.transform.Find("panelInfoAboutPacient").transform.Find("tbDiagnos").GetComponent<InputField>().text = "";
                queueWindow.RemoveAt(queueWindow.Count - 1);
                queueWindow.RemoveAt(queueWindow.Count - 1);
                CloseAddPacientWindow();
                OpenChoosePacientWindow();
            }
        }
    }

    public void AddLesson()//добавление занятия по кнопке
    {
        text3.SetActive(false);
        //InsertLesson();
        //SelectLessons();
        OpenTestWindow();       
    }
    
    public void GetGameObject()//получение объекта при нажатии на него
    {
        clickedGameObject = EventSystem.current.currentSelectedGameObject;
        Debug.Log("Нажатый объект " + clickedGameObject.name);
    }

    public void SelectCourses()//выборка по курсам
    {
        //сначала очистка списка
        var children = new List<GameObject>();
        foreach (Transform child in listCourses.transform)
            children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        Database db = GetComponent<Database>();
        //получаем id пользователя по имени
        idUser = db.GetId("SELECT users.idUser FROM users WHERE users.username = '" + userName + "';");
        Debug.Log("id Пользователя = " + idUser);
        string query = "SELECT " +
            "courses.idCourse, " +
            "courses.idUser, " +
            "courses.name, " +
            "courses.date, " +
            "courses.time, " +
            "courses.section1, " +
            "courses.section2, " +
            "courses.section3, " +
            "courses.section4, " +
            "users.username " +
            "FROM users INNER JOIN courses ON users.idUser = courses.idUser WHERE users.idUser = '" + idUser.ToString() + "';";
        db.OpenDataBase("testDB.db");
        reader = db.SelectQuery(query);
        LongClick lc = GetComponent<LongClick>();
        if (!reader.HasRows)
        {
            text1.SetActive(true);
        }
        else
        {
            text1.SetActive(false);
            while (reader.Read())
            {
                var clone = Instantiate(courseItem, listCourses.transform, false);
                clone.transform.Find("PanelLayer").transform.Find("textStat").GetComponent<Button>().onClick.AddListener(() => GetGameObject());
                clone.transform.Find("PanelLayer").transform.Find("textStat").GetComponent<Button>().onClick.AddListener(() => CloseChooseCourseWindow());
                clone.transform.Find("PanelLayer").transform.Find("textStat").GetComponent<Button>().onClick.AddListener(() => OpenStatCourseWindow());
                clone.transform.Find("PanelLayer").transform.Find("Name").GetComponent<Text>().text = reader[2].ToString();
                clone.transform.Find("PanelLayer").transform.Find("Date").GetComponent<Text>().text = reader[3].ToString();
                clone.transform.Find("PanelLayer").transform.Find("Time").GetComponent<Text>().text = reader[4].ToString();
            }
        }
        db.CloseDataBase();
        reader.Close();
        reader = null;
    }

    public void PointerDownCourse()
    {
        GetGameObject();
        GetCourseName();

        Database db = GetComponent<Database>();
        idCourse = db.GetId("SELECT idCourse FROM courses WHERE " +
    "time = '" + clickedGameObject.transform.Find("Time").GetComponent<Text>().text + "' " +
    "AND date = '" + clickedGameObject.transform.Find("Date").GetComponent<Text>().text + "';");

        CloseChooseCourseWindow();
        OpenChoosePacientWindow();
        SelectPacients();
    }

    public void InsertCourse()//добавление курса
    {
        Database db = GetComponent<Database>();
        db.OpenDataBase("testDB.db");
        string query = "INSERT INTO courses (idUser, name, date, time, section1, section2, section3, section4) " +
            "VALUES('" + idUser.ToString() + "', " +
                   "'" + nameCourse.textComponent.text + "', " +
                   "'" + System.DateTime.Now.ToShortDateString() + "', " +
                   "'" + System.DateTime.Now.ToLongTimeString() + "', " + 
                   "'" + section[0].ToString() + "', " +
                   "'" + section[1].ToString() + "', " +
                   "'" + section[2].ToString() + "', " +
                   "'" + section[3].ToString() + "');";
        db.InsertInto(query);
    }

    public void SelectPacients()//выборка по пациентам
    {
        //сначала очистка списка
        var children = new List<GameObject>();
        foreach (Transform child in listPacients.transform)
            children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        Database db = GetComponent<Database>();

        string query = "SELECT " +
            "pacients.idPacient, " +
            "pacients.idCourse, " +
            "pacients.fullname, " +
            "pacients.birthdate, " +
            "pacients.diagnos " +
            "FROM (users INNER JOIN courses ON users.idUser = courses.idUser) " +
            "INNER JOIN pacients ON courses.idCourse = pacients.idCourse WHERE(((courses.idCourse) = '" + idCourse.ToString() + "'))";
        db.OpenDataBase("testDB.db");
        reader = db.SelectQuery(query);
        LongClick lc = GetComponent<LongClick>();
        if (!reader.HasRows)
        {
            text2.SetActive(true);
            reader.Close();
            reader = null;
        }
        else
        {
            text2.SetActive(false);
            while (reader.Read())
            {
                var clone = Instantiate(pacientItem, listPacients.transform, false);

                clone.transform.Find("PanelLayer").transform.Find("textStat").GetComponent<Button>().onClick.AddListener(() => GetGameObject());
                clone.transform.Find("PanelLayer").transform.Find("textStat").GetComponent<Button>().onClick.AddListener(() => CloseChoosePacientWindow());
                clone.transform.Find("PanelLayer").transform.Find("textStat").GetComponent<Button>().onClick.AddListener(() => OpenStatPacientWindow());
                clone.transform.Find("PanelLayer").transform.Find("Name").GetComponent<Text>().text = reader[2].ToString();
                clone.transform.Find("PanelLayer").transform.Find("Birthdate").GetComponent<Text>().text = reader[3].ToString();
                //clone.transform.Find("PanelLayer").transform.Find("Diagnos").GetComponent<Text>().text = reader[4].ToString();
            }
            reader.Close();
            reader = null;
        }
        Debug.Log("id Выбранного курса = " + idCourse);

        //сохраним выбранные раздела курса
        query = "SELECT section1, section2, section3, section4 FROM courses WHERE idCourse = '" + idCourse.ToString() + "'";
        db.OpenDataBase("testDB.db");
        reader = db.SelectQuery(query);
        while (reader.Read())
        {
            section[0] = reader.GetInt32(0);
            section[1] = reader.GetInt32(1);
            section[2] = reader.GetInt32(2);
            section[3] = reader.GetInt32(3);
        }
    }

    public void PointerDownPacient()
    {
        GetGameObject();
        GetPacientName();

        Database db = GetComponent<Database>();
        idPacient = db.GetId("SELECT pacients.idPacient FROM pacients WHERE pacients.fullname = '" + pacientName.ToString() + "';");

        CloseChoosePacientWindow();
        OpenChooseLessonWindow();
        SelectLessons();
    }

    public void InsertPacient()
    {
        Database db = GetComponent<Database>();
        db.OpenDataBase("testDB.db");
        string query = "INSERT INTO pacients (idCourse, diagnos, birthdate, fullname) " +
            "VALUES ('" + idCourse.ToString() + "', '" + diagnosPacient.textComponent.text + "', '" + dropBirthdate[0].transform.Find("Label").GetComponent<Text>().text + "." + dropBirthdate[1].transform.Find("Label").GetComponent<Text>().text + "." + dropBirthdate[2].transform.Find("Label").GetComponent<Text>().text + "', '" + namePacient.textComponent.text + "');";
        db.InsertInto(query);
    }

    public void SelectLessons()//выборка по занятиям
    {
        //сначала очистка списка
        var children = new List<GameObject>();
        foreach (Transform child in listLessons.transform)
            children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        Database db = GetComponent<Database>();
        string query = "SELECT " +
            "lessons.idLesson, " +
            "lessons.idPacient, " +
            "lessons.name, " +
            "lessons.date, " +
            "pacients.fullname " +
            "FROM ((users INNER JOIN courses ON users.idUser = courses.idUser) " +
            "INNER JOIN pacients ON courses.idCourse = pacients.idCourse) " +
            "INNER JOIN lessons ON pacients.idPacient = lessons.idPacient WHERE (((pacients.idPacient) = '" + idPacient.ToString() + "'));";
        db.OpenDataBase("testDB.db");
        reader = db.SelectQuery(query);
        if (!reader.HasRows)
        {
            text3.SetActive(true);
            reader.Close();
            reader = null;
        }
        else
        {
            text3.SetActive(false);
            while (reader.Read())
            {
                var clone = Instantiate(lessonItem, listLessons.transform, false);
                clone.transform.Find("PanelLayer").GetComponent<Button>().onClick.AddListener(() => GetGameObject());
                clone.transform.Find("PanelLayer").GetComponent<Button>().onClick.AddListener(() => GetLessonName());
                clone.transform.Find("PanelLayer").GetComponent<Button>().onClick.AddListener(() => CheckIfTestDone());
                clone.transform.Find("PanelLayer").transform.Find("Name").GetComponent<Text>().text = reader[2].ToString();
                clone.transform.Find("PanelLayer").transform.Find("Date").GetComponent<Text>().text = "От " + reader[3].ToString();
            }
            reader.Close();
            reader = null;
        }
        Debug.Log("id Выбранного пациента = " + idPacient);

        //Выведем статистику по пациенту на экране выборе занятий (тестирований)
        infoPacient.text = "";
        db.OpenDataBase("testDB.db");
        query = "SELECT " +
            "pacients.fullname, " +
            "pacients.birthdate, " +
            "pacients.diagnos " +
            "FROM pacients WHERE pacients.idPacient = '" + idPacient.ToString() + "';";
        reader = db.SelectQuery(query);
        while (reader.Read())
        {
            infoPacient.text = "\n\n\n  " + reader[0].ToString() + "\n  " +
                                      reader[1].ToString() + "\n  " +
                                      reader[2].ToString() + "\n  ";
        }
        db.CloseDataBase();
        reader.Close();
        reader = null;

        //запрос на получение количества занятий
        db.OpenDataBase("testDB.db");
        query = "select count(idLesson) from lessons where lessons.idPacient = '" + idPacient.ToString() + "'";
        reader = db.SelectQuery(query);
        if (!reader.HasRows)
            countLessons = 0;
        else
        {
            while (reader.Read())
            {
                countLessons = reader.GetInt32(0);//получаем количество занятий
            }
        }
        db.CloseDataBase();
        reader.Close();
        reader = null;
        //получить количество занятий
        infoPacient.text += "Количество тестирований: " + countLessons.ToString();
        //infoPacient.text += "\n  Последнее пройденное тестирование:\n"; 
    }

    public void InsertLesson()
    {
        Database db = GetComponent<Database>();
        db.OpenDataBase("testDB.db");
        string query = "INSERT INTO lessons (name, date, idPacient) " +
            "VALUES ('" + ("Тестирование №" + (countLessons + 1).ToString()) + "', '" + System.DateTime.Now.ToShortDateString() + "', '" + idPacient.ToString() + "');";
        db.InsertInto(query);
        db.CloseDataBase();
    }

    public void ShowYesNo(string text)
    {
        panelYesNo.GetComponent<Animator>().SetInteger("YesNo", 2);
        panelYesNo.transform.Find("Text").GetComponent<Text>().text = text;
        YesNoIsShow = true;
    }

    public void HideYesNo()
    {
        panelYesNo.GetComponent<Animator>().SetInteger("YesNo", 1);
        YesNoIsShow = false;
    }

    public void Delete()
    {
        CloseStatWindow();
        if (headerWindow.GetComponent<Text>().text == "Статистика курса")
        {
            DeleteCourse();
            OpenChooseCourseWindow();
        }
        else if (headerWindow.GetComponent<Text>().text == "Статистика пациента")
        {
            DeletePacient();
            OpenChoosePacientWindow();
        }
        else
        {
            DeleteLesson();
            OpenChooseLessonWindow();
        }
        HideYesNo();
    }

    public void DeletePacient()
    {
        Database db = GetComponent<Database>();
        db.OpenDataBase("testDB.db");
        db.DeleteQuery("DELETE FROM pacients WHERE pacients.idPacient = '" + idPacient.ToString() + "';");
        Debug.Log("Пациент удален");
        queueWindow.RemoveAt(queueWindow.Count - 1);
        queueWindow.RemoveAt(queueWindow.Count - 1);
    }

    public void DeleteCourse()
    {
        Database db = GetComponent<Database>();
        db.OpenDataBase("testDB.db");
        db.DeleteQuery("DELETE FROM courses WHERE courses.idCourse = '" + idCourse.ToString() + "';");
        Debug.Log("Курс удален");
        queueWindow.RemoveAt(queueWindow.Count - 1);
        queueWindow.RemoveAt(queueWindow.Count - 1);
    }

    public void DeleteLesson()
    {
        Database db = GetComponent<Database>();
        db.OpenDataBase("testDB.db");
        db.DeleteQuery("DELETE FROM lessons WHERE lessons.idLesson = '" + idLesson.ToString() + "';");
        Debug.Log("Занятие удалено");
        queueWindow.RemoveAt(queueWindow.Count - 1);
        queueWindow.RemoveAt(queueWindow.Count - 1);
    }

    public void GetCourseName()
    {
        courseName = clickedGameObject.transform.Find("Name").GetComponent<Text>().text;
    }

    public void GetPacientName()
    {
        pacientName = clickedGameObject.transform.Find("Name").GetComponent<Text>().text;
    }

    public void GetLessonName()
    {
        lessonName = clickedGameObject.transform.Find("Name").GetComponent<Text>().text;
    }

    public void SelectRazdel1()
    {
        if (sectionButtons[0].isOn)
            section[0] = 1;
        else
            section[0] = 0;
    }

    public void SelectRazdel2()
    {
        if (sectionButtons[1].isOn)
            section[1] = 1;
        else
            section[1] = 0;
    }

    public void SelectRazdel3()
    {
        if (sectionButtons[2].isOn)
            section[2] = 1;
        else
            section[2] = 0;
    }

    public void SelectRazdel4()
    {
        if (sectionButtons[3].isOn)
            section[3] = 1;
        else
            section[3] = 0;
    }

    public void DropdownYearLoad()
    {
        dropBirthdate[2].ClearOptions();
        List<string> listTextYear = new List<string>();
        for (int i = System.DateTime.Now.Year - 100; i <= System.DateTime.Now.Year; i++)
        {
            listTextYear.Add(i.ToString());
        }
        dropBirthdate[2].AddOptions(listTextYear);
        dropBirthdate[2].value = 81;//установка стандартного отображения года рождения на 2000
    }

    public void ShowButtonDelete()
    {
        StopAllCoroutines();
        if (headerWindow.GetComponent<Text>().text == "Статистика курса")
            buttonsDelete[0].GetComponent<Animator>().SetInteger("Open", 2);
        else if (headerWindow.GetComponent<Text>().text == "Статистика пациента")
            buttonsDelete[1].GetComponent<Animator>().SetInteger("Open", 2);
        else
            buttonsDelete[2].GetComponent<Animator>().SetInteger("Open", 2);
        StartCoroutine(TimerToHide());
    }

    public void HideButtonDelete()
    {
        if (headerWindow.GetComponent<Text>().text == "Статистика курса")
            buttonsDelete[0].GetComponent<Animator>().SetInteger("Open", 1);
        else if (headerWindow.GetComponent<Text>().text == "Статистика пациента")
            buttonsDelete[1].GetComponent<Animator>().SetInteger("Open", 1);
        else
            buttonsDelete[2].GetComponent<Animator>().SetInteger("Open", 1);
    }

    IEnumerator TimerToHide()
    {
        yield return new WaitForSeconds(3f);
        HideButtonDelete();
    }
}
