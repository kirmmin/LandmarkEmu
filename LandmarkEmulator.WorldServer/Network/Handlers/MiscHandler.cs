﻿using LandmarkEmulator.WorldServer.Network.Message;
using LandmarkEmulator.WorldServer.Network.Message.Model;
using Newtonsoft.Json;
using NLog;
using System;
using System.IO;

namespace LandmarkEmulator.WorldServer.Network.Handlers
{
    public class MiscHandler
    {
        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();

        [ZoneMessageHandler(ZoneMessageOpcode.ClientLog)]
        public static void HandleClientLog(WorldSession session, ClientLog clientLog)
        {
            log.Debug(JsonConvert.SerializeObject(clientLog, Formatting.Indented));
        }

        [ZoneMessageHandler(ZoneMessageOpcode.GameTimeSync)]
        public static void GameTimeSync(WorldSession session, GameTimeSync gameTimeSync)
        {
            session.EnqueueMessage(new GameTimeSync
            {
                Time = gameTimeSync.Time - 0.003d,
                CycleSpeed = 9.1f,
                Unknown0 = false
            });
        }

        [ZoneMessageHandler(ZoneMessageOpcode.ClientIsReady)]
        public static void ClientIsReady(WorldSession session, ClientIsReady clientIsReady)
        {
            if (session.ReadySent)
                return;

            session.ReadySent = true;
            session.EnqueueMessage(ZoneMessageOpcode.QuickChatBase, Convert.FromHexString("01-00-00-00-00-00".Replace("-", "")));
            session.EnqueueMessage(new ClientUpdateDoneSendingPreloadCharacters());
            // Achievement
            // Achievement
            session.EnqueueMessage(ZoneMessageOpcode.AchievementBase, File.ReadAllBytes("Resources\\PacketDumps\\71-AchievementBase").AsSpan().Slice(2).ToArray());
            session.EnqueueMessage(ZoneMessageOpcode.AchievementBase, Convert.FromHexString("0B-01-00-00-00-AD-02-00-00".Replace("-", "")));
            session.EnqueueMessage(ZoneMessageOpcode.ActivityManagerBase, Convert.FromHexString("01-00-00-00-00".Replace("-", "")));
            session.EnqueueMessage(ZoneMessageOpcode.PlayerUpdateEffectTagCompositeEffectsEnable, Convert.FromHexString("00-00-01".Replace("-", "")));
            //session.EnqueueMessage((ZoneMessageOpcode)0x29002E00, Convert.FromHexString("91-06-20-72-AE-1B-56-4B-09-00-00-00-01-00-00-00-00-00-00-00-01-00-00-00-02-00-00-00-00-00-00-00-02-00-00-00-03-00-00-00-00-00-00-00-B3-B1-08-00-04-00-00-00-00-00-00-00-58-B2-08-00-05-00-00-00-00-00-00-00-59-B2-08-00-06-00-00-00-00-00-00-00-5A-B2-08-00-07-00-00-00-00-00-00-00-5B-B2-08-00-08-00-00-00-00-00-00-00-5C-B2-08-00-09-00-00-00-00-00-00-00-5D-B2-08-00".Replace("-", "")));
            session.EnqueueMessage(ZoneMessageOpcode.ResourcesBase, Convert.FromHexString("B7-5D-D4-BB-03-91-06-20-72-AE-1B-56-4B-4D-00-00-00-01-00-00-00-08-07-00-00-00-00-00-00-00-00-00-00-00-00-00-00-A6-9B-C4-3B-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-80-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-01".Replace("-", "")));
            //session.EnqueueMessage(ZoneMessageOpcode.ClientUpdateUpdateCalculatedStats, Convert.FromHexString("91-06-20-72-AE-1B-56-4B-2F-00-00-00-01-00-00-00-00-00-E1-44-02-00-00-00-00-00-00-00-03-00-00-00-00-00-00-00-04-00-00-00-00-00-00-00-05-00-00-00-00-00-40-41-06-00-00-00-00-00-00-00-07-00-00-00-00-00-00-00-08-00-00-00-00-00-00-00-09-00-00-00-00-00-00-00-0A-00-00-00-00-00-8C-42-0B-00-00-00-00-00-00-00-0C-00-00-00-00-00-00-00-0D-00-00-00-00-00-00-00-0E-00-00-00-00-00-00-00-0F-00-00-00-00-00-00-00-10-00-00-00-00-00-00-00-11-00-00-00-00-00-00-00-12-00-00-00-00-00-F0-41-13-00-00-00-00-00-70-42-14-00-00-00-00-00-20-42-15-00-00-00-00-00-00-40-16-00-00-00-00-00-C0-40-17-00-00-00-00-00-00-00-18-00-00-00-00-00-20-43-19-00-00-00-00-00-00-00-1A-00-00-00-00-00-00-00-1B-00-00-00-00-00-00-00-1C-00-00-00-00-00-80-3F-1D-00-00-00-00-00-00-00-1E-00-00-00-00-00-00-00-23-00-00-00-00-00-00-00-24-00-00-00-00-00-00-00-25-00-00-00-00-00-00-00-26-00-00-00-00-00-00-00-27-00-00-00-00-00-00-00-28-00-00-00-00-00-00-00-29-00-00-00-00-00-00-00-2A-00-00-00-00-00-00-00-2B-00-00-00-00-00-00-00-2C-00-00-00-00-00-00-00-34-00-00-00-00-00-00-00-35-00-00-00-00-00-00-00-36-00-00-00-00-00-48-42-37-00-00-00-00-00-00-00-39-00-00-00-00-00-00-00-3C-00-00-00-00-00-00-00-3E-00-00-00-00-00-00-00".Replace("-", "")));
            //session.EnqueueMessage(ZoneMessageOpcode.ClientUpdateUpdateStat, Convert.FromHexString("25-00-00-00-01-00-00-00-00-08-07-00-00-00-00-00-00-03-00-00-00-01-66-66-26-3F-00-00-00-00-04-00-00-00-01-00-00-80-3E-00-00-00-00-05-00-00-00-01-00-00-80-3F-00-00-00-00-06-00-00-00-01-00-00-80-3F-00-00-00-00-07-00-00-00-01-00-00-40-3F-00-00-00-00-08-00-00-00-01-CD-CC-CC-3D-00-00-00-00-09-00-00-00-01-CD-CC-4C-3E-00-00-00-00-0A-00-00-00-01-CD-CC-CC-3D-00-00-00-00-0B-00-00-00-00-B8-0B-00-00-00-00-00-00-0C-00-00-00-01-00-00-00-40-00-00-00-00-0D-00-00-00-00-FA-00-00-00-00-00-00-00-0E-00-00-00-01-00-00-80-3F-00-00-00-00-0F-00-00-00-01-00-00-00-00-00-00-00-00-10-00-00-00-01-00-00-00-00-00-00-00-00-15-00-00-00-01-CD-CC-4C-3E-00-00-00-00-16-00-00-00-01-9A-99-19-3E-00-00-00-00-17-00-00-00-01-00-00-40-42-00-00-00-00-18-00-00-00-01-CD-CC-4C-3E-00-00-00-00-19-00-00-00-01-CD-CC-4C-3E-00-00-00-00-1A-00-00-00-01-CD-CC-4C-3E-00-00-00-00-1B-00-00-00-01-9A-99-19-3E-00-00-00-00-1C-00-00-00-01-9A-99-19-3E-00-00-00-00-1D-00-00-00-01-9A-99-19-3E-00-00-00-00-20-00-00-00-01-00-00-00-00-00-00-00-00-21-00-00-00-01-00-00-00-00-00-00-00-00-22-00-00-00-01-00-00-80-3F-00-00-00-00-25-00-00-00-01-00-00-80-3F-00-00-00-00-26-00-00-00-01-00-00-00-00-00-00-00-00-27-00-00-00-01-00-00-00-00-00-00-00-00-28-00-00-00-01-00-00-80-3F-00-00-00-00-2A-00-00-00-01-00-00-00-00-00-00-00-00-2B-00-00-00-01-00-00-00-00-00-00-00-00-2C-00-00-00-01-00-00-00-00-00-00-00-00-2D-00-00-00-01-00-00-00-00-00-00-00-00-2E-00-00-00-01-00-00-00-00-00-00-00-00-2F-00-00-00-01-00-00-00-00-00-00-00-00".Replace("-", "")));
            //session.EnqueueMessage(ZoneMessageOpcode.PlayerUpdateUpdateCharacterStateDelta, Convert.FromHexString("00-00-91-06-20-72-AE-1B-56-4B-00-00-00-00-00-00-00-00-01-00-00-00-00-10-00-00-00-00-00-00-00-00-00-00-B7-5D-D4-BB".Replace("-", "")));
            session.EnqueueMessage(ZoneMessageOpcode.ResourcesBase, Convert.FromHexString("B7-5D-D4-BB-01-91-06-20-72-AE-1B-56-4B-03-00-00-00-0C-00-00-00-1D-00-00-00-0C-00-00-00-00-00-00-00-32-00-00-00-32-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-02-00-00-00-39-00-00-00-02-00-00-00-01-00-00-00-E6-D6-7C-2C-E6-D6-7C-2C-00-00-00-00-00-00-20-43-A0-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-01-00-00-00-4D-00-00-00-01-00-00-00-01-00-00-00-E6-D6-7C-2C-E6-D6-7C-2C-00-00-00-00-00-00-E1-44-08-07-00-00-00-00-00-00-00-00-00-00-00-00-00-00-A6-9B-C4-3B-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00".Replace("-", "")));
            session.EnqueueMessage(ZoneMessageOpcode.LoadoutsBase, Convert.FromHexString("01-03-00-00-00-00-00-00-00-00-00-00-00-01-00-00-00-2E-00-00-00-0F-00-00-00-44-65-66-61-75-6C-74-20-4C-6F-61-64-6F-75-74-00-00-00-00-04-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-05-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-10-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00".Replace("-", "")));
            session.EnqueueMessage(ZoneMessageOpcode.CommandBase, Convert.FromHexString("44-00-01-00-00-00-09-00-00-00-65-6D-6F-74-65-6C-69-73-74-01-00-00-00".Replace("-", "")));
            session.EnqueueMessage(ZoneMessageOpcode.InventoryBase, Convert.FromHexString("18-00-01".Replace("-","")));

            session.EnqueueMessage(new ZoneDoneSendingInitialData());
            // Zone Details are sent a second time for some reason in logs.
            session.EnqueueMessage(ZoneMessageOpcode.SendZoneDetails, Convert.FromHexString("14-00-00-00-4F-70-65-6E-42-65-74-61-47-65-6F-6D-65-74-72-79-5F-34-30-36-05-00-00-00-00-07-00-00-00-53-6B-79-2E-78-6D-6C-00-00-00-00-96-01-00-00-FF-FF-FF-FF-FF-FF-FF-FF-96-01-00-00-02-00-00-00-10-8F-08-00-96-01-00-00-05-00-00-00-4C-B8-08-00-15-00-00-00-4F-70-65-6E-42-65-74-61-43-6F-6E-74-69-6E-65-6E-74-5F-34-30-36-21-00-00-00-4F-70-65-6E-42-65-74-61-5F-44-6F-5F-4E-4F-54-5F-43-68-61-6E-67-65-54-68-69-73-54-65-78-74-5F-31-36-00-00-00-00-00-00-44-40-00-00-00-00-E0-84-43-41-00-00-00-00-00-40-9F-40-00-00-00-00-11-81-43-41-00-00-00-00-00-40-BF-40-00-00-00-00-00-40-AF-40-00-00-00-00-00-40-BF-40-01-00-1A-00-00-00-43-6F-6E-74-69-6E-65-6E-74-5F-42-69-6F-6D-65-49-64-73-5F-34-30-36-2E-62-6D-70-CD-CC-CC-40-17-00-00-00-43-6F-6E-74-69-6E-65-6E-74-5F-53-68-61-70-65-5F-34-30-36-2E-62-6D-70-CD-CC-CC-40-20-00-00-00-43-6F-6E-74-69-6E-65-6E-74-5F-48-65-69-67-68-74-4C-61-79-65-72-49-64-73-5F-34-30-36-2E-62-6D-70-00-00-C8-43-10-27-00-00-00-00-80-3E-00-00-80-3E-00-00-80-3E-00-00-40-3F-00-00-00-3F-00-00-00-40-03-00-00-00-0A-D7-23-3C-00-40-9C-44-00-C0-41-45-00-00-F0-41-01-00-00-00-00-00-00-00-00-6E-A7-40-DA-01-00-00-00-00-00-00-00-B5-36-00-00-01-00-00-80-3E-CD-CC-CC-3D-01-00-00-00-00-00-00-00-00".Replace("-", "")));
            session.EnqueueMessage(new ClientUpdateZonePopulation());


            session.EnqueueMessage(ZoneMessageOpcode.LoadWelcomeScreen, Convert.FromHexString("01-00-00-00-00-00-00-00-00-78-23-00-00-00-00-00-00".Replace("-","")));
            session.EnqueueMessage(ZoneMessageOpcode.ExperienceBase, Convert.FromHexString("02-01-00-00-00-00-00-00-00-01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-0F-0D-01-00-00-00-00-00-24-07-00-00-00-00-00-00-73-0D-01-00-00-00-00-00-5C-06-00-00-00-00-00-00-AB-0C-01-00-00-00-00-00-C0-06-00-00-00-00-00-00".Replace("-", "")));
            session.EnqueueMessage(ZoneMessageOpcode.PropBase, Convert.FromHexString("17-91-06-20-72-AE-1B-56-4B-01-01-00-00-00-01-00-00-00-01-00-00-00-19-C3-08-00-00-00-00-00-00-00-00-00-3B-00-00-00-E8-03-00-00-E9-03-00-00-EA-03-00-00-EB-03-00-00-EC-03-00-00-ED-03-00-00-EE-03-00-00-EF-03-00-00-F0-03-00-00-F1-03-00-00-F2-03-00-00-F3-03-00-00-F4-03-00-00-F5-03-00-00-F6-03-00-00-F7-03-00-00-F8-03-00-00-F9-03-00-00-FD-03-00-00-FE-03-00-00-FF-03-00-00-00-04-00-00-01-04-00-00-02-04-00-00-03-04-00-00-04-04-00-00-07-04-00-00-08-04-00-00-09-04-00-00-0A-04-00-00-0B-04-00-00-0C-04-00-00-0D-04-00-00-0E-04-00-00-0F-04-00-00-10-04-00-00-11-04-00-00-12-04-00-00-13-04-00-00-14-04-00-00-15-04-00-00-16-04-00-00-17-04-00-00-18-04-00-00-19-04-00-00-1A-04-00-00-1B-04-00-00-1D-04-00-00-1E-04-00-00-22-04-00-00-23-04-00-00-24-04-00-00-25-04-00-00-26-04-00-00-27-04-00-00-28-04-00-00-29-04-00-00-2A-04-00-00-2C-04-00-00-3B-00-00-00-E8-03-00-00-E8-03-00-00-1A-C3-08-00-17-00-00-00-53-2E-00-00-15-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-4F-72-63-68-69-64-42-6C-75-65-97-B5-AD-67-E9-03-00-00-E9-03-00-00-1E-C3-08-00-1B-00-00-00-57-2E-00-00-14-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-47-6F-6C-64-65-6E-72-6F-64-97-60-41-FC-EA-03-00-00-EA-03-00-00-1C-C3-08-00-2B-00-00-00-69-2E-00-00-14-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-43-68-65-72-72-79-52-65-64-18-4C-F3-F8-EB-03-00-00-EB-03-00-00-47-C4-08-00-32-00-00-00-6F-2E-00-00-12-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-20-5F-43-6C-6F-76-65-72-9C-D4-E9-04-EC-03-00-00-EC-03-00-00-1D-C3-08-00-0F-00-00-00-49-2E-00-00-13-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-45-67-67-70-6C-61-6E-74-EB-95-05-71-ED-03-00-00-ED-03-00-00-26-C3-08-00-0C-00-00-00-46-2E-00-00-10-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-4D-61-75-76-65-FB-DB-3A-CF-EE-03-00-00-EE-03-00-00-25-C3-08-00-3A-00-00-00-78-2E-00-00-14-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-44-6F-76-65-20-47-72-65-79-66-59-64-BE-EF-03-00-00-EF-03-00-00-22-C3-08-00-22-00-00-00-5F-2E-00-00-11-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-43-6F-70-70-65-72-CE-75-F4-E9-F0-03-00-00-F0-03-00-00-24-C3-08-00-1A-00-00-00-56-2E-00-00-13-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-43-6F-72-6E-73-69-6C-6B-62-4F-73-5E-F1-03-00-00-F1-03-00-00-2A-C3-08-00-13-00-00-00-4D-2E-00-00-14-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-54-75-72-71-75-6F-69-73-65-73-37-39-13-F2-03-00-00-F2-03-00-00-21-C3-08-00-0A-00-00-00-44-2E-00-00-1B-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-20-5F-56-69-6F-6C-65-6E-74-20-46-75-63-68-73-69-61-0F-21-43-23-F3-03-00-00-F3-03-00-00-29-C3-08-00-3D-00-00-00-7C-2E-00-00-15-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-53-6F-66-74-20-42-6C-61-63-6B-53-B0-9A-DA-F4-03-00-00-F4-03-00-00-20-C3-08-00-25-00-00-00-62-2E-00-00-12-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-20-5F-53-69-65-6E-6E-61-6D-54-B4-5B-F5-03-00-00-F5-03-00-00-27-C3-08-00-11-00-00-00-4B-2E-00-00-15-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-4D-69-6E-74-20-43-72-65-61-6D-6A-32-57-25-F6-03-00-00-F6-03-00-00-1F-C3-08-00-04-00-00-00-71-2E-00-00-16-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-20-5F-43-6F-72-6E-66-6C-6F-77-65-72-F2-A2-44-02-F7-03-00-00-F7-03-00-00-34-C4-08-00-1C-00-00-00-58-2E-00-00-10-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-48-6F-6E-65-79-1D-64-FD-E5-F8-03-00-00-F8-03-00-00-28-C3-08-00-16-00-00-00-52-2E-00-00-14-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-52-6F-79-61-6C-62-6C-75-65-EB-84-E0-F1-F9-03-00-00-F9-03-00-00-23-C3-08-00-08-00-00-00-80-2E-00-00-10-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-43-6F-72-61-6C-10-04-09-EE-FD-03-00-00-FD-03-00-00-32-C4-08-00-18-00-00-00-54-2E-00-00-14-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-42-75-74-74-65-72-63-75-70-C0-F7-33-59-FE-03-00-00-FE-03-00-00-4A-C4-08-00-35-00-00-00-73-2E-00-00-11-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-50-69-63-6B-6C-65-E7-DA-43-97-FF-03-00-00-FF-03-00-00-49-C4-08-00-34-00-00-00-72-2E-00-00-15-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-4F-6C-69-76-65-20-44-72-61-62-BB-86-E5-7A-00-04-00-00-00-04-00-00-26-C4-08-00-02-00-00-00-5A-2E-00-00-0F-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-45-63-72-75-E4-26-4E-8A-01-04-00-00-01-04-00-00-30-C4-08-00-14-00-00-00-4F-2E-00-00-13-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-53-6B-79-20-42-6C-75-65-B1-BF-77-07-02-04-00-00-02-04-00-00-4D-C4-08-00-38-00-00-00-76-2E-00-00-14-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-53-74-65-65-6C-42-6C-75-65-18-7C-FE-87-03-04-00-00-03-04-00-00-2F-C4-08-00-12-00-00-00-4C-2E-00-00-0F-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-54-65-61-6C-6C-5A-33-DE-04-04-00-00-04-04-00-00-4C-C4-08-00-37-00-00-00-75-2E-00-00-18-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-50-72-75-73-73-69-61-6E-20-42-6C-75-65-B9-D1-E5-FA-07-04-00-00-07-04-00-00-10-C5-08-00-0D-00-00-00-47-2E-00-00-14-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-44-75-73-74-79-52-6F-73-65-C4-0E-5D-A8-08-04-00-00-08-04-00-00-2D-C4-08-00-0E-00-00-00-48-2E-00-00-13-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-4C-61-76-65-6E-64-65-72-76-B0-5A-DE-09-04-00-00-09-04-00-00-48-C4-08-00-33-00-00-00-70-2E-00-00-12-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-45-6D-65-72-61-6C-64-55-60-DD-E0-0A-04-00-00-0A-04-00-00-42-C4-08-00-2D-00-00-00-6B-2E-00-00-16-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-59-65-6C-6C-6F-77-47-72-65-65-6E-3B-11-88-A6-0B-04-00-00-0B-04-00-00-43-C4-08-00-2E-00-00-00-6C-2E-00-00-15-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-43-68-61-72-74-72-65-75-73-65-28-08-ED-90-0C-04-00-00-0C-04-00-00-46-C4-08-00-31-00-00-00-6E-2E-00-00-16-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-53-70-72-69-6E-67-47-72-65-65-6E-03-84-C5-61-0D-04-00-00-0D-04-00-00-38-C4-08-00-20-00-00-00-5D-2E-00-00-14-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-54-61-6E-67-65-72-69-6E-65-DF-CF-D1-83-0E-04-00-00-0E-04-00-00-39-C4-08-00-21-00-00-00-5E-2E-00-00-12-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-50-75-6D-70-6B-69-6E-93-B1-F7-94-0F-04-00-00-0F-04-00-00-36-C4-08-00-1E-00-00-00-5B-2E-00-00-10-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-50-65-61-63-68-84-2D-72-19-10-04-00-00-10-04-00-00-44-C4-08-00-2F-00-00-00-6D-2E-00-00-0F-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-50-65-61-72-2F-3F-CC-6E-11-04-00-00-11-04-00-00-33-C4-08-00-19-00-00-00-55-2E-00-00-11-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-42-61-6E-61-6E-61-5A-ED-4E-5E-12-04-00-00-12-04-00-00-35-C4-08-00-1D-00-00-00-59-2E-00-00-0F-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-59-6F-6C-6B-13-C2-5F-36-13-04-00-00-13-04-00-00-3C-C4-08-00-26-00-00-00-63-2E-00-00-16-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-42-75-72-6E-74-4F-72-61-6E-67-65-29-DE-F9-E3-14-04-00-00-14-04-00-00-3F-C4-08-00-29-00-00-00-66-2E-00-00-14-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-46-69-72-65-42-72-69-63-6B-1F-DB-DA-AD-15-04-00-00-15-04-00-00-2C-C4-08-00-0B-00-00-00-45-2E-00-00-10-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-52-6F-73-69-65-02-EA-0C-FD-16-04-00-00-16-04-00-00-41-C4-08-00-2C-00-00-00-6A-2E-00-00-11-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-4D-61-72-6F-6F-6E-98-F7-15-56-17-04-00-00-17-04-00-00-3E-C4-08-00-28-00-00-00-65-2E-00-00-12-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-43-72-69-6D-73-6F-6E-09-A1-7D-E6-18-04-00-00-18-04-00-00-3D-C4-08-00-27-00-00-00-64-2E-00-00-1F-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-42-75-72-6E-69-73-68-65-64-20-54-65-72-72-61-43-6F-74-74-61-5C-B4-B5-25-19-04-00-00-19-04-00-00-25-C4-08-00-01-00-00-00-4E-2E-00-00-10-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-49-76-6F-72-79-80-65-89-FA-1A-04-00-00-1A-04-00-00-2A-C4-08-00-07-00-00-00-7F-2E-00-00-17-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-4F-72-61-6E-67-65-20-43-72-65-61-6D-16-4C-67-94-1B-04-00-00-1B-04-00-00-4E-C4-08-00-39-00-00-00-77-2E-00-00-10-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-53-6C-61-74-65-64-89-55-E0-1D-04-00-00-1D-04-00-00-40-C4-08-00-2A-00-00-00-68-2E-00-00-15-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-43-65-72-69-73-65-66-6F-69-6C-AB-65-25-18-1E-04-00-00-1E-04-00-00-4F-C4-08-00-3B-00-00-00-79-2E-00-00-0F-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-58-72-61-79-02-F1-EE-3A-22-04-00-00-22-04-00-00-45-C4-08-00-30-00-00-00-82-2E-00-00-13-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-4C-69-6D-65-6C-61-6D-65-A4-BC-C7-49-23-04-00-00-23-04-00-00-2E-C4-08-00-10-00-00-00-4A-2E-00-00-16-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-49-72-72-69-64-65-73-63-65-6E-74-F8-01-A8-1A-24-04-00-00-24-04-00-00-50-C4-08-00-3C-00-00-00-7A-2E-00-00-15-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-42-6C-61-63-6B-4C-61-74-65-78-42-D0-5E-35-25-04-00-00-25-04-00-00-27-C4-08-00-03-00-00-00-67-2E-00-00-10-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-50-65-61-72-6C-F5-91-1B-A6-26-04-00-00-26-04-00-00-3A-C4-08-00-23-00-00-00-60-2E-00-00-14-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-50-61-6C-6C-61-64-69-75-6D-8D-72-DB-F0-27-04-00-00-27-04-00-00-4B-C4-08-00-36-00-00-00-74-2E-00-00-14-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-47-72-65-65-6E-47-6F-6C-64-87-89-86-F1-28-04-00-00-28-04-00-00-37-C4-08-00-1F-00-00-00-5C-2E-00-00-13-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-52-6F-73-65-47-6F-6C-64-9F-D1-0C-08-29-04-00-00-29-04-00-00-29-C4-08-00-06-00-00-00-7D-2E-00-00-13-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-50-6C-61-74-69-6E-75-6D-03-FE-DD-6F-2A-04-00-00-2A-04-00-00-3B-C4-08-00-24-00-00-00-61-2E-00-00-10-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-42-72-61-73-73-E6-9D-7A-E6-2C-04-00-00-2C-04-00-00-2B-C4-08-00-09-00-00-00-81-2E-00-00-18-00-00-00-47-6C-6F-62-61-6C-54-69-6E-74-5F-42-61-6C-6C-65-74-53-6C-69-70-70-65-72-61-9E-8E-1E".Replace("-","")));
            session.EnqueueMessage(ZoneMessageOpcode.ZoneCoordinateInfo, Convert.FromHexString("00-00-00-00-18-66-78-41-00-00-00-00-00-88-D3-40-00-00-00-40-55-61-78-41-00-40-9C-47-00-40-1C-47-00-40-9C-47-00-00-00-00-00-00-00-00-00-00-00-00-00-00-7A-C5-00-00-FA-C4-00-00-7A-C5-00-00-80-3F-00-00-7A-45-00-00-FA-44-00-00-7A-45-00-00-80-3F".Replace("-","")));
            session.EnqueueMessage(ZoneMessageOpcode.EquipmentBase, Convert.FromHexString("01-2E-00-00-00-91-06-20-72-AE-1B-56-4B-01-00-00-00-42-00-00-00-43-68-61-72-5F-42-69-70-65-64-5F-5B-52-41-43-45-5D-5B-47-45-4E-44-45-52-5D-5F-45-6E-74-69-74-69-65-73-5F-47-75-6E-73-6C-69-6E-67-65-72-5F-4D-65-64-69-75-6D-5F-30-30-31-5F-43-68-65-73-74-2E-61-64-72-00-00-00-00-0D-00-00-00-47-75-6E-73-6C-69-6E-67-65-72-5F-30-34-00-00-00-00-00-00-00-00-00-00-00-00-08-00-00-00-00-00-00-00-00".Replace("-","")));
            session.EnqueueMessage(ZoneMessageOpcode.WorldDisplayInfo, Convert.FromHexString("00-00-00-00".Replace("-","")));
            session.EnqueueMessage(ZoneMessageOpcode.ClientUpdateMovementVersion, Convert.FromHexString("01".Replace("-","")));
            session.EnqueueMessage(ZoneMessageOpcode.ClientSettings, Convert.FromHexString("1F-00-00-00-68-74-74-70-3A-2F-2F-77-77-77-2E-70-6C-61-6E-65-74-73-69-64-65-32-2E-63-6F-6D-2F-68-65-6C-70-1F-00-00-00-68-74-74-70-3A-2F-2F-77-77-77-2E-70-6C-61-6E-65-74-73-69-64-65-32-2E-63-6F-6D-2F-73-68-6F-70-1F-00-00-00-68-74-74-70-3A-2F-2F-77-77-77-2E-70-6C-61-6E-65-74-73-69-64-65-32-2E-63-6F-6D-2F-73-68-6F-70-01-00-00-00-00".Replace("-","")));


            ////session.EnqueueMessage((ZoneMessageOpcode)0x29000600, Convert.FromHexString("25-00-00-00-01-00-00-00-00-08-07-00-00-00-00-00-00-03-00-00-00-01-66-66-26-3F-00-00-00-00-04-00-00-00-01-00-00-80-3E-00-00-00-00-05-00-00-00-01-00-00-80-3F-00-00-00-00-06-00-00-00-01-00-00-80-3F-00-00-00-00-07-00-00-00-01-00-00-40-3F-00-00-00-00-08-00-00-00-01-CD-CC-CC-3D-00-00-00-00-09-00-00-00-01-CD-CC-4C-3E-00-00-00-00-0A-00-00-00-01-CD-CC-CC-3D-00-00-00-00-0B-00-00-00-00-B8-0B-00-00-00-00-00-00-0C-00-00-00-01-00-00-00-40-00-00-00-00-0D-00-00-00-00-FA-00-00-00-00-00-00-00-0E-00-00-00-01-00-00-80-3F-00-00-00-00-0F-00-00-00-01-00-00-00-00-00-00-00-00-10-00-00-00-01-00-00-00-00-00-00-00-00-15-00-00-00-01-CD-CC-4C-3E-00-00-00-00-16-00-00-00-01-9A-99-19-3E-00-00-00-00-17-00-00-00-01-00-00-40-42-00-00-00-00-18-00-00-00-01-CD-CC-4C-3E-00-00-00-00-19-00-00-00-01-CD-CC-4C-3E-00-00-00-00-1A-00-00-00-01-CD-CC-4C-3E-00-00-00-00-1B-00-00-00-01-9A-99-19-3E-00-00-00-00-1C-00-00-00-01-9A-99-19-3E-00-00-00-00-1D-00-00-00-01-9A-99-19-3E-00-00-00-00-20-00-00-00-01-00-00-00-00-00-00-00-00-21-00-00-00-01-00-00-00-00-00-00-00-00-22-00-00-00-01-00-00-80-3F-00-00-00-00-25-00-00-00-01-00-00-80-3F-00-00-00-00-26-00-00-00-01-00-00-00-00-00-00-00-00-27-00-00-00-01-00-00-00-00-00-00-00-00-28-00-00-00-01-00-00-80-3F-00-00-00-00-2A-00-00-00-01-00-00-00-00-00-00-00-00-2B-00-00-00-01-00-00-00-00-00-00-00-00-2C-00-00-00-01-00-00-00-00-00-00-00-00-2D-00-00-00-01-00-00-00-00-00-00-00-00-2E-00-00-00-01-00-00-00-00-00-00-00-00-2F-00-00-00-01-00-00-00-00-00-00-00-00".Replace("-", "")));
            session.EnqueueMessage(ZoneMessageOpcode.TerrainCellModifiedCellManifest, session.TerrainManifest.AsSpan().Slice(4).ToArray());
            session.EnqueueMessage(ZoneMessageOpcode.AreaDefinitionSetDefinitions, Convert.FromHexString("00-00-00-00-00-00-00-00-00-00-00-00-00-00-00".Replace("-", "")));
            ////session.EnqueueMessage(ZoneMessageOpcode.PreferencePacket, Convert.FromHexString("01-00-00-00-8E-00-00-00-3C-52-6F-6F-74-20-56-65-72-73-69-6F-6E-3D-22-31-2E-30-22-3E-3C-57-6F-72-6C-64-2F-3E-3C-5A-6F-6E-65-2F-3E-3C-43-6C-69-65-6E-74-3E-3C-55-49-3E-3C-43-68-61-74-3E-3C-43-68-61-74-57-69-6E-64-6F-77-73-20-49-73-4C-69-73-74-3D-22-31-22-2F-3E-3C-2F-43-68-61-74-3E-3C-41-63-74-69-6F-6E-42-61-72-20-41-72-65-41-63-74-69-6F-6E-42-61-72-73-4C-6F-63-6B-65-64-3D-22-30-22-2F-3E-3C-2F-55-49-3E-3C-2F-43-6C-69-65-6E-74-3E-3C-2F-52-6F-6F-74-3E".Replace("-", "")));
            ////session.EnqueueMessage(ZoneMessageOpcode.PlayerUpdateSetPlayerBio, Convert.FromHexString("00-00-00-00-00-00".Replace("-", "")));
            ////session.EnqueueMessage(ZoneMessageOpcode.MailBase, Convert.FromHexString("0F-00-00-00-01-00".Replace("-", "")));
            session.EnqueueMessage(ZoneMessageOpcode.PropBase, File.ReadAllBytes("Resources\\PacketDumps\\145-PropBase").AsSpan().Slice(2).ToArray());
            session.EnqueueMessage(ZoneMessageOpcode.ActivityBase, Convert.FromHexString("01-01-02-00-00-00-00-00-00-00".Replace("-", "")));
        }

