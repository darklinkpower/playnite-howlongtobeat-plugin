﻿using HowLongToBeat.Services;
using HowLongToBeat.Views;
using Playnite.SDK;
using Playnite.SDK.Events;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HowLongToBeat.Models;
using CommonPluginsShared;
using System.Windows.Threading;
using CommonPluginsShared.PlayniteExtended;
using CommonPluginsShared.Controls;
using HowLongToBeat.Controls;

namespace HowLongToBeat
{
    public class HowLongToBeat : PluginExtended<HowLongToBeatSettingsViewModel, HowLongToBeatDatabase>
    {
        public override Guid Id { get; } = Guid.Parse("e08cd51f-9c9a-4ee3-a094-fde03b55492f");

        private OldToNew oldToNew;


        public HowLongToBeat(IPlayniteAPI api) : base(api)
        {
            PluginDatabase.InitializeClient(this);

            // Old database
            oldToNew = new OldToNew(this.GetPluginUserDataPath());

            // Custom theme button
            EventManager.RegisterClassHandler(typeof(Button), Button.ClickEvent, new RoutedEventHandler(OnCustomThemeButtonClick));

            // Custom elements integration
            AddCustomElementSupport(new AddCustomElementSupportArgs
            {
                ElementList = new List<string> { "PluginButton", "PluginProgressBar", "PluginViewItem" },
                SourceName = "HowLongToBeat"
            });

            // Settings integration
            AddSettingsSupport(new AddSettingsSupportArgs
            {
                SourceName = "HowLongToBeat",
                SettingsRoot = $"{nameof(PluginSettings)}.{nameof(PluginSettings.Settings)}"
            });
        }


