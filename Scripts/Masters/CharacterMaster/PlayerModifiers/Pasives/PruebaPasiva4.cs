using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaPasiva4 : MonoBehaviour, IPasive
{
    public Texture2D image;
    public string pasiveName;
    PlayerMaster.PasiveStruct pasiveStruct;

    public void ApplyPasive(Texture2D sprite, string text){
        if(!PlayerMaster.actualPasives.ContainsKey(pasiveName)){
            pasiveStruct = new PlayerMaster.PasiveStruct(pasiveName,sprite,this,text);
            PlayerMaster.actualPasives.Add(pasiveName,pasiveStruct);
            PlayerMaster.actualPasiveNames.Add(pasiveName);

            PlayerMaster.playerData.graciaBoost += 2; 

            UIMaster.UpdatePasivePanel();
        }    
    }
    public void UnapplyPasive(){
        if(!PlayerMaster.actualPasives.ContainsKey(pasiveName)){
            PlayerMaster.actualPasives.Remove(pasiveName);
            PlayerMaster.actualPasiveNames.Remove(pasiveName);
            PlayerMaster.playerData.graciaBoost -= 2; 
            UIMaster.UpdatePasivePanel();
        }    
    }
}
