using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaEquipableInferior2 : MonoBehaviour, IPasive
{
    public Texture2D image;

    public void ApplyPasive(Texture2D sprite, string text){
        PlayerMaster.playerData.valentiaBoost += 1; 
        UIMaster.UpdatePasivePanel();
    }
    public void UnapplyPasive(){
        PlayerMaster.playerData.valentiaBoost -= 1; 
        UIMaster.UpdatePasivePanel();
    }
}
