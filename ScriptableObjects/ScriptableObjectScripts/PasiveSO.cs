using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPasive", menuName = "ScriptableObject/PasiveSO")]
public class PasiveSO : ScriptableObject
{
    public Texture2D sprite;
    public GameObject pasiveContainer;
    [field: TextArea()] public string pasiveInfo;
}
