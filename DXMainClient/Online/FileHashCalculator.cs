﻿using ClientCore;
using DTAClient.domain.CnCNet;
using Rampastring.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Utilities = Rampastring.Tools.Utilities;

namespace DTAClient.Online
{
    public class FileHashCalculator
    {
        FileHashes fh;

        string[] fileNamesToCheck = new string[]
        {
            "spawner.xdp",
            "INI\\Rules.ini",
            "INI\\Enhance.ini",
            "INI\\Art.ini",
            "INI\\ArtE.ini",
            "INI\\AI.ini",
            "INI\\AIE.ini",
            "INI\\GlobalCode.ini",
        };

        public void CalculateHashes(List<GameMode> gameModes)
        {
            fh = new FileHashes();
            fh.GameOptionsHash = Utilities.CalculateSHA1ForFile(ProgramConstants.GamePath + ProgramConstants.BASE_RESOURCE_PATH + "GameOptions.ini");
            fh.ClientHash = Utilities.CalculateSHA1ForFile(AppDomain.CurrentDomain.FriendlyName);
            fh.ClientGUIHash = Utilities.CalculateSHA1ForFile(ProgramConstants.GamePath + "Resources\\Binaries\\ClientGUI.dll");
            fh.ClientCoreHash = Utilities.CalculateSHA1ForFile(ProgramConstants.GamePath + "Resources\\Binaries\\ClientCore.dll");
            fh.MainExeHash = Utilities.CalculateSHA1ForFile(ProgramConstants.GamePath + DomainController.Instance().GetGameExecutableName(0));
            fh.MPMapsHash = Utilities.CalculateSHA1ForFile(ProgramConstants.GamePath + DomainController.Instance().GetMPMapsIniPath());

            fh.INIHashes = string.Empty;

            foreach (string filePath in fileNamesToCheck)
                fh.INIHashes = AddToStringIfFileExists(fh.INIHashes, filePath);

            if (Directory.Exists(ProgramConstants.GamePath + "INI\\Map Code"))
            {
                foreach (GameMode gameMode in gameModes)
                {
                    fh.INIHashes = AddToStringIfFileExists(fh.INIHashes, "INI\\Map Code\\" + gameMode.Name + ".ini");
                }
            }

            if (Directory.Exists(ProgramConstants.GamePath + "INI\\Game Options"))
            {
                List<string> files = Directory.GetFiles(
                    ProgramConstants.GamePath + "INI\\Game Options",
                    "*", SearchOption.AllDirectories).ToList();

                files.Sort();

                foreach (string fileName in files)
                {
                    fh.INIHashes = fh.INIHashes + Utilities.CalculateSHA1ForFile("INI\\Game Options\\" + fileName);
                }
            }

            fh.INIHashes = Utilities.CalculateSHA1ForString(fh.INIHashes);
        }

        string AddToStringIfFileExists(string str, string path)
        {
            if (File.Exists(path))
            {
                return str + Utilities.CalculateSHA1ForFile(ProgramConstants.GamePath + path);
            }

            return str;
        }

        public string GetCompleteHash()
        {
            string str = fh.GameOptionsHash;
            str = str + fh.ClientHash;
            str = str + fh.ClientGUIHash;
            str = str + fh.ClientCoreHash;
            str = str + fh.MainExeHash;
            str = str + fh.INIHashes;
            str = str + fh.MPMapsHash;

            return Utilities.CalculateSHA1ForString(str);
        }
    }

    struct FileHashes
    {
        public string GameOptionsHash { get; set; }
        public string ClientHash { get; set; }
        public string ClientGUIHash { get; set; }
        public string ClientCoreHash { get; set; }
        public string INIHashes { get; set; }
        public string MPMapsHash { get; set; }
        public string MainExeHash { get; set; }
    }
}
