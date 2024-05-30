using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuff", menuName = "ScriptableObject/BuffSO")]
public class BuffSO : ScriptableObject
{
    public Texture2D sprite; 
    public GameObject buffContainer;
    [field: TextArea()] public string buffInfo;
}
