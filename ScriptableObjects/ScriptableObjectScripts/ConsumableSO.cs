using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewConsumable", menuName = "ScriptableObject/ConsumableSO")]
public class ConsumableSO : ScriptableObject
{
    public Texture2D buffSprite; 
    public Texture2D consumableSprite;
    public GameObject consumableContainer;
    public int time;
    [field: TextArea()] public string consumableInfo;
    [field: TextArea()] public string consumableBuffInfo;
}
