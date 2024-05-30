using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPasive
{
    public void ApplyPasive(Texture2D sprite, string text);
    public void UnapplyPasive();
}
