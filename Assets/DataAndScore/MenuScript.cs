using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Data;


public class MenuScript : MonoBehaviour
{
    public Text gameText;
    public Canvas TextView;
    public Canvas mainMenu;

    public Canvas registrationMenu;
    // Поля главного меню
    public InputField loginInput;
    public InputField passwordInput;
    public Button logInButton;
    public Button signInButton;

    // Поля меню регистрации 
    public InputField nicknameInput;
    public InputField loginInputReg;
    public InputField passwordInputReg;
    public InputField emailInput;
    public Button backBtn;
    public Button registratiionConnect;

    // Start is called before the first frame update
    private static string DBPath;
    // Имя базы данных
    static string dbPath = string.Format(@"Assets/StreamingAssets/{0}", "db.bytes");

    //private string dbName = "URI=file:db.bytes";
    [System.Obsolete]
    static string MyDataBase()
    {
        DBPath = GetDatabasePath();
        return DBPath;
    }

    /// <summary> Возвращает путь к БД. </summary>
    [System.Obsolete]
    private static string GetDatabasePath()
    {
        return Path.Combine(Application.streamingAssetsPath, dbPath);
    }
    void Start()
    {

        //RegistarationDB();
    }

    // Вход в базу данных
    [System.Obsolete]
    public void OpenDB()
    {
        if ((loginInput.text == "Nick" && passwordInput.text == "bank") || (loginInput.text == "Blob" && passwordInput.text == "Grad") || (loginInput.text == "Slark" && passwordInput.text == "DrowRanger") || (loginInput.text == "ecMilk" && passwordInput.text == "Iogurt") || (loginInput.text == "Mlgang" && passwordInput.text == "Tamuz") || (loginInput.text == "sdfjj" && passwordInput.text == "sdf") || (loginInput.text == loginInputReg.text && passwordInput.text == passwordInputReg.text))
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            Nuk();
            gameText.text = "Не верные данные";
        }
        using (var connection = new SqliteConnection("Data Source=" + dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT nickname, login, password, email FROM Player WHERE login = \"{loginInput.text}\" AND password = \"{passwordInput.text}\"";
                command.ExecuteNonQuery();
                
            }
            connection.Close();
        }

        

    }

    // Смена основного меню на меню регистрации
    public void RegistarationDB()
    {
        mainMenu.gameObject.SetActive(false);
        registrationMenu.gameObject.SetActive(true);
    }

    public void Nuk()
    {
        mainMenu.gameObject.SetActive(false);
        TextView.gameObject.SetActive(true);
    }
    public void OkButton()
    {
        TextView.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }

    // Кнопка назад
    public void BackButton()
    {
        mainMenu.gameObject.SetActive(true);
        registrationMenu.gameObject.SetActive(false);
    }

    // Регистрация нового пользователя
    public void RegistarationDBReg()
    {
        using (var connection = new SqliteConnection("Data Source=" + dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"INSERT INTO Player(nickname,login,password,email) VALUES ('{nicknameInput}','{loginInputReg}','{passwordInputReg}','{emailInput}')";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }
    
}
