using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text BestScore;
    public Text text1;
    public Text text2;
    public Text text3;
    // Start is called before the first frame update
    void Start()
    {
        // �������� ��������������� ������� �������
        DataTable scoreboard = MyDataBase.GetTable("SELECT Total_Score.[id_player], nickname, email, total_score FROM Player, Total_Score WHERE Total_Score.[id_player]=Player.[id_player] ORDER BY Total_Score DESC;");
        // �������� id ������� ������
        int idBestPlayer = int.Parse(scoreboard.Rows[0][0].ToString());
        // �������� ��� ������� ������
        string nickname = MyDataBase.ExecuteQueryWithAnswer($"SELECT nickname FROM Player WHERE id_player = {idBestPlayer};");
        Debug.Log($"������ ����� {nickname} ������ {scoreboard.Rows[0][3]} �����.");
        BestScore.text = ($"�����        ���              Email                         ����    ");
        text1.text = ($"    {scoreboard.Rows[0][0]}           {scoreboard.Rows[0][1]}               {scoreboard.Rows[0][2]}         {scoreboard.Rows[0][3]}");
        text2.text = ($"    {scoreboard.Rows[1][0]}           {scoreboard.Rows[1][1]}      {scoreboard.Rows[1][2]}     {scoreboard.Rows[1][3]}");
        text3.text = ($"    {scoreboard.Rows[2][0]}           {scoreboard.Rows[2][1]}      {scoreboard.Rows[2][2]}          {scoreboard.Rows[2][3]}");
    }
}
