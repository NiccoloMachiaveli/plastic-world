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
        // �������� ���������� � ����� ������
        connection.Open();

        // �������� ������� �� ���������� ������
        string query = "INSERT INTO Scores (PlayerName, Email, Score) VALUES (@PlayerName, @Email, @Score)";
        SqliteCommand command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@PlayerName", playerName);
        command.Parameters.AddWithValue("@Email", email);
        command.Parameters.AddWithValue("@Score", score);

        // ���������� ������� �� ���������� ������
        command.ExecuteNonQuery();

        // �������� ���������� � ����� ������
        connection.Close();
    }

    public void LoadScores()
    {
        // �������� ���������� � ����� ������
        connection.Open();

        // �������� ������� �� ������� ������
        string query = "SELECT PlayerName, Score FROM Scores ORDER BY Score DESC LIMIT 10";
        SqliteCommand command = new SqliteCommand(query, connection);
        SqliteDataReader reader = command.ExecuteReader();

        // �������� ���������� ��� �������� ������ �� ����� ������� �������
        string leaderboardText = "������� �������:\n";

        // ����� ������ �� �����
        while (reader.Read())
        {
            string playerName = reader.GetString(0);
            int score = reader.GetInt32(1);
            leaderboardText += (playerName + ": " + score + "\n");
        }

        // ����� ������� ������� �� �����
        textLeader.text = leaderboardText;

        // �������� ���������� � ����� ������
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
            errorText.text = "�� ��� �������� ���������";
        }
        
    }
}
