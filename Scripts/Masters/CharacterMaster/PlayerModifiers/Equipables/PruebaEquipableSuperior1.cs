using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaEquipableSuperior1 : MonoBehaviour, IPasive
{
    public Texture2D image;

    public void ApplyPasive(Texture2D sprite, string text){
        PlayerMaster.playerData.lucidezBoost += 1; 
        UIMaster.UpdatePasivePanel();
    }
    public void UnapplyPasive(){
        PlayerMaster.playerData.lucidezBoost -= 1; 
        UIMaster.UpdatePasivePanel();
    }
}
