using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveableEntity : MonoBehaviour
{   
    //*******************************************************************************************//
    //***********************************      ATTRIBUTES      **********************************//
    //*******************************************************************************************//
    //**********************************    MAIN ATTRIBUTES    **********************************//
    [SerializeField] private string id;
    public string ID => id;

    //*******************************************************************************************//
    //*************************************      METODS      ************************************//
    //*******************************************************************************************//
    [ContextMenu("Generate ID")] private void GenerateID(){id = Guid.NewGuid().ToString();}

    public object SaveState(){
        var state = new Dictionary<string, object>();
        foreach(var saveable in GetComponents<ISaveableComponent>()){state[saveable.GetType().ToString()] = saveable.SaveState();}
        return state;
    }

    public object CreateState(){
        var state = new Dictionary<string, object>();
        foreach(var saveable in GetComponents<ISaveableComponent>()){state[saveable.GetType().ToString()] = saveable.CreateState();}
        return state;
    }
    
    public void LoadState(object state){
        var stateDictionary = (Dictionary<string, object>)state;
        foreach(var saveable in GetComponents<ISaveableComponent>()){ string typeName = saveable.GetType().ToString(); if(stateDictionary.TryGetValue(typeName, out object savedState)){ saveable.LoadState(savedState); }}
    }
}
