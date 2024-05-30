using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainEquipableSO : ScriptableObject
{
    public enum EquipableType{SUPERIOR, INFERIOR, ACESORIO};
    [SerializeField] public EquipableType equipableType;
    public GameObject passiveContainer;
    public Texture2D sprite;
    public string name;
}
