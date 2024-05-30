using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomSaveFileData
{
    [SerializeField] public SceneMaster.SaveData sceneMasterSaveData;
    [SerializeField] public PlayerMaster.SaveData playerMasterSaveData;
}

