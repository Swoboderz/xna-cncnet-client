﻿using System;
using System.Collections.Generic;
using ClientCore.Statistics.GameParsers;
using Rampastring.Tools;

namespace ClientCore.Statistics
{
    public class MatchStatistics
    {
        public MatchStatistics() { }

        public MatchStatistics(string gameVersion, int gameId, string mapName, string gameMode, int numHumans, bool mapIsCoop = false)
        {
            GameVersion = gameVersion;
            GameID = gameId;
            DateAndTime = DateTime.Now;
            MapName = mapName;
            GameMode = gameMode;
            NumberOfHumanPlayers = numHumans;
            MapIsCoop = mapIsCoop;
        }

        public List<PlayerStatistics> Players = new List<PlayerStatistics>();

        public int LengthInSeconds { get; set; }

        public DateTime DateAndTime { get; set; }

        public string GameVersion { get; set; }

        public string MapName { get; set; }

        public string GameMode { get; set; }

        public bool SawCompletion { get; set; }

        public int NumberOfHumanPlayers { get; set; }

        public int AverageFPS { get; set; }

        public int GameID { get; set; }

        public bool MapIsCoop { get; set; }

        public void AddPlayer(string name, bool isLocal, bool isAI, bool isSpectator,
            int side, int team, int color, int aiLevel)
        {
            PlayerStatistics ps = new PlayerStatistics(name, isLocal, isAI, isSpectator, 
                side, team, color, aiLevel);
            Players.Add(ps);
        }

        public void AddPlayer(PlayerStatistics ps)
        {
            Players.Add(ps);
        }

        public void ParseStatistics(string gamePath, string gameName, bool isLoadedGame)
        {
            Logger.Log("Parsing game statistics.");

            LengthInSeconds = (int)(DateTime.Now - DateAndTime).TotalSeconds;

            var parser = new LogFileStatisticsParser(this, isLoadedGame);
            parser.ParseStats(gamePath, ClientConfiguration.Instance.StatisticsLogFileName);
        }

        public PlayerStatistics GetEmptyPlayerByName(string playerName)
        {
            foreach (PlayerStatistics ps in Players)
            {
                if (ps.Name == playerName && ps.Losses == 0 && ps.Score == 0)
                    return ps;
            }

            return null;
        }

        public PlayerStatistics GetFirstEmptyPlayer()
        {
            foreach (PlayerStatistics ps in Players)
            {
                if (ps.Losses == 0 && ps.Score == 0)
                    return ps;
            }

            return null;
        }

        public int GetPlayerCount()
        {
            return Players.Count;
        }

        public PlayerStatistics GetPlayer(int index)
        {
            return Players[index];
        }
    }
}
