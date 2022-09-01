using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;

namespace ForVyeshal___Monogame
{
  
    public static class ConsoleClass
    {
        public static int Line = 0;

        public static string GetConsoleText()
        {           
            return Game.ConsoleString;
        }

        public static void HandleConsole(string text, bool mess = false)
        {
            if (!mess)
            {
                string[] split = text.Split(' ');
                split[0] = split[0].ToUpper();
                if (split[0] == "CLEAR")
                    Game.ConsoleString = "";
                else if (split[0] == "HELP")
                {
                    if (split.Count() == 1)
                        Game.ConsoleString += "\nTIME \nTEST \nMAPMODE \nRENEW \nRENEWINFO \nSETUNIT \nCALCDIST \nCONNECTRAIL \nCALCAVRGINCOME \nINCOME \nMEINCOME \nCRTFILE \nGOLD \nEQUIPMENT \nDEBUG \nTAG" +
                            "\nMODS \nTAGS \nCHECKID \nERROR \nCONTROL \nPROD \nCONTROLS \nANNEX";
                    else
                    {
                        string split2 = split[1].ToUpper();
                        switch (split2)
                        {
                            case "TIME":
                                Game.ConsoleString += "\nUse: TIME";
                                Game.ConsoleString += "\nShows ammount of DrawLoops made, TotalRunningTime and AverageTime per frame in ms";
                                break;
                            case "TEST":
                                Game.ConsoleString += "\nUse: TEST";
                                Game.ConsoleString += "\nUsed for testing, current use: Adds unit to player's capital";
                                break;
                            case "MAPMODE":
                                Game.ConsoleString += "\nUse: MAPMODE [MAPMODE_NAME]";
                                Game.ConsoleString += "\nManualy changes mapmodes, currently available mapmodes: development \nregion \ntrade \nterrain \ncontrol \npath \ncapital \nrail \nunit";
                                break;
                            case "RENEW":
                                Game.ConsoleString += "\nUse: RENEW";
                                Game.ConsoleString += "\nReneews all paths, may cause lag";
                                break;
                            case "RENEWINFO":
                                Game.ConsoleString += "\nUse: RENEWINFO";
                                Game.ConsoleString += "\nShows info about path renews, current status: " + Game.RenewInfo;
                                break;
                            case "SETUNIT":
                                Game.ConsoleString += "\nUse: SETUNIT";
                                Game.ConsoleString += "\nStarts SetUnit mode for setting unit position on provinces";
                                break;
                            case "CALCDIST":
                                Game.ConsoleString += "\nUse: CALCDIST [1ID] [2ID]";
                                Game.ConsoleString += "\nCalculates distance between two provinces which id's are provided as arguments";
                                break;
                            case "CONNECTRAIL":
                                Game.ConsoleString += "\nUse: CONNECTRAIL [1ID] [2ID]";
                                Game.ConsoleString += "\nConnect two provinces with a railroad via the shortest rout. Provinces id's must be provided as arguments";
                                break;
                            case "CALCAVRGINCOME":
                                Game.ConsoleString += "\nUse: CALCAVRGINCOME";
                                Game.ConsoleString += "\nDisplays info about world's average income";
                                break;
                            case "INCOME":
                                Game.ConsoleString += "\nUse: INCOME";
                                Game.ConsoleString += "\nDisplays income of each country";
                                break;
                            case "MEINCOME":
                                Game.ConsoleString += "\nUse: MEINCOME";
                                Game.ConsoleString += "\nDisplays military equipment income of each country";
                                break;
                            case "CRTFILE":
                                Game.ConsoleString += "\nUse: CRTFILE";
                                Game.ConsoleString += "\nUsed for testing, current use: Creates file with unit positions";
                                break;
                            case "GOLD":
                                Game.ConsoleString += "\nUse: GOLD [AMMOUNT]";
                                Game.ConsoleString += "\nAdds gold to player's country treasury";
                                break;
                            case "EQUIPMENT":
                                Game.ConsoleString += "\nUse: EQUIPMENT [AMMOUNT]";
                                Game.ConsoleString += "\nAdds military equipment to player's country";
                                break;
                            case "DEBUG":
                                Game.ConsoleString += "\nUse: DEBUG";
                                Game.ConsoleString += "\nTurns off/on debug mode, current status: " + Game.DebugMode;
                                break;
                            case "TAG":
                                Game.ConsoleString += "\nUse: TAG [TAG]";
                                Game.ConsoleString += "\nChanges player country to a different tag";
                                break;
                            case "MODS":
                                Game.ConsoleString += "\nUse: MODS <TAG>";
                                Game.ConsoleString += "\nDisplays all modifiers for player's country or for provided country";
                                break;
                            case "TAGS":
                                Game.ConsoleString += "\nUse: TAGS";
                                Game.ConsoleString += "\nDisplays all available country tags";
                                break;
                            case "CHECKID":
                                Game.ConsoleString += "\nUse: CHECKID";
                                Game.ConsoleString += "\nDisplays ID of selected province";
                                break;
                            case "ERROR":
                                Game.ConsoleString += "\nUse: ERROR";
                                Game.ConsoleString += "\nTurns off/on error display mode, current status: " + Game.DisplayErrors;
                                break;
                            case "CONTROL":
                                Game.ConsoleString += "\nUse: CONTROL [ID] [TAG]";
                                Game.ConsoleString += "\nChanges control of a provided province to provided country";
                                break;
                            case "PROD":
                                Game.ConsoleString += "\nUse: PROD <TRADEGOOD_NAME>";
                                Game.ConsoleString += "\nDisplays total world production of all/given tradegood/s";
                                break;
                            case "CONTROLS":
                                Game.ConsoleString += "\nUse: CONTROLS [TAG]";
                                Game.ConsoleString += "\nChanges control of selected province to provided country";
                                break;
                            case "ANNEX":
                                Game.ConsoleString += "\nUse: ANNEX [TAG]";
                                Game.ConsoleString += "\nAnnexes provided country to player's country";
                                break;
                        }
                    }
                }
                else if (split[0] == "TIME")
                {
                    Game.ConsoleString += "\nTotal loops: " + Game.TotalLoops;
                    Game.ConsoleString += "\nTotal time: " + Game.TotalTime;
                    Game.ConsoleString += "\nAverage time: " + Game.TotalTime / Game.TotalLoops;
                }
                else if (split[0] == "TEST")
                {
                    Game.PlayerCountry.CreateUnit(Game.PlayerCountry.Capital, Game.PlayerCountry.UnitTemplates["Infantry"]);
                }
                else if (split[0] == "MAPMODE")
                {
                    if (split.Count() > 1)
                    {
                        switch (split[1].ToUpper())
                        {
                            case "DEV":
                                Game.Mapmode = "DEVELOPMENT";
                                break;
                            case "REG":
                                Game.Mapmode = "REGION";
                                break;
                            case "TRA":
                                Game.Mapmode = "TRADE";
                                break;
                            case "TER":
                                Game.Mapmode = "TERRAIN";
                                break;
                            case "CON":
                                Game.Mapmode = "CONTROL";
                                break;
                            case "PAT":
                                Game.Mapmode = "PATH";
                                break;
                            case "CAP":
                                Game.Mapmode = "CAPITAL";
                                break;
                            case "RAL":
                                Game.Mapmode = "RAIL";
                                break;
                            case "UNI":
                                Game.Mapmode = "UNIT";
                                break;

                        }
                    }
                    else
                    {
                        Game.ConsoleString += "\nINVALID ARGUMENT, EXPECTED: MAPMODE [STRING]";
                    }
                }
                else if (split[0] == "RENEW")
                {
                    Game.RenewPaths();
                }
                else if (split[0] == "RENEWINFO")
                {
                    Game.RenewInfo = !Game.RenewInfo;
                }
                else if (split[0] == "SETUNIT")
                {
                    Game.SetUnitMode = !Game.SetUnitMode;
                }                
                else if (split[0] == "CALCDIST")
                {
                    if (split.Count() > 2)
                    {
                        int p1i, p2i;
                        if (int.TryParse(split[1], out p1i) && int.TryParse(split[2], out p2i))
                        {
                            if (Game.Provinces.Any(x => x.ID == p1i) && Game.Provinces.Any(x => x.ID == p2i))
                            {
                                Province p1 = Game.Provinces[p1i];
                                Province p2 = Game.Provinces[p2i];
                                Province.Pathway path = p1.GetPathway(p2);
                                Game.ConsoleString += "\nDistance: " + path.TotalTime;
                            }
                            else
                            {
                                Game.ConsoleString += "\nNO PROVINCE WITH SUCH ID EXISTS";
                            }
                        }
                        else
                        {
                            Game.ConsoleString += "\nWRONG ARGUMENTS, EXPECTED: CALCDIST [INT] [INT]";
                        }
                    }
                }
                else if (split[0] == "CONNECTRAIL")
                {
                    if (split.Count() > 2)
                    {
                        int p1i, p2i;
                        if (int.TryParse(split[1], out p1i) && int.TryParse(split[2], out p2i))
                        {
                            if (Game.Provinces.Any(x => x.ID == p1i) && Game.Provinces.Any(x => x.ID == p2i))
                            {
                                Province p1 = Game.Provinces[p1i];
                                Province p2 = Game.Provinces[p2i];

                                Province.Pathway path = p1.GetPathway(p2);
                                if (path.Path.Any())
                                {
                                    p1.Railways[path.Path.First()] = 2;
                                    path.Path.First().Railways[p1] = 2;
                                    p2.Railways[path.Path.Last()] = 2;
                                    path.Path.Last().Railways[p2] = 2;
                                    for (int a = 0; a < path.Path.Count - 1; a++)
                                    {
                                        path.Path[a].Railways[path.Path[a + 1]] = 2;
                                        path.Path[a + 1].Railways[path.Path[a]] = 2;
                                    }

                                }
                                else
                                {
                                    p1.Railways[p2] = 2;
                                    p2.Railways[p1] = 2;
                                }
                            }
                            else
                            {
                                Game.ConsoleString += "\nNO PROVINCE WITH SUCH ID EXISTS";
                            }
                        }
                        else
                        {
                            Game.ConsoleString += "\nINVALID ARGUMENT, EXPECTED: CONNECTRAIL [INT] [INT]";
                        }
                    }
                    else
                    {
                        Game.ConsoleString += "\nINVALID ARGUMENT, EXPECTED: CONNECTRAIL [INT] [INT]";
                    }
                }
                else if (split[0] == "CALCAVRGINCOME")
                {
                    int a = 0;
                    decimal b = 0;
                    decimal bb = 0;
                    decimal s = 1000;
                    foreach (Country c in Game.Countries.Values)
                    {
                        a++;
                        b += c.LastTurnIncome;
                        if (c.LastTurnIncome > bb)
                            bb = c.LastTurnIncome;
                        if (c.LastTurnIncome < s && c.LastTurnIncome != 0)
                            s = c.LastTurnIncome;
                    }
                    Game.ConsoleString += "\nAverage income: " + b / a;
                    Game.ConsoleString += "\nLowest income: " + s;
                    Game.ConsoleString += "\nHighest income: " + bb;
                }
                else if (split[0] == "INCOME")
                {
                    foreach (Country c in Game.Countries.Values)
                    {
                        Game.ConsoleString += "\n" + c.FullName + ": " + c.LastTurnIncome;
                    }
                }
                else if (split[0] == "MEINCOME")
                {
                    foreach (Country c in Game.Countries.Values)
                    {
                        Game.ConsoleString += "\n" + c.FullName + ": " + c.LastTurnMilitaryEquipmentProduction;
                    }
                }
                else if (split[0] == "CRTUNIT")
                {

                }
                else if (split[0] == "CRTFILE")
                {
                    File.WriteAllText("unitposition.txt", "");
                    foreach (Province p in Game.Provinces)
                    {
                        if (p.ID != 0)
                        {
                            string t = "\n" + p.ID + ":" + p.UnitPosition.X + "." + p.UnitPosition.Y;
                            File.AppendAllText("unitposition.txt", t);
                        }
                    }
                    //File.WriteAllText("initialpathfinding.txt", s);
                }
                else if (split[0] == "GOLD")
                {
                    if (split.Count() == 1)
                    {
                        Game.PlayerCountry["Gold"] = (decimal)Game.PlayerCountry["Gold"] + 1000;
                    }
                    else
                    {
                        if (int.TryParse(split[1], out int res))
                        {
                            Game.PlayerCountry.Gold += res;
                        }
                        else
                        {
                            Game.ConsoleString += "\nINVALID ARGUMENT, EXPECTED: GOLD [INT]";
                        }
                    }

                }
                else if (split[0] == "EQUIPMENT")
                {
                    if (split.Count() == 1)
                    {
                        Game.PlayerCountry.MilitaryEquipment += 100000;
                    }
                    else
                    {
                        if (int.TryParse(split[1], out int res))
                        {
                            Game.PlayerCountry.MilitaryEquipment += res;
                        }
                        else
                        {
                            Game.ConsoleString += "\nINVALID ARGUMENT, EXPECTED: EQUIPMENT [INT]";
                        }
                    }

                }
                else if (split[0] == "DEBUG")
                {
                    Game.DebugMode = !Game.DebugMode;
                }
                else if (split[0] == "TAG")
                {
                    if (split.Count() == 1)
                    {
                        Game.ConsoleString += "\nNO ARGUMENT GIVEN, EXPECTED: TAG [STRING]";
                    }
                    else
                    {
                        if (Game.Countries.Keys.Contains(split[1].ToUpper()))
                        {
                            Game.PlayerCountry = Game.Countries[split[1].ToUpper()];
                        }
                        else
                        {
                            Game.ConsoleString += "\nTHIS TAG DOES NOT EXIST";
                        }
                    }

                }
                else if (split[0] == "MODS")
                {
                    if (split.Count() == 1)
                    {
                        foreach (Modifier m in Game.PlayerCountry.Modifiers)
                        {
                            Game.ConsoleString += "\n" + m.Title;
                            foreach (Modifier.Variable v in m.Variables)
                            {
                                Game.ConsoleString += "\n   " + v.Name + " | " + v.Value;
                            }
                        }
                    }
                    else
                    {
                        string s = "";
                        foreach (Country c in Game.Countries.Values)
                        {
                            if (c.TAG.Contains(split[1]))
                            {
                                if (Game.Font12.MeasureString(s).X > 220)
                                {
                                    Game.ConsoleString += "\n" + s;
                                    s = "";
                                }
                                s += c.TAG + ",";
                            }
                        }
                        if (s != "")
                            Game.ConsoleString += "\n" + s;
                    }
                }
                else if (split[0] == "TAGS")
                {
                    if (split.Count() == 1)
                    {
                        string s = "";
                        foreach (Country c in Game.Countries.Values)
                        {
                            if (Game.Font12.MeasureString(s).X > 220)
                            {
                                Game.ConsoleString += "\n" + s;
                                s = "";
                            }
                            s += c.TAG + " ";
                        }
                        if (s != "")
                            Game.ConsoleString += "\n" + s;
                    }
                    else
                    {
                        string s = "";
                        foreach (Country c in Game.Countries.Values)
                        {
                            if (c.TAG.Contains(split[1]))
                            {
                                if (Game.Font12.MeasureString(s).X > 220)
                                {
                                    Game.ConsoleString += "\n" + s;
                                    s = "";
                                }
                                s += c.TAG + ",";
                            }
                        }
                        if (s != "")
                            Game.ConsoleString += "\n" + s;
                    }
                }
                else if (split[0] == "CHECKID")
                {
                    if (Game.SelectedProvince == null)
                    {
                        Game.ConsoleString += "\nNO SELECTED PROVINCE";
                    }
                    else
                    {
                        Game.ConsoleString += "\nPROVINCE ID: " + Game.SelectedProvince.ID;
                    }
                }
                else if (split[0] == "ERROR")
                {
                    Game.DisplayErrors = !Game.DisplayErrors;
                    Game.ConsoleString += "\nDISPLAY ERRORS SET TO: " + Game.DisplayErrors.ToString().ToUpper();
                }

                else if (split[0] == "CONTROL")
                {
                    if (split.Count() < 3)
                    {
                        Game.ConsoleString += "\nNO ARGUMENT GIVEN, EXPECTED: CONTROL [INT] [STRING]";
                    }
                    else
                    {
                        if (Game.Countries.Keys.Contains(split[2].ToUpper()))
                        {
                            if (int.TryParse(split[1], out int res))
                            {
                                Game.Provinces.Find(x => x.ID == res).Control = Game.Countries[split[2].ToUpper()];
                            }
                            else
                            {
                                Game.ConsoleString += "\nINVALID ARGUMENT, EXPECTED: CONTROL [INT] [STRING]";
                            }

                        }
                        else
                        {
                            Game.ConsoleString += "\nTHIS TAG DOES NOT EXIST";
                        }
                    }
                }
                else if (split[0] == "PROD")
                {
                    if (split.Count() < 2)
                    {
                        foreach (TradeGood tg in Game.TradeGoods.Values)
                        {
                            Game.ConsoleString += "\nTOTAL PRODUCTION OF " + tg.Name + ": " + tg.GetTotalProduction();
                        }
                    }
                    else
                    {
                        string s = split[1].ToLower();
                        char l = s[0];
                        string ls = l.ToString();
                        ls = ls.ToUpper();
                        s = s.Remove(0, 1);
                        ls += s;
                        if (Game.TradeGoods.Keys.Contains(ls))
                        {
                            Game.ConsoleString += "\nTOTAL PRODUCTION OF " + Game.TradeGoods[ls].Name + ": " + Game.TradeGoods[ls].GetTotalProduction();
                        }
                    }
                }
                
                else if (split[0] == "CONTROLS")
                {
                    if (split.Count() < 2)
                    {
                        Game.ConsoleString += "\nNO ARGUMENT GIVEN, EXPECTED: CONTROLS [STRING]";
                    }
                    else
                    {
                        if (Game.Countries.Keys.Contains(split[1]))
                        {
                            if (Game.SelectedProvince != null)
                            {
                                Game.SelectedProvince.Control = Game.Countries[split[1]];
                            }
                            else
                            {
                                Game.ConsoleString += "\nNO SELECTED PROVINCE";
                            }

                        }
                        else
                        {
                            Game.ConsoleString += "\nTHIS TAG DOES NOT EXIST";
                        }
                    }
                }
                else if (split[0] == "ANNEX")
                {
                    if (split.Count() < 2)
                    {
                        Game.ConsoleString += "\nNO ARGUMENT GIVEN, EXPECTED: ANNEX [STRING]";
                    }
                    else
                    {
                        if (Game.Countries.Keys.Contains(split[1]))
                        {
                            foreach (Province p in Game.Provinces)
                            {
                                if (p.Control == Game.Countries[split[1]])
                                    p.Control = Game.PlayerCountry;
                            }

                        }
                        else
                        {
                            Game.ConsoleString += "\nTHIS TAG DOES NOT EXIST";
                        }
                    }
                }
            }
            else
            {
                Game.ConsoleString += text;
            }
            if (Game.ContentLoaded)
            {
                string[] lines = Game.ConsoleString.Split('\n');
                List<string> newlines = new List<string>();
                string newconsolestring = "";
                foreach(string s in lines)
                {                    
                    string newline = s;
                    int tries = 0;
                    while(Game.Font12.MeasureString(newline).X > GlobalControls.ConsoleWindow.GetControl("ConsoleTextContainer").Size.X + 15)
                    {
                        newline = s.Insert(s.Length - tries, "\n");
                        tries++;
                    }
                    newlines.Add(newline + "\n");
                }
                foreach(string s in newlines)
                {
                    if(s != "\n" && s!= "")
                    {
                        newconsolestring += s;
                    }
                }
                Game.ConsoleString = newconsolestring;
                lines = Game.ConsoleString.Split('\n');
                GlobalControls.ConsoleWindow.GetControl("ConsoleTextContainer").Controls.Find(x => x is Label).Size.Y = lines.Count() * (Game.Font12.LineSpacing - 3);
                GlobalControls.ConsoleWindow.GetControl("ConsoleTextContainer").RenewScroll();                
                if (GlobalControls.ConsoleWindow.GetControl("ConsoleTextContainer").Scroll.ScrollPosition >= GlobalControls.ConsoleWindow.GetControl("ConsoleTextContainer").Scroll.MaxScroll - 3)
                    GlobalControls.ConsoleWindow.GetControl("ConsoleTextContainer").ScrollHandle(3);
            }

        }
    }
}
