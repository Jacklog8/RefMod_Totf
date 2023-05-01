using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using TotF;
using UnityEngine;
using System.Reflection;
using System.IO;
using System;

namespace RefMod
{
    /// <summary>
    /// Entry point for the mod
    /// </summary>
    [BepInPlugin(myGUID, pluginName, versionString)]
    public class Main : BaseUnityPlugin
    {
        /// <summary>
        /// Identifing if you are fighting against the referee
        /// </summary>
        public static bool fighterIsRef = false;

        /// <summary>
        /// Asset bundle for loading models and such
        /// </summary>
        public static AssetBundle assets = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "refassets"));

        private const string myGUID = "com.JackLog.RefMod";
        private const string pluginName = "Referee Mod";
        private const string versionString = "1.0.0";

        private static readonly Harmony harmony = new Harmony(myGUID);

        /// <summary>
        /// Used for logging information
        /// </summary>
        public static ManualLogSource logger;

        /// <summary>
        /// Opponent details for the referee
        /// </summary>
        public static FightMenuManager.OpponentDetails referee = new FightMenuManager.OpponentDetails()
        {
            boxerName = "The Referee",
            boxerBio = "Nickname: F*F@,­!U)±|M]pî\nBirthplace: Mars",
            boutRules = "Format: ¿",
            boxerResourceName = "JackLog.referee",
            unlockCondition = "moneymakerDefeat",
            customizeCondition = "refereeDefeat",
            numberOfRounds = 3,
            roundTime = 180,
            sceneName = "Arena",
            boxerImage = assets.LoadAsset<Texture2D>("Poster")
        };

        private void Awake()
        {
            //Add in all of the patches to the game
            harmony.PatchAll();

            //Logging stuff
            Logger.LogInfo($"Referee mod version \"{versionString}\"was sucessfully loaded!");
            logger = Logger;
        }
    }
}