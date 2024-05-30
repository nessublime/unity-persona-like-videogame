using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaEquipableAccesorio : MonoBehaviour, IPasive
{
    public Texture2D image;

    public void ApplyPasive(Texture2D sprite, string text){
        PlayerMaster.playerData.ardidBoost += 1; 
        UIMaster.UpdatePasivePanel();
    }
    public void UnapplyPasive(){
        PlayerMaster.playerData.ardidBoost -= 1; 
        UIMaster.UpdatePasivePanel();
    }
}
