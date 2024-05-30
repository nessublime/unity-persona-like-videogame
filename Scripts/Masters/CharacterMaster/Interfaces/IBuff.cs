using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuff
{
    public IEnumerator ApplyBuff(int minutes, Texture2D sprite, string text);
}
