using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static ForVyeshal___Monogame.GlobalControls;
using static ForVyeshal___Monogame.GlobalLabels;
using System;

namespace ForVyeshal___Monogame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {

        public static Control Con;

        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static bool ContentLoaded = false;

        public static bool Fullscreen = false;
        public static int Res = 0;
        public static bool SetUnitMode = false;

        public static Vector2 CameraPosition = new Vector2();
        public static Vector2 ScreenResolution = new Vector2();
        public static List<UIWindow> UIWindows = new List<UIWindow>();
        public static Province SelectedProvince = null;
        public static Vector2 LastMousePosition = new Vector2();        
        public static UIWindow Grabbed = null;
        public static int ProvinceCheckDelay = 0;
        List<UIWindow> Copy = new List<UIWindow>();

        public static float TotalTime = 0;
        public static int TotalLoops = 0;

        public static int ScrollValue = 0;

        int RenewCountdown = 0;
        
        public static string Mapmode = "CONTROL";
        public Texture2D Map;
        public static SpriteFont Font10;
        public static SpriteFont Font12;
        public static SpriteFont Font16;
        public static SpriteFont Font24;
        public Texture2D Unit;
        public Texture2D Capital;

        public Texture2D SoldierLongcoats;
        public Texture2D SoldierCommonfolk;
        public Texture2D SoldierCivilized;
        public Texture2D SoldierKSR;
        public Texture2D SoldierGeneric;


        int GlobalTick = 0;

        public static bool SoldierClicked = false;
      
        public static string ConsoleString = "For Vyeshal - Console";
        public static bool DisplayErrors = true;

        public static bool DebugMode = false;
        public static bool RenewInfo = false;

        public static Country PlayerCountry = null;
               
        public static TextInput FocusedText = null;
        public Keys LastKey = Keys.W;

        public static Dictionary<string, Country> Countries = new Dictionary<string, Country>();

        public static List<Province> Provinces = new List<Province>();

        public static Dictionary<string, TradeGood> TradeGoods = new Dictionary<string, TradeGood>();

        public static Dictionary<string, MapRegion> Regions = new Dictionary<string, MapRegion>();

        public static Dictionary<string, Terrain> Terrains = new Dictionary<string, Terrain>();

        KeyboardControl KeyboardC = new KeyboardControl();

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

           

            if (Res == 1)
            {
                ScreenResolution.X = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                ScreenResolution.Y = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                graphics.PreferredBackBufferWidth = (int)ScreenResolution.X;
                graphics.PreferredBackBufferHeight = (int)ScreenResolution.Y;
            }
            else if (Res == 0)
            {
                ScreenResolution.X = 1280;
                ScreenResolution.Y = 854;
                graphics.PreferredBackBufferWidth = (int)ScreenResolution.X;
                graphics.PreferredBackBufferHeight = (int)ScreenResolution.Y;
               
            }
            
