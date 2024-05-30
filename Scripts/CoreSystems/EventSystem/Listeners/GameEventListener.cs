using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class GameEventListener<T,E,UER>:MonoBehaviour,IGameEventListener<T> where E:BaseGameEventSO<T> where UER:UnityEvent<T>
{
    //###########################################################################################//
    //##################################    ATRIBUTES    ########################################//
    //###########################################################################################//
    [SerializeField] private E gameEvent;
    public E GameEvent{get{return gameEvent;} set{gameEvent = value;}}
    [SerializeField] private UER unityEventResponse;
    //###########################################################################################//
    //##################################    METODS    ###########################################//
    //###########################################################################################//
    private void OnEnable(){
        if(gameEvent == null){return;}
        GameEvent.RegisterListener(this);
    }
    private void OnDisable(){
        if(gameEvent == null){return;}
        GameEvent.UnregisterListener(this);
    }
    public void OnEventRaised(T item){
        if(unityEventResponse != null){
            unityEventResponse.Invoke(item);
        }
    }
}
