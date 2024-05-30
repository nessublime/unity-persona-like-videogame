using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSO : ScriptableObject
{
    public enum CollectibleType{BOTTLE, LIGHTER};
    [SerializeField] public CollectibleType collectibleType;
    public Sprite sprite;
    public string name;
}
