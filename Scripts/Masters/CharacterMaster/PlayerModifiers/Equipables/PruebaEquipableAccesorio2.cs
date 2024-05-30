using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaEquipableAccesorio2 : MonoBehaviour, IPasive
{
    public Texture2D image;

    public void ApplyPasive(Texture2D sprite, string text){
        PlayerMaster.playerData.graciaBoost += 2; 
        UIMaster.UpdatePasivePanel();
    }
    public void UnapplyPasive(){
        PlayerMaster.playerData.graciaBoost -= 2; 
        UIMaster.UpdatePasivePanel();
    }
}
