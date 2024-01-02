using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class Titer : MonoBehaviour
{
    public Text textField1;
    public Text textField2;
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.visible = true;
        StartCoroutine(ChangeTextValues());
    }

    void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    IEnumerator ChangeTextValues()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.0f);
            textField1.text = "�����������";
            textField2.text = "������� �����";
            yield return new WaitForSeconds(2.0f);
            textField1.text = "3D-��������";
            textField2.text = "����� �������";
            yield return new WaitForSeconds(2.0f);
            textField1.text = "���������";
            textField2.text = "������� �����";
            yield return new WaitForSeconds(2.0f);
            textField1.text = "�������� ����";
            textField2.text = "����� �������";
            yield return new WaitForSeconds(2.0f);
            textField1.text = "���������";
            textField2.text = "��������";
            yield return new WaitForSeconds(2.0f);
            button.gameObject.SetActive(true);
            textField1.gameObject.SetActive(false);
            textField2.gameObject.SetActive(false);
            yield break;
        }
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(5);
    }

}
