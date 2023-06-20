global using ServerSend = MonoBehaviourPublicInInUnique;
global using SteamManager = MonoBehaviourPublicObInUIgaStCSBoStcuCSUnique;
global using LobbyManager = MonoBehaviourPublicCSDi2UIInstObUIloDiUnique;
global using Chatbox = MonoBehaviourPublicRaovTMinTemeColoonCoUnique;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using SteamworksNative;
using System.IO;
using System.Collections.Generic;
using BepInEx.IL2CPP;

namespace MoneyDrop
{
    [BepInPlugin("DRNL.DropMoney", "DropMoney", "1.0.0")]
    public class MainPlugin : BasePlugin
    {
        public static string[] cmd_Args;
        public static int countmoney = 0;
        public static ulong GetMyID()
        {
            return SteamManager.Instance.field_Private_CSteamID_0.m_SteamID;
        }
        public static int GetArgInt(string message)
        {
            return System.Int32.Parse(message);
        }
        public static ulong GetHostID()
        {
            return SteamManager.Instance.field_Private_CSteamID_1.m_SteamID;
        }
        public static bool IsHost()
        {
            return SteamManager.Instance.IsLobbyOwner() && !LobbyManager.Instance.Method_Public_Boolean_0();
        }
        public static int GetModeID()
        {
            return LobbyManager.Instance.gameMode.id;
        }


        public override void Load()
        {
            Harmony.CreateAndPatchAll(typeof(MainPlugin));
        }

        [HarmonyPatch(typeof(SteamManager), nameof(SteamManager.Update))]
        [HarmonyPostfix]
        public static void SteamManagerUpdate(SteamManager __instance)
        {
            if (countmoney > 0)
            {
                countmoney--;
                ServerSend.DropMoney(GetHostID(), 1, 0);
            }

        }

        [HarmonyPatch(typeof(ServerSend), nameof(ServerSend.SendChatMessage))]
        [HarmonyPrefix]
        public static bool ServerSendSendChatMessagePost(ulong param_0, string param_1)
        {
            if (IsHost() || GetModeID() == 13)
            {

                cmd_Args = param_1.Split(' ');
                string msg = cmd_Args[0].ToLower();
                if (param_0 == GetMyID() && msg.StartsWith("!"))
                {
                    switch (msg)
                    {
                        case "!dmoney":
                            int n = GetArgInt(cmd_Args[1]);
                            int[] uselessar = new int[n];
                            foreach (int damn in uselessar)
                            {
                                ServerSend.DropMoney(GetHostID(), 1, 0);
                            }
                            break;
                        case "!wmoney":
                            countmoney = GetArgInt(cmd_Args[1]);
                            break;
                        default:
                            Chatbox.Instance.AppendMessage(1, "Invalid Command, try !dmoney or !wmoney", "");
                            break;
                    }
                    return false;
                }
                else return true;
            }
            return true;
        }

        [HarmonyPatch(typeof(MonoBehaviourPublicGataInefObInUnique), "Method_Private_Void_GameObject_Boolean_Vector3_Quaternion_0")]
        [HarmonyPatch(typeof(MonoBehaviourPublicCSDi2UIInstObUIloDiUnique), "Method_Private_Void_0")]
        [HarmonyPatch(typeof(MonoBehaviourPublicVesnUnique), "Method_Private_Void_0")]
        [HarmonyPatch(typeof(MonoBehaviourPublicObjomaOblogaTMObseprUnique), "Method_Public_Void_PDM_2")]
        [HarmonyPatch(typeof(MonoBehaviourPublicTeplUnique), "Method_Private_Void_PDM_32")]
        [HarmonyPrefix]
        public static bool Prefix(System.Reflection.MethodBase __originalMethod)
        {
            return false;
        }
    }
}