        #region Custom event
        public void OnCustomThemeButtonClick(object sender, RoutedEventArgs e)
        {
            string ButtonName = string.Empty;
            try
            {
                ButtonName = ((Button)sender).Name;
                if (ButtonName == "PART_CustomHowLongToBeatButton")
                {
                    Common.LogDebug(true, $"OnCustomThemeButtonClick()");

                    var ViewExtension = new HowLongToBeatUserView();
                    ViewExtension.Height = 660;
                    ViewExtension.Width = 1290;
                    Window windowExtension = PlayniteUiHelper.CreateExtensionWindow(PlayniteApi, "HowLongToBeat", ViewExtension);
                    windowExtension.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Common.LogError(ex, false);
            }
        }

        private void Games_ItemCollectionChanged(object sender, ItemCollectionChangedEventArgs<Game> e)
        {
            if (PluginSettings.Settings.AutoImport)
            {
                foreach (Game game in e.AddedItems)
                {
                    PluginDatabase.AddData(game);
                }
            }
        }
        #endregion


        #region Theme integration
        // Button on top panel
        public override IEnumerable<TopPanelItem> GetTopPanelItems()
        {
            if (PluginSettings.Settings.EnableIntegrationButtonHeader)
            {
                yield return new TopPanelItem()
                {
                    Icon = new TextBlock
                    {
                        Text = "\ue90d",
                        FontSize = 20,
                        FontFamily = resources.GetResource("CommonFont") as FontFamily
                    },
                    Title = resources.GetString("LOCHowLongToBeat"),
                    Activated = () =>
                    {
                        var ViewExtension = new HowLongToBeatUserView();
                        ViewExtension.Height = 660;
                        ViewExtension.Width = 1290;
                        Window windowExtension = PlayniteUiHelper.CreateExtensionWindow(PlayniteApi, "HowLongToBeat", ViewExtension);
                        windowExtension.ShowDialog();
                    }
                };
            }

            yield break;
        }

        // List custom controls
        public override Control GetGameViewControl(GetGameViewControlArgs args)
        {
            if (args.Name == "PluginButton")
            {
                return new PluginButton();
            }

            if (args.Name == "PluginProgressBar")
            {
                return new PluginProgressBar();
            }

            if (args.Name == "PluginViewItem")
            {
                return new PluginViewItem();
            }

            return null;
        }

        // SidebarItem
        public class HowLongToBeatViewSidebar : SidebarItem
        {
            public HowLongToBeatViewSidebar()
            {
                Type = SiderbarItemType.View;
                Title = resources.GetString("LOCHowLongToBeat");
                Icon = new TextBlock
                {
                    Text = "\ue90d",
                    FontFamily = resources.GetResource("CommonFont") as FontFamily
                };
                Opened = () =>
                {
                    SidebarItemControl sidebarItemControl = new SidebarItemControl(PluginDatabase.PlayniteApi);
                    sidebarItemControl.SetTitle(resources.GetString("LOCHowLongToBeat"));
                    sidebarItemControl.AddContent(new HowLongToBeatUserView());

                    return sidebarItemControl;
                };
            }
        }

        public override IEnumerable<SidebarItem> GetSidebarItems()
        {
            var items = new List<SidebarItem>
            {
                new HowLongToBeatViewSidebar()
            };
            return items;
        }
        #endregion


        #region Menus
        // To add new game menu items override GetGameMenuItems
        public override IEnumerable<GameMenuItem> GetGameMenuItems(GetGameMenuItemsArgs args)
        {
            Game GameMenu = args.Games.First();
            List<Guid> Ids = args.Games.Select(x => x.Id).ToList();
            GameHowLongToBeat gameHowLongToBeat = PluginDatabase.Get(GameMenu, true);

            List<GameMenuItem> gameMenuItems = new List<GameMenuItem>
            {
                new GameMenuItem {
                    MenuSection = resources.GetString("LOCHowLongToBeat"),
                    Description = resources.GetString("LOCHowLongToBeatPluginView"),
                    Action = (gameMenuItem) =>
                    {
                        try
                        {
                            gameHowLongToBeat = PluginDatabase.Get(GameMenu);

                            if (gameHowLongToBeat.HasData || gameHowLongToBeat.HasDataEmpty)
                            {
                                var ViewExtension = new HowLongToBeatView(gameHowLongToBeat);
                                Window windowExtension = PlayniteUiHelper.CreateExtensionWindow(PlayniteApi, "HowLongToBeat", ViewExtension);
                                windowExtension.ShowDialog();
                            }
                        }
                        catch (Exception ex)
                        {
                            Common.LogError(ex, false, $"Error to load game data for {args.Games.First().Name}");
                            PlayniteApi.Dialogs.ShowErrorMessage(resources.GetString("LOCDatabaseErroTitle"), "HowLongToBeat");
                        }
                    }
                }
            };
            
            if (gameHowLongToBeat.HasData || gameHowLongToBeat.HasDataEmpty)
            {
                gameMenuItems.Add(new GameMenuItem
                {
                    MenuSection = resources.GetString("LOCHowLongToBeat"),
                    Description = "-"
                });

                gameMenuItems.Add(new GameMenuItem
                {
                    MenuSection = resources.GetString("LOCHowLongToBeat"),
                    Description = resources.GetString("LOCHowLongToBeatSetCurrentTimeManual"),
                    Action = (mainMenuItem) =>
                    {
                        GlobalProgressOptions globalProgressOptions = new GlobalProgressOptions(
                            $"HowLongToBeat - {resources.GetString("LOCCommonProcessing")}",
                            false
                        );
                        globalProgressOptions.IsIndeterminate = true;

                        PlayniteApi.Dialogs.ActivateGlobalProgress((activateGlobalProgress) =>
                        {
                            PluginDatabase.SetCurrentPlayTime(GameMenu, 0);
                        }, globalProgressOptions);
                    }
                });

                // Refresh plugin data for the selected game
                gameMenuItems.Add(new GameMenuItem
                {
                    MenuSection = resources.GetString("LOCHowLongToBeat"),
                    Description = resources.GetString("LOCCommonRefreshGameData"),
                    Action = (gameMenuItem) =>
                    {
                        if (Ids.Count == 1)
                        {
                            PluginDatabase.Refresh(GameMenu.Id);
                        }
                        else
                        {
                            PluginDatabase.Refresh(Ids);
                        }
                    }
                });

                gameMenuItems.Add(new GameMenuItem
                {
                    MenuSection = resources.GetString("LOCHowLongToBeat"),
                    Description = "-"
                });

                gameMenuItems.Add(new GameMenuItem
                {
                    MenuSection = resources.GetString("LOCHowLongToBeat"),
                    Description = resources.GetString("LOCCommonDeleteGameData"),
                    Action = (gameMenuItem) =>
                    {
                        if (Ids.Count == 1)
                        {
                            PluginDatabase.Remove(GameMenu.Id);
                        }
                        else
                        {
                            PluginDatabase.Remove(Ids);
                        }
                    }
                });
            }

#if DEBUG
            gameMenuItems.Add(new GameMenuItem
            {
                MenuSection = resources.GetString("LOCHowLongToBeat"),
                Description = "-"
            });
            gameMenuItems.Add(new GameMenuItem
            {
                MenuSection = resources.GetString("LOCHowLongToBeat"),
                Description = "Test",
                Action = (mainMenuItem) => { }
            });
#endif

            return gameMenuItems;
        }

        // To add new main menu items override GetMainMenuItems
        public override IEnumerable<MainMenuItem> GetMainMenuItems(GetMainMenuItemsArgs args)
        {
            string MenuInExtensions = string.Empty;
            if (PluginSettings.Settings.MenuInExtensions)
            {
                MenuInExtensions = "@";
            }

            List <MainMenuItem> mainMenuItems = new List<MainMenuItem>
            {
                new MainMenuItem
                {
                    MenuSection = MenuInExtensions + resources.GetString("LOCHowLongToBeat"),
                    Description = resources.GetString("LOCHowLongToBeatPluginUserView"),
                    Action = (mainMenuItem) =>
                    {
                        var ViewExtension = new HowLongToBeatUserView();
                        ViewExtension.Height = 660;
                        ViewExtension.Width = 1290;
                        Window windowExtension = PlayniteUiHelper.CreateExtensionWindow(PlayniteApi, "HowLongToBeat", ViewExtension);
                        windowExtension.ShowDialog();
                    }
                },

                new MainMenuItem
                {
                    MenuSection = MenuInExtensions + resources.GetString("LOCHowLongToBeat"),
                    Description = "-"
                },

                new MainMenuItem
                {
                    MenuSection = MenuInExtensions + resources.GetString("LOCHowLongToBeat"),
                    Description = resources.GetString("LOCCommonDownloadPluginData"),
                    Action = (mainMenuItem) =>
                    {
                        PluginDatabase.GetSelectData();
                    }
                },
                new MainMenuItem
                {
                    MenuSection = MenuInExtensions + resources.GetString("LOCHowLongToBeat"),
                    Description = resources.GetString("LOCCommonClearAllDatas"),
                    Action = (mainMenuItem) =>
                    {
                        if (PluginDatabase.ClearDatabase())
                        {
                            PlayniteApi.Dialogs.ShowMessage(resources.GetString("LOCCommonDataRemove"), "HowLongToBeat");
                        }
                        else
                        {
                            PlayniteApi.Dialogs.ShowErrorMessage(resources.GetString("LOCCommonDataErrorRemove"), "HowLongToBeat");
                        }
                    }
                },

                new MainMenuItem
                {
                    MenuSection = MenuInExtensions + resources.GetString("LOCHowLongToBeat"),
                    Description = "-"
                },

                new MainMenuItem
                {
                    MenuSection = MenuInExtensions + resources.GetString("LOCHowLongToBeat"),
                    Description = resources.GetString("LOCHowLongToBeatActualiseUserData"),
                    Action = (mainMenuItem) =>
                    {
                        PluginDatabase.RefreshUserData();
                    }
                },

                new MainMenuItem
                {
                    MenuSection = MenuInExtensions + resources.GetString("LOCHowLongToBeat"),
                    Description = resources.GetString("LOCCommonRefreshAllData"),
                    Action = (mainMenuItem) =>
                    {
                        PluginDatabase.RefreshAll();
                    }
                }
            };


            if (PluginDatabase.PluginSettings.Settings.EnableTag)
            {
                mainMenuItems.Add(new MainMenuItem
                {
                    MenuSection = MenuInExtensions + resources.GetString("LOCHowLongToBeat"),
                    Description = "-"
                });

                // Tag menus
                mainMenuItems.Add(new MainMenuItem
                {
                    MenuSection = MenuInExtensions + resources.GetString("LOCHowLongToBeat"),
                    Description = resources.GetString("LOCCommonAddAllTags"),
                    Action = (mainMenuItem) =>
                    {
                        PluginDatabase.AddTagAllGame();
                    }
                });
                mainMenuItems.Add(new MainMenuItem
                {
                    MenuSection = MenuInExtensions + resources.GetString("LOCHowLongToBeat"),
                    Description = resources.GetString("LOCCommonRemoveAllTags"),
                    Action = (mainMenuItem) =>
                    {
                        PluginDatabase.RemoveTagAllGame();
                    }
                });
            }


#if DEBUG
            mainMenuItems.Add(new MainMenuItem
            {
                MenuSection = MenuInExtensions + resources.GetString("LOCHowLongToBeat"),
                Description = "-"
            });
            mainMenuItems.Add(new MainMenuItem
            {
                MenuSection = MenuInExtensions + resources.GetString("LOCHowLongToBeat"),
                Description = "Test",
                Action = (mainMenuItem) => 
                {

                }
            });
#endif

            return mainMenuItems;
        }
        #endregion


        #region Game event
        public override void OnGameSelected(OnGameSelectedEventArgs args)
        {
            // Old database
            if (oldToNew.IsOld)
            {
                oldToNew.ConvertDB(PlayniteApi);
            }

            try
            {
                if (args.NewValue?.Count == 1)
                {
                    PluginDatabase.GameContext = args.NewValue[0];
                    PluginDatabase.SetThemesResources(PluginDatabase.GameContext);
                }
            }
            catch (Exception ex)
            {
                Common.LogError(ex, false);
            }
        }

        // Add code to be executed when game is finished installing.
        public override void OnGameInstalled(OnGameInstalledEventArgs args)
        {

        }

        // Add code to be executed when game is uninstalled.
        public override void OnGameUninstalled(OnGameUninstalledEventArgs args)
        {

        }

        // Add code to be executed when game is preparing to be started.
        public override void OnGameStarting(OnGameStartingEventArgs args)
        {

        }

        // Add code to be executed when game is started running.
        public override void OnGameStarted(OnGameStartedEventArgs args)
        {

        }

        // Add code to be executed when game is preparing to be started.
        public override void OnGameStopped(OnGameStoppedEventArgs args)
        {
            try
            {
                var TaskGameStopped = Task.Run(() =>
                {
                    if (args.Game.Id == PluginDatabase.GameContext.Id)
                    {
                        PluginDatabase.SetThemesResources(PluginDatabase.GameContext);
                    }
                });
            }
            catch (Exception ex)
            {
                Common.LogError(ex, false);
            }

            // AutoSetCurrentPlayTime
            if (PluginSettings.Settings.AutoSetCurrentPlayTime)
            {
                try
                {
                    MessageBoxResult result = MessageBoxResult.Yes;
                    if (!PluginSettings.Settings.AutoSetCurrentPlayTimeWithoutConfirmation)
                    {
                        result = PlayniteApi.Dialogs.ShowMessage(resources.GetString("LOCHowLongToBeatSetCurrentTime"), "HowLongToBeat", MessageBoxButton.YesNo);
                    }
                    
                    if (result == MessageBoxResult.Yes)
                    {
                        Task.Run(() => 
                        {
                            PluginDatabase.SetCurrentPlayTime(args.Game, args.ElapsedSeconds);
                        });
                    }

                }
                catch (Exception ex)
                {
                    Common.LogError(ex, false);
                }
            }
        }
        #endregion


        #region Application event
        // Add code to be executed when Playnite is initialized.
        public override void OnApplicationStarted(OnApplicationStartedEventArgs args)
        {
            PlayniteApi.Database.Games.ItemCollectionChanged += Games_ItemCollectionChanged; 
        }

        // Add code to be executed when Playnite is shutting down.
        public override void OnApplicationStopped(OnApplicationStoppedEventArgs args)
        {

        }
        #endregion


        // Add code to be executed when library is updated.
        public override void OnLibraryUpdated(OnLibraryUpdatedEventArgs args)
        {

        }


        #region Settings
        public override ISettings GetSettings(bool firstRunSettings)
        {
            return PluginSettings;
        }

        public override UserControl GetSettingsView(bool firstRunSettings)
        {
            return new HowLongToBeatSettingsView(PlayniteApi, this.GetPluginUserDataPath(), PluginSettings.Settings);
        }
        #endregion
    }
}
