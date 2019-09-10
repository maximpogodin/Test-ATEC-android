using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class LoginRegistration : MonoBehaviour
{
    public GameObject userName;
    public GameObject userPassword;
    List<string> listUsers = new List<string>();//список пользователей
    SqliteDataReader reader;
    SqliteCommand command;
    public ManageWindow mw;
    public string name;
    public Text usernameText;
    public GameObject panelReg;
    
    public void OpenReg()
    {
        panelReg.GetComponent<Animator>().SetInteger("Open", 2);
        panelReg.transform.Find("tbUser").GetComponent<InputField>().text = "";
        panelReg.transform.Find("tbPass").GetComponent<InputField>().text = "";
    }

    public void CloseReg()
    {
        if (panelReg.GetComponent<Animator>().GetInteger("Open") == 2)
            panelReg.GetComponent<Animator>().SetInteger("Open", 1);
    }

    public void Registration()
    {
        Database db = GetComponent<Database>();
        if (panelReg.transform.Find("tbUser").GetComponent<InputField>().text.Length != 0 && panelReg.transform.Find("tbPass").GetComponent<InputField>().text.Length != 0)
        {
            string query = "SELECT * FROM users WHERE username = '" + panelReg.transform.Find("tbUser").GetComponent<InputField>().text + "';";
            db.OpenDataBase("testDB.db");
            reader = db.SelectQuery(query);
            if (reader.HasRows)
            {
                mw.ShowError("Имя пользователя уже занято.");
            }
            else
            {
                query = "INSERT INTO users (`username`, `password`) VALUES ('" + panelReg.transform.Find("tbUser").GetComponent<InputField>().text + "', '" + panelReg.transform.Find("tbPass").GetComponent<InputField>().text + "');";
                db.InsertInto(query);
                mw.ShowError("Регистрация успешна! Теперь вы можете войти.");
                CloseReg();
            }
            reader.Close();
            reader = null;
        }
        else
        {
            mw.ShowError("Заполните поля для регистрации: имя пользователя и пароль");
        }
    }

    public void Login()
    {
        if (userName.GetComponent<InputField>().text.Length != 0 && userPassword.GetComponent<InputField>().text.Length != 0)
        {
            string query = "SELECT * FROM users WHERE username = '" + userName.GetComponent<InputField>().text + "' " +
                                                 "AND password = '" + userPassword.GetComponent<InputField>().text + "'";
            Database db = GetComponent<Database>();
            db.OpenDataBase("testDB.db");
            reader = db.SelectQuery(query);
            if (!reader.HasRows)
            {
                mw.ShowError("Не удалось войти. Имя пользователя или пароль неверные.");
            }
            else
            {
                mw.userName = userName.GetComponent<InputField>().text;
                mw.CloseMainWindow();
                mw.OpenChooseCourseWindow();
                usernameText.text = userName.GetComponent<InputField>().text;//пишем в меню имя пользователя
                userName.GetComponent<InputField>().text = "";
                userPassword.GetComponent<InputField>().text = "";
            }
            reader.Close();
            reader = null;
        }
        else
        {
            mw.ShowError("Заполните поля для входа: имя пользователя и пароль");
        }
    }
}
