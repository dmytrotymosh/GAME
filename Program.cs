using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using GAME;
using GAME.Windows;

namespace GAME
{
    /*internal class Program
    {
        static void Main(string[] args)
        {
            string WITH_ACCOUNT;
            string LEVEL;
            MainMenu: MainMenu mainMenu = new MainMenu();
            string mainMenuChosen = mainMenu.Print();
            WITH_ACCOUNT = mainMenuChosen;
            if (mainMenuChosen == "WITH_ACCOUNT")
            {
                HaveAcocunt: HaveAccountMenu haveAccountMenu = new HaveAccountMenu();
                string haveAccountMenuChosen = haveAccountMenu.Print();
                if (haveAccountMenuChosen == "YES")
                {
                    Autorization: AutorizationMenu autorazationMenu = new AutorizationMenu();
                    (string, string?) autorizationMenuChosen = autorazationMenu.Print();
                    if (autorizationMenuChosen.Item1 == "RETURN")
                    {
                        goto HaveAcocunt;
                    }
                    if (autorizationMenuChosen.Item1 == "CHANGE_THE_NICKNAME")
                    {
                        goto Autorization;
                    }
                    if (autorizationMenuChosen.Item1 == "TRUE")
                    {
                        string? nickname = autorizationMenuChosen.Item2;
                        AccountMenu: AccountMenu accountMenu = new AccountMenu(nickname);
                        string accountMenuChosen = accountMenu.Print();
                        if (accountMenuChosen == "PLAY")
                        {
                            LevelMenu: LevelMenu levelMenu = new LevelMenu();
                            string levelMenuChosen = levelMenu.Print();
                            LEVEL = levelMenuChosen;
                            if (levelMenuChosen == "RETURN")
                            {
                                goto AccountMenu;
                            }
                            else
                            {
                                    InstructionMenu instructionMenu = new InstructionMenu();
                                    string instructionMenuChosen = instructionMenu.Print();
                                    if (instructionMenuChosen == "OK")
                                    {
                                        Game game = new Game();
                                        (string, Dictionary<string, int>?) result = game.StartGame(LEVEL);
                                        if (result.Item1 == "DEFEAT")
                                        {
                                            DefeatWindow defeatWidow = new DefeatWindow();
                                            string defeatWindowChosen = defeatWidow.Print();
                                            if (defeatWindowChosen == "RETURN")
                                            {
                                                goto MainMenu;
                                            }
                                        }
                                        else if (result.Item1 == "WIN")
                                        {
                                        VictoryWindow victoryWidow = new VictoryWindow();
                                        string victoryWindowChosen = victoryWidow.Print(result.Item2);
                                        accountMenu.AddToStatistic(result.Item2, LEVEL);
                                        if (victoryWindowChosen == "RETURN")
                                        {
                                            goto MainMenu;
                                        }
                                    }
                                    }
                                    if (instructionMenuChosen == "RETURN")
                                    {
                                        goto LevelMenu;
                                    }
                            }
                        }
                        if (accountMenuChosen == "SHOW_STATISTIC")
                        {
                            ShowStatistics showStatistics = new ShowStatistics();
                            string showStatisticsChosen = showStatistics.Print(nickname);
                            if (showStatisticsChosen == "RETURN")
                            {
                                goto AccountMenu;
                            }
                        }
                        if (accountMenuChosen == "CHANGE_THE_PASSWORD")
                        {
                            ChangePassword: ChangePassword changePassword = new ChangePassword();
                            string changePasswordChosen = changePassword.Print(nickname);
                            if (changePasswordChosen == "RETURN")
                            {
                                goto AccountMenu;
                            }
                            if (changePasswordChosen == "ENTER_ANOTHER")
                            {
                                goto ChangePassword;
                            }
                            if (changePasswordChosen == "EMPTY_DATA")
                            {
                                InvalidData invalidData = new InvalidData("You can not have empty password!");
                                string invalidDataChosen = invalidData.Print();
                                if (invalidDataChosen == "RETURN")
                                {
                                    goto ChangePassword;
                                }
                            }
                        }
                        if (accountMenuChosen == "RETURN")
                        {
                            goto Autorization;
                        }
                    }
                    if (autorizationMenuChosen.Item1 == "WRONG_DATA")
                    {
                        InvalidData invalidData = new InvalidData("Wrong nickname or password!");
                        string invalidDataChosen = invalidData.Print();
                        if (invalidDataChosen == "RETURN")
                        {
                            goto Autorization;
                        }
                    }
                    if (autorizationMenuChosen.Item1 == "EMPTY_DATA")
                    {
                        InvalidData invalidData = new InvalidData("Nickname or password can not be empty!");
                        string invalidDataChosen = invalidData.Print();
                        if (invalidDataChosen == "RETURN")
                        {
                            goto Autorization;
                        }
                    }
                }
                if (haveAccountMenuChosen == "NO")
                {
                    RegisterMenu: RegisterMenu registerMenu = new RegisterMenu();
                    string registerMenuChosen = registerMenu.Print();
                    if (registerMenuChosen == "RETURN")
                    {
                        goto HaveAcocunt;
                    }
                    if (registerMenuChosen == "CHANGE_THE_NICKNAME")
                    {
                        goto RegisterMenu;
                    }
                    if (registerMenuChosen == "TRUE")
                    {
                        goto HaveAcocunt;
                    }
                    if (registerMenuChosen == "WRONG_DATA")
                    {
                        AccountIsExist accountIsExist = new AccountIsExist();
                        string accountIsExistChosen = accountIsExist.Print();
                        if (accountIsExistChosen == "RETURN")
                        {
                            goto RegisterMenu;
                        }
                    }
                    if (registerMenuChosen == "EMPTY_DATA")
                    {
                        InvalidData invalidData = new InvalidData("Nickname or password can not be empty!");
                        string invalidRegistrationDataChosen = invalidData.Print();
                        if (invalidRegistrationDataChosen == "RETURN")
                        {
                            goto RegisterMenu;
                        }
                    }
                }
                if (haveAccountMenuChosen == "RETURN")
                {
                    goto MainMenu;
                }
            }
            else if (mainMenuChosen == "WITHOUT_ACCOUNT")
            {
                LevelMenu: LevelMenu levelMenu = new LevelMenu();
                string levelMenuChosen = levelMenu.Print();
                LEVEL = levelMenuChosen;
                if (levelMenuChosen == "RETURN")
                {
                    goto MainMenu;
                } 
                else
                {
                        InstructionMenu instructionMenu = new InstructionMenu();
                        string instructionMenuChosen = instructionMenu.Print();
                        if (instructionMenuChosen == "OK")
                        {
                            Game game = new Game();
                            (string, Dictionary<string, int>?) result = game.StartGame(LEVEL);
                            if (result.Item1 == "DEFEAT")
                            {
                                DefeatWindow defeatWidow = new DefeatWindow();
                                string defeatWindowChosen = defeatWidow.Print();
                                if (defeatWindowChosen == "RETURN")
                                {
                                    goto MainMenu;
                                }
                            }
                            else if (result.Item1 == "WIN")
                            {
                                VictoryWindow victoryWidow = new VictoryWindow();
                                string victoryWindowChosen = victoryWidow.Print(result.Item2);
                                if (victoryWindowChosen == "RETURN")
                                {
                                    goto MainMenu;
                                }
                            }
                        }
                        if (instructionMenuChosen == "RETURN")
                        {
                            goto LevelMenu;
                        }
                }
            }
            else if (mainMenuChosen == "EXIT")
            {
                return;
            }
        }
    }*/
    class Program
    {
        static void Main()
        {
            MainMenu mainMenu = new MainMenu();
            mainMenu.Print();
        }
    }
}