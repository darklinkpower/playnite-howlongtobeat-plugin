﻿using HowLongToBeat.Models;
using HowLongToBeat.Views;
using Playnite.SDK;
using Playnite.SDK.Data;
using Playnite.SDK.Models;
using CommonPluginsShared.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CommonPluginsShared;
using CommonPluginsControls.Controls;
using System.Windows.Threading;
using FuzzySharp;
using CommonPlayniteShared.Common;
using CommonPluginsShared.Extensions;

namespace HowLongToBeat.Services
{
    public class HowLongToBeatDatabase : PluginDatabaseObject<HowLongToBeatSettingsViewModel, GameHowLongToBeatCollection, GameHowLongToBeat, HltbDataUser>
    {
        public HowLongToBeat Plugin;
        public HowLongToBeatClient howLongToBeatClient;

        public List<HltbPlatform> hltbPlatforms = new List<HltbPlatform>
        {
            new HltbPlatform() { Name = "3DO", Category = "All Platforms" },
            new HltbPlatform() { Name = "Amiga", Category = "All Platforms" },
            new HltbPlatform() { Name = "Amstrad CPC", Category = "All Platforms" },
            new HltbPlatform() { Name = "Android", Category = "All Platforms" },
            new HltbPlatform() { Name = "Apple II", Category = "All Platforms" },
            new HltbPlatform() { Name = "Arcade", Category = "All Platforms" },
            new HltbPlatform() { Name = "Atari 2600", Category = "All Platforms" },
            new HltbPlatform() { Name = "Atari 5200", Category = "All Platforms" },
            new HltbPlatform() { Name = "Atari 7800", Category = "All Platforms" },
            new HltbPlatform() { Name = "Atari 8-bit Family", Category = "All Platforms" },
            new HltbPlatform() { Name = "Atari Jaguar", Category = "All Platforms" },
            new HltbPlatform() { Name = "Atari Jaguar CD", Category = "All Platforms" },
            new HltbPlatform() { Name = "Atari Lynx", Category = "All Platforms" },
            new HltbPlatform() { Name = "Atari ST", Category = "All Platforms" },
            new HltbPlatform() { Name = "BBC Micro", Category = "All Platforms" },
            new HltbPlatform() { Name = "Browser", Category = "All Platforms" },
            new HltbPlatform() { Name = "ColecoVision", Category = "All Platforms" },
            new HltbPlatform() { Name = "Commodore 64", Category = "All Platforms" },
            new HltbPlatform() { Name = "Dreamcast", Category = "All Platforms" },
            new HltbPlatform() { Name = "Emulated", Category = "All Platforms" },
            new HltbPlatform() { Name = "FM Towns", Category = "All Platforms" },
            new HltbPlatform() { Name = "Game & Watch", Category = "All Platforms" },
            new HltbPlatform() { Name = "Game Boy", Category = "All Platforms" },
            new HltbPlatform() { Name = "Game Boy Advance", Category = "All Platforms" },
            new HltbPlatform() { Name = "Game Boy Color", Category = "All Platforms" },
            new HltbPlatform() { Name = "Gear VR", Category = "All Platforms" },
            new HltbPlatform() { Name = "Google Stadia", Category = "All Platforms" },
            new HltbPlatform() { Name = "Intellivision", Category = "All Platforms" },
            new HltbPlatform() { Name = "Interactive Movie", Category = "All Platforms" },
            new HltbPlatform() { Name = "iOS", Category = "All Platforms" },
            new HltbPlatform() { Name = "Linux", Category = "All Platforms" },
            new HltbPlatform() { Name = "Mac", Category = "All Platforms" },
            new HltbPlatform() { Name = "Mobile", Category = "All Platforms" },
            new HltbPlatform() { Name = "MSX", Category = "All Platforms" },
            new HltbPlatform() { Name = "N-Gage", Category = "All Platforms" },
            new HltbPlatform() { Name = "NEC PC-8800", Category = "All Platforms" },
            new HltbPlatform() { Name = "NEC PC-9801/21", Category = "All Platforms" },
            new HltbPlatform() { Name = "NEC PC-FX", Category = "All Platforms" },
            new HltbPlatform() { Name = "Neo Geo", Category = "All Platforms" },
            new HltbPlatform() { Name = "Neo Geo CD", Category = "All Platforms" },
            new HltbPlatform() { Name = "Neo Geo Pocket", Category = "All Platforms" },
            new HltbPlatform() { Name = "NES", Category = "All Platforms" },
            new HltbPlatform() { Name = "Nintendo 3DS", Category = "All Platforms" },
            new HltbPlatform() { Name = "Nintendo 64", Category = "All Platforms" },
            new HltbPlatform() { Name = "Nintendo DS", Category = "All Platforms" },
            new HltbPlatform() { Name = "Nintendo GameCube", Category = "All Platforms" },
            new HltbPlatform() { Name = "Nintendo Switch", Category = "All Platforms" },
            new HltbPlatform() { Name = "Oculus Go", Category = "All Platforms" },
            new HltbPlatform() { Name = "Oculus Quest", Category = "All Platforms" },
            new HltbPlatform() { Name = "OnLive", Category = "All Platforms" },
            new HltbPlatform() { Name = "Ouya", Category = "All Platforms" },
            new HltbPlatform() { Name = "PC", Category = "All Platforms" },
            new HltbPlatform() { Name = "PC VR", Category = "All Platforms" },
            new HltbPlatform() { Name = "Philips CD-i", Category = "All Platforms" },
            new HltbPlatform() { Name = "Philips Videopac G7000", Category = "All Platforms" },
            new HltbPlatform() { Name = "PlayStation", Category = "All Platforms" },
            new HltbPlatform() { Name = "PlayStation 2", Category = "All Platforms" },
            new HltbPlatform() { Name = "PlayStation 3", Category = "All Platforms" },
            new HltbPlatform() { Name = "PlayStation 4", Category = "All Platforms" },
            new HltbPlatform() { Name = "PlayStation 5", Category = "All Platforms" },
            new HltbPlatform() { Name = "PlayStation Mobile", Category = "All Platforms" },
            new HltbPlatform() { Name = "PlayStation Now", Category = "All Platforms" },
            new HltbPlatform() { Name = "PlayStation Portable", Category = "All Platforms" },
            new HltbPlatform() { Name = "PlayStation Vita", Category = "All Platforms" },
            new HltbPlatform() { Name = "PlayStation VR", Category = "All Platforms" },
            new HltbPlatform() { Name = "Plug & Play", Category = "All Platforms" },
            new HltbPlatform() { Name = "Sega 32X", Category = "All Platforms" },
            new HltbPlatform() { Name = "Sega CD", Category = "All Platforms" },
            new HltbPlatform() { Name = "Sega Game Gear", Category = "All Platforms" },
            new HltbPlatform() { Name = "Sega Master System", Category = "All Platforms" },
            new HltbPlatform() { Name = "Sega Mega Drive/Genesis", Category = "All Platforms" },
            new HltbPlatform() { Name = "Sega Saturn", Category = "All Platforms" },
            new HltbPlatform() { Name = "SG-1000", Category = "All Platforms" },
            new HltbPlatform() { Name = "Sharp X68000", Category = "All Platforms" },
            new HltbPlatform() { Name = "Super Nintendo", Category = "All Platforms" },
            new HltbPlatform() { Name = "Tiger Handheld", Category = "All Platforms" },
            new HltbPlatform() { Name = "TurboGrafx-16", Category = "All Platforms" },
            new HltbPlatform() { Name = "TurboGrafx-CD", Category = "All Platforms" },
            new HltbPlatform() { Name = "Virtual Boy", Category = "All Platforms" },
            new HltbPlatform() { Name = "Wii", Category = "All Platforms" },
            new HltbPlatform() { Name = "Wii U", Category = "All Platforms" },
            new HltbPlatform() { Name = "Windows Phone", Category = "All Platforms" },
            new HltbPlatform() { Name = "WonderSwan", Category = "All Platforms" },
            new HltbPlatform() { Name = "Xbox", Category = "All Platforms" },
            new HltbPlatform() { Name = "Xbox 360", Category = "All Platforms" },
            new HltbPlatform() { Name = "Xbox One", Category = "All Platforms" },
            new HltbPlatform() { Name = "Xbox Series X/S", Category = "All Platforms" },
            new HltbPlatform() { Name = "ZX Spectrum", Category = "All Platforms" }
        };


