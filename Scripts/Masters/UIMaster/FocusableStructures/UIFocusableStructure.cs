using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUIFocusableStructure", menuName = "ScriptableObject/UIFocusableStructure")]
public class UIFocusableStructure : ScriptableObject
{
    public List<string> focusableElementList;
    public ArrayLayout data;
}


