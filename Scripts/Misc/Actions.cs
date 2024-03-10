using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Profiling;

public static class Actions 
{
    // Inventory Actions
    public static Action<Describable> UpdateInventory;
    public static Action<string, bool> UpdateItemEquipped;

}