using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogMange : MonoBehaviour
{
    public Text nameText;
    public Text dialogText;

    private Queue<string> sentences; // предложение
    private string currentSpeaker; // текущий говорящий
    private Dialog currentDialog; // добавить поле currentDialog
    private bool isOutputtingSentence = false;
    private int currentSentenceIndex = 0; // добавляем переменную-счетчик

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialog(Dialog dialog)
    {
        currentDialog = dialog; // сохранить текущий диалог
        nameText.text = dialog.characterNames[0];
        
        sentences.Clear();
        currentSpeaker = dialog.characterNames[0];

        foreach (string sentence in dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0 || currentSentenceIndex == currentDialog.sentences.Length)
        {
            EndDialog();
            return;
        }

        string sentence = sentences.Dequeue();

        StartCoroutine(TypeSentence(sentence)); // Выводим текст на экран
        currentSpeaker = currentSpeaker == currentDialog.characterNames[0] ? currentDialog.characterNames[1] : currentDialog.characterNames[0];
        nameText.text = currentSpeaker;
        currentSentenceIndex++; // увеличиваем значение счетчика
    }


    IEnumerator TypeSentence(string sentence)
    {
        if (isOutputtingSentence) 
        {
            yield return new WaitUntil(() => !isOutputtingSentence); // если уже выводится предыдущее предложение, прерываем функцию
        } 

        isOutputtingSentence = true; // устанавливаем флаг вывода нового предложения

        dialogText.text = "";

        if (sentence.Length > 0 && sentence[0] != ' ')
        {
            dialogText.text += "";
        }

        foreach (char letter in sentence)
        {
            dialogText.text += letter;
            yield return null;
        }

        isOutputtingSentence = false;
    }


    public void EndDialog()
    {
        if (currentDialog != null)
        {
            Debug.Log("End");
        }
    }
}
