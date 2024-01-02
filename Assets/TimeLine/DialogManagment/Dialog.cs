using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    public string[] characterNames;

    [TextArea(5, 12)]
    public string[] sentences;
}
