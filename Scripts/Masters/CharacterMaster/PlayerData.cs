using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int ardid;
    public int erudicion;
    public int gracia;
    public int lucidez;
    public int templanza;
    public int valentia;

    public int ardidBoost;
    public int erudicionBoost;
    public int graciaBoost;
    public int lucidezBoost;
    public int templanzaBoost;
    public int valentiaBoost;

    public int ardidFinal;
    public int erudicionFinal;
    public int graciaFinal;
    public int lucidezFinal;
    public int templanzaFinal;
    public int valentiaFinal;

    public int ardidExp;
    public int erudicionExp;
    public int templanzaExp;
    public int graciaExp;
    public int lucidezExp;
    public int valentiaExp;

    public int stressLevel;
    public int stressBarValue;


    public float expBoost = 1f;
    public float stressExpBoost = 1f;


    public bool[] bottleCollectibles = new bool[12];
    public bool[] lighterCollectibles = new bool[12];

    public bool[] mainEquipableAccesoryCollectibles = new bool[3];
    public bool[] mainEquipableInferiorCollectibles = new bool[3];
    public bool[] mainEquipableSuperiorCollectibles = new bool[3];
    
    public PlayerMaster.MainEquipableAccesoryStruct actualMainAccesory;
    public PlayerMaster.MainEquipableInferiorStruct actualMainInferior;
    public PlayerMaster.MainEquipableSuperiorStruct actualMainSuperior;


    public float actualBankMoney;
    public float actualWalletMoney;

    public bool[] cardKeyCollectibles = new bool[10];
    public CardKeySO[] actualCards = new CardKeySO[3];

    public bool[] keyKeyCollectibles = new bool[10];
    public KeyKeySO[] actualKeys = new KeyKeySO[20];
}