        public HowLongToBeatDatabase(IPlayniteAPI PlayniteApi, HowLongToBeatSettingsViewModel PluginSettings, string PluginUserDataPath) : base(PlayniteApi, PluginSettings, "HowLongToBeat", PluginUserDataPath)
        {
            TagBefore = "[HLTB]";
        }


        public void InitializeClient(HowLongToBeat Plugin)
        {
            this.Plugin = Plugin;
            howLongToBeatClient = new HowLongToBeatClient();
        }


        protected override bool LoadDatabase()
        {
            try
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                Database = new GameHowLongToBeatCollection(Paths.PluginDatabasePath);
                Database.SetGameInfo<HltbDataUser>(PlayniteApi);

                Database.UserHltbData = howLongToBeatClient.LoadUserData();

                DeleteDataWithDeletedGame();

                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                logger.Info($"LoadDatabase with {Database.Count} items - {string.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10)}");
            }
            catch (Exception ex)
            {
                Common.LogError(ex, false, true, "SuccessStory");
                return false;
            }

            return true;
        }


        public override void GetSelectData()
        {
            var View = new OptionsDownloadData(PlayniteApi);
            Window windowExtension = PlayniteUiHelper.CreateExtensionWindow(PlayniteApi, PluginName + " - " + resources.GetString("LOCCommonSelectData"), View);
            windowExtension.ShowDialog();

            var PlayniteDb = View.GetFilteredGames();
            bool OnlyMissing = View.GetOnlyMissing();

            if (PlayniteDb == null)
            {
                return;
            }

            if (OnlyMissing)
            {
                PlayniteDb = PlayniteDb.FindAll(x => !Get(x.Id, true).HasData);
            }

            GlobalProgressOptions globalProgressOptions = new GlobalProgressOptions(
                $"{PluginName} - {resources.GetString("LOCCommonGettingData")}",
                true
            );
            globalProgressOptions.IsIndeterminate = false;

            PlayniteApi.Dialogs.ActivateGlobalProgress((activateGlobalProgress) =>
            {
                try
                {
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();

                    activateGlobalProgress.ProgressMaxValue = (double)PlayniteDb.Count();

                    string CancelText = string.Empty;

                    foreach (Game game in PlayniteDb)
                    {
                        if (activateGlobalProgress.CancelToken.IsCancellationRequested)
                        {
                            CancelText = " canceled";
                            break;
                        }

                        Thread.Sleep(10);
                        AddData(game);

                        activateGlobalProgress.CurrentProgressValue++;
                    }

                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;
                    logger.Info($"Task GetSelectData(){CancelText} - {string.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10)} for {activateGlobalProgress.CurrentProgressValue}/{(double)PlayniteDb.Count()} items");
                }
                catch (Exception ex)
                {
                    Common.LogError(ex, false, true, PluginName);
                }
            }, globalProgressOptions);
        }

        
        public override GameHowLongToBeat Get(Guid Id, bool OnlyCache = false, bool Force = false)
        {
            GameHowLongToBeat gameHowLongToBeat = GetOnlyCache(Id);

            if (!OnlyCache && gameHowLongToBeat != null && !gameHowLongToBeat.HasData && !gameHowLongToBeat.HasDataEmpty)
            {
                gameHowLongToBeat = null;
            }

            if ((gameHowLongToBeat == null && !OnlyCache) || Force)
            {
                gameHowLongToBeat = howLongToBeatClient.SearchData(PlayniteApi.Database.Games.Get(Id));

                if (gameHowLongToBeat != null)
                {
                    AddOrUpdate(gameHowLongToBeat);
                }
            }

            if (gameHowLongToBeat == null)
            {
                Game game = PlayniteApi.Database.Games.Get(Id);
                if (game != null)
                {
                    gameHowLongToBeat = GetDefault(game);
                }
            }

            return gameHowLongToBeat;
        }


