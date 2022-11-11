using BepInEx;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using R2API;
using R2API.Utils;
using System.Collections.Generic;
using UnityEngine;
using EntityStates;
using IL.EntityStates.Huntress.HuntressWeapon;
using EntityStates.Huntress.HuntressWeapon;

namespace DeathMarkFix
{
    //Loads R2API Submodules
    [R2APISubmoduleDependency(nameof(LanguageAPI))]

    //This is an example plugin that can be put in BepInEx/plugins/ExamplePlugin/ExamplePlugin.dll to test out.
    //It's a small plugin that adds a relatively simple item to the game, and gives you that item whenever you press F2.

    //This attribute is required, and lists metadata for your plugin.
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]

    //This is the main declaration of our plugin class. BepInEx searches for all classes inheriting from BaseUnityPlugin to initialize on startup.
    //BaseUnityPlugin itself inherits from MonoBehaviour, so you can use this as a reference for what you can declare and use in your plugin class: https://docs.unity3d.com/ScriptReference/MonoBehaviour.html
    public class HuntressBuff : BaseUnityPlugin
    {
        //The Plugin GUID should be a unique ID for this plugin, which is human readable (as it is used in places like the config).
        //If we see this PluginGUID as it is on thunderstore, we will deprecate this mod. Change the PluginAuthor and the PluginName !
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "OakPrime";
        public const string PluginName = "HuntressBuff";
        public const string PluginVersion = "0.1.0";

        private readonly Dictionary<string, string> DefaultLanguage = new Dictionary<string, string>();

        //The Awake() method is run at the very start when the game is initialized.
        public void Awake()
        {
            try
            {
                RoR2.RoR2Application.onLoad += () =>
                {
                    EntityStates.Huntress.HuntressWeapon.ThrowGlaive.damageCoefficientPerBounce = 1.2f;
                    EntityStates.Huntress.HuntressWeapon.ThrowGlaive.glaiveProcCoefficient = 1.0f;
                };
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message + " - " + e.StackTrace);
            };
        }
        private void ReplaceDeathMarkText()
        {
            this.ReplaceString("ITEM_DEATHMARK_PICKUP", "Enemies with 3 or more debuffs are marked for death, taking bonus damage.");
            this.ReplaceString("ITEM_DEATHMARK_DESC", "Enemies with <style=cIsDamage>3</style> or more debuffs are <style=cIsDamage>marked for death</style>" +
                ", increasing damage taken by <style=cIsDamage>35%</style> from all sources for <style=cIsUtility>7</style> <style=cStack>(+7 per stack)</style> seconds.");
        }

        private void ReplaceString(string token, string newText)
        {
            this.DefaultLanguage[token] = Language.GetString(token);
            LanguageAPI.Add(token, newText);
        }
    }
}
