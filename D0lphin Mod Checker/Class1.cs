using System;
using UnityEngine;
using HarmonyLib;
using BepInEx;

namespace D0lphin_Mod_Checker
{
    [BepInPlugin("D0lphin.Checker","D0lphin Mod Checker", "0.0.1")]
    public class Class1 :BaseUnityPlugin
    {
        void Awake()
        {
            Harmony harmony = new Harmony("D0lphin.Checker");
            harmony.PatchAll();

            Debug.Log("Loaded");
        }

        void Update()
        {
            if (ControllerInputPoller.instance.leftControllerPrimaryButton)
            {
                Debug.Log("Pressed");
            }
        }

    }
}
