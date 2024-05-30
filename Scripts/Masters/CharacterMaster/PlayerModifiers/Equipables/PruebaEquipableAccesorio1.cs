using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaEquipableAccesorio1 : MonoBehaviour, IPasive
{
    public Texture2D image;

    public void ApplyPasive(Texture2D sprite, string text){
        PlayerMaster.playerData.graciaBoost += 1; 
        UIMaster.UpdatePasivePanel();
    }
    public void UnapplyPasive(){
        PlayerMaster.playerData.graciaBoost -= 1; 
        UIMaster.UpdatePasivePanel();
    }
}