        [ZoneMessageHandler(ZoneMessageOpcode.ClientFinishedLoading)]
        public static void ClientFinishedLoading(WorldSession session, ClientFinishedLoading clientFinishedLoading)
        {
        }

        [ZoneMessageHandler(ZoneMessageOpcode.Synchronization)]
        public static void Sychronization(WorldSession session, Synchronization synchronization)
        {
            if (synchronization.Unknown0)
                return;

            var sync = new Synchronization
            {
                Time1 = synchronization.Time1,
                Time2 = synchronization.Time2,
                ClientTime = synchronization.ClientTime,
                ServerTime = synchronization.ClientTime / 2d,
                ServerTime2 = synchronization.ClientTime / 2d,
                Time3 = synchronization.ClientTime
            };
            session.EnqueueMessage(sync);

            log.Info($"{JsonConvert.SerializeObject(sync, Formatting.Indented)}");
        }

        [ZoneMessageHandler(ZoneMessageOpcode.AbilityCasterInfoRequest)]
        public static void AbilityCasterInfoRequest(WorldSession session, AbilityCasterInfoRequest abilityRequest)
        {
            log.Info($"{JsonConvert.SerializeObject(abilityRequest, Formatting.Indented)}");
            if (abilityRequest.Unknown0 == 221)
                session.EnqueueMessage(ZoneMessageOpcode.AbilityCasterInfoReply, Convert.FromHexString("DD-00-00-00-5F-5E-B0-DF-02-00-00-00-00-00-00-00-00-00-00-00-00-00-00-F0-41-00-00-00-00-00-00-00-00-00-00-00-00-F7-89-08-00-00-00-00-F0-41-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-DD-00-00-00-01-01-00-00-00-00-F5-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-4B-98-08-00-01-00-00-00-00-00-00-00-01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00".Replace("-", "")));
            if (abilityRequest.Unknown0 == 1098)
                session.EnqueueMessage(ZoneMessageOpcode.AbilityCasterInfoReply, Convert.FromHexString("4A-04-00-00-5F-5E-B0-DF-02-00-00-00-00-00-00-00-01-00-00-00-00-00-00-70-42-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-4A-04-00-00-01-01-00-00-80-3F-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-01-00-00-01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00".Replace("-", "")));
            if (abilityRequest.Unknown0 == 326)
                session.EnqueueMessage(ZoneMessageOpcode.AbilityCasterInfoReply, Convert.FromHexString("46-01-00-00-5F-5E-B0-DF-02-00-00-00-00-00-00-00-00-00-00-00-00-00-00-F0-41-00-00-00-00-00-00-00-00-00-00-00-00-F7-89-08-00-00-00-00-F0-41-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-DD-00-00-00-01-01-00-00-00-00-F5-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-4B-98-08-00-01-00-00-00-00-00-00-00-01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00".Replace("-", "")));
            if (abilityRequest.Unknown0 == 1122)
                session.EnqueueMessage(ZoneMessageOpcode.AbilityCasterInfoReply, Convert.FromHexString("62-04-00-00-5F-5E-B0-DF-02-00-00-00-00-00-00-00-00-00-00-00-00-00-00-F0-41-00-00-00-00-00-00-00-00-00-00-00-00-F7-89-08-00-00-00-00-F0-41-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-DD-00-00-00-01-01-00-00-00-00-F5-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-4B-98-08-00-01-00-00-00-00-00-00-00-01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00".Replace("-", "")));
        }

        [ZoneMessageHandler(ZoneMessageOpcode.PlayerUpdateSetPlayerBio)]
        public static void PlayerSetPlayerBio(WorldSession session, PlayerUpdateSetPlayerBio playerBio)
        {
            session.EnqueueMessage(ZoneMessageOpcode.PlayerUpdateSetPlayerBio, Convert.FromHexString("00-00-00-00-00-00".Replace("-", "")));
        }

        [ZoneMessageHandler(ZoneMessageOpcode.AbilityDefaultInfoRequest)]
        public static void AbilityDefaultInfoRequest(WorldSession session, AbilityDefaultInfoRequest abilityDefaultRequest)
        {
            session.EnqueueMessage(ZoneMessageOpcode.AbilityDefaultInfoReply, Convert.FromHexString("00-00-00-00".Replace("-", "")));
        }
    }
}