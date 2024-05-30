using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMaster : Singleton<AnimationMaster>
{
    //*******************************************************************************************//
    //***********************************      ATTRIBUTES      **********************************//
    //*******************************************************************************************//
    //**********************************    MAIN ATTRIBUTES    **********************************//
    public static Dictionary<string,IAnimable> animatedGODictionary = new Dictionary<string,IAnimable>();

    //*******************************************************************************************//
    //**************************************      MAIN      *************************************//
    //*******************************************************************************************//
    void Start(){
        
    }

    void Update(){
        
    }
    //*******************************************************************************************//
    //*************************************      METHODS      ***********************************//
    //*******************************************************************************************//  
    public void RegisterAnimables(){
        IAnimable[] animableArray = (IAnimable[])FindObjectsOfType (typeof(IAnimable));
		foreach (IAnimable animable in animableArray) {
            MonoBehaviour mb = animable as MonoBehaviour;
			if(mb != null){animatedGODictionary.Add(mb.gameObject.name,animable);}
		}
    }

    public static void RegisterAnimableAtInstantiate(GameObject animableGO){
        animatedGODictionary.Add(animableGO.name,animableGO.GetComponent<IAnimable>());
    }

    public static void AnimateGameObject(string gameObjectName, string animation){
        IAnimable animable = animatedGODictionary[gameObjectName];
        animable.AnimationStateMachine(animation);
    }  
}
