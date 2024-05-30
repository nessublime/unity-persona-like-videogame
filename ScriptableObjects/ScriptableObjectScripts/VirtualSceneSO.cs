using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewVirtualScene", menuName = "ScriptableObject/VirtualSceneSO")]
public class VirtualSceneSO : ScriptableObject
{
    //###########################################################################################//
    //###############################     MAIN ATRIBUTES     ####################################//
    //###########################################################################################//
    [Header("MAIN VIRTUAL SCENE ATRIBUTES")]
    public int sceneIndex;
    public string sceneName;
}
