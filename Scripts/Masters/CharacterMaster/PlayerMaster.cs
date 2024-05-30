using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PlayerMaster: Singleton<PlayerMaster>, ISaveableComponent
{
    //*******************************************************************************************//
    //***********************************      ATTRIBUTES      **********************************//
    //*******************************************************************************************//
    //**********************************    MAIN ATTRIBUTES    **********************************//
    [Header("PREFABS")]
    [SerializeField] private GameObject playerCharacterGOPrefab;
    private Transform playerSpawnerSSGO;

    //Para pruebas
    public PasiveSO pasiveSO;
    public PasiveSO pasiveSO1;
    public PasiveSO pasiveSO2;
    public PasiveSO pasiveSO3;
    public PasiveSO pasiveSO4;

    public BuffSO buffSO1;
    public BuffSO buffSO2;
    public BuffSO buffSO3;
    public BuffSO buffSO4;
    public BuffSO buffSO5;
    public BuffSO buffSO6;

    public ConsumableSO consumable1;
    public ConsumableSO consumable2;
    public ConsumableSO consumable3;

    public MainEquipableAccesorioSO accesory;
    public MainEquipableSuperiorSO superior;
    public MainEquipableInferiorSO inferior;

    public MainEquipableSuperiorSO defaultMainSuperiorEquipable;
    public MainEquipableInferiorSO defaultMainInferiorEquipable;
    public MainEquipableAccesorioSO defaultMainAccesoryEquipable;


    //********************************    SAVEABLE ATTRIBUTES    ********************************//
    public static PlayerData playerData = new PlayerData();

    public static Dictionary<string,BuffStruct> actualBuffs = new Dictionary<string,BuffStruct>();
    public static Dictionary<string,PasiveStruct> actualPasives = new Dictionary<string,PasiveStruct>();
    public static List<string> actualBuffNames = new List<string>(5);
    public static List<string> actualPasiveNames = new List<string>();

    public struct PasiveStruct{
        public string name;
        public Texture2D image;
        public IPasive pasive;
        public string text;
        public PasiveStruct(string Name, Texture2D sprite, IPasive Pasive, string Text){
            name = Name;
            image = sprite;
            pasive = Pasive;
            text = Text;
        }
    }
    public struct BuffStruct{
        public string name;
        public int duration;
        public Texture2D image;
        public IBuff buff;
        public BuffStruct(string Name, Texture2D sprite, int Duration,IBuff Buff){
            name = Name;
            image = sprite;
            duration = Duration;
            buff = Buff;
        }
    }
    public struct MainEquipableAccesoryStruct{
        public string name;
        public Texture2D image;
        public IPasive equipable;
        public MainEquipableAccesoryStruct(string Name, Texture2D sprite, IPasive Equipable){
            name = Name;
            image = sprite;
            equipable = Equipable;
        }
    }
    public struct MainEquipableSuperiorStruct{
        public string name;
        public Texture2D image;
        public IPasive equipable;
        public MainEquipableSuperiorStruct(string Name, Texture2D sprite, IPasive Equipable){
            name = Name;
            image = sprite;
            equipable = Equipable;
        }
    }
    public struct MainEquipableInferiorStruct{
        public string name;
        public Texture2D image;
        public IPasive equipable;
        public MainEquipableInferiorStruct(string Name, Texture2D sprite, IPasive Equipable){
            name = Name;
            image = sprite;
            equipable = Equipable;
        }
    }
    //*********************************    STATIC ATTRIBUTES    *********************************//
    public static int playerSpawnerIndex = 0;

    //**********************************    TEST ATTRIBUTES    **********************************//
    [Serializable] public struct TestData{
        public int ardidTest;
        public int erudicionTest;
        public int graciaTest;
        public int lucidezTest;
        public int templanzaTest;
        public int valentiaTest;

        public int ardidExpTest;
        public int erudicionExpTest;
        public int templanzaExpTest;
        public int graciaExpTest;
        public int lucidezExpTest;
        public int valentiaExpTest;

        public int stressLevelTest;
        public int stressBarValueTest;
    }
    
    //*******************************************************************************************//
    //**************************************      MAIN      *************************************//
    //*******************************************************************************************//
    //***********************************    AWAKE & START    ***********************************//
    protected override void Awake(){
        base.Awake();
        
    }
    public void Start(){
        //ApplyPasive(pasiveSO);
        //ApplyPasive(pasiveSO1);
        //ApplyPasive(pasiveSO2);
        //ApplyPasive(pasiveSO3);
        //ApplyPasive(pasiveSO4);
        //ApplyBuff(buffSO2,4);
        //ApplyBuff(buffSO1,2);
        //ApplyBuff(buffSO3,3);
        //ApplyBuff(buffSO4,4);
        //ApplyBuff(buffSO5,5);
        //ApplyBuff(buffSO6,6);
        //TakeConsumable(consumable1);
        //TakeConsumable(consumable2);
        //TakeConsumable(consumable3);
        ApplyEquipable(accesory);
        ApplyEquipable(superior);
        ApplyEquipable(inferior);

        UIMaster.UpdateStatApp();
        UIMaster.UpdateStressTopPanel();
        UIMaster.UpdateCollectibleApp();
    }

    //*******************************************************************************************//
    //*************************************      METHODS      ***********************************//
    //*******************************************************************************************//
    //***********************************    PRIVATE METODS     *********************************//
    private void SpawnPlayer(int playerSpawnerIndex){
        GameObject Player = Instantiate(playerCharacterGOPrefab,playerSpawnerSSGO.transform.GetChild(playerSpawnerIndex).position,playerSpawnerSSGO.transform.GetChild(playerSpawnerIndex).rotation);
        Player.name = "Player";
    }

    //************************************    STATIC METODS     *********************************//
    public static void SetSpawnPlayerIndex(int newIndex){playerSpawnerIndex = newIndex;}

    public static int GetPlayerSpawnerIndex(){return playerSpawnerIndex;}

    public static int GetStat(string statName){
        if(statName == "Ardid"){return playerData.ardidFinal;}
        else if(statName == "Lucidez"){return playerData.lucidezFinal;}
        else if(statName == "Gracia"){return playerData.graciaFinal;}
        else if(statName == "Valentia"){return playerData.valentiaFinal;}
        else if(statName == "Templanza"){return playerData.templanzaFinal;}
        else if(statName == "Erudicion"){return playerData.erudicionFinal;}
        else{int a = 0; return a;}
    }

    public static int GetStatExp(string statName){
        if(statName == "Ardid"){return playerData.ardidExp;}
        else if(statName == "Lucidez"){return playerData.lucidezExp;}
        else if(statName == "Gracia"){return playerData.graciaExp;}
        else if(statName == "Valentia"){return playerData.valentiaExp;}
        else if(statName == "Templanza"){return playerData.templanzaExp;}
        else if(statName == "Erudicion"){return playerData.erudicionExp;}
        else{int a = 0; return a;}
    }

    public static void GetExp(string statName, int expAmount){
        expAmount = (int) (expAmount * playerData.expBoost);
        if(statName == "Ardid"){playerData.ardidExp = playerData.ardidExp + expAmount; if(playerData.ardidExp >= 100){StatLevelUp("Ardid");}}
        else if(statName == "Lucidez"){playerData.lucidezExp = playerData.lucidezExp + expAmount; if(playerData.lucidezExp >= 100){StatLevelUp("Lucidez");}}
        else if(statName == "Gracia"){playerData.graciaExp = playerData.graciaExp + expAmount; if(playerData.graciaExp >= 100){StatLevelUp("Gracia");}}
        else if(statName == "Valentia"){playerData.valentiaExp = playerData.valentiaExp + expAmount; if(playerData.valentiaExp >= 100){StatLevelUp("Valentia");}}
        else if(statName == "Templanza"){playerData.templanzaExp = playerData.templanzaExp + expAmount; if(playerData.templanzaExp >= 100){StatLevelUp("Templanza");}}
        else if(statName == "Erudicion"){playerData.erudicionExp = playerData.erudicionExp + expAmount; if(playerData.erudicionExp >= 100){StatLevelUp("Erudicion");}}
        UIMaster.UpdateStatApp();
    }

    private static void StatLevelUp(string statName){
        if(statName == "Ardid"){playerData.ardid++; playerData.ardidExp = playerData.ardidExp - 100;}
        else if(statName == "Lucidez"){playerData.lucidez++; playerData.lucidezExp = playerData.lucidezExp - 100;}
        else if(statName == "Gracia"){playerData.gracia++; playerData.graciaExp = playerData.graciaExp - 100;}
        else if(statName == "Valentia"){playerData.valentia++; playerData.valentiaExp = playerData.valentiaExp - 100;}
        else if(statName == "Templanza"){playerData.templanza++; playerData.templanzaExp = playerData.templanzaExp - 100;}
        else if(statName == "Erudicion"){playerData.erudicion++; playerData.erudicionExp = playerData.erudicionExp - 100;}
        UIMaster.UpdateStatApp();
    }

    public static void UpdateFinalStats(){
        playerData.erudicionFinal = playerData.erudicion + playerData.erudicionBoost;
        playerData.templanzaFinal = playerData.templanza + playerData.templanzaBoost;
        playerData.ardidFinal = playerData.ardid + playerData.ardidBoost;
        playerData.lucidezFinal = playerData.lucidez + playerData.lucidezBoost;
        playerData.valentiaFinal = playerData.valentia + playerData.valentiaBoost;
        playerData.graciaFinal = playerData.gracia + playerData.graciaBoost;
    }

    //************************************    STRESS METHODS    **********************************//
    public static void UpdateStressBar(int stressAmount){
        int diference = playerData.stressBarValue + stressAmount;
        if(diference >= 0 && diference < 100){playerData.stressBarValue = diference;}
        else if(diference >= 100){playerData.stressBarValue = diference - 100; playerData.stressLevel++;}
        else{playerData.stressBarValue = 0;}
    }

    public static void SetStressLevel(int level){
        if(level >= 0 && level <= 10){playerData.stressLevel = level; playerData.stressBarValue = 0;}
    }

    public static void UpdateStressLevel(int levelAmount){
        if(playerData.stressLevel + levelAmount <= 10 && playerData.stressLevel + levelAmount >= 0){playerData.stressLevel = playerData.stressLevel + levelAmount;}
        else if(playerData.stressLevel + levelAmount > 10){playerData.stressLevel = 10;}
        else if(playerData.stressLevel + levelAmount < 0){playerData.stressLevel = 0;}
    }
    public static int GetStressLevel(){return playerData.stressLevel;}

    public static int GetStressBarValue(){return playerData.stressBarValue; }

    //*********************************    BUFF&PASIVE METHODS    ********************************//
    public void ApplyBuff(BuffSO buff, int minutes){
        Texture2D buffSprite = buff.sprite;
        string buffInfoText = buff.buffInfo;
        Coroutine rutine = StartCoroutine(buff.buffContainer.GetComponent<IBuff>().ApplyBuff(minutes,buffSprite,buffInfoText));
        UpdateFinalStats();
    }

    public static void ApplyPasive(PasiveSO pasive){
        Texture2D pasiveSprite = pasive.sprite;
        string pasiveInfoText = pasive.pasiveInfo;
        pasive.pasiveContainer.GetComponent<IPasive>().ApplyPasive(pasiveSprite,pasiveInfoText);
        UpdateFinalStats();
        //UIMaster.UpdatePasivePanel();
    }

    public static void UnapplyPasive(PasiveSO pasive){
        pasive.pasiveContainer.GetComponent<IPasive>().UnapplyPasive();
        UpdateFinalStats();
        //UIMaster.UpdatePasivePanel();
    }

    //*********************************    CONSUMABLE METHODS    ********************************//
    public void TakeConsumable(ConsumableSO consumable){
        Coroutine rutine = StartCoroutine(consumable.consumableContainer.GetComponent<IBuff>().ApplyBuff(consumable.time,consumable.buffSprite,consumable.consumableBuffInfo));
        UpdateFinalStats();
    }

    //*********************************    COLLECTIBLE METHODS    ********************************//
    public static void GetCollectible(CollectibleSO collectible){
        if(collectible.collectibleType == CollectibleSO.CollectibleType.BOTTLE){
            BottleCollectibleSO bottleCollectible = (BottleCollectibleSO)collectible;
            playerData.bottleCollectibles[bottleCollectible.bottleID] = true;
        }else{
            LighterCollectibleSO lighterCollectible = (LighterCollectibleSO)collectible;
            playerData.lighterCollectibles[lighterCollectible.lighterID] = true;
        }
        UIMaster.UpdateCollectibleApp();
    }

    //**********************************    EQUIPABLE METHODS    *********************************//
    public static void GetEquipable(MainEquipableSO equipable){
        if(equipable.equipableType == MainEquipableSO.EquipableType.SUPERIOR){
            MainEquipableSuperiorSO superiorEquipable = (MainEquipableSuperiorSO) equipable;
            if(!playerData.mainEquipableSuperiorCollectibles[superiorEquipable.mainEquipableSuperiorID]){
                playerData.mainEquipableSuperiorCollectibles[superiorEquipable.mainEquipableSuperiorID] = true;
            }
        }else if(equipable.equipableType == MainEquipableSO.EquipableType.INFERIOR){
            MainEquipableInferiorSO inferiorEquipable = (MainEquipableInferiorSO) equipable;
            if(!playerData.mainEquipableInferiorCollectibles[inferiorEquipable.mainEquipableInferiorID]){
                playerData.mainEquipableInferiorCollectibles[inferiorEquipable.mainEquipableInferiorID] = true;
            }
        }else{
            MainEquipableAccesorioSO accesoryEquipable = (MainEquipableAccesorioSO) equipable;
            if(!playerData.mainEquipableAccesoryCollectibles[accesoryEquipable.mainEquipableAccesoryID]){
                playerData.mainEquipableAccesoryCollectibles[accesoryEquipable.mainEquipableAccesoryID] = true;
            }
        }
    }

    public void ApplyEquipable(MainEquipableSO equipable){
        if(equipable.equipableType == MainEquipableSO.EquipableType.SUPERIOR){
            Debug.Log("entre sup");
            playerData.actualMainSuperior = new MainEquipableSuperiorStruct(equipable.name,equipable.sprite,equipable.passiveContainer.GetComponent<IPasive>());
            ApplyEquipablePassive(equipable);
        }else if(equipable.equipableType == MainEquipableSO.EquipableType.INFERIOR){
            Debug.Log("entre inf");
            playerData.actualMainInferior = new MainEquipableInferiorStruct(equipable.name,equipable.sprite,equipable.passiveContainer.GetComponent<IPasive>());
            ApplyEquipablePassive(equipable);
        }else{
            Debug.Log("entre acc");
            playerData.actualMainAccesory = new MainEquipableAccesoryStruct(equipable.name,equipable.sprite,equipable.passiveContainer.GetComponent<IPasive>());
            ApplyEquipablePassive(equipable);
        }
        UIMaster.UpdateEquipationApp();
    }
    
    public void UnapplyEquipable(MainEquipableSO.EquipableType type){
        if(type ==  MainEquipableSO.EquipableType.SUPERIOR){
            Debug.Log("entre sup");
            UnapplyEquipablePassive(MainEquipableSO.EquipableType.SUPERIOR);
            playerData.actualMainSuperior = new MainEquipableSuperiorStruct(defaultMainSuperiorEquipable.name,defaultMainSuperiorEquipable.sprite,defaultMainSuperiorEquipable.passiveContainer.GetComponent<IPasive>());
        }else if(type == MainEquipableSO.EquipableType.INFERIOR){
            Debug.Log("entre sup");
            UnapplyEquipablePassive(MainEquipableSO.EquipableType.INFERIOR);
            playerData.actualMainInferior = new MainEquipableInferiorStruct(defaultMainInferiorEquipable.name,defaultMainSuperiorEquipable.sprite,defaultMainSuperiorEquipable.passiveContainer.GetComponent<IPasive>());
        }else{
            Debug.Log("entre sup");
            UnapplyEquipablePassive(MainEquipableSO.EquipableType.ACESORIO);
            playerData.actualMainAccesory = new MainEquipableAccesoryStruct(defaultMainAccesoryEquipable.name,defaultMainSuperiorEquipable.sprite,defaultMainSuperiorEquipable.passiveContainer.GetComponent<IPasive>());
        }
        UIMaster.UpdateEquipationApp();
    }

    public void ChangeEquipable(MainEquipableSO equipable){
        if(equipable.equipableType == MainEquipableSO.EquipableType.SUPERIOR){
            UnapplyEquipablePassive(MainEquipableSO.EquipableType.SUPERIOR);
            playerData.actualMainSuperior = new MainEquipableSuperiorStruct(equipable.name,equipable.sprite,equipable.passiveContainer.GetComponent<IPasive>());
            ApplyEquipablePassive(equipable);
        }else if(equipable.equipableType == MainEquipableSO.EquipableType.INFERIOR){
            UnapplyEquipablePassive(MainEquipableSO.EquipableType.INFERIOR);
            playerData.actualMainInferior = new MainEquipableInferiorStruct(equipable.name,equipable.sprite,equipable.passiveContainer.GetComponent<IPasive>());
            ApplyEquipablePassive(equipable);
        }else{
            UnapplyEquipablePassive(MainEquipableSO.EquipableType.ACESORIO);
            playerData.actualMainAccesory = new MainEquipableAccesoryStruct(equipable.name,equipable.sprite,equipable.passiveContainer.GetComponent<IPasive>());
            ApplyEquipablePassive(equipable);
        }
        UIMaster.UpdateEquipationApp();
    }

    public void ApplyEquipablePassive(MainEquipableSO equipable){
        Texture2D pasiveSprite = equipable.sprite;
        string pasiveInfoText = "";
        equipable.passiveContainer.GetComponent<IPasive>().ApplyPasive(pasiveSprite,pasiveInfoText);
        UpdateFinalStats();
    }

    public void UnapplyEquipablePassive(MainEquipableSO.EquipableType type){
        if(type == MainEquipableSO.EquipableType.SUPERIOR){
            playerData.actualMainSuperior.equipable.UnapplyPasive();
            UpdateFinalStats();
        }else if(type == MainEquipableSO.EquipableType.INFERIOR){
            playerData.actualMainInferior.equipable.UnapplyPasive();
            UpdateFinalStats();
        }else{
            playerData.actualMainAccesory.equipable.UnapplyPasive();
            UpdateFinalStats();
        }
    }

    //***********************************    KEY ITEM METHODS    *********************************//
    public void GetMoneyToWallet(int moneyAmount){
        playerData.actualWalletMoney += moneyAmount;
    }
    
    public void GetMoneyToBank(int moneyAmount){
        playerData.actualBankMoney += moneyAmount;
    }

    public void SpendMoneyFromWallet(int moneyAmount){
        playerData.actualWalletMoney -= moneyAmount;
    }

    public void SpendMoneyFromBank(int moneyAmount){
        playerData.actualBankMoney -= moneyAmount;
    }


    public void GetCardKeyItem(CardKeySO card){
        playerData.cardKeyCollectibles[card.cardID] = true;
    }

    public void ChangeActualCard(int pos, CardKeySO newCard){
        playerData.actualCards[pos] = newCard;
    }


    public void GetKeyKeyItem(KeyKeySO key){
        playerData.keyKeyCollectibles[key.keyID] = true;
    }
    public void UseKeyItem(int keyIndex){
        playerData.actualKeys[keyIndex].uses--;
        if(playerData.actualKeys[keyIndex].uses == 0){
            //playerData.actualKeys.Remove(playerData.actualKeys[keyIndex]);
        }
    }
    //************************************    EVENT METHODS     **********************************//
    public void SpawnPlayerAtSceneChange(){
        playerSpawnerSSGO = GameObject.Find("PlayerSpawner").transform;
        SpawnPlayer(playerSpawnerIndex);
        playerSpawnerIndex = 0;
    }

    //*******************************     SAVE SYSTEM METHODS     *******************************//

    public object CreateState(){return GameObject.Find("GameMaster").GetComponent<GameMaster>().customSaveFileData.playerMasterSaveData;}

    public object SaveState(){
        return new SaveData(){
            ardid = playerData.ardid,
            erudicion = playerData.erudicion,
            gracia = playerData.gracia,
            lucidez = playerData.lucidez,
            templanza = playerData.templanza,
            valentia = playerData.valentia,
            ardidExp = playerData.ardidExp,
            erudicionExp = playerData.erudicionExp,
            graciaExp = playerData.graciaExp,
            lucidezExp = playerData.lucidezExp,
            templanzaExp = playerData.templanzaExp,
            valentiaExp = playerData.valentiaExp,

            stressLevel = playerData.stressLevel,
            stressBarValue = playerData.stressBarValue
        };
    }

    public void LoadState(object state){
        var saveData = (SaveData)state;
        playerData.ardid = saveData.ardid;
        playerData.erudicion = saveData.erudicion;
        playerData.gracia = saveData.gracia;
        playerData.lucidez = saveData.lucidez;
        playerData.templanza = saveData.templanza;
        playerData.valentia = saveData.valentia;
        playerData.ardidExp = saveData.ardidExp;
        playerData.erudicionExp = saveData.erudicionExp;
        playerData.graciaExp = saveData.graciaExp;
        playerData.lucidezExp = saveData.lucidezExp;
        playerData.templanzaExp = saveData.templanzaExp;
        playerData.valentiaExp = saveData.valentiaExp;

        playerData.stressLevel = saveData.stressLevel;
        playerData.stressBarValue = saveData.stressBarValue;
    }
    
    [Serializable] public struct SaveData{
        public int ardid;
        public int erudicion;
        public int gracia;
        public int lucidez;
        public int templanza;
        public int valentia;
        public int ardidExp;
        public int erudicionExp;
        public int graciaExp;
        public int lucidezExp;
        public int templanzaExp;
        public int valentiaExp;

        public int stressLevel;
        public int stressBarValue;
    }

    //**********************************     TEST METHODS     ***********************************//
    public static void TestPlayMode(TestData testData){
        playerData.ardid = testData.ardidTest;
        playerData.erudicion = testData.erudicionTest;
        playerData.gracia = testData.graciaTest;
        playerData.lucidez = testData.lucidezTest;
        playerData.templanza = testData.templanzaTest;
        playerData.valentia = testData.valentiaTest;
        playerData.ardidExp = testData.ardidExpTest;
        playerData.erudicionExp = testData.erudicionExpTest;
        playerData.graciaExp = testData.graciaExpTest;
        playerData.lucidezExp = testData.lucidezExpTest;
        playerData.templanzaExp = testData.templanzaExpTest;
        playerData.valentiaExp = testData.valentiaExpTest;

        playerData.stressLevel = testData.stressLevelTest;
        playerData.stressBarValue = testData.stressBarValueTest;
    }
}
