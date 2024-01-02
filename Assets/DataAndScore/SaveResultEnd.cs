using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using UnityEngine.UI;
//using System.Data;
//using Mono.Data.SqliteClient;
//using Microsoft.Data.Sqlite;

public class SaveResultEnd : MonoBehaviour
{
    public InputField NicknameInputField;
    public InputField EmailInputField;
    public InputField ScoreInputField;
    public Text textLeader;
    public Text errorText;
    private const string connectionString = "URI=file:myDatabase.db";
    int totalScore = WandererAI.levelOneScore + GoldFunctions.levelTwoScore;
    private Mono.Data.Sqlite.SqliteConnection connection = new Mono.Data.Sqlite.SqliteConnection(connectionString);

    private void Start()
    {
        ScoreInputField.text = totalScore.ToString();
    }

    private void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void SaveScore(string playerName, string email, int score)
    {
        // Открытие соединения с базой данных
        connection.Open();

        // Создание запроса на добавление данных
        string query = "INSERT INTO Scores (PlayerName, Email, Score) VALUES (@PlayerName, @Email, @Score)";
        SqliteCommand command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@PlayerName", playerName);
        command.Parameters.AddWithValue("@Email", email);
        command.Parameters.AddWithValue("@Score", score);

        // Выполнение запроса на добавление данных
        command.ExecuteNonQuery();

        // Закрытие соединения с базой данных
        connection.Close();
    }

    public void LoadScores()
    {
        // Открытие соединения с базой данных
        connection.Open();

        // Создание запроса на выборку данных
        string query = "SELECT PlayerName, Score FROM Scores ORDER BY Score DESC LIMIT 10";
        SqliteCommand command = new SqliteCommand(query, connection);
        SqliteDataReader reader = command.ExecuteReader();

        // Создание переменной для хранения вывода на экран таблицы лидеров
        string leaderboardText = "Таблица лидеров:\n";

        // Вывод данных на экран
        while (reader.Read())
        {
            string playerName = reader.GetString(0);
            int score = reader.GetInt32(1);
            leaderboardText += (playerName + ": " + score + "\n");
        }

        // Вывод таблицы лидеров на экран
        textLeader.text = leaderboardText;

        // Закрытие соединения с базой данных
        reader.Close();
        connection.Close();
    }

    public void CallSaveScore()
    {
        if (NicknameInputField.text != "" && EmailInputField.text != "")
        {
            SaveScore(NicknameInputField.text, EmailInputField.text, int.Parse(ScoreInputField.text));
            errorText.text = "";
        }
        else
        {
            errorText.text = "Не все значения заполнены";
        }
        
    }
}
