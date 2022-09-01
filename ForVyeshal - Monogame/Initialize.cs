using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace ForVyeshal___Monogame
{
    public static class Initialize
    {
        public static bool DonePathfinding = true;
        public static void ReadPathfinding()
        {
            string[] InitialPathfinding = File.ReadAllLines(@"Content\MapFiles\initialpathfinding.txt");
            foreach (string s in InitialPathfinding)
            {
                string[] split = s.Split(':');
                string[] split1 = split[0].Split('-');                
                List<Province> l = new List<Province>();
                if (split.Count() > 2)
                {
                    string[] split2 = split[2].Split('-');
                    foreach (string ss in split2)
                    {
                        l.Add(Game.Provinces[int.Parse(ss)]);
                    }
                }
                Game.Provinces[int.Parse(split1[0])].Pathfinding.Add(Game.Provinces[int.Parse(split1[1])], new Province.Pathway(Game.Provinces[int.Parse(split1[1])], l, int.Parse(split[1])));
            }
            DonePathfinding = true;
            
        }

        public static void Load()
        {
            string[] PixelList = File.ReadAllLines(@"Content\MapFiles\PixelList.txt");
            string[] ProvinceList = File.ReadAllLines(@"Content\MapFiles\ProvinceList.txt");
            string[] CountryList = File.ReadAllLines(@"Content\MapFiles\CountryList.txt");
            string[] Bordering = File.ReadAllLines(@"Content\MapFiles\bordering.txt");
            string[] TradeGoodList = File.ReadAllLines(@"Content\MapFiles\TradegoodsList.txt");
            string[] RegionList = File.ReadAllLines(@"Content\MapFiles\RegionList.txt");
            string[] TerrainList = File.ReadAllLines(@"Content\MapFiles\TerrainList.txt");
            string[] ProvinceCapitalsList = File.ReadAllLines(@"Content\MapFiles\ProvinceCapitals.txt");
            string[] Distances = File.ReadAllLines(@"Content\MapFiles\distances.txt");
            string[] Railroads = File.ReadAllLines(@"Content\MapFiles\Railways.txt");
            string[] Units = File.ReadAllLines(@"Content\MapFiles\Units.txt");
            string[] UnitPosition = File.ReadAllLines(@"Content\MapFiles\unitposition.txt");
            Thread t = new Thread(ReadPathfinding);

           

            MapRegion PreviousMapRegion = null;
            foreach (string ss in RegionList)
            {
                string s = ss.Trim();
                if (s.StartsWith("#"))
                {
                    PreviousMapRegion = new MapRegion
                    {
                        Name = s.Remove(0, 1)
                    };
                    Game.Regions.Add(PreviousMapRegion.Name, PreviousMapRegion);
                }
                else if (s.StartsWith("FULLNAME="))
                {
                    string[] split = s.Split('=');
                    PreviousMapRegion.FullName = split[1];
                }
                else if (s.StartsWith("COLOR="))
                {
                    string[] split = s.Split('=');
                    string[] color = split[1].Split('.');
                    PreviousMapRegion.Color = new Color(int.Parse(color[0]), int.Parse(color[1]), int.Parse(color[2]));
                    PreviousMapRegion.InvertedColor = Game.InvertColor(PreviousMapRegion.Color);
                }
            }

            Terrain PreviousTerrain = null;
            foreach (string ss in TerrainList)
            {
                string s = ss.Trim();
                if (s.StartsWith("#"))
                {
                    PreviousTerrain = new Terrain
                    {
                        Name = s.Remove(0, 1)
                    };
                    Game.Terrains.Add(PreviousTerrain.Name, PreviousTerrain);
                }
                else if (s.StartsWith("COLOR="))
                {
                    string[] split = s.Split('=');
                    string[] color = split[1].Split('.');
                    PreviousTerrain.Color = new Color(int.Parse(color[0]), int.Parse(color[1]), int.Parse(color[2]));
                    PreviousTerrain.InvertedColor = Game.InvertColor(PreviousTerrain.Color);
                }
                else if (s.StartsWith("TIME="))
                {
                    string[] split = s.Split('=');
                    PreviousTerrain.TravelTime = int.Parse(split[1]);
                }
            }

            


            Province PreviousProvince = null;

            TradeGood PreviousTradeGood = null;
            foreach (string ss in TradeGoodList)
            {
                string s = ss.Trim();
                if (s.StartsWith("#"))
                {
                    PreviousTradeGood = new TradeGood
                    {                    
                        Name = s.Remove(0, 1)                       
                    };
                    if (PreviousTradeGood.Name == "Weapons")
                        PreviousTradeGood.Name = "MilitaryEquipment";
                    Game.TradeGoods.Add(PreviousTradeGood.Name, PreviousTradeGood);                  
                    PreviousTradeGood.CountryModifier.Title = "Tradegood modifier: " + PreviousTradeGood.Name;
                }
                else if (s.StartsWith("BASE="))
                {
                    string[] split = s.Split('=');
                    PreviousTradeGood.SetPrice(decimal.Parse(split[1].Replace('.', ',')));
                }
                else if (s.StartsWith("$"))
                {
                    string[] split = s.Split('=');
                    PreviousTradeGood.ProvinceModifier.Variables.Add(new Modifier.Variable(split[0].Remove(0, 1), decimal.Parse(split[1].Replace('.', ','))));
                }
                else if (s.StartsWith("%"))
                {
                    string[] split = s.Split('=');
                    PreviousTradeGood.CountryModifier.Variables.Add(new Modifier.Variable(split[0].Remove(0, 1), decimal.Parse(split[1].Replace('.', ','))));
                }
                if (s.StartsWith("COLOR="))
                {
                    string[] split = s.Split('=');
                    string[] color = split[1].Split('.');
                    PreviousTradeGood.Color = new Color(int.Parse(color[0]), int.Parse(color[1]), int.Parse(color[2]));
                    PreviousTradeGood.InvertedColor = Game.InvertColor(PreviousTradeGood.Color);
                }

            }



            Country Null = new Country
            {
                Color = new Color(255, 255, 255),
                InvertedColor = new Color(0, 0, 0),
                TAG = "NUL",
                FullName = "None",
                FactionTAG = "NON"
            };
            Game.Countries.Add("", Null);
            Country PreviousCountry = null;
            int CreatorSetting = 1;
            bool UnitModifier = false;
            string M_NAME = "";
            List<Modifier.Variable> M_VARIABLES = new List<Modifier.Variable>();
            int M_DURATION = -1;
            string M_DESCRIPTION = "";
            string M_CHARACTER = "";
            string M_UNITS = "";
           

            foreach (string ss in CountryList)
            {
                string s = ss.Trim();
                if (ss.Contains("/"))
                    continue;
                if (CreatorSetting == 1)
                {
                    if (s.StartsWith("#"))
                    {
                        if (PreviousCountry != null)
                            PreviousCountry.Stability = PreviousCountry.MaxStability;
                        string TAG = s.Remove(0, 1);
                        Country c = new Country
                        {
                            TAG = TAG
                        };
                        PreviousCountry = c;
                        Game.Countries.Add(TAG, c);
                    }
                    else if (s.Trim().StartsWith("FULL"))
                    {
                        string[] split = s.Split('=');
                        PreviousCountry.FullName = split[1];
                    }
                    else if (s.Trim().StartsWith("CAPITAL"))
                    {
                        string[] split = s.Split('=');
                        PreviousCountry.CapitalInt = int.Parse(split[1]);
                    }
                    else if (s.Trim().StartsWith("COLOR"))
                    {
                        string[] split = s.Split('=');
                        string[] color = split[1].Split('.');
                        PreviousCountry.Color = new Color(int.Parse(color[0]), int.Parse(color[1]), int.Parse(color[2]));
                        PreviousCountry.InvertedColor = new Color(255 - PreviousCountry.Color.R, 255 - PreviousCountry.Color.G, 255 - PreviousCountry.Color.B);
                    }
                    else if (s.Trim().StartsWith("FACTION"))
                    {
                        string[] split = s.Split('=');
                        PreviousCountry.FactionTAG = split[1];
                    }
                    else if (s.Trim().StartsWith("MODIFIERS"))
                    {
                        CreatorSetting = 2;
                        UnitModifier = false;
                    }
                    else if (s.Trim().StartsWith("UNITMODIFIERS"))
                    {
                        CreatorSetting = 2;
                        UnitModifier = true;
                    }
                }
                else if (CreatorSetting == 2)
                {
                    if (s != ";" && s != ":")
                    {
                        if (s.StartsWith("MODIFIER="))
                        {
                            string[] split = s.Split('=');
                            M_NAME = split[1];
                            M_VARIABLES = new List<Modifier.Variable>();
                            M_DURATION = -1;
                            M_DESCRIPTION = "";
                            M_CHARACTER = "";
                        }
                        else if (s.StartsWith("VARIABLES"))
                        {
                            CreatorSetting = 3;
                        }
                        else if (s.StartsWith("DURATION"))
                        {
                            string[] split = s.Split('=');
                            M_DURATION = int.Parse(split[1]);
                        }
                        else if (s.StartsWith("UNITS"))
                        {
                            string[] split = s.Split('=');
                            M_UNITS = split[1];
                        }
                        else if (s.StartsWith("DESCRIPTION"))
                        {
                            string[] split = s.Split('=');
                            M_DESCRIPTION = split[1];
                        }
                        else if (s.StartsWith("CHARACTER"))
                        {
                            string[] split = s.Split('=');
                            M_CHARACTER = split[1];
                        }
                    }
                    else if (s == ";")
                    {
                        CreatorSetting = 1;
                    }
                    else if (s == ":")
                    {
                        if (!UnitModifier)
                        {
                            Country.CountryModifier m = new Country.CountryModifier(M_NAME, M_VARIABLES, M_DURATION, M_DESCRIPTION, M_CHARACTER);
                            PreviousCountry.AddModifier(m);
                        }
                        else
                        {
                            Country.UnitTemplateModifier m = new Country.UnitTemplateModifier(M_NAME, M_VARIABLES, M_UNITS,M_DURATION, M_DESCRIPTION);
                            PreviousCountry.UnitTemplatesINITIALIZE.Add(m);
                        }
                    }

                }
                else if (CreatorSetting == 3)
                {
                    if (s != ";")
                    {
                        string[] split = s.Split('=');
                        M_VARIABLES.Add(new Modifier.Variable(split[0], decimal.Parse(split[1].Replace('.', ','))));
                    }
                    else
                        CreatorSetting = 2;
                }

            }
            Game.PlayerCountry = Game.Countries["QRT"];
            foreach (string s in ProvinceList)
            {
                if (s.StartsWith("#"))
                {
                    string color = s.Remove(0, 1);
                    Province p = new Province();
                    PreviousProvince = p;
                    int R = int.Parse(color[0] + "" + color[1], System.Globalization.NumberStyles.HexNumber);
                    int G = int.Parse(color[2] + "" + color[3], System.Globalization.NumberStyles.HexNumber);
                    int B = int.Parse(color[4] + "" + color[5], System.Globalization.NumberStyles.HexNumber);
                    p.Color = new Color(R, G, B);

                    Game.Provinces.Add(p);
                }
                else if (s.Trim().StartsWith("NAME"))
                {
                    string[] split = s.Split('=');
                    PreviousProvince.Name = split[1];
                }                
                else if (s.Trim().StartsWith("DEVELOPMENT"))
                {
                    string[] split = s.Split('=');
                    PreviousProvince.Development = int.Parse(split[1]);
                }
                else if (s.Trim().StartsWith("TRADEGOOD"))
                {
                    string[] split = s.Split('=');
                    if (split[1] == "Weapons")
                        split[1] = "MilitaryEquipment";
                    PreviousProvince.Trade = Game.TradeGoods[split[1]];
                    PreviousProvince.AddModifier(PreviousProvince.Trade.ProvinceModifier);
                }
                else if (s.Trim().StartsWith("CONTROL"))
                {
                    string[] split = s.Split('=');
                    PreviousProvince.Control = Game.Countries[split[1]];
                }
                else if (s.Trim().StartsWith("TERRAIN"))
                {
                    string[] split = s.Split('=');
                    PreviousProvince.Terrain = Game.Terrains[split[1]];
                }
                else if (s.Trim().StartsWith("REGION"))
                {
                    string[] split = s.Split('=');
                    PreviousProvince.Region = Game.Regions[split[1]];
                }
                else if (s.Trim().StartsWith("ID"))
                {
                    string[] split = s.Split('=');
                    PreviousProvince.ID = int.Parse(split[1]);
                }
            }
            PreviousProvince = null;

            //t.Start();

            foreach(Country c in Game.Countries.Values)
            {
                if (c.CapitalInt != 0)
                {
                    c.Capital = Game.Provinces[c.CapitalInt];
                }
            }

            foreach(string s in ProvinceCapitalsList)
            {
                string ne = s.Remove(0, 1);
                string[] split = ne.Split(':');
                int R = int.Parse(split[0][0] + "" + split[0][1], System.Globalization.NumberStyles.HexNumber);
                int G = int.Parse(split[0][2] + "" + split[0][3], System.Globalization.NumberStyles.HexNumber);
                int B = int.Parse(split[0][4] + "" + split[0][5], System.Globalization.NumberStyles.HexNumber);
                Color c = new Color(R, G, B);
                foreach(Province p in Game.Provinces)
                {
                    if(p.Color == c)
                    {
                        p.ProvinceCapital = new Vector2(int.Parse(split[1].Split('.')[0]), int.Parse(split[1].Split('.')[1]));
                        break;
                    }
                }
            }


            foreach (string s in PixelList)
            {
                if (s.StartsWith("#"))
                {
                    string color = s.Remove(0, 1);
                    int R = int.Parse(color[0] + "" + color[1], System.Globalization.NumberStyles.HexNumber);
                    int G = int.Parse(color[2] + "" + color[3], System.Globalization.NumberStyles.HexNumber);
                    int B = int.Parse(color[4] + "" + color[5], System.Globalization.NumberStyles.HexNumber);
                    Color c = new Color(R, G, B);
                    PreviousProvince = Game.Provinces.Find(x => x.Color == c);
                }
                else
                {
                    string[] coordinates = s.Split(':');
                    PreviousProvince.Pixels.Add(new Point(int.Parse(coordinates[0]), int.Parse(coordinates[1])));
                }
            }

            foreach (string s in Bordering)
            {
                string[] split = s.Split(':');
                string[] bord = split[1].Split('.');
                foreach (string b in bord)
                {
                    Game.Provinces[int.Parse(split[0])].Bordering.Add(Game.Provinces[int.Parse(b)]);                   
                }               
            }
            foreach (string s in Distances)
            {
                string[] split = s.Split(':');
                Game.Provinces[int.Parse(split[0])].Distance.Add(Game.Provinces[int.Parse(split[0])].Bordering[int.Parse(split[1])], int.Parse(split[2]));
            }

            Country.UnitTemplate PreviousUnit = new Country.UnitTemplate();
            foreach (string s in Units)
            {
                if (s.StartsWith("#"))
                {
                    PreviousUnit.Name = s.Remove(0, 1);
                }
                else if (s == ";")
                {
                    foreach (Country c in Game.Countries.Values)
                    {
                        Country.UnitTemplate ut = new Country.UnitTemplate();
                        ut.Class = PreviousUnit.Class;
                        ut.Attack = PreviousUnit.Attack;
                        ut.Defence = PreviousUnit.Defence;
                        ut.Movement = PreviousUnit.Movement;
                        ut.MaxEntrenchment = PreviousUnit.MaxEntrenchment;
                        ut.MaxMorale = PreviousUnit.MaxMorale;
                        ut.GoldBuildCost = PreviousUnit.GoldBuildCost;
                        ut.GoldUpkeep = PreviousUnit.GoldUpkeep;
                        ut.MEBuildCost = PreviousUnit.MEBuildCost;
                        ut.MEUpkeep = PreviousUnit.MEUpkeep;
                        ut.HorseBuildCost = PreviousUnit.HorseBuildCost;
                        ut.HorseUpkeep = PreviousUnit.HorseUpkeep;
                        ut.NormalTerrain = PreviousUnit.NormalTerrain;
                        ut.AridTerrain = PreviousUnit.AridTerrain;
                        ut.RoughTerrain = PreviousUnit.RoughTerrain;
                        ut.MountainTerrain = PreviousUnit.MountainTerrain;
                        ut.InfantryBonus = PreviousUnit.InfantryBonus;
                        ut.CavalryBonus = PreviousUnit.CavalryBonus;
                        ut.ArtilleryBonus = PreviousUnit.ArtilleryBonus;
                        ut.Size = PreviousUnit.Size;
                        ut.Description = PreviousUnit.Description;
                        ut.Name = PreviousUnit.Name;
                        c.UnitTemplates.Add(ut.Name, ut);
                    }
                    PreviousUnit = new Country.UnitTemplate();
                }
                else if (!s.Contains("//") && s != "")
                {
                    string[] spl = s.Split('=');
                    spl[1] = spl[1].Replace('.', ',');
                    if (PreviousUnit[spl[0]].GetType().Name == "String")
                        PreviousUnit[spl[0]] = spl[1];
                    else if (PreviousUnit[spl[0]].GetType().Name == "Decimal")
                        PreviousUnit[spl[0]] = decimal.Parse(spl[1]);
                    else
                        PreviousUnit[spl[0]] = int.Parse(spl[1]);
                }

            }
            foreach(Country c in Game.Countries.Values)
            {
                foreach(Country.UnitTemplateModifier utm in c.UnitTemplatesINITIALIZE)
                {
                    c.AddModifier(utm);
                }
                c.UnitTemplatesINITIALIZE.Clear();
            }


            foreach (string s in Railroads)
            {
                string[] split = s.Split(':');
                Province p1 = Game.Provinces[int.Parse(split[0])];
                Province p2 = Game.Provinces[int.Parse(split[1])];
                p1.Railways[p2] = int.Parse(split[2]);
                p2.Railways[p1] = int.Parse(split[2]);
            }
            foreach (string s in UnitPosition)
            {
                string[] split = s.Split(':');
                string[] split2 = split[1].Split('.');
                Province p1 = Game.Provinces[int.Parse(split[0])];
                p1.UnitPosition = new Vector2(float.Parse(split2[0]), float.Parse(split2[1]));
            }

            foreach (Province p in Game.Provinces)
            {

                foreach (Point point in p.Pixels)
                {
                    if (point.Y < p.DrawPosition.Y)
                        p.DrawPosition.Y = point.Y;
                    if (point.X < p.DrawPosition.X)
                        p.DrawPosition.X = point.X;
                    if (point.X > p.Size.X)
                        p.Size.X = point.X;
                    if (point.Y > p.Size.Y)
                        p.Size.Y = point.Y;
                }

                p.Size = p.Size - p.DrawPosition;
                int width = (int)p.Size.X;
                int height = (int)p.Size.Y;

                if (p.ID == 0)
                {
                    width = 250;
                    height = 250;
                }
                width += 1;
                height += 1;

                p.Texture = new Texture2D(Game.graphics.GraphicsDevice, width: width, height: height);
                p.SelectedTexture = new Texture2D(Game.graphics.GraphicsDevice, width, height);

                Color[] data = new Color[width * height];
                Color[] BorderData = new Color[width * height];
                Color Transparent = new Color(0, 0, 0, 0);
                for (int a = 0; a < height; a++)
                {
                    for (int b = 0; b < width; b++)
                    {
                        data[a * width + b] = Transparent;
                        BorderData[a * width + b] = Transparent;
                    }
                }
                foreach (Point point in p.Pixels)
                {
                    data[((point.Y - (int)p.DrawPosition.Y) * width) + (point.X - (int)p.DrawPosition.X)] = Color.White;
                }
                p.Texture.SetData(data);


                foreach (Point point in p.Pixels)
                {
                    if ((point.Y - (int)p.DrawPosition.Y) > 0)
                    {

                        if ((point.X - (int)p.DrawPosition.X) > 0)
                        {
                            if (data[(point.Y - (int)p.DrawPosition.Y - 1) * width + (point.X - (int)p.DrawPosition.X - 1)] == Transparent)
                            {
                                BorderData[(point.Y - (int)p.DrawPosition.Y) * width + (point.X - (int)p.DrawPosition.X)] = Color.White;
                            }
                        }


                        if (data[(point.Y - (int)p.DrawPosition.Y - 1) * width + (point.X - (int)p.DrawPosition.X)] == Transparent)
                        {
                            BorderData[(point.Y - (int)p.DrawPosition.Y) * width + (point.X - (int)p.DrawPosition.X)] = Color.White;
                        }


                        if ((point.X - (int)p.DrawPosition.X) < width)
                        {
                            if (data[(point.Y - (int)p.DrawPosition.Y - 1) * width + (point.X - (int)p.DrawPosition.X) + 1] == Transparent)
                            {
                                BorderData[(point.Y - (int)p.DrawPosition.Y) * width + (point.X - (int)p.DrawPosition.X)] = Color.White;
                            }
                        }

                    }
                    else if ((point.Y - (int)p.DrawPosition.Y) == 0)
                    {
                        BorderData[(point.Y - (int)p.DrawPosition.Y) * width + (point.X - (int)p.DrawPosition.X)] = Color.White;
                    }

                    if ((point.X - (int)p.DrawPosition.X) > 0)
                    {
                        if (data[(point.Y - (int)p.DrawPosition.Y) * width + (point.X - (int)p.DrawPosition.X) - 1] == Transparent)
                        {
                            BorderData[(point.Y - (int)p.DrawPosition.Y) * width + (point.X - (int)p.DrawPosition.X)] = Color.White;
                        }
                    }
                    else if ((point.X - (int)p.DrawPosition.X) == 0)
                    {
                        BorderData[(point.Y - (int)p.DrawPosition.Y) * width + (point.X - (int)p.DrawPosition.X)] = Color.White;
                    }



                    if ((point.X - (int)p.DrawPosition.X) < width - 1)
                    {
                        if (data[(point.Y - (int)p.DrawPosition.Y) * width + (point.X - (int)p.DrawPosition.X) + 1] == Transparent)
                        {
                            BorderData[(point.Y - (int)p.DrawPosition.Y) * width + (point.X - (int)p.DrawPosition.X)] = Color.White;
                        }
                    }
                    else if ((point.X - (int)p.DrawPosition.X) == width - 1)
                    {
                        BorderData[(point.Y - (int)p.DrawPosition.Y) * width + (point.X - (int)p.DrawPosition.X)] = Color.White;
                    }



                    if ((point.Y - (int)p.DrawPosition.Y) < height - 1)
                    {

                        if ((point.X - (int)p.DrawPosition.X) > 0)
                        {

                            if (data[((point.Y - (int)p.DrawPosition.Y) + 1) * width + (point.X - (int)p.DrawPosition.X) - 1] == Transparent)
                            {
                                BorderData[(point.Y - (int)p.DrawPosition.Y) * width + (point.X - (int)p.DrawPosition.X)] = Color.White;
                            }

                        }



                        if (data[(point.Y - (int)p.DrawPosition.Y + 1) * width + (point.X - (int)p.DrawPosition.X)] == Transparent)
                        {
                            BorderData[(point.Y - (int)p.DrawPosition.Y) * width + (point.X - (int)p.DrawPosition.X)] = Color.White;
                        }



                        if ((point.X - (int)p.DrawPosition.X) < width - 1)
                        {
                            if (data[(point.Y - (int)p.DrawPosition.Y + 1) * width + (point.X - (int)p.DrawPosition.X) + 1] == Transparent)
                            {
                                BorderData[(point.Y - (int)p.DrawPosition.Y) * width + (point.X - (int)p.DrawPosition.X)] = Color.White;
                            }
                        }


                    }
                    else if ((point.Y - (int)p.DrawPosition.Y) == height - 1)
                    {
                        BorderData[(point.Y - (int)p.DrawPosition.Y) * width + (point.X - (int)p.DrawPosition.X)] = Color.White;
                    }

                }
                for (int a = 0; a < height; a++)
                {
                    for (int b = 0; b < width; b++)
                    {
                        if (BorderData[a * width + b] == Color.Green || BorderData[a * width + b] == Color.Blue)
                        {
                            BorderData[a * width + b] = Color.White;
                        }
                    }
                }



                p.SelectedTexture.SetData(BorderData);



            }

            int csx = 0;
            int csy = 0;

            int prix = 0;
            int priy = 0;
            int priS = 0;
            int priex = 0;

            int slsx = 0;
            int slsy = 0;
            int slpy = 0;

            int lmsx = 0;
            int lmsy = 0;
            int lmpy = 0;

            int mmm = 0;

            int upsy = 0;
            int upsm = 0;

           

        

            SpriteFont BigFont = null;
            SpriteFont MediumFont = null;
            SpriteFont SmallFont = null;


            switch (Game.Res)
            {
                case 0:
                    csx = 350;
                    csy = 400;
                    priy = 200;
                    prix = 600;
                    priS = 25;
                    priex = 350;

                    slpy = 137;
                    slsy = 120;
                    slsx = 300;

                    lmpy = 100;
                    lmsx = 60;
                    lmsy = 400;

                    mmm = 10;

                    upsy = 60;
                    upsm = -100;

                    SmallFont = Game.Font10;
                    MediumFont = Game.Font12;
                    BigFont = Game.Font16;                   
                    break;
                case 1:
                    csx = 400;
                    csy = 500;
                    priy = 250;
                    prix = 800;
                    priS = 30;
                    priex = 400;

                    lmpy = 160;
                    lmsx = 60;
                    lmsy = 460;

                    upsy = 80;
                    upsm = 0;

                    SmallFont = Game.Font12;
                    MediumFont = Game.Font16;
                    BigFont = Game.Font24;
                    break;
            }

            

            UIWindow Console = new UIWindow
            {
                Title = "Console",
                Closable = true,
                Position = new Vector2(500, 500),
                Movable = true,               
                Size = new Vector2(csx, csy),
                Visible = false
                
            };
            Container ConsoleTextContainer = new Container("ConsoleTextContainer", new Vector2(10, 60), new Vector2(csx - 15, csy - 100));
            Console.AddControl(ConsoleTextContainer);
            ConsoleTextContainer.AddControl(new Label("ConsoleText", "", Color.Black, new Vector2(0,0), SmallFont));
            Console.GetAllChildren().Find(x => x.Name == "ConsoleText").Scrolable = true;
            (Console.GetAllChildren().Find(x => x.Name == "ConsoleText") as Label).Alignment = TextAlignemt.TopLeft;
            Console.AddControl(new TextInput(new Vector2(20, csy - 40), SmallFont, csx - 40));
            Console.Type = "Console";
            Game.UIWindows.Add(Console);
            ConsoleTextContainer.AttachScroll(0, (int)Game.Font12.LineSpacing - 3);
            GlobalControls.ConsoleWindow = Console;
            
            UIWindow ProvinceI = new UIWindow
            {
                Position = new Vector2(-3, Game.ScreenResolution.Y - priy),
                Size = new Vector2(prix, priy),
                AllowClipping = true               
            };

            UIWindow SoldiersI = new UIWindow
            {
                Position = new Vector2(-3, Game.ScreenResolution.Y - priy - slpy),
                Size = new Vector2(slsx, slsy),
                AllowClipping = true
            };
            GlobalControls.SoldierMenu = SoldiersI;
            SoldiersI.AddControl(new Container("SoldiersContainer", new Vector2(0,20), new Vector2(slsx - 5, slsy - 20)));
            SoldiersI.AddControl(new Label("TotalSoldiers", "INF: 0     CAV: 0     ART: 0", Color.Black, new Vector2(10, 5), SmallFont));
            Game.UIWindows.Add(SoldiersI);         
            SoldiersI.Controls.First().AttachScroll(0,20);

            ProvinceI.AddControl(new Label("Control", "Control: ", Color.Black, new Vector2(40, 20), MediumFont));
            ProvinceI.AddControl(new Label("Terrain", "Terrain: ", Color.Black, new Vector2(40, 20 + priS), MediumFont));
            ProvinceI.AddControl(new Label("Development", "Development: ", Color.Black, new Vector2(40, 20 + priS * 2), MediumFont));
            ProvinceI.AddControl(new Label("Region", "Region: ", Color.Black, new Vector2(40, 20 + priS * 3), MediumFont));
            ProvinceI.AddControl(new Label("Trade", "Trade: ", Color.Black, new Vector2(40, 20 + priS * 4), MediumFont));
            
            ProvinceI.AddControl(new Label("Tax", "Tax: ", Color.Black, new Vector2(40, 20 + priS * 6), MediumFont));
            ProvinceI.AddControl(new ExtraLine(new Vector2(priex, 0), new Vector2(priex, priy), Color.Black, 4));
            ProvinceI.AddControl(new Label("MEProd", "Mil. Eq. Production: ", Color.Black, new Vector2(priex + 40, 20), MediumFont));

            GlobalLabels.ControlLabel = (Label)ProvinceI.Controls.Find(x => x.Name == "Control");
            GlobalLabels.TerrainLabel = (Label)ProvinceI.Controls.Find(x => x.Name == "Terrain");
            GlobalLabels.DevelopmentLabel = (Label)ProvinceI.Controls.Find(x => x.Name == "Development");
            GlobalLabels.RegionLabel = (Label)ProvinceI.Controls.Find(x => x.Name == "Region");
            GlobalLabels.TradeLabel = (Label)ProvinceI.Controls.Find(x => x.Name == "Trade");
            GlobalLabels.TaxLabel = (Label)ProvinceI.Controls.Find(x => x.Name == "Tax");
            GlobalLabels.MEPLabel = (Label)ProvinceI.Controls.Find(x => x.Name == "MEProd");

            Game.UIWindows.Add(ProvinceI);


            UIWindow LeftMenu = new UIWindow
            {
                Position = new Vector2(-4, lmpy),
                Size = new Vector2(lmsx, lmsy),
                Type = "LeftMenu",               
            };
            Game.UIWindows.Add(LeftMenu);
            LeftMenu.AddControl(new Button("Trade", "T", Color.Black, new Vector2(10, 10),SmallFont, new Vector2(40, 40), new Action<string>(Game.OpenMenu), "TRA"));
            GlobalControls.TradeMenu = new UIWindow
            {
                Position = new Vector2(200, 200),
                Size = new Vector2(635, 572),               
                Type = "TradeMenu",
                Title = "Trade menu",
                Closable = true,
                Movable = true,
                Visible = false,
                
            };
            GlobalControls.TradeMenu.AddControl(new Label("TradeMenu_Fixed_Name", "Tradegood", Color.Black, new Vector2(0, 60), new Vector2(150, 25), MediumFont));
            Game.UIWindows.Add(GlobalControls.TradeMenu);
            GlobalControls.TradeMenu.AddControl(new ExtraLine(new Vector2(0,90), new Vector2(635, 90), Color.Black, 3));
            GlobalControls.TradeMenu.AddControl(new ExtraLine(new Vector2(150, 50), new Vector2(150, 572), Color.Black, 2));
            GlobalControls.TradeMenu.AddControl(new Label("TradeMenu_Fixed_BaseValue", "Price", Color.Black, new Vector2(150,60), new Vector2(70, 25), MediumFont));
            GlobalControls.TradeMenu.AddControl(new ExtraLine(new Vector2(220, 50), new Vector2(220, 572), Color.Black, 2));

            GlobalControls.TradeMenu.AddControl(new Label("TradeMenu_Fixed_PriceModifier", "Price\nModifier", Color.Black, new Vector2(220, 60), new Vector2(70, 25), SmallFont, false));
            GlobalControls.TradeMenu.AddControl(new ExtraLine(new Vector2(290, 50), new Vector2(290, 572), Color.Black, 2));

            GlobalControls.TradeMenu.AddControl(new Label("TradeMenu_Fixed_Production", "Produced", Color.Black, new Vector2(290, 60), new Vector2(100, 25), MediumFont));
            GlobalControls.TradeMenu.AddControl(new ExtraLine(new Vector2(390, 50), new Vector2(390, 572), Color.Black, 2));

            GlobalControls.TradeMenu.AddControl(new Label("TradeMenu_Fixed_ProductionModifier", "Production\nModifier", Color.Black, new Vector2(390, 60), new Vector2(80, 25), SmallFont));
            GlobalControls.TradeMenu.AddControl(new ExtraLine(new Vector2(470, 50), new Vector2(470, 572), Color.Black, 2));

            GlobalControls.TradeMenu.AddControl(new Label("TradeMenu_Fixed_TotalWorldProduction", "World\nProduction", Color.Black, new Vector2(470, 60), new Vector2(80, 25), SmallFont));
            GlobalControls.TradeMenu.AddControl(new ExtraLine(new Vector2(550, 50), new Vector2(550, 572), Color.Black, 2));

            GlobalControls.TradeMenu.AddControl(new Label("TradeMenu_Fixed_TotalIncome", "Income", Color.Black, new Vector2(550, 60), new Vector2(80, 25), MediumFont));

            int n = 0;
            foreach(TradeGood tg in Game.TradeGoods.Values)
            {
                if (tg.Name.Length > 2)
                {
                    Container c = new Container("Container_" + tg.Name, new Vector2(0, 90 + 30 * n), new Vector2(635, 30));
                    if(tg.Name == "MilitaryEquipment")
                        c.AddControl(new Label("Name", tg.Name, Color.Black, new Vector2(0, 0), new Vector2(150, 30), SmallFont));
                    else 
                        c.AddControl(new Label("Name", tg.Name, Color.Black, new Vector2(0, 0), new Vector2(150, 30), MediumFont));
                    c.AddControl(new Label("BaseValue", tg.GetPrice().ToString(), Color.Black, new Vector2(150, 0), new Vector2(70, 30), MediumFont));
                    c.AddControl(new Label("PriceModifier", (Game.PlayerCountry.TradeGoodIncome[tg] - 1) * 100 + "%", Color.Black, new Vector2(220, 0), new Vector2(70, 30), MediumFont));
                    c.AddControl(new Label("Production", Game.PlayerCountry.GetTotalProduction(tg).ToString(), Color.Black, new Vector2(290, 0), new Vector2(100, 30), MediumFont));
                    c.AddControl(new Label("ProductionModifier", (double)(Math.Round(Game.PlayerCountry.TradeGoodEf[tg], 2) - 1 + Game.PlayerCountry.ProductionEf - 1) * 100 + "%", Color.Black, new Vector2(390, 0), new Vector2(80, 30), MediumFont));
                    c.AddControl(new Label("TotalWorldProduction", tg.GetTotalProduction().ToString(), Color.Black, new Vector2(470, 0), new Vector2(80, 30), MediumFont));
                    c.AddControl(new Label("TotalIncome", Game.PlayerCountry.GetTotalTradeIncome(tg).ToString(), Color.Black, new Vector2(550, 0), new Vector2(80, 30), MediumFont));
                    GlobalControls.TradeMenu.AddControl(c);
                    GlobalControls.TradeMenu.AddControl(new ExtraLine(new Vector2(0, 120 + 30 * n), new Vector2(635, 120 + 30 * n), Color.Black, 2));
                    n++;
                }
            }
            
            UIWindow Mapmodes = new UIWindow
            {
                Position = new Vector2(Game.ScreenResolution.X / 2 + mmm, Game.ScreenResolution.Y - 80),
                Size = new Vector2((int)Game.ScreenResolution.X / 2 + 6 + mmm, 86),
                Type = "Mapmodes",              
            };

            int Space = (int)Game.ScreenResolution.X / 2 - mmm;
            
            int butSpace = Space / 5;
            int butSize = butSpace * 3 / 4;
            int spacing = butSpace / 4;

            Mapmodes.AddControl(new Button("DevButton", "Development", Color.Black, new Vector2(30, 23), SmallFont,new Vector2(butSize, 40), new Action<string>(Game.MapmodeChange), "DEV"));
            Mapmodes.AddControl(new Button("ConButton", "Control", Color.Black, new Vector2(butSize + 30 + spacing, 23), SmallFont, new Vector2(butSize, 40), new Action<string>(Game.MapmodeChange), "CON"));
            Mapmodes.AddControl(new Button("TraButton", "Trade", Color.Black, new Vector2(butSize * 2 + 30 + spacing * 2, 23), SmallFont, new Vector2(butSize, 40), new Action<string>(Game.MapmodeChange), "TRA"));
            Mapmodes.AddControl(new Button("TerButton", "Terrain", Color.Black, new Vector2(butSize * 3 + 30 + spacing * 3, 23), SmallFont, new Vector2(butSize, 40), new Action<string>(Game.MapmodeChange), "TER"));
            Mapmodes.AddControl(new Button("RegButton", "Region", Color.Black, new Vector2(butSize * 4 + 30 + spacing * 4, 23), SmallFont, new Vector2(butSize, 40), new Action<string>(Game.MapmodeChange), "REG"));
            (Mapmodes.Controls.Find(x => x.Name == "ConButton") as Button).Clicked = true;
            Game.UIWindows.Add(Mapmodes);
            GlobalControls.Mapmodes = Mapmodes;


            UIWindow UpPanel = new UIWindow
            {
                Position = new Vector2(-3, 0),
                Size = new Vector2((int)Game.ScreenResolution.X + 6, upsy)
            };


            UpPanel.AddControl(new Label("GoldLabel", "Gold: ", Color.Black, new Vector2(0, 0), new Vector2(350 + upsm, upsy), BigFont));
            GlobalLabels.GoldLabel = (Label)UpPanel.Controls.Find(x => x.Name == "GoldLabel");
            UpPanel.AddControl(new Label("MilitaryEquipmentLabel", "Military Equipment: ", Color.Black, new Vector2(400 + upsm, 0), new Vector2(400 + upsm, upsy), BigFont));
            GlobalLabels.MELabel = (Label)UpPanel.Controls.Find(x => x.Name == "MilitaryEquipmentLabel");
            UpPanel.AddControl(new Label("CoalLabel", "Coal: ", Color.Black, new Vector2(850 + upsm * 2, 0), new Vector2(250 + upsm, upsy), BigFont));
            GlobalLabels.CoalLabel = (Label)UpPanel.Controls.Find(x => x.Name == "CoalLabel");
            UpPanel.AddControl(new Label("StabilityLabel", "Stability: ", Color.Black, new Vector2(1150 + upsm * 3, 0), new Vector2(350 + upsm, upsy), BigFont));
            GlobalLabels.StabilityLabel = (Label)UpPanel.Controls.Find(x => x.Name == "StabilityLabel");

            Game.UIWindows.Add(UpPanel);

            
            

            

        }
    }
}
