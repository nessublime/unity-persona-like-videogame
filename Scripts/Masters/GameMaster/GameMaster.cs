using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameMaster : Singleton<GameMaster>
{
    //*******************************************************************************************//
    //***********************************      ATTRIBUTES      **********************************//
    //*******************************************************************************************//
    //**********************************    MAIN ATTRIBUTES    **********************************//
    //[Header("MAIN ATRIBUTES")]
    public enum PlayMode{DEFAULT, TEST, CUSTOM};
    [SerializeField] public PlayMode actualPlayMode;
    [SerializeField] private List<GameObject> masterPrefabList;
    public CustomSaveFileData customSaveFileData;
    public TestSaveFileData testSaveFileData;
    
    //*******************************    SAVE SYSTEM ATTRIBUTES    ******************************//
    public string customPath => $"{Application.persistentDataPath}/custom.txt";
    public string save1Path => $"{Application.persistentDataPath}/save1.txt";
    public string save2Path => $"{Application.persistentDataPath}/save2.txt";
    public string save3Path => $"{Application.persistentDataPath}/save3.txt";
    public string newGamePath => $"{Application.persistentDataPath}/newGame.txt";

    //************************************    INPUT METHODS    ***********************************//
    public enum ActionMap{Player, UI};
    public static DefaultInputActions DefaultInputActions;
    private static ActionMap actualActionMap;

    //*******************************************************************************************//
    //**************************************      MAIN      *************************************//
    //*******************************************************************************************//
    //***********************************    AWAKE & START    ***********************************//
    protected override void Awake(){
        base.Awake();
        DefaultInputActions = new DefaultInputActions();
        DefaultInputActions.Player.Enable();
    }

    protected void Start(){
        switch(actualPlayMode){
            case PlayMode.DEFAULT: InstantiateMasters(); LoadFile(newGamePath);
                break;
            case PlayMode.TEST: InstantiateMasters(); TestPlayMode();
                break;       
            case PlayMode.CUSTOM: InstantiateMasters(); LoadFile(customPath); 
                break;        
        }
    }
    
    //*******************************************************************************************//
    //*************************************      METHODS      ***********************************//
    //*******************************************************************************************//
    private void InstantiateMasters(){foreach(GameObject masterPrefab in masterPrefabList){GameObject newMaster = Instantiate(masterPrefab, Vector3.zero, Quaternion.identity);}}

    //*********************************    SAVE SYSTEM METHODS    ********************************//
    [ContextMenu("Create NewGame Save File")] public void CreateNewGameSaveFile(){
        var state =  new Dictionary<string, object>();
        if(!File.Exists(newGamePath)){state =  new Dictionary<string, object>();}
        else{using(FileStream stream = File.Open(newGamePath,FileMode.Open)){var formatter = new BinaryFormatter(); state = (Dictionary<string, object>)formatter.Deserialize(stream);}}
        foreach(var saveable in FindObjectsOfType<SaveableEntity>()){state[saveable.ID] = saveable.CreateState();}
        using(var stream = File.Open(newGamePath,FileMode.Create)){var formatter = new BinaryFormatter(); formatter.Serialize(stream, state);}
    }

    [ContextMenu("Create Custom Save File")] public void CreateCustomSaveFile(){
        var state =  new Dictionary<string, object>();
        if(!File.Exists(customPath)){state =  new Dictionary<string, object>();}
        else{using(FileStream stream = File.Open(customPath,FileMode.Open)){var formatter = new BinaryFormatter(); state = (Dictionary<string, object>)formatter.Deserialize(stream);}}
        foreach(var saveable in FindObjectsOfType<SaveableEntity>()){state[saveable.ID] = saveable.CreateState();}
        using(var stream = File.Open(customPath,FileMode.Create)){var formatter = new BinaryFormatter(); formatter.Serialize(stream, state);}
    } 

    public void SaveFile(string filePath){
        var state =  new Dictionary<string, object>();
        if(!File.Exists(filePath)){state = new Dictionary<string, object>(); Debug.Log("No");}
        using(FileStream stream = File.Open(filePath,FileMode.Open)){var formatter = new BinaryFormatter(); state = (Dictionary<string, object>)formatter.Deserialize(stream);}
        foreach(var saveable in FindObjectsOfType<SaveableEntity>()){state[saveable.ID] = saveable.SaveState();}
        using(var stream = File.Open(filePath,FileMode.Create)){var formatter = new BinaryFormatter(); formatter.Serialize(stream, state);}
    }

    public void LoadFile(string filePath){
        var state =  new Dictionary<string, object>();
        if(!File.Exists(filePath)){state = new Dictionary<string, object>();}
        using(FileStream stream = File.Open(filePath,FileMode.Open)){var formatter = new BinaryFormatter(); state = (Dictionary<string, object>)formatter.Deserialize(stream);}
        foreach(var saveable in FindObjectsOfType<SaveableEntity>()){if(state.TryGetValue(saveable.ID,out object savedState)){saveable.LoadState(savedState);}}
    }

    //************************************    INPUT METHODS    ***********************************//
    public static void ChangeActionMap(ActionMap map){
        switch(map){
            case ActionMap.Player: DisableActualActionMap(); DefaultInputActions.Player.Enable();
                break;
            case ActionMap.UI: DisableActualActionMap(); DefaultInputActions.UI.Enable();
                break;    
        }
    }

    public static void DisableActualActionMap(){
        switch(actualActionMap){
            case ActionMap.Player: DefaultInputActions.Player.Disable();
                break;
            case ActionMap.UI:DefaultInputActions.UI.Disable();
                break;    
        }
    }



    public void TestPlayMode(){
        PlayerMaster.TestPlayMode(testSaveFileData.playerMasterTestData);
    }
}

    

























    //###########################################################################################//
    //#################################        OLD        #######################################//
    //###########################################################################################//
    /*[ContextMenu("Save")] public void Save(){
        var state = LoadFile();
        SaveState(state);
        SaveFile(state);
    }*/
    /*public void SaveFile(object state, string filePath){
        using(var stream = File.Open(filePath,FileMode.Create)){
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }*/
    /*void SaveState(Dictionary<string, object> state){
        foreach(var saveable in FindObjectsOfType<SaveableEntity>()){
            state[saveable.ID] = saveable.SaveState();
        }
    }*/
    /*Dictionary<string, object> LoadFile(){
        if(!File.Exists(saveFilePath)){return new Dictionary<string, object>();}
        using(FileStream stream = File.Open(saveFilePath,FileMode.Open)){var formatter = new BinaryFormatter();return (Dictionary<string, object>)formatter.Deserialize(stream);}
    }
    void LoadState(Dictionary<string, object> state){
        foreach(var saveable in FindObjectsOfType<SaveableEntity>()){
            if(state.TryGetValue(saveable.ID,out object savedState)){
                saveable.LoadState(savedState);
            }
        }
    }
    [ContextMenu("Load")]public void Load(){
        var state = LoadFile();
        LoadState(state);
    }*/