            if(Fullscreen)
                graphics.IsFullScreen = true;          
            graphics.ApplyChanges();

                      
        }

        static List<Province> FirstHalf = new List<Province>();
        static List<Province> SecondHalf = new List<Province>();
        static List<Province> ThirdHalf = new List<Province>();
        static List<Province> FourthHalf = new List<Province>();


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Font10 = Content.Load<SpriteFont>("Font10");
            Font12 = Content.Load<SpriteFont>("Font12");
            Font16 = Content.Load<SpriteFont>("Font16");
            Font24 = Content.Load<SpriteFont>("Font24");
            Capital = Content.Load<Texture2D>(@"ProvinceFiles\star");
            base.Initialize();

            while (!ForVyeshal___Monogame.Initialize.DonePathfinding)
            {

            }

            foreach (Province p in Provinces)
            {
                foreach (Province ap in Provinces)
                {
                    if (!p.Pathfinding.ContainsKey(ap))
                        p.Pathfinding.Add(ap, new Province.Pathway(ap, new List<Province>(), 999999));                  
                }
            }
            foreach(Province p in Provinces)
            {
                foreach(Province b in p.Bordering)
                {
                    if(!p.Railways.ContainsKey(b))
                        p.Railways.Add(b, 0);
                }
            }
            



            /*Vector2 dist = startingpoint.ProvinceCapital - p.ProvinceCapital;
            foreach (Province p in Provinces[1].Bordering)
            {
                int distance = Provinces[1].Distance[p];
                Province.Pathway pat = new Province.Pathway(p, new List<Province>() { }, (p.Terrain.TravelTime * distance / 10) + (Provinces[1].Terrain.TravelTime * distance / 10));
                Provinces[1].Pathfinding[p] = pat;
            }
            */

            
            FirstHalf.AddRange(Provinces.GetRange(0, 220));
            SecondHalf.AddRange(Provinces.GetRange(221, 220));
            ThirdHalf.AddRange(Provinces.GetRange(441, 220));
            FourthHalf.AddRange(Provinces.GetRange(661, 247));
            
            Thread t = new Thread(V);
            t.Start();
            Thread t2 = new Thread(V2);
            t2.Start();
            Thread t3 = new Thread(V3);
            t3.Start();

            foreach (Province p in FirstHalf)
            {
                CreateRoutes(p, false);
            }            
            
            
        }
        public static void V0()
        {
            foreach (Province p in FirstHalf)
            {
                CreateRoutes(p);
            }
            Pathfinding[0] = false;
            if(RenewInfo)
                ConsoleClass.HandleConsole("\nrenew 0: done", true);
        }
        public static void V()
        {
            foreach (Province p in SecondHalf)
            {
                CreateRoutes(p);
            }
            Pathfinding[1] = false;
            if (RenewInfo)
                ConsoleClass.HandleConsole("\nrenew 1: done", true);
        }
        public static void V2()
        {
            foreach (Province p in ThirdHalf)
            {
                CreateRoutes(p);
            }
            Pathfinding[2] = false;
            if (RenewInfo)
                ConsoleClass.HandleConsole("\nrenew 2: done", true);
        }
        public static void V3()
        {
            foreach (Province p in FourthHalf)
            {
                CreateRoutes(p);
            }
            Pathfinding[3] = false;
            if (RenewInfo)
                ConsoleClass.HandleConsole("\nrenew 3: done", true);
        }
        public static void Vsingle()
        {
            foreach (Province p in FirstHalf)
            {
                CreateRoutes(p);
            }
            Pathfinding[0] = false;
            if (RenewInfo)
                ConsoleClass.HandleConsole("\nrenew 0: done", true);
            foreach (Province p in SecondHalf)
            {
                CreateRoutes(p);
            }
            Pathfinding[1] = false;
            if (RenewInfo)
                ConsoleClass.HandleConsole("\nrenew 1: done", true);
            foreach (Province p in ThirdHalf)
            {
                CreateRoutes(p);
            }
            Pathfinding[2] = false;
            if (RenewInfo)
                ConsoleClass.HandleConsole("\nrenew 2: done", true);
            foreach (Province p in FourthHalf)
            {
                CreateRoutes(p);
            }
            Pathfinding[3] = false;
            if (RenewInfo)
                ConsoleClass.HandleConsole("\nrenew 3: done", true);
        }

        public static bool[] Pathfinding = new bool[] { false, false, false, false };

        public static void RenewPaths(bool SingleThread = false)
        {
            if (Pathfinding.All(x => !x))
            {
                Pathfinding[0] = true;
                Pathfinding[1] = true;
                Pathfinding[2] = true;
                Pathfinding[3] = true;
                if (!SingleThread)
                {
                    Thread t0 = new Thread(V0);
                    t0.Start();
                    Thread t = new Thread(V);
                    t.Start();
                    Thread t2 = new Thread(V2);
                    t2.Start();
                    Thread t3 = new Thread(V3);
                    t3.Start();
                }
                else
                {
                    Thread tsingle = new Thread(Vsingle);
                    tsingle.Start();
                }
            }
        }

        public static void OpenMenu(string menu)
        {
            switch (menu)
            {
                case "TRA":
                    TradeMenu.Visible = true;
                    break;
            }
        }

        public static void MapmodeChange(string arg)
        {
            foreach (Button b in Mapmodes.Controls.Where(x=> x is Button))
            {
                b.Clicked = false;
            }
            if (arg == "DEV")
            {
                Mapmode = "DEVELOPMENT";
                (Mapmodes.Controls.Find(x => x.Name == "DevButton") as Button).Clicked = true;               
            }
            else if (arg == "CON")
            {
                Mapmode = "CONTROL";
                (Mapmodes.Controls.Find(x => x.Name == "ConButton")as Button).Clicked = true;
            }
            else if (arg == "TRA")
            {
                Mapmode = "TRADE";
                (Mapmodes.Controls.Find(x => x.Name == "TraButton") as Button).Clicked = true;
            }
            else if (arg == "TER")
            {
                Mapmode = "TERRAIN";
                (Mapmodes.Controls.Find(x => x.Name == "TerButton") as Button).Clicked = true;
            }
            else if (arg == "REG")
            {
                Mapmode = "REGION";
                (Mapmodes.Controls.Find(x => x.Name == "RegButton") as Button).Clicked = true;
            }
        }

      
        protected override void LoadContent()
        {            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Map = Content.Load<Texture2D>("MapFiles/BlankMap");
            Unit = Content.Load<Texture2D>("UnitFiles/Unit");
            SoldierGeneric = Content.Load<Texture2D>("UnitFiles/SoldierNoe");
            SoldierLongcoats = Content.Load<Texture2D>("UnitFiles/SoldierLongcoats");
            SoldierCommonfolk = Content.Load<Texture2D>("UnitFiles/SoldierCommonfolk");
            SoldierCivilized = Content.Load<Texture2D>("UnitFiles/SoldierCivilized");
            SoldierKSR = Content.Load<Texture2D>("UnitFiles/SoldierKSR");
            ForVyeshal___Monogame.Initialize.Load();
            
            ContentLoaded = true;
        }
     
        protected override void UnloadContent()
        {
           
        }

        public static Color InvertColor(Color original)
        {
            return new Color(255-original.R, 255-original.G, 255-original.B);
        }


        public static void StartBattle(Province p, List<Unit> Defender, List<Unit> Attacker)
        {

        }
        


        public int RefreshProvinceCountdown = 0;
        protected override void Update(GameTime gameTime)
        {
            
            if (GlobalTick < 100)
                GlobalTick++;
            else
                GlobalTick = 1;

            if(GlobalTick % 25 == 0)
            {
                GoldLabel.Text = "Gold: " + PlayerCountry.Gold + " (" + PlayerCountry.LastTurnIncome + ")";
                MELabel.Text = "Military Equipment: " + PlayerCountry.MilitaryEquipment;
                CoalLabel.Text = "Coal: " + PlayerCountry.Coal;
                switch (PlayerCountry.FactionTAG)
                {
                    case "KSR":
                        StabilityLabel.Text = "Order: " + PlayerCountry.Stability;
                        break;
                    case "COM":
                        StabilityLabel.Text = "Unity: " + PlayerCountry.Stability;
                        break;
                    case "LON":
                        StabilityLabel.Text = "Stability: " + PlayerCountry.Stability;
                        break;
                    case "CIV":
                        StabilityLabel.Text = "Devotion: " + PlayerCountry.Stability;
                        break;
                    default:
                        StabilityLabel.Text = "Stability: " + PlayerCountry.Stability;
                        break;
                }
                RefreshProvinceCountdown++;
                if (RefreshProvinceCountdown == 20)
                { 
                    RefreshProvinceCountdown = 0;
                    UpdateProvinceInfo();
                    if(false)
                        ConsoleClass.HandleConsole("\nUpdated province info!", true);
                }
                
            }
            else if(GlobalTick % 10 == 0)
            {
                Copy = new List<UIWindow>();
                Copy.AddRange(UIWindows);
                Copy.Reverse();                               
            }
            if (GlobalTick % 100 == 0)
            {
                RenewCountdown++;
                if(RenewCountdown == 15)
                {
                    ConsoleClass.HandleConsole("\nStarting automatic renew!", true);
                    RenewCountdown = 0;
                    RenewPaths(true);
                }
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

           
        
            if (ContentLoaded)
            {                             
                if (FocusedText == null)
                {                           
                    if (KeyboardC.KeyDelay < 4)
                    {
                        Keys k = KeyboardC.GetControlKey();
                        switch (k)
                        {
                            case Keys.OemTilde:
                                ConsoleWindow.Visible = !ConsoleWindow.Visible;
                                break;
                            case Keys.Enter:
                                EndTurn();
                                break;
                            case Keys.Up:
                                if (ConsoleWindow.Visible && ConsoleClass.Line > 0)
                                    ConsoleClass.Line -= 1;
                                break;
                            case Keys.Down:
                                if (ConsoleWindow.Visible)
                                    ConsoleClass.Line += 1;
                                break;
                            case Keys.Tab:
                                if (Mapmode == "CONTROL")
                                    MapmodeChange("TRA");
                                else if (Mapmode == "TRADE")
                                    MapmodeChange("TER");
                                else if (Mapmode == "TERRAIN")
                                    MapmodeChange("REG");
                                else if (Mapmode == "REGION")
                                    MapmodeChange("DEV");
                                else if (Mapmode == "DEVELOPMENT")
                                    MapmodeChange("CON");
                                break;
                        }
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right))
                    {
                        CameraPosition.X -= 8f;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left))
                    {
                        CameraPosition.X += 8f;         
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up))
                    {
                        CameraPosition.Y += 8f;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down))
                    {
                        CameraPosition.Y -= 8f;
                    }                 
                }

                else if (KeyboardC.KeyDelay < 4)
                {
                    FocusedText.Text += KeyboardC.GetTextKey();
                    Keys k = KeyboardC.GetControlKey();
                    switch (k)
                    {
                        case Keys.Back:
                            if (FocusedText.Text.Length > 0)
                                FocusedText.Text = FocusedText.Text.Remove(FocusedText.Text.Length - 1);
                            break;
                        case Keys.Enter:
                            if (FocusedText.Text.Length > 0)
                            {
                                if (ConsoleWindow.Controls.Contains(FocusedText))
                                {
                                    ConsoleString += "\n" + FocusedText.Text;
                                    ConsoleInput(FocusedText.Text);
                                    FocusedText.Text = "";
                                }
                            }
                            break;
                        case Keys.OemTilde:
                            ConsoleWindow.Visible = !ConsoleWindow.Visible;                               
                            break;
                    }
                }

                if (KeyboardC.KeyDelay > 0)
                    KeyboardC.KeyDelay--;

                HandleMouse.Handle(Mouse.GetState());
                
                if (GlobalTick % 20 == 0)
                {
                    if (TradeMenu.Visible)
                    {
                        foreach(Container con in TradeMenu.Controls.Where(x=> x is Container))
                        {
                            string[] splitname = con.Name.Split('_');
                            string name = "";
                            if (splitname.Count() > 1)
                                name = splitname[1];
                            else
                                name = "";

                            TradeGood t = TradeGoods[name];
                            foreach (Label c in con.Controls.Where(x=>x is Label))
                            {
                                switch (c.Name)
                                {
                                    case "Name":
                                        c.Text = t.Name;
                                        break;
                                    case "BaseValue":
                                        c.Text = t.GetPrice().ToString();
                                        break;
                                    case "PriceModifier":
                                        c.Text = ((decimal)PlayerCountry.TradeGoodIncome[t] - 1) * 100 + "%";
                                        break;
                                    case "Production":
                                        c.Text = PlayerCountry.GetTotalProduction(t).ToString();
                                        break;
                                    case "ProductionModifier":
                                        c.Text = (Math.Round((decimal)PlayerCountry.TradeGoodEf[t], 2) - 1  + PlayerCountry.ProductionEf - 1) * 100 + "%";
                                        break;
                                }
                            }
                        }
                    }
                }

                if (ProvinceCheckDelay > 0)
                    ProvinceCheckDelay--;

                LastMousePosition.X = Mouse.GetState().Position.X;
                LastMousePosition.Y = Mouse.GetState().Position.Y;

                if (CameraPosition.Y > 0)
                    CameraPosition.Y = 0;
                if (CameraPosition.X > 0)
                    CameraPosition.X = 0;
                if (CameraPosition.X < -3840 + ScreenResolution.X)
                    CameraPosition.X = -3840 + ScreenResolution.X;
                if (CameraPosition.Y < -2160 + ScreenResolution.Y)
                    CameraPosition.Y = -2160 + ScreenResolution.Y;
            }
            base.Update(gameTime);
            
        }

      
        public static void CreateRoutes(Province startingpoint, bool clean = true)
        {           
            if(clean)
            {
                foreach(Province p in Provinces)
                {
                    startingpoint.Pathfinding[p] = new Province.Pathway(p, new List<Province>() { }, 999999);
                }
            }
               

            List<Province.Pathway> n = new List<Province.Pathway>();
            foreach (Province p in startingpoint.Bordering)
            {
                //Vector2 dist = startingpoint.ProvinceCapital - p.ProvinceCapital;
                int distance = startingpoint.Distance[p];                               
                Province.Pathway pat = new Province.Pathway(p, new List<Province>() { }, CalculateTravelTime(p, startingpoint));                        
                startingpoint.Pathfinding[p] = pat;
                n.Add(pat);                                  
            }

            int hops = 0;
            do
            {
                hops++;
                List<Province.Pathway> tn = new List<Province.Pathway>();
                foreach(Province.Pathway pa in n)
                {
                    foreach(Province p in pa.Destination.Bordering)
                    {
                        if(p != startingpoint && !pa.Path.Contains(p))
                        {
                            // Vector2 dist = pa.Destination.ProvinceCapital - p.ProvinceCapital;
                            //int distance = (int)Math.Abs(dist.X) + (int)Math.Abs(dist.Y);
                            int distance = pa.Destination.Distance[p];
                            int tott = pa.TotalTime + CalculateTravelTime(p, pa.Destination, distance);
                            if(p.Pathfinding[startingpoint].TotalTime < tott && p.Pathfinding[startingpoint].TotalTime < startingpoint.Pathfinding[p].TotalTime)
                            {
                                    List<Province> rev = new List<Province>();
                                    rev.AddRange(p.Pathfinding[startingpoint].Path);
                                    rev.Reverse();
                                    Province.Pathway np = new Province.Pathway(p, rev, p.Pathfinding[startingpoint].TotalTime);
                                    startingpoint.Pathfinding[p] = np;
                                    tn.Add(np);
                                   
                            }
                            else if (startingpoint.Pathfinding[p].TotalTime > tott)
                            {
                                List<Province> npa = new List<Province>();
                                npa.AddRange(pa.Path);
                                npa.Add(pa.Destination);
                                Province.Pathway np = new Province.Pathway(p, npa, tott);
                                startingpoint.Pathfinding[p] = np;                               
                                tn.Add(np);
                            }



                        }
                    }
                }
                n.Clear();
                n.AddRange(tn);                
            }
            while (n.Any());

        }        

        public static int GetDistance(Province p1, Province p2)
        {
            Vector2 dist = p1.ProvinceCapital - p2.ProvinceCapital;
            return (int)Math.Abs(dist.X) + (int)Math.Abs(dist.Y);
        }

        public static int CalculateTravelTime(Province p1, Province p2, int distance = -1)
        {
            if(distance == -1)
                distance = GetDistance(p1, p2);
            int traveltime = (p1.Terrain.TravelTime * distance / 10) + (p2.Terrain.TravelTime * distance / 10);
            if (p1.Railways.ContainsKey(p2) && p2.Railways.ContainsKey(p1))
            {
                if (p1.Railways[p2] == 2 || p2.Railways[p1] == 2)
                    traveltime = (traveltime / 100000);
            }
            return traveltime;
        }

        

        public int Turn = 0;

        public void EndTurn()
        {
            foreach (Country c in Countries.Values)
            {
                c.Stability = c.Stability + c.StabilityGain;
                c.LastTurnIncome = 0;
                c.LastTurnMilitaryEquipmentProduction = 0;

                foreach(TradeGood tg in TradeGoods.Values)
                {
                    c.RemoveModifier(c.Modifiers.Find(x => x.Title == tg.CountryModifier.Title));
                    List<Modifier.Variable> Vars = new List<Modifier.Variable>();
                    foreach(Modifier.Variable var in tg.CountryModifier.Variables)
                    {
                        decimal v = c.GetTotalProduction(tg);
                        decimal tot = tg.GetTotalProduction();
                        if (v > tot / 3)
                            v = tot;               
                        Vars.Add(new Modifier.Variable(var.Name, var.Value * v));
                    }
                    c.AddModifier(new Country.CountryModifier(tg.CountryModifier.Title, Vars));
                }
            }
            foreach (Province p in Provinces)
            {
               
                p.Control.LastTurnIncome += p.GetTaxIncome() + p.GetTradeIncome();
                p.Control.Gold += p.GetTaxIncome() + p.GetTradeIncome();
                p.Control.LastTurnMilitaryEquipmentProduction += p.GetMilitaryEquipmentIncome();
                p.Control.MilitaryEquipment += p.GetMilitaryEquipmentIncome();
                
            }
            
            UpdateProvinceInfo();
            Turn++;
        }

        public static void UpdateProvinceInfo()
        {
            if (SelectedProvince != null)
            {
                ControlLabel.Text = "Control: " + SelectedProvince.Control.FullName;
                TerrainLabel.Text = "Terrain: " + SelectedProvince.Terrain.Name;
                TradeLabel.Text = "Trade: " + SelectedProvince.Trade.Name + " (" + SelectedProvince.Trade.GetPrice() + ")";
                RegionLabel.Text = "Region: " + SelectedProvince.Region.FullName;
                DevelopmentLabel.Text = "Development: " + SelectedProvince.Development;
               
                TaxLabel.Text = "Tax: " + SelectedProvince.GetTaxIncome().ToString();
                MEPLabel.Text = "Mil. Eq. Production: " + SelectedProvince.GetMilitaryEquipmentIncome();
                while (TaxLabel.Text.Length < 12)
                {
                    TaxLabel.Text += " ";
                }                               
                TaxLabel.Text +="|   Production: " + SelectedProvince.GetTradeIncome() + " (" + SelectedProvince.GetTradeProduction() + ")";
                if (DebugMode)
                {
                    ControlLabel.Text += " [" + SelectedProvince.Control.TAG + "]";
                    RegionLabel.Text += " [" + SelectedProvince.ID + "]";
                }
                SoldierMenu.Controls.First().Controls.RemoveAll(x => !(x is Scroll));
                if (SelectedProvince.Units.Any())
                {
                    int a = 0;
                    int totalinf = 0, totalcav = 0, totalart = 0;

                    foreach (Unit u in SelectedProvince.Units)
                    {
                        switch (u.Class)
                        {
                            case "Infantry":
                                totalinf++;
                                break;
                            case "Cavalry":
                                totalcav++;
                                break;
                            case "Artillery":
                                totalart++;
                                break;
                        }
                        Container c = new Container("SoldierC" + a, new Vector2(5, 40 * a), new Vector2(280, 30))
                        {
                            Scrolable = true
                        };
                        a++;
                        c.AddControl(new Label("name", u.Name, Color.Black, new Vector2(10, 2), Font10));
                        c.AddControl(new Label("size", u.Size + "/" + u.MaxSize, Color.Black, new Vector2(10, 15), Font10));
                        c.AddControl(new ExtraLine(new Vector2(80, 22), new Vector2(240, 22), Color.Red, 6));
                        c.AddControl(new ExtraLine(new Vector2(80, 22), new Vector2((u.Size / u.MaxSize) * 160 + 80, 22), Color.Green, 6));
                        c.AddControl(new ExtraLine(new Vector2(0, 30), new Vector2(260, 30), Color.Black, 2));
                        SoldierMenu.Controls.First().AddControl(c);
                    }
                    (SoldierMenu.Controls.Find(x => x is Label) as Label).Text = $"INF: {totalinf}     CAV: {totalcav}     ART: {totalart}";
                }
                else
                    (SoldierMenu.Controls.Find(x => x is Label) as Label).Text = $"INF: 0     CAV: 0     ART: 0";
                SoldierMenu.Controls.First().RenewScroll();
            }
        }

        public void ConsoleInput(string text)
        {
            ConsoleClass.HandleConsole(text);         
        }

        RasterizerState _rasterizerState = new RasterizerState() { ScissorTestEnable = true,  };

        protected override void Draw(GameTime gameTime)
        {
           
                GraphicsDevice.Clear(Color.CornflowerBlue);
                spriteBatch.Begin(samplerState: SamplerState.PointClamp, rasterizerState: _rasterizerState);
                if (ContentLoaded)
                {
                    spriteBatch.Draw(Map, CameraPosition, Color.White);
                    foreach (Province p in Provinces)
                    {
                        switch (Mapmode)
                        {
                            case "CONTROL":
                                spriteBatch.Draw(p.Texture, CameraPosition + p.DrawPosition, p.Control.Color);
                                p.InvertedColor = p.Control.InvertedColor;
                                if (p.Control != PlayerCountry && !p.Bordering.Any(x => x.Control == PlayerCountry))
                                    spriteBatch.Draw(p.Texture, CameraPosition + p.DrawPosition, Color.FromNonPremultiplied(32, 32, 32, 90));                               
                                break;
                            case "PATH":
                                if (SelectedProvince != null && p != SelectedProvince)
                                {
                                    if (SelectedProvince.Pathfinding.Keys.Contains(p))
                                    {
                                        int c = SelectedProvince.GetPathway(p).TotalTime;
                                        if (c < 999999)
                                        {
                                            c = c / 10;
                                            if (c > 255)
                                                c = 255;
                                            spriteBatch.Draw(p.Texture, CameraPosition + p.DrawPosition, new Color(c, 255 - c, 255 - c));
                                        }
                                        else
                                        {
                                            spriteBatch.Draw(p.Texture, CameraPosition + p.DrawPosition, Color.White);
                                        }
                                        p.InvertedColor = Color.Black;
                                    }
                                }
                                else
                                {
                                    spriteBatch.Draw(p.Texture, CameraPosition + p.DrawPosition, Color.White);
                                    p.InvertedColor = Color.Black;
                                }

                                break;
                            case "CAPITAL":
                                spriteBatch.Draw(p.Texture, CameraPosition + p.DrawPosition, Color.DarkBlue);
                                spriteBatch.DrawLine(p.ProvinceCapital + CameraPosition, p.ProvinceCapital + new Vector2(1,1) + CameraPosition, Color.Red, 1);
                                break;
                            case "RAIL":
                                spriteBatch.Draw(p.Texture, CameraPosition + p.DrawPosition, Color.White);                                                        
                                break;
                            case "UNIT":
                                spriteBatch.Draw(p.Texture, CameraPosition + p.DrawPosition, p.Control.Color);
                                p.InvertedColor = p.Control.InvertedColor;
                                if (p.Control != PlayerCountry && !p.Bordering.Any(x => x.Control == PlayerCountry))
                                    spriteBatch.Draw(p.Texture, CameraPosition + p.DrawPosition, Color.FromNonPremultiplied(32, 32, 32, 90));
                                break;
                            case "TRADE":
                                spriteBatch.Draw(p.Texture, CameraPosition + p.DrawPosition, p.Trade.Color);
                                p.InvertedColor = p.Trade.InvertedColor;
                                break;
                            case "REGION":
                                spriteBatch.Draw(p.Texture, CameraPosition + p.DrawPosition, p.Region.Color);
                                p.InvertedColor = p.Region.InvertedColor;
                                break;
                            case "TERRAIN":
                                spriteBatch.Draw(p.Texture, CameraPosition + p.DrawPosition, p.Terrain.Color);
                                p.InvertedColor = p.Terrain.InvertedColor;
                                break;
                            case "DEVELOPMENT":
                                if (p.Development < 4)
                                {
                                    spriteBatch.Draw(p.Texture, CameraPosition + p.DrawPosition, new Color(255, 0, 0));
                                    p.InvertedColor = new Color(255 - 255, 255 - 0, 255 - 0);
                                }
                                else if (p.Development < 8)
                                {
                                    spriteBatch.Draw(p.Texture, CameraPosition + p.DrawPosition, new Color(255, 131, 0));
                                    p.InvertedColor = new Color(255 - 255, 255 - 131, 255 - 0);
                                }
                                else if (p.Development < 12)
                                {
                                    spriteBatch.Draw(p.Texture, CameraPosition + p.DrawPosition, new Color(255, 209, 0));
                                    p.InvertedColor = new Color(255 - 255, 255 - 209, 255 - 0);
                                }
                                else if (p.Development < 15)
                                {
                                    spriteBatch.Draw(p.Texture, CameraPosition + p.DrawPosition, new Color(193, 255, 0));
                                    p.InvertedColor = new Color(255 - 193, 255 - 255, 255 - 0);
                                }
                                else if (p.Development < 18)
                                {
                                    spriteBatch.Draw(p.Texture, CameraPosition + p.DrawPosition, new Color(29, 160, 0));
                                    p.InvertedColor = new Color(255 - 29, 255 - 160, 255 - 0);
                                }
                                else if (p.Development < 21)
                                {
                                    spriteBatch.Draw(p.Texture, CameraPosition + p.DrawPosition, new Color(6, 189, 123));
                                    p.InvertedColor = new Color(255 - 6, 255 - 189, 255 - 123);
                                }
                                break;
                        }
                    }
                    if (SelectedProvince != null)
                    {
                        spriteBatch.Draw(SelectedProvince.Texture, CameraPosition + SelectedProvince.DrawPosition, Color.FromNonPremultiplied(255, 255, 255, 64));
                        if (SelectedProvince.InvertedColor != null)
                        {
                            spriteBatch.Draw(SelectedProvince.SelectedTexture, CameraPosition + SelectedProvince.DrawPosition, SelectedProvince.InvertedColor);
                        }
                        if(Mapmode == "PATH")
                        {
                            foreach(Province ppath in SelectedProvince.Pathfinding.Keys)
                            {
                                Province.Pathway pt = SelectedProvince.GetPathway(ppath);
                                if (ppath.ID != 0 && pt.TotalTime < 999999)
                                {
                                    if (pt.Path.Any())
                                    {
                                        spriteBatch.DrawLine(CameraPosition + SelectedProvince.ProvinceCapital, CameraPosition + pt.Path[0].ProvinceCapital, Color.Black);
                                        for (int a = 1; a < pt.Path.Count - 2; a++)
                                        {
                                            spriteBatch.DrawLine(CameraPosition + pt.Path[a].ProvinceCapital, CameraPosition + pt.Path[a + 1].ProvinceCapital, Color.Black);
                                        }
                                        spriteBatch.DrawLine(CameraPosition + pt.Path.Last().ProvinceCapital, CameraPosition + ppath.ProvinceCapital, Color.Black);
                                    }
                                    else
                                    {
                                        spriteBatch.DrawLine(CameraPosition + SelectedProvince.ProvinceCapital, CameraPosition + ppath.ProvinceCapital, Color.Black);
                                    }
                                }
                            }
                        }

                    }
                    if (Mapmode == "RAIL")
                    {
                        foreach (Province p in Provinces)
                        {
                            foreach (Province b in p.Railways.Keys)
                            {
                                if (p.Railways[b] == 1)
                                    spriteBatch.DrawLine(p.ProvinceCapital + CameraPosition, b.ProvinceCapital + CameraPosition, Color.Red, 5);
                                else if (p.Railways[b] == 2)
                                    spriteBatch.DrawLine(p.ProvinceCapital + CameraPosition, b.ProvinceCapital + CameraPosition, Color.DarkGreen, 5);
                            }
                        }
                    }
                    else if(Mapmode == "CONTROL")
                    {
                        foreach(Province p in Provinces)
                        {
                            if (p.UnitPosition != null && p.Units.Any())
                            {
                                switch (p.Control.FactionTAG)
                                {
                                    case "LON":
                                        spriteBatch.Draw(SoldierLongcoats, p.UnitPosition + CameraPosition - new Vector2(32, 32), color: Color.White, scale: new Vector2(1f, 1f));
                                        break;
                                    case "KSR":
                                        spriteBatch.Draw(SoldierKSR, p.UnitPosition + CameraPosition - new Vector2(32, 32), color: Color.White, scale: new Vector2(1f, 1f));
                                        break;
                                    case "CIV":
                                        spriteBatch.Draw(SoldierCivilized, p.UnitPosition + CameraPosition - new Vector2(32, 32), color: Color.White, scale: new Vector2(1f, 1f));
                                        break;
                                    case "COM":
                                        spriteBatch.Draw(SoldierCommonfolk, p.UnitPosition + CameraPosition - new Vector2(32, 32), color: Color.White, scale: new Vector2(1f, 1f));
                                        break;
                                    default:
                                        spriteBatch.Draw(SoldierGeneric, p.UnitPosition + CameraPosition - new Vector2(32, 32), color: Color.White, scale: new Vector2(1f, 1f));
                                        break;
                                }
                            }
                            
                        }
                    }
                    else if (Mapmode == "CONTROL")
                    {
                        foreach (Country c in Countries.Values)
                        {
                            if(c.Capital != null)
                                spriteBatch.Draw(Capital, c.Capital.ProvinceCapital + CameraPosition - new Vector2(6, 6), color: Color.White, scale: new Vector2(0.04083f, 0.04083f));
                        }
                    }

                //UI

                //Copy
                spriteBatch.End();

                foreach (UIWindow window in Copy)
                {

                    if (window.Visible)
                    {
                        spriteBatch.Begin(samplerState: SamplerState.PointClamp, rasterizerState: _rasterizerState);
                        DLine.DrawLine(spriteBatch, new Vector2(window.Position.X, window.Position.Y + window.Size.Y / 2), new Vector2(window.Position.X + window.Size.X, window.Position.Y + window.Size.Y / 2), Color.White, window.Size.Y);
                        DLine.DrawLine(spriteBatch, window.Position, new Vector2(window.Position.X + window.Size.X, window.Position.Y), Color.Black, 6);
                        DLine.DrawLine(spriteBatch, new Vector2(window.Position.X + window.Size.X - 3, window.Position.Y), new Vector2(window.Position.X + window.Size.X - 3, window.Position.Y + window.Size.Y), Color.Black, 6);
                        DLine.DrawLine(spriteBatch, new Vector2(window.Position.X + window.Size.X, window.Position.Y + window.Size.Y), new Vector2(window.Position.X, window.Position.Y + window.Size.Y), Color.Black, 6);
                        DLine.DrawLine(spriteBatch, new Vector2(window.Position.X + 3, window.Position.Y + window.Size.Y), window.Position + new Vector2(3, 0), Color.Black, 6);


                        //Title

                        if (window.Title != "")
                        {
                            Vector2 size = Font24.MeasureString(window.Title);
                            spriteBatch.DrawString(Font24, window.Title, new Vector2(window.Position.X + window.Size.X / 2 - size.X / 2, window.Position.Y + 25 - size.Y / 2), Color.Black);
                            DLine.DrawLine(spriteBatch, new Vector2(window.Position.X + 3, window.Position.Y + 50), new Vector2(window.Position.X + window.Size.X - 3, window.Position.Y + 50), Color.Black, 6);
                        }

                        if (window.Closable)
                        {
                            DLine.DrawLine(spriteBatch, new Vector2(window.Position.X + window.Size.X - 40, window.Position.Y + 10), new Vector2(window.Position.X + window.Size.X - 10, window.Position.Y + 40), Color.Black, 2);
                            DLine.DrawLine(spriteBatch, new Vector2(window.Position.X + window.Size.X - 10, window.Position.Y + 10), new Vector2(window.Position.X + window.Size.X - 40, window.Position.Y + 40), Color.Black, 2);
                        }

                        spriteBatch.End();
                        foreach (Control c in window.GetAllChildren())
                        {
                            spriteBatch.Begin(samplerState: SamplerState.PointClamp, rasterizerState: _rasterizerState);
                            //spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle(new Point((int)c.GetAbsolutePosition().X, (int)c.GetAbsolutePosition().Y), new Point((int)c.Size.X, (int)c.Size.Y));
                            if (window.Title != "")
                            {
                                spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle(new Point((int)window.Position.X, (int)window.Position.Y + 52), new Point((int)window.Size.X, (int)window.Size.Y - 50));
                            }
                            else
                            {
                                spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle(new Point((int)window.Position.X, (int)window.Position.Y), new Point((int)window.Size.X, (int)window.Size.Y));
                            }
                            if (c.GetAllParents().Any(x => x is Container))
                            {
                                Control w = c.GetAllParents().Find(x => x is Container);
                                spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle(new Point((int)w.GetAbsolutePosition().X, (int)w.GetAbsolutePosition().Y), new Point((int)w.Size.X, (int)w.Size.Y));
                            }
                            
                            if (c is Label l)
                            {
                                if (l.Size.X == 0 && l.Size.Y == 0)
                                {
                                    int line = 0;
                                    bool bo = false;
                                    foreach (string s in l.Text.Split('\n'))
                                    {
                                        Vector2 NewPosition = l.Position + new Vector2(0, l.Font.LineSpacing * line);
                                        string displayedstring = s;
                                        spriteBatch.DrawString(l.Font, displayedstring, l.GetAbsolutePosition() + new Vector2(0, l.Font.LineSpacing * line), l.Color);
                                        line++;
                                    }

                                }
                                else
                                {
                                    int line = 0;
                                    string[] split = l.Text.Split('\n');
                                    int totalsizey = split.Length * l.Font.LineSpacing;
                                    foreach (string s in split)
                                    {
                                        Vector2 PositionToWrite = new Vector2();
                                        Vector2 size = l.Font.MeasureString(s);
                                        Vector2 AbsolutePosition = l.GetAbsolutePosition();

                                        if (l.Alignment == TextAlignemt.TopLeft || l.Alignment == TextAlignemt.Top || l.Alignment == TextAlignemt.TopRight)
                                        {
                                            PositionToWrite.Y = AbsolutePosition.Y;
                                        }
                                        else if (l.Alignment == TextAlignemt.MiddleLeft || l.Alignment == TextAlignemt.Center || l.Alignment == TextAlignemt.MiddleRight)
                                        {
                                            PositionToWrite.Y = AbsolutePosition.Y + l.Size.Y / 2 - totalsizey / 2;
                                        }
                                        else if (l.Alignment == TextAlignemt.BottomLeft || l.Alignment == TextAlignemt.Bottom || l.Alignment == TextAlignemt.BottomRight)
                                        {
                                            PositionToWrite.Y = AbsolutePosition.Y + l.Size.Y - totalsizey;
                                        }

                                        if (l.Alignment == TextAlignemt.TopLeft || l.Alignment == TextAlignemt.MiddleLeft || l.Alignment == TextAlignemt.BottomLeft)
                                        {
                                            PositionToWrite.X = AbsolutePosition.X;
                                        }
                                        else if (l.Alignment == TextAlignemt.Top || l.Alignment == TextAlignemt.Center || l.Alignment == TextAlignemt.Bottom)
                                        {
                                            PositionToWrite.X = AbsolutePosition.X + l.Size.X / 2 - size.X / 2;
                                        }
                                        else if (l.Alignment == TextAlignemt.TopRight || l.Alignment == TextAlignemt.MiddleRight || l.Alignment == TextAlignemt.BottomRight)
                                        {
                                            PositionToWrite.X = AbsolutePosition.X + l.Size.X - size.X;
                                        }
                                        PositionToWrite.Y += l.Font.LineSpacing * line;
                                        spriteBatch.DrawString(l.Font, s, PositionToWrite, l.Color);
                                        line++;

                                    }
                                }
                            }
                            else if(c is Scroll s)
                            {
                                DLine.DrawLine(spriteBatch, s.GetAbsolutePosition(), s.GetAbsolutePosition() + new Vector2(s.Size.X,0), Color.Black, 2);
                                DLine.DrawLine(spriteBatch, s.GetAbsolutePosition() + s.Size, s.GetAbsolutePosition() + new Vector2(s.Size.X, 0), Color.Black, 2);
                                DLine.DrawLine(spriteBatch, s.GetAbsolutePosition() + s.Size, s.GetAbsolutePosition() + new Vector2(0, s.Size.Y), Color.Black, 2);
                                DLine.DrawLine(spriteBatch, s.GetAbsolutePosition(), s.GetAbsolutePosition() + new Vector2(0, s.Size.Y), Color.Black, 2);
                                float size = s.Size.Y / (s.MaxScroll + 1 + s.ScrollShown);
                                DLine.DrawLine(spriteBatch, s.GetAbsolutePosition() + new Vector2(10, size * s.ScrollPosition + 3), s.GetAbsolutePosition() + new Vector2(10, size* (s.ScrollPosition + 1 + s.ScrollShown) - 3), Color.Black, 12);
                            }
                            else if (c is ExtraLine el)
                            {
                                DLine.DrawLine(spriteBatch, el.GetAbsolutePosition(), el.GetAbsolutePosition(el.SecondPosition), el.Color, el.LineWidth);
                            }
                            else if (c is TextInput t)
                            {
                                Vector2 size = t.Font.MeasureString(t.Text + " ");
                                DLine.DrawLine(spriteBatch, new Vector2(t.GetAbsolutePosition().X, t.GetAbsolutePosition().Y + size.Y / 2), new Vector2(t.GetAbsolutePosition().X + t.Width, t.GetAbsolutePosition().Y + size.Y / 2), Color.White, size.Y);

                                DLine.DrawLine(spriteBatch, window.Position + t.Position, new Vector2(t.GetAbsolutePosition().X + t.Width, t.GetAbsolutePosition().Y), Color.Black, 4);
                                DLine.DrawLine(spriteBatch, new Vector2(t.GetAbsolutePosition().X + t.Width - 2, t.GetAbsolutePosition().Y), new Vector2(t.GetAbsolutePosition().X + t.Width - 2, t.GetAbsolutePosition().Y + size.Y), Color.Black, 4);
                                DLine.DrawLine(spriteBatch, new Vector2(t.GetAbsolutePosition().X + t.Width, t.GetAbsolutePosition().Y + size.Y), new Vector2(t.GetAbsolutePosition().X, t.GetAbsolutePosition().Y + size.Y), Color.Black, 4);
                                DLine.DrawLine(spriteBatch, new Vector2(t.GetAbsolutePosition().X + 2, t.GetAbsolutePosition().Y + size.Y), t.GetAbsolutePosition() + new Vector2(2, 0), Color.Black, 4);

                                string finaltext = t.Text;
                                if (GlobalTick > 50 && t == FocusedText)
                                {
                                    finaltext += "|";
                                }
                                if (size.X < t.Width - 20)
                                    spriteBatch.DrawString(t.Font, finaltext, t.GetAbsolutePosition() + new Vector2(10, 0), Color.Black);
                                else
                                {
                                    string newstring = finaltext;
                                    Vector2 newsize = new Vector2();
                                    do
                                    {
                                        newstring = newstring.Remove(0, 1); 
                                        newsize = t.Font.MeasureString(newstring);
                                    }
                                    while (newsize.X > t.Width - 20);
                                    spriteBatch.DrawString(t.Font, newstring, t.GetAbsolutePosition()+ new Vector2(10, 0), Color.Black);
                                }
                            }
                            else if (c is Button b)
                            {
                                DLine.DrawLine(spriteBatch, new Vector2(b.GetAbsolutePosition().X, b.GetAbsolutePosition().Y + b.Size.Y / 2), new Vector2(b.GetAbsolutePosition().X + b.Size.X, b.GetAbsolutePosition().Y + b.Size.Y / 2), b.Backcolor, b.Size.Y);
                                if (b.Shadow || b.Clicked)
                                    DLine.DrawLine(spriteBatch, new Vector2(b.Position.X + window.Position.X, b.Position.Y + window.Position.Y + b.Size.Y / 2), new Vector2(b.GetAbsolutePosition().X + b.Size.X, b.GetAbsolutePosition().Y + b.Size.Y / 2), new Color(0, 0, 0, 128), b.Size.Y);
                                DLine.DrawLine(spriteBatch, window.Position + b.Position, new Vector2(b.GetAbsolutePosition().X + b.Size.X, b.GetAbsolutePosition().Y), Color.Black, 6);
                                DLine.DrawLine(spriteBatch, new Vector2(b.GetAbsolutePosition().X + b.Size.X - 3, b.GetAbsolutePosition().Y), new Vector2(b.GetAbsolutePosition().X + b.Size.X - 3, b.GetAbsolutePosition().Y + b.Size.Y), Color.Black, 6);
                                DLine.DrawLine(spriteBatch, new Vector2(b.GetAbsolutePosition().X + b.Size.X, b.GetAbsolutePosition().Y + b.Size.Y), new Vector2(b.GetAbsolutePosition().X, b.GetAbsolutePosition().Y + b.Size.Y), Color.Black, 6);
                                DLine.DrawLine(spriteBatch, new Vector2(b.GetAbsolutePosition().X + 3, b.Position.Y + window.Position.Y + b.Size.Y), b.GetAbsolutePosition() + new Vector2(3, 0), Color.Black, 6);
                                Vector2 size = Font12.MeasureString(b.Text);
                                spriteBatch.DrawString(Font12, b.Text, new Vector2(b.GetAbsolutePosition().X + b.Size.X / 2 - size.X / 2, b.GetAbsolutePosition().Y + b.Size.Y / 2 - size.Y / 2), b.Color);
                            }
                            spriteBatch.End();
                            spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle(0, 0, (int)ScreenResolution.X, (int)ScreenResolution.Y);

                        }

                    }
                }
            }               
                //spriteBatch.DrawString(Font, ConsoleText, new Vector2(0, 50), Color.Black);                
                TotalTime += gameTime.ElapsedGameTime.Milliseconds;
                TotalLoops += 1;
                //spriteBatch.End();

                base.Draw(gameTime);
           
        }



        public int GetTotalDevelopment(Country c)
        {
            int output = 0;
            foreach(Province p in Provinces)
            {
                if(p.Control == c)
                {
                    output += p.Development;
                }
            }
            return output;
        }

        public int GetTotalDevelopment(string TAG)
        {
            int output = 0;
            foreach (Province p in Provinces)
            {
                if (p.Control.TAG == TAG)
                {
                    output += p.Development;
                }
            }
            return output;
        }



    }

    public static class DLine
    {
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float thickness = 1f)
        {
            var distance = Vector2.Distance(point1, point2);
            var angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            DrawLine(spriteBatch, point1, distance, angle, color, thickness);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point, float length, float angle, Color color, float thickness = 1f)
        {
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(length, thickness);
            spriteBatch.Draw(GetTexture(spriteBatch), point, null, color, angle, origin, scale, SpriteEffects.None, 0);
        }

        public static Texture2D _texture;
        private static Texture2D GetTexture(SpriteBatch spriteBatch)
        {
            if (_texture == null)
            {
                _texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                _texture.SetData(new[] { Color.White });
            }
            return _texture;
        }
    }    

   

    public class MapRegion
    {
        public string Name = "";
        public string FullName = "";
        public Color Color = new Color(0, 0, 0);
        public Color InvertedColor = new Color(0, 0, 0);
    }

    public class Terrain
    {
        public string Name = "";
        public Color Color = new Color(0, 0, 0);
        public Color InvertedColor = new Color(0, 0, 0);
        public int TravelTime = 0;
    }
  
    
}