        public void AddData(Game game)
        {
            GameHowLongToBeat gameHowLongToBeat = Get(game, true);

            if (gameHowLongToBeat.Items.Count > 0)
            {
                logger.Warn($"Data is already added for {game.Name}");
                return;
            }

            List<HltbDataUser> dataSearch = HowLongToBeat.PluginDatabase.howLongToBeatClient.Search(PlayniteTools.NormalizeGameName(game.Name));

            if (dataSearch.Count == 1 && PluginSettings.Settings.AutoAccept)
            {
                gameHowLongToBeat.Items = new List<HltbDataUser>() { dataSearch.First() };
                AddOrUpdate(gameHowLongToBeat);
            }
            else
            {
                if (dataSearch.Count > 0 && PluginSettings.Settings.UseMatchValue)
                {
                    var FuzzList = dataSearch.Select(x => new { MatchPercent = Fuzz.Ratio(game.Name.ToLower(), x.Name.ToLower()), Data = x }).OrderByDescending(x => x.MatchPercent).ToList();

                    if (FuzzList.First().MatchPercent >= PluginSettings.Settings.MatchValue)
                    {
                        gameHowLongToBeat.Items = new List<HltbDataUser>() { FuzzList.First().Data };
                        AddOrUpdate(gameHowLongToBeat);
                        return;
                    }
                }

                if (dataSearch.Count > 0 && PluginSettings.Settings.ShowWhenMismatch)
                {
                    Get(game, false, true);
                }
            }
        }


