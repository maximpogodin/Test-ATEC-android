using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;

public class Test : MonoBehaviour
{
    SqliteDataReader reader;
    public Text nameQuestion;
    public Text nameRazdel;
    public GameObject btnNext;
    public GameObject[] radioBtn;
    public List<int> idsChousenQuestion = new List<int>();//список id вопросов для записи в БД
    public List<int> idsChousenAnswer = new List<int>();//список id выбранных вариантов ответа для записи в БД
    ManageWindow mw;
    Database db;
    public int i = 0;//номер вопроса
    public int sum1 = 0, sum2 = 0, sum3 = 0, sum4 = 0;

    void Start()
    {
        mw = GetComponent<ManageWindow>();
        db = GetComponent<Database>();
        radioBtn[0].transform.Find("Toggle").GetComponent<Toggle>().isOn = true;
        radioBtn[1].transform.Find("Toggle").GetComponent<Toggle>().isOn = false;
        radioBtn[2].transform.Find("Toggle").GetComponent<Toggle>().isOn = false;
        radioBtn[3].transform.Find("Toggle").GetComponent<Toggle>().isOn = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && mw.isTest)
        {
            Back();
        }
    }

    public void TestAnimation()//анимация вопросов в тесте
    {
        mw.testWindow.transform.Find("panelTest").GetComponent<Animator>().Play("PanelTestNext", -1, 0f);
    }

    public void Next()//следующий вопрос
    {
        if (i >= mw.listQuestion.Count - 1)//если вопросы закончились, открываем результаты
        {
            RememberAnswer();
            CountResult();
            mw.OpenResultWindow();
            mw.resultWindow.transform.Find("btnSave").transform.Find("Button Layer").GetComponent<Button>().interactable = true;
            mw.CloseTestWindow();
        }
        else
        {
            TestAnimation();
            RememberAnswer();//записать ответы
            i++;//перейти к следующему вопросу
            ShowQuestionAndAnswer();//показать ответы и вопросы
        }
    }

    public void Back()//предыдущий вопрос
    {
        i--;
        if (i < 0)//вернуться к выбору тестирования
        {
            mw.CloseTestWindow();
            mw.isTest = false;
            mw.OpenChooseLessonWindow();
            mw.CloseResultWindow();
            mw.queueWindow.Remove(mw.chooseLessonWindow);
        }
        else if (mw.headerWindow.GetComponent<Text>().text == "Результаты тестирования")
        {
            mw.CloseTestWindow();
            mw.isTest = false;
            mw.OpenChooseLessonWindow();
            mw.CloseResultWindow();
        }
        else
            ShowQuestionAndAnswer();//показать ответы и вопросы
    }

    public void ShowQuestionAndAnswer()//метод для отображения вопросов и вариантов ответа
    {
        nameQuestion.text = mw.idsQuestion[i].ToString() + ". " + mw.listQuestion[i];
        if (mw.listIds[i] >= 1 && mw.listIds[i] <= 14)
        {
            nameRazdel.text = "Речь/Язык/Коммуникативные навыки";
            radioBtn[0].transform.Find("Text").GetComponent<Text>().text = mw.listAnswerVariants[0];
            radioBtn[1].transform.Find("Text").GetComponent<Text>().text = mw.listAnswerVariants[1];
            radioBtn[2].transform.Find("Text").GetComponent<Text>().text = mw.listAnswerVariants[2];
            radioBtn[3].SetActive(false);
        }
        else if (mw.listIds[i] >= 15 && mw.listIds[i] <= 34)
        {
            nameRazdel.text = "Социализация";
            radioBtn[0].transform.Find("Text").GetComponent<Text>().text = mw.listAnswerVariants[3];
            radioBtn[1].transform.Find("Text").GetComponent<Text>().text = mw.listAnswerVariants[4];
            radioBtn[2].transform.Find("Text").GetComponent<Text>().text = mw.listAnswerVariants[5];
            radioBtn[3].SetActive(false);
        }
        else if (mw.listIds[i] >= 35 && mw.listIds[i] <= 52)
        {
            nameRazdel.text = "Сенсорные навыки/Познавательные способности";
            radioBtn[0].transform.Find("Text").GetComponent<Text>().text = mw.listAnswerVariants[0];
            radioBtn[1].transform.Find("Text").GetComponent<Text>().text = mw.listAnswerVariants[1];
            radioBtn[2].transform.Find("Text").GetComponent<Text>().text = mw.listAnswerVariants[2];
            radioBtn[3].SetActive(false);
        }
        else
        {
            nameRazdel.text = "Здоровье/Физическое развитие/Поведение";
            radioBtn[3].SetActive(true);
            radioBtn[0].transform.Find("Text").GetComponent<Text>().text = mw.listAnswerVariants[6];
            radioBtn[1].transform.Find("Text").GetComponent<Text>().text = mw.listAnswerVariants[7];
            radioBtn[2].transform.Find("Text").GetComponent<Text>().text = mw.listAnswerVariants[8];
            radioBtn[3].transform.Find("Text").GetComponent<Text>().text = mw.listAnswerVariants[9];
        }
    }

    public void RememberAnswer()//записываем ответы
    {
        if (mw.listIds[i] >= 1 && mw.listIds[i] <= 14) //если первый раздел
        {
            if (radioBtn[0].transform.Find("Toggle").GetComponent<Toggle>().isOn)//и выбран первый ответ
            {
                idsChousenAnswer.Add(mw.idsAnswerVariants[0]);// то добавляем выбранный вариант ответа в список для записи в бд
            }
            else if (radioBtn[1].transform.Find("Toggle").GetComponent<Toggle>().isOn)//иначе если выбран второй ответ
            {
                idsChousenAnswer.Add(mw.idsAnswerVariants[1]);
            }
            else
            {
                idsChousenAnswer.Add(mw.idsAnswerVariants[2]);
            }
        }
        else if (mw.listIds[i] >= 15 && mw.listIds[i] <= 34) //иначе если второй раздел
        {
            if (radioBtn[0].transform.Find("Toggle").GetComponent<Toggle>().isOn)//и выбран первый ответ
            {
                idsChousenAnswer.Add(mw.idsAnswerVariants[3]);// то добавляем выбранный вариант ответа в список для записи в бд
            }
            else if (radioBtn[1].transform.Find("Toggle").GetComponent<Toggle>().isOn)//иначе если выбран второй ответ
            {
                idsChousenAnswer.Add(mw.idsAnswerVariants[4]);
            }
            else
            {
                idsChousenAnswer.Add(mw.idsAnswerVariants[5]);
            }
        }
        else if (mw.listIds[i] >= 35 && mw.listIds[i] <= 52) //иначе если третий раздел
        {
            if (radioBtn[0].transform.Find("Toggle").GetComponent<Toggle>().isOn)//и выбран первый ответ
            {
                idsChousenAnswer.Add(mw.idsAnswerVariants[0]);// то добавляем выбранный вариант ответа в список для записи в бд
            }
            else if (radioBtn[1].transform.Find("Toggle").GetComponent<Toggle>().isOn)//иначе если выбран второй ответ
            {
                idsChousenAnswer.Add(mw.idsAnswerVariants[1]);
            }
            else
            {
                idsChousenAnswer.Add(mw.idsAnswerVariants[2]);
            }
        }
        else //иначе если четвертый раздел
        {
            if (radioBtn[0].transform.Find("Toggle").GetComponent<Toggle>().isOn)//и выбран первый ответ
            {
                idsChousenAnswer.Add(mw.idsAnswerVariants[6]);// то добавляем выбранный вариант ответа в список для записи в бд
            }
            else if (radioBtn[1].transform.Find("Toggle").GetComponent<Toggle>().isOn)//иначе если выбран второй ответ
            {
                idsChousenAnswer.Add(mw.idsAnswerVariants[7]);
            }
            else if (radioBtn[2].transform.Find("Toggle").GetComponent<Toggle>().isOn)
            {
                idsChousenAnswer.Add(mw.idsAnswerVariants[8]);
            }
            else
            {
                idsChousenAnswer.Add(mw.idsAnswerVariants[9]);
            }
        }
    }

    public void CountResult()//подсчет результатов
    {
        //подсчет баллов
        sum1 = 0; sum2 = 0; sum3 = 0; sum4 = 0;
        for (int i = 0; i < mw.listIds.Count; i++)
        {
            if (mw.listIds[i] >= 1 && mw.listIds[i] <= 14)
            {//если первый раздел
                if (idsChousenAnswer[i] == 1)
                    sum1 += mw.listPoints[0];
                else if (idsChousenAnswer[i] == 2)
                    sum1 += mw.listPoints[1];
                else if (idsChousenAnswer[i] == 3)
                    sum1 += mw.listPoints[2];
            }
            else if (mw.listIds[i] >= 15 && mw.listIds[i] <= 34)//2 раздел
            {
                if (idsChousenAnswer[i] == 4)
                    sum2 += mw.listPoints[3];
                else if (idsChousenAnswer[i] == 5)
                    sum2 += mw.listPoints[4];
                else if (idsChousenAnswer[i] == 6)
                    sum2 += mw.listPoints[5];
            }
            else if (mw.listIds[i] >= 35 && mw.listIds[i] <= 52)//3 раздел
            {
                if (idsChousenAnswer[i] == 1)
                    sum3 += mw.listPoints[0];
                else if (idsChousenAnswer[i] == 2)
                    sum3 += mw.listPoints[1];
                else if (idsChousenAnswer[i] == 3)
                    sum3 += mw.listPoints[2];
            }
            else//4 раздел
            {
                if (idsChousenAnswer[i] == 7)
                    sum4 += mw.listPoints[6];
                else if (idsChousenAnswer[i] == 8)
                    sum4 += mw.listPoints[7];
                else if (idsChousenAnswer[i] == 9)
                    sum4 += mw.listPoints[8];
                else if (idsChousenAnswer[i] == 10)
                    sum4 += mw.listPoints[9];
            }
        }

        //записываем баллы на экран результатов
        if (mw.section[0] == 1)
            mw.resultWindow.transform.Find("panelResult").transform.Find("Panel").transform.Find("rb1").transform.Find("Text").GetComponent<Text>().text = "Набрано баллов: " + sum1.ToString();
        else
            mw.resultWindow.transform.Find("panelResult").transform.Find("Panel").transform.Find("rb1").transform.Find("Text").GetComponent<Text>().text = "Раздел не был выбран";
        if (mw.section[1] == 1)
            mw.resultWindow.transform.Find("panelResult").transform.Find("Panel").transform.Find("rb2").transform.Find("Text").GetComponent<Text>().text = "Набрано баллов: " + sum2.ToString();
        else
            mw.resultWindow.transform.Find("panelResult").transform.Find("Panel").transform.Find("rb2").transform.Find("Text").GetComponent<Text>().text = "Раздел не был выбран";
        if (mw.section[2] == 1)
            mw.resultWindow.transform.Find("panelResult").transform.Find("Panel").transform.Find("rb3").transform.Find("Text").GetComponent<Text>().text = "Набрано баллов: " + sum3.ToString();
        else
            mw.resultWindow.transform.Find("panelResult").transform.Find("Panel").transform.Find("rb3").transform.Find("Text").GetComponent<Text>().text = "Раздел не был выбран";
        if (mw.section[3] == 1)
            mw.resultWindow.transform.Find("panelResult").transform.Find("Panel").transform.Find("rb4").transform.Find("Text").GetComponent<Text>().text = "Набрано баллов: " + sum4.ToString();
        else
            mw.resultWindow.transform.Find("panelResult").transform.Find("Panel").transform.Find("rb4").transform.Find("Text").GetComponent<Text>().text = "Раздел не был выбран";

        mw.resultWindow.transform.Find("panelResult").transform.Find("resultText").GetComponent<Text>().text = "Набрано всего баллов: " + (sum1 + sum2 + sum3 + sum4).ToString();
        mw.resultWindow.transform.Find("panelResult").transform.Find("Panel").transform.Find("rb1").transform.Find("Text (1)").GetComponent<Text>().text = "Речь/Язык/Коммуникативные навыки";
        mw.resultWindow.transform.Find("panelResult").transform.Find("Panel").transform.Find("rb2").transform.Find("Text (1)").GetComponent<Text>().text = "Социализация";
        mw.resultWindow.transform.Find("panelResult").transform.Find("Panel").transform.Find("rb3").transform.Find("Text (1)").GetComponent<Text>().text = "Сенсорные навыки/Познавательные способности";
        mw.resultWindow.transform.Find("panelResult").transform.Find("Panel").transform.Find("rb4").transform.Find("Text (1)").GetComponent<Text>().text = "Здоровье/Физическое развитие/Поведение";
    }

    public void Save()//запись в бд результатов
    {
        mw.InsertLesson();//записываем тестирование в базу чтобы был новый id занятия

        db.OpenDataBase("testDB.db");
        string query = "SELECT idLesson FROM lessons ORDER BY idLesson desc LIMIT 1";
        reader = db.SelectQuery(query);
        while (reader.Read())
        {
            mw.idLesson = reader.GetInt32(0);
        }
        Debug.Log("id сохраняемого тестирования: " + mw.idLesson);
        db.CloseDataBase();
        reader.Close();
        reader = null;

        //записали idLesson в tests
        db.OpenDataBase("testDB.db");
        query = "INSERT INTO tests (idLesson) VALUES ('" + mw.idLesson.ToString() + "')";
        db.InsertInto(query);
        db.CloseDataBase();

        //получаем id теста
        db.OpenDataBase("testDB.db");
        query = "SELECT idTest FROM tests ORDER BY idTest DESC LIMIT 1";
        reader = db.SelectQuery(query);
        while (reader.Read())
        {
            mw.idTest = reader.GetInt32(0);
        }
        db.CloseDataBase();
        reader.Close();
        reader = null;
        Debug.Log("id теста: " + mw.idTest.ToString());

        //записываем все ответы в БД
        db.OpenDataBase("testDB.db");
        for (int i = 0; i < idsChousenAnswer.Count; i++)
        {
            query = "INSERT INTO answers (idAnswer, idQuestion, idTest) " +
                "VALUES ('" + idsChousenAnswer[i].ToString() + "', '" + mw.listIds[i].ToString() + "', '" + mw.idTest.ToString() + "')";
            try
            {
                //если результат записался
                db.InsertInto(query);
                Debug.Log("Успешно сохранено!");
            }
            catch (SqliteException ex)
            {
                //иначе выведем ошибку записи
                mw.ShowError(ex.Message);
            }
        }
        db.CloseDataBase();
        //возвращаемся на экран тестов
        mw.resultWindow.transform.Find("btnSave").transform.Find("Button Layer").GetComponent<Button>().interactable = false;
        mw.CloseResultWindow();
        mw.CloseTestWindow();
        mw.isTest = false;
        mw.OpenChooseLessonWindow();
    }

}
