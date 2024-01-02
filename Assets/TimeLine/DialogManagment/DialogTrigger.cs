using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogTrigger : MonoBehaviour
{
    public Dialog dialog;
    public GameObject dialogMenu;
    public GameObject myObject;
    public GameObject portalTriggerOn;

    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<DialogMange>().StartDialog(dialog);
        if (dialogMenu != null)
        {
            dialogMenu.SetActive(true); // �������� canvas
        }
    }
    private void OnTriggerExit(Collider other)
    {
        FindObjectOfType<DialogMange>().StartDialog(dialog);
        if (dialogMenu != null)
        {
            dialogMenu.SetActive(false); // �������� canvas
            myObject.SetActive(true);
            portalTriggerOn.SetActive(true);
        }
    }
}
