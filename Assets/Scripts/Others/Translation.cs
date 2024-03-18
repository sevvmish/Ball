using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Translations", menuName = "Languages", order = 1)]
public class Translation : ScriptableObject
{
    public string Level;
    public string WinText;

    public string Tutorial1_sign1;
    public string Tutorial1_sign2;


    public Translation() { }
}
