using HarmonyLib;
using TotF;
using UnityEngine;

namespace RefMod.Patches
{
    [HarmonyPatch(typeof(EnemyController))]
    public static class EnemyControllerPatch
    {
        /// <summary>
        /// Set up the referees stats and such
        /// </summary>
        [HarmonyPatch(nameof(EnemyController.Awake))]
        [HarmonyPrefix]
        public static void Awake_Prefix(EnemyController __instance)
        {
            if (Main.fighterIsRef)
            {
                __instance.enemyDefeatName = "refereeDefeat";
                __instance.enemyName = "The Referee";
                __instance.canBleed = false;
                __instance.cutoutPhoto = Main.assets.LoadAsset<Texture2D>("CutOut");
                __instance.farthestDistance = 999;
                __instance.closestDistance = 0;
                __instance.baseMovementReactionTime = 0.01f;
                __instance.baseTargetObservationTime = 0.01f;
                __instance.attackSpeed = 1.2f;
                __instance.effectiveMassModifier = 1.025f;
                __instance.defenseModifier = 1.5f;
                __instance.punchStaminaCostModifier = 0.75f;
            }
        }

        /// <summary>
        /// Set up the visuals by importing a new armature and setting all of the bodyparts to the exact same transform as the original boxers body parts.
        /// Also disables the original boxers mesh
        /// </summary>
        [HarmonyPatch(nameof(EnemyController.Start))]
        [HarmonyPostfix]
        public static void Start_Postfix(EnemyController __instance)
        {
            if (Main.fighterIsRef)
            {
                GameObject boxer = __instance.transform.parent.gameObject;

                //Create the referee and set it up
                Transform referee = GameObject.Instantiate(Main.assets.LoadAsset<GameObject>("Referee"), boxer.transform.position, boxer.transform.rotation).transform;
                referee.gameObject.AddComponent<TransformToTarget>().target = boxer.transform;

                Transform boxerArmature = __instance.transform.Find("Armature");

                //Read method summary
                SetupArmature(ref referee, ref boxerArmature);

                //Set Materials and shaders
                foreach (SkinnedMeshRenderer renderer in referee.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    renderer.materials = RefereeController.Instance.GetComponentInChildren<SkinnedMeshRenderer>().materials;
                    renderer.sharedMaterials = RefereeController.Instance.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials;
                }

                GameObject.Find("glove").GetComponent<MeshRenderer>().materials[0].shader = Shader.Find("TotF/StandardDynamic");
                GameObject.Find("glove (1)").GetComponent<MeshRenderer>().materials[0].shader = Shader.Find("TotF/StandardDynamic");

                //Disable the original mesh of the boxer and mess with the original referee
                __instance.transform.Find("Mesh").gameObject.SetActive(false);
                foreach (Collider collider in RefereeController.Instance.GetComponentsInChildren<Collider>())
                    Object.Destroy(collider);
                foreach (AudioSource source in RefereeController.Instance.GetComponentsInChildren<AudioSource>())
                    Object.Destroy(source);
                RefereeController.Instance.transform.Find("Mesh").gameObject.SetActive(false);
                RefereeController.Instance.transform.Find("Referee Shadow").gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Sets up the imported armature to follow the original boxers armature
        /// </summary>
        public static void SetupArmature(ref Transform referee, ref Transform boxerArmature)
        {
            //Identify each transform of the referees rig
            Transform refHips = referee.Find("mixamorig:Hips"); Main.logger.LogMessage("Transform set");

            Transform refLeftUpLeg = refHips.Find("mixamorig:LeftUpLeg"); Main.logger.LogMessage("Transform set");
            Transform refLeftLeg = refLeftUpLeg.Find("mixamorig:LeftLeg"); Main.logger.LogMessage("Transform set");
            Transform refLeftFoot = refLeftLeg.Find("mixamorig:LeftFoot"); Main.logger.LogMessage("Transform set");
            Transform refLeftToeBase = refLeftFoot.Find("mixamorig:LeftToeBase"); Main.logger.LogMessage("Transform set");

            Transform refRightUpLeg = refHips.Find("mixamorig:RightUpLeg"); Main.logger.LogMessage("Transform set");
            Transform refRightLeg = refRightUpLeg.Find("mixamorig:RightLeg"); Main.logger.LogMessage("Transform set");
            Transform refRightFoot = refRightLeg.Find("mixamorig:RightFoot"); Main.logger.LogMessage("Transform set");
            Transform refRightToeBase = refRightFoot.Find("mixamorig:RightToeBase"); Main.logger.LogMessage("Transform set");

            Transform refSpine = refHips.Find("mixamorig:Spine"); Main.logger.LogMessage("Transform set");
            Transform refSpine1 = refSpine.Find("mixamorig:Spine1"); Main.logger.LogMessage("Transform set");
            Transform refSpine2 = refSpine1.Find("Spine2"); Main.logger.LogMessage("Transform set");

            Transform refLeftShoulder = refSpine2.Find("mixamorig:LeftShoulder"); Main.logger.LogMessage("Transform set");
            Transform refLeftArm = refLeftShoulder.Find("mixamorig:LeftArm"); Main.logger.LogMessage("Transform set");
            Transform refLeftForeArm = refLeftArm.Find("LeftForeArm"); Main.logger.LogMessage("Transform set");
            Transform refLeftHand = refLeftForeArm.Find("LeftHand"); Main.logger.LogMessage("Transform set");

            Transform refRightShoulder = refSpine2.Find("mixamorig:RightShoulder"); Main.logger.LogMessage("Transform set");
            Transform refRightArm = refRightShoulder.Find("mixamorig:RightArm"); Main.logger.LogMessage("Transform set");
            Transform refRightForeArm = refRightArm.Find("RightForeArm"); Main.logger.LogMessage("Transform set");
            Transform refRightHand = refRightForeArm.Find("RightHand"); Main.logger.LogMessage("Transform set");

            Transform refNeck = refSpine2.Find("mixamorig:Neck"); Main.logger.LogMessage("Transform set");
            Transform refHead = refNeck.Find("Head"); Main.logger.LogMessage("Transform set");



            //Identify each transform of the boxers rig
            Transform boxerHips = boxerArmature.Find("mixamorig:Hips"); Main.logger.LogMessage("Transform set");

            Transform boxerLeftUpLeg = boxerHips.Find("mixamorig:LeftUpLeg"); Main.logger.LogMessage("Transform set");
            Transform boxerLeftLeg = boxerLeftUpLeg.Find("mixamorig:LeftLeg"); Main.logger.LogMessage("Transform set");
            Transform boxerLeftFoot = boxerLeftLeg.Find("mixamorig:LeftFoot"); Main.logger.LogMessage("Transform set");
            Transform boxerLeftToeBase = boxerLeftFoot.Find("mixamorig:LeftToeBase"); Main.logger.LogMessage("Transform set");

            Transform boxerRightUpLeg = boxerHips.Find("mixamorig:RightUpLeg"); Main.logger.LogMessage("Transform set");
            Transform boxerRightLeg = boxerRightUpLeg.Find("mixamorig:RightLeg"); Main.logger.LogMessage("Transform set");
            Transform boxerRightFoot = boxerRightLeg.Find("mixamorig:RightFoot"); Main.logger.LogMessage("Transform set");
            Transform boxerRightToeBase = boxerRightFoot.Find("mixamorig:RightToeBase"); Main.logger.LogMessage("Transform set");

            Transform boxerSpine = boxerHips.Find("mixamorig:Spine"); Main.logger.LogMessage("Transform set");
            Transform boxerSpine1 = boxerSpine.Find("mixamorig:Spine1"); Main.logger.LogMessage("Transform set");
            Transform boxerSpine2 = boxerSpine1.Find("mixamorig:Spine2"); Main.logger.LogMessage("Transform set");

            Transform boxerLeftShoulder = boxerSpine2.Find("mixamorig:LeftShoulder"); Main.logger.LogMessage("Transform set");
            Transform boxerLeftArm = boxerLeftShoulder.Find("mixamorig:LeftArm"); Main.logger.LogMessage("Transform set");
            Transform boxerLeftForeArm = boxerLeftArm.Find("mixamorig:LeftForeArm"); Main.logger.LogMessage("Transform set");
            Transform boxerLeftHand = boxerLeftForeArm.Find("mixamorig:LeftHand"); Main.logger.LogMessage("Transform set");

            Transform boxerRightShoulder = boxerSpine2.Find("mixamorig:RightShoulder"); Main.logger.LogMessage("Transform set");
            Transform boxerRightArm = boxerRightShoulder.Find("mixamorig:RightArm"); Main.logger.LogMessage("Transform set");
            Transform boxerRightForeArm = boxerRightArm.Find("mixamorig:RightForeArm"); Main.logger.LogMessage("Transform set");
            Transform boxerRightHand = boxerRightForeArm.Find("mixamorig:RightHand"); Main.logger.LogMessage("Transform set");

            Transform boxerNeck = boxerSpine2.Find("mixamorig:Neck"); Main.logger.LogMessage("Transform set");
            Transform boxerHead = boxerNeck.Find("mixamorig:Head"); Main.logger.LogMessage("Transform set");



            //Parent the referees rig to the boxers rig
            refHips.parent = boxerHips;
            refLeftUpLeg.parent = boxerLeftUpLeg;
            refLeftLeg.parent = boxerLeftLeg;
            refLeftFoot.parent = boxerLeftFoot;
            refLeftToeBase.parent = boxerLeftToeBase;
            refRightUpLeg.parent = boxerRightUpLeg;
            refRightLeg.parent = boxerRightLeg;
            refRightFoot.parent = boxerRightFoot;
            refRightToeBase.parent = boxerRightToeBase;
            refSpine.parent = boxerSpine;
            refSpine1.parent = boxerSpine1;
            refSpine2.parent = boxerSpine2;
            refLeftShoulder.parent = boxerLeftShoulder;
            refLeftArm.parent = boxerLeftArm;
            refLeftForeArm.parent = boxerLeftForeArm;
            refLeftHand.parent = boxerLeftHand;
            refRightShoulder.parent = boxerRightShoulder;
            refRightArm.parent = boxerRightArm;
            refRightForeArm.parent = boxerRightForeArm;
            refRightHand.parent = boxerRightHand;
            refNeck.parent = boxerNeck;
            refHead.parent = boxerHead;
        }
    }
}