        public override void Refresh(Guid Id)
        {
            GlobalProgressOptions globalProgressOptions = new GlobalProgressOptions(
                $"{PluginName} - {resources.GetString("LOCCommonProcessing")}",
                false
            );
            globalProgressOptions.IsIndeterminate = true;

            PlayniteApi.Dialogs.ActivateGlobalProgress((activateGlobalProgress) =>
            {
                RefreshElement(Id);
            }, globalProgressOptions);
        }

        public override void Refresh(List<Guid> Ids)
        {
            GlobalProgressOptions globalProgressOptions = new GlobalProgressOptions(
                $"{PluginName} - {resources.GetString("LOCCommonProcessing")}",
                true
            );
            globalProgressOptions.IsIndeterminate = false;

            PlayniteApi.Dialogs.ActivateGlobalProgress((activateGlobalProgress) =>
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                activateGlobalProgress.ProgressMaxValue = Ids.Count;

                string CancelText = string.Empty;

                foreach (Guid Id in Ids)
                {
                    if (activateGlobalProgress.CancelToken.IsCancellationRequested)
                    {
                        CancelText = " canceled";
                        break;
                    }

                    RefreshElement(Id);
                    activateGlobalProgress.CurrentProgressValue++;
                }

                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                logger.Info($"Task Refresh(){CancelText} - {string.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10)} for {activateGlobalProgress.CurrentProgressValue}/{Ids.Count} items");
            }, globalProgressOptions);
        }

        public void RefreshAll()
        {
            GlobalProgressOptions globalProgressOptions = new GlobalProgressOptions(
                $"{PluginName} - {resources.GetString("LOCCommonProcessing")}",
                true
            );
            globalProgressOptions.IsIndeterminate = false;

            PlayniteApi.Dialogs.ActivateGlobalProgress((activateGlobalProgress) =>
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                string CancelText = string.Empty;

                var db = Database.Where(x => x.HasData);
                activateGlobalProgress.ProgressMaxValue = (double)db.Count();

                foreach (var item in db)
                {
                    if (activateGlobalProgress.CancelToken.IsCancellationRequested)
                    {
                        CancelText = " canceled";
                        break;
                    }

                    if (item.DateLastRefresh.AddMonths(1) < DateTime.Now)
                    {
                        RefreshElement(item.Id);
                    }

                    activateGlobalProgress.CurrentProgressValue++;
                }

                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                logger.Info($"Task RefreshAll(){CancelText} - {string.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10)} for {activateGlobalProgress.CurrentProgressValue}/{(double)db.Count()} items");
            }, globalProgressOptions);
        }

        private void RefreshElement(Guid Id)
        {
            var loadedItem = Get(Id, true);
            List<HltbDataUser> dataSearch = howLongToBeatClient.Search(loadedItem.GetData().Name);

            HltbDataUser webDataSearch = dataSearch.Find(x => x.Id == loadedItem.GetData().Id);
            if (webDataSearch != null)
            {
                if (!ReferenceEquals(loadedItem.GetData(), webDataSearch))
                {
                    loadedItem.Items = new List<HltbDataUser> { webDataSearch };
                    loadedItem.DateLastRefresh = DateTime.Now;
                    Update(loadedItem);
                }
            }
        }

        public override void RefreshWithNoData(List<Guid> Ids)
        {
            GlobalProgressOptions globalProgressOptions = new GlobalProgressOptions(
                $"{PluginName} - {resources.GetString("LOCCommonProcessing")}",
                true
            );
            globalProgressOptions.IsIndeterminate = false;

            PlayniteApi.Dialogs.ActivateGlobalProgress((activateGlobalProgress) =>
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                activateGlobalProgress.ProgressMaxValue = Ids.Count;

                string CancelText = string.Empty;

                foreach (Guid Id in Ids)
                {
                    if (activateGlobalProgress.CancelToken.IsCancellationRequested)
                    {
                        CancelText = " canceled";
                        break;
                    }

                    Game game = PlayniteApi.Database.Games.Get(Id);
                    if (game != null)
                    {
                        AddData(game);
                    }
                    
                    activateGlobalProgress.CurrentProgressValue++;
                }

                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                logger.Info($"Task Refresh(){CancelText} - {string.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10)} for {activateGlobalProgress.CurrentProgressValue}/{Ids.Count} items");
            }, globalProgressOptions);
        }


        #region Tag
        public override void AddTag(Game game, bool noUpdate = false)
        {
            GetPluginTags();
            GameHowLongToBeat gameHowLongToBeat = Get(game, true);

            if (gameHowLongToBeat.HasData)
            {
                try
                {
                    HltbDataUser hltbDataUser = gameHowLongToBeat.GetData();
                    Guid? TagId = FindGoodPluginTags(hltbDataUser);

                    if (TagId != null)
                    {
                        if (game.TagIds != null)
                        {
                            game.TagIds.Add((Guid)TagId);
                        }
                        else
                        {
                            game.TagIds = new List<Guid> { (Guid)TagId };
                        }

                        if (!noUpdate)
                        {
                            Application.Current.Dispatcher?.Invoke(() =>
                            {
                                PlayniteApi.Database.Games.Update(game);
                                game.OnPropertyChanged();
                            }, DispatcherPriority.Send);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Common.LogError(ex, false, $"Tag insert error with {game.Name}", true, PluginName, string.Format(resources.GetString("LOCCommonNotificationTagError"), game.Name));
                }
            }
        }

        private Guid? FindGoodPluginTags(HltbDataUser hltbDataUser)
        {
            long hltbTime = hltbDataUser.GameHltbData.TimeToBeat;

            // Add tag
            if (hltbTime != 0)
            {
                if (hltbTime < 3600)
                {
                    return CheckTagExist($"{resources.GetString("LOCCommon0to1")}");
                }
                if (hltbTime < 18000)
                {
                    return CheckTagExist($"{resources.GetString("LOCCommon1to5")}");
                }
                if (hltbTime < 36000)
                {
                    return CheckTagExist($"{resources.GetString("LOCCommon5to10")}");
                }
                if (hltbTime < 72000)
                {
                    return CheckTagExist($"{resources.GetString("LOCCommon10to20")}");
                }
                if (hltbTime < 108000)
                {
                    return CheckTagExist($"{resources.GetString("LOCCommon20to30")}");
                }
                if (hltbTime < 144000)
                {
                    return CheckTagExist($"{resources.GetString("LOCCommon30to40")}");
                }
                if (hltbTime < 180000)
                {
                    return CheckTagExist($"{resources.GetString("LOCCommon40to50")}");
                }
                if (hltbTime < 216000)
                {
                    return CheckTagExist($"{resources.GetString("LOCCommon50to60")}");
                }
                if (hltbTime < 252000)
                {
                    return CheckTagExist($"{resources.GetString("LOCCommon60to70")}");
                }
                if (hltbTime < 288000)
                {
                    return CheckTagExist($"{resources.GetString("LOCCommon70to80")}");
                }
                if (hltbTime < 324000)
                {
                    return CheckTagExist($"{resources.GetString("LOCCommon80to90")}");
                }
                if (hltbTime < 360000)
                {
                    return CheckTagExist($"{resources.GetString("LOCCommon90to100")}");
                }
                if (hltbTime >= 360000)
                {
                    return CheckTagExist($"{resources.GetString("LOCCommon100plus")}");
                }
            }

            return null;
        }
        #endregion


        #region User data
        public TitleList GetUserHltbData(int HltbId)
        {
            try
            {
                if (Database.UserHltbData.TitlesList == null || Database.UserHltbData.TitlesList.Count == 0)
                {
                    return null;
                }
                return Database.UserHltbData.TitlesList.Find(x => x.Id == HltbId);
            }
            catch (Exception ex)
            {
                Common.LogError(ex, true);
                logger.Warn($"No HltbData for {HltbId}");
                return null;
            }
        }
        
        public TitleList GetUserHltbDataCurrent(int HltbId, string UserGameId = "")
        {
            try
            {
                List<TitleList> all = GetUserHltbDataAll(HltbId);
                if (all?.Count == 0)
                {
                    return null;
                }

                if (UserGameId.IsNullOrEmpty())
                {
                    return all.OrderByDescending(x => x.GameStatuses.Where(y => y.Status == StatusType.Playing)?.Count() > 0).ThenByDescending(x => x.LastUpdate).First();
                }
                else
                {
                    return all.Where(x => x.UserGameId.IsEqual(UserGameId)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Common.LogError(ex, true);
                logger.Warn($"No HltbData for {HltbId}");
                return null;
            }
        }

        public List<TitleList> GetUserHltbDataAll(int HltbId)
        {
            try
            {
                if (Database.UserHltbData.TitlesList == null || Database.UserHltbData.TitlesList.Count == 0)
                {
                    return null;
                }
                return Database.UserHltbData.TitlesList.FindAll(x => x.Id == HltbId).ToList();
            }
            catch (Exception ex)
            {
                Common.LogError(ex, true);
                logger.Warn($"No HltbData for {HltbId}");
                return null;
            }
        }


        public void RefreshUserData()
        {
            logger.Info("RefreshUserData()");

            GlobalProgressOptions globalProgressOptions = new GlobalProgressOptions(
                $"{PluginName} - {resources.GetString("LOCHowLongToBeatPluginGetUserView")}",
                false
            );
            globalProgressOptions.IsIndeterminate = true;

            PlayniteApi.Dialogs.ActivateGlobalProgress((activateGlobalProgress) =>
            {
                try
                {
                    HltbUserStats UserHltbData = howLongToBeatClient.GetUserData();

                    if (UserHltbData != null)
                    {
                        logger.Info($"Find {UserHltbData.TitlesList?.Count ?? 0} games");
                        FileSystem.WriteStringToFileSafe(Path.Combine(Paths.PluginUserDataPath, "HltbUserStats.json"), Serialization.ToJson(UserHltbData));
                        Database.UserHltbData = UserHltbData;
                    }
                    else
                    {
                        logger.Info($"Find no data");
                    }
                }
                catch (Exception ex)
                {
                    Common.LogError(ex, false, true, PluginName);
                }
            }, globalProgressOptions);
        }

        public void RefreshUserData(int game_id)
        {
            Task.Run(() => 
            {
                try
                {
                    TitleList titleList = howLongToBeatClient.GetUserData(game_id);

                    if (titleList != null)
                    {
                        int index = Database.UserHltbData.TitlesList.FindIndex(x => x.Id == game_id);

                        if (index > -1)
                        {
                            Database.UserHltbData.TitlesList[index] = titleList;
                        }
                        else
                        {
                            Database.UserHltbData.TitlesList.Add(titleList);
                        }

                        FileSystem.WriteStringToFileSafe(Path.Combine(Paths.PluginUserDataPath, "HltbUserStats.json"), Serialization.ToJson(Database.UserHltbData));
                    }
                }
                catch (Exception ex)
                {
                    Common.LogError(ex, false, true, PluginName);
                }
            });
        }

        public bool SetCurrentPlayTime(Game game, ulong ElapsedSeconds = 0, bool NoPlaying = false, bool IsCompleted = false, bool IsMain = false, bool IsMainSide = false, bool Is100 = false)
        {
            try
            {
                if (howLongToBeatClient.GetIsUserLoggedIn())
                {
                    GameHowLongToBeat gameHowLongToBeat = Database.Get(game.Id);

                    if (gameHowLongToBeat != null)
                    {
                        TimeSpan time = TimeSpan.FromSeconds(game.Playtime + ElapsedSeconds);

                        // TODO Enough?
                        string Platform = string.Empty;
                        List<HltbPlatform> finded = hltbPlatforms.FindAll(x => game.Platforms?.FirstOrDefault().Name.Contains(x.Name, StringComparison.InvariantCultureIgnoreCase) ?? false);
                        if (finded?.Count > 0)
                        {
                            Platform = finded.First().Name ?? string.Empty;
                        }
                        
                        if (Platform.IsNullOrEmpty())
                        {
                            logger.Warn($"No platform find for {game.Name}");
                        }

                        string StorefrontName = string.Empty;
                        Storefront storefront = PluginSettings.Settings.Storefronts.Where(x => x.SourceId != default(Guid) && x.SourceId == game.SourceId).FirstOrDefault();
                        if (storefront != null)
                        {
                            StorefrontName = storefront.HltbStorefrontName;
                        }

                        TitleList HltbData = GetUserHltbDataCurrent(gameHowLongToBeat.GetData().Id, gameHowLongToBeat.UserGameId);
                        int edit_id = 0;
                        HltbPostData hltbPostData = new HltbPostData();
                        if (HltbData != null)
                        {
                            if (howLongToBeatClient.EditIdExist(HltbData.UserGameId))
                            {
                                edit_id = int.Parse(HltbData.UserGameId);
                                hltbPostData = howLongToBeatClient.GetSubmitData(gameHowLongToBeat.Name, edit_id.ToString());
                            }
                        }
                        else
                        {
                            // Find existing in website
                            HltbDataUser hltbDataUser = gameHowLongToBeat.GetData();
                            if (hltbDataUser != null)
                            {
                                string tmpEditId = howLongToBeatClient.FindIdExisting(hltbDataUser.Id.ToString());
                                if (!tmpEditId.IsNullOrEmpty())
                                {
                                    edit_id = int.Parse(tmpEditId);
                                    hltbPostData = howLongToBeatClient.GetSubmitData(gameHowLongToBeat.Name, tmpEditId);
                                }
                                else
                                {
                                    logger.Info($"No existing data in website find for {game.Name}");
                                }
                            }
                        }

                        if (hltbPostData == null)
                        {
                            logger.Warn($"No hltbPostData for {game.Name}");
                            return false;
                        }

                        hltbPostData.user_id = Database.UserHltbData.UserId;
                        hltbPostData.edit_id = edit_id;
                        hltbPostData.game_id = gameHowLongToBeat.GetData().Id;
                        hltbPostData.custom_title = gameHowLongToBeat.GetData().Name;
                        hltbPostData.platform = Platform;
                        hltbPostData.storefront = StorefrontName;

                        if (!NoPlaying)
                        {
                            hltbPostData.list_p = "1";
                        }
                        else if (hltbPostData.list_b.IsNullOrEmpty() && hltbPostData.list_c.IsNullOrEmpty() && hltbPostData.list_cp.IsNullOrEmpty() 
                            && hltbPostData.list_p.IsNullOrEmpty() && hltbPostData.list_r.IsNullOrEmpty() &&hltbPostData.list_rt.IsNullOrEmpty())
                        {
                            hltbPostData.list_p = "1";
                        }

                        if (IsCompleted)
                        {
                            hltbPostData.list_cp = "1";
                            hltbPostData.play_num = hltbPostData.play_num == 0 ? 1 : hltbPostData.play_num;

                            if (IsMain)
                            {
                                hltbPostData.c_main_h = (time.Hours + (24 * time.Days)).ToString();
                                hltbPostData.c_main_m = time.Minutes.ToString();
                                hltbPostData.c_main_s = time.Seconds.ToString();

                                hltbPostData.compday = ((DateTime)game.LastActivity).Day.ToString();
                                hltbPostData.compmonth = ((DateTime)game.LastActivity).Month.ToString();
                                hltbPostData.compyear = ((DateTime)game.LastActivity).Year.ToString();
                            }

                            if (IsMainSide)
                            {
                                hltbPostData.c_plus_h = (time.Hours + (24 * time.Days)).ToString();
                                hltbPostData.c_plus_m = time.Minutes.ToString();
                                hltbPostData.c_plus_s = time.Seconds.ToString();

                                if (hltbPostData.compday.IsNullOrEmpty())
                                {
                                    hltbPostData.compday = ((DateTime)game.LastActivity).Day.ToString();
                                    hltbPostData.compmonth = ((DateTime)game.LastActivity).Month.ToString();
                                    hltbPostData.compyear = ((DateTime)game.LastActivity).Year.ToString();
                                }
                            }

                            if (Is100)
                            {
                                hltbPostData.c_100_h = (time.Hours + (24 * time.Days)).ToString();
                                hltbPostData.c_100_m = time.Minutes.ToString();
                                hltbPostData.c_100_s = time.Seconds.ToString();

                                if (hltbPostData.compday.IsNullOrEmpty())
                                {
                                    hltbPostData.compday = ((DateTime)game.LastActivity).Day.ToString();
                                    hltbPostData.compmonth = ((DateTime)game.LastActivity).Month.ToString();
                                    hltbPostData.compyear = ((DateTime)game.LastActivity).Year.ToString();
                                }
                            }
                        }

                        hltbPostData.protime_h = (time.Hours + (24 * time.Days)).ToString();
                        hltbPostData.protime_m = time.Minutes.ToString();
                        hltbPostData.protime_s = time.Seconds.ToString();


                        return howLongToBeatClient.PostData(game, hltbPostData).GetAwaiter().GetResult(); ;
                    }
                }
                else
                {
                    PlayniteApi.Notifications.Add(new NotificationMessage(
                        $"{PluginName}-NotLoggedIn-Error",
                        PluginName + System.Environment.NewLine + resources.GetString("LOCCommonNotLoggedIn"),
                        NotificationType.Error,
                        () => Plugin.OpenSettingsView()
                    ));
                    return false;
                }
            }
            catch (Exception ex)
            {
                Common.LogError(ex, false, true, PluginName);
            }

            return false;
        }


        public double GetAvgGameByMonth()
        {
            double result = 0;

            Dictionary<string, int> DataByMonth = new Dictionary<string, int>();
            foreach (TitleList titleList in Database.UserHltbData.TitlesList)
            {
                string Month = titleList.Completion?.ToString("yyyy-MM");
                if (!Month.IsNullOrEmpty())
                {
                    if (DataByMonth.ContainsKey(Month))
                    {
                        DataByMonth[Month]++;
                    }
                    else
                    {
                        DataByMonth.Add(Month, 1);
                    }
                }
            }

            if (DataByMonth.Count > 0)
            {
                foreach (KeyValuePair<string, int> data in DataByMonth)
                {
                    result += data.Value;
                }
                result /= DataByMonth.Count;
            }

            return result;
        }

        public long GetAvgTimeByGame()
        {
            long result = 0;
            double count = 0;
            
            foreach (TitleList titleList in Database.UserHltbData.TitlesList)
            {
                if (titleList.Completion != null && titleList.HltbUserData.TimeToBeat != 0)
                {
                    count++;
                    result += titleList.HltbUserData.TimeToBeat;
                }
            }

            if (count > 0)
            {
                result = (long)(result / count);
            }

            return result;
        }

        public int GetCountGameBeatenBeforeTime()
        {
            return Database.UserHltbData.TitlesList
                .Where(x => x.HltbUserData.TimeToBeat != 0 && x.Completion != null 
                            && Get(x.GameId, true) != null && Get(x.GameId, true).GetData() != null && Get(x.GameId, true).GetData().GameHltbData != null && Get(x.GameId, true).GetData().GameHltbData.TimeToBeat != 0
                            && Get(x.GameId, true).GetData().GameHltbData.TimeToBeat > x.CurrentTime).Count();
        }

        public int GetCountGameBeatenAfterTime()
        {
            return Database.UserHltbData.TitlesList
                .Where(x => x.HltbUserData.TimeToBeat != 0 && x.Completion != null
                        && Get(x.GameId, true) != null && Get(x.GameId, true).GetData() != null && Get(x.GameId, true).GetData().GameHltbData != null && Get(x.GameId, true).GetData().GameHltbData.TimeToBeat != 0
                        && Get(x.GameId, true).GetData().GameHltbData.TimeToBeat <= x.CurrentTime).Count();
        }

        public int GetCountGameBeatenReplays()
        {
            return Database.UserHltbData.TitlesList.Where(x => x.IsReplay).Count();
        }

        public int GetCountGameRetired()
        {
            return Database.UserHltbData.TitlesList.Where(x => x.IsRetired).Count();
        }
        #endregion


        public override void SetThemesResources(Game game)
        {
            GameHowLongToBeat gameHowLongToBeat = Get(game, true);

            if (gameHowLongToBeat == null || !gameHowLongToBeat.HasData)
            {
                PluginSettings.Settings.HasData = false;
                PluginSettings.Settings.MainStory = 0;
                PluginSettings.Settings.MainStoryFormat = "--";
                PluginSettings.Settings.MainExtra = 0;
                PluginSettings.Settings.MainExtraFormat = "--";
                PluginSettings.Settings.Completionist = 0;
                PluginSettings.Settings.CompletionistFormat = "--";
                PluginSettings.Settings.Solo = 0;
                PluginSettings.Settings.SoloFormat = "--";
                PluginSettings.Settings.CoOp = 0;
                PluginSettings.Settings.CoOpFormat = "--";
                PluginSettings.Settings.Vs = 0;
                PluginSettings.Settings.VsFormat = "--";

                PluginSettings.Settings.TimeToBeat = 0;
                PluginSettings.Settings.TimeToBeatFormat = "--";

                return;
            }

            PluginSettings.Settings.HasData = gameHowLongToBeat.HasData;
            PluginSettings.Settings.MainStory = gameHowLongToBeat.GetData().GameHltbData.MainStory;
            PluginSettings.Settings.MainStoryFormat = gameHowLongToBeat.GetData().GameHltbData.MainStoryFormat;
            PluginSettings.Settings.MainExtra = gameHowLongToBeat.GetData().GameHltbData.MainExtra;
            PluginSettings.Settings.MainExtraFormat = gameHowLongToBeat.GetData().GameHltbData.MainExtraFormat;
            PluginSettings.Settings.Completionist = gameHowLongToBeat.GetData().GameHltbData.Completionist;
            PluginSettings.Settings.CompletionistFormat = gameHowLongToBeat.GetData().GameHltbData.CompletionistFormat;
            PluginSettings.Settings.Solo = gameHowLongToBeat.GetData().GameHltbData.Solo;
            PluginSettings.Settings.SoloFormat = gameHowLongToBeat.GetData().GameHltbData.SoloFormat;
            PluginSettings.Settings.CoOp = gameHowLongToBeat.GetData().GameHltbData.CoOp;
            PluginSettings.Settings.CoOpFormat = gameHowLongToBeat.GetData().GameHltbData.CoOpFormat;
            PluginSettings.Settings.Vs = gameHowLongToBeat.GetData().GameHltbData.Vs;
            PluginSettings.Settings.VsFormat = gameHowLongToBeat.GetData().GameHltbData.VsFormat;

            PluginSettings.Settings.TimeToBeat = gameHowLongToBeat.GetData().GameHltbData.TimeToBeat;
            PluginSettings.Settings.TimeToBeatFormat = gameHowLongToBeat.GetData().GameHltbData.TimeToBeatFormat;
        }
    }
}
