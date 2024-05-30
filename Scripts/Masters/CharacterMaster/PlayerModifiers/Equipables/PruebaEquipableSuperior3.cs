using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaEquipableSuperior3 : MonoBehaviour, IPasive
{
    public Texture2D image;

    public void ApplyPasive(Texture2D sprite, string text){
        UIMaster.UpdatePasivePanel();
    }
    public void UnapplyPasive(){
        UIMaster.UpdatePasivePanel();
    }
}
