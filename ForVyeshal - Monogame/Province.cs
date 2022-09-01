using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForVyeshal___Monogame
{
    public class Province
    {
        public int ID = 0;
        public string Name = "";
        public TradeGood Trade;
        public Terrain Terrain;
        public MapRegion Region = null;
        public int Development = 0;
        public Country Control = null;
        public Color Color;
        public List<Point> Pixels = new List<Point>();
        public Texture2D Texture;
        public Texture2D SelectedTexture;
        public Vector2 DrawPosition = new Vector2(5000, 3000);
        public Vector2 Size = new Vector2(0, 0);
        public Color InvertedColor;
        public List<Province> Bordering = new List<Province>();
        public Dictionary<Province, int> Distance = new Dictionary<Province, int>();
        public Vector2 TownPosition = new Vector2();
        public Dictionary<Province, int> Railways = new Dictionary<Province, int>();
        public Dictionary<Province,Pathway> Pathfinding = new Dictionary<Province, Pathway>();
        public Vector2 ProvinceCapital = new Vector2();
        public Vector2 UnitPosition = new Vector2();

        public List<Unit> Units = new List<Unit>();

        public Pathway GetPathway(Province p, bool ovrr = false)
        {
            if (!ovrr)
            {
                while (Game.Pathfinding.Any(x => x))
                {

                }
            }
            return Pathfinding[p];
        }

        public object this[string propertyName]
        {
            get
            {
                if (GetType().GetField(propertyName) != null)
                    return GetType().GetField(propertyName).GetValue(this);
                else
                {
                    ConsoleClass.HandleConsole("\nUnknown variable: " + propertyName, true);
                    return null;
                }
            }
            set
            {
                if (GetType().GetField(propertyName) != null)
                    this.GetType().GetField(propertyName).SetValue(this, value);
                else
                    ConsoleClass.HandleConsole("\nUnknown variable: " + propertyName, true);
            }
        }



        public decimal TaxIncome = 1;
        public decimal ProductionEf = 1;
        public decimal MilitaryEquipmentEf = 1;
        public decimal BuildingsCost = 1;
        public decimal RailroadCost = 1;
        public decimal LocalSupport = 100;
        public decimal DefenderBonus = 1;
        public decimal AttackerBonus = 1;
        

        public Province()
        {
               
        }

        public int GetMilitaryEquipmentIncome()
        {
            int Base = Development / 2;         
            decimal Multiplyer = 1;
            Multiplyer += MilitaryEquipmentEf - 1;
            Multiplyer += Control.MilitaryEquipmentEf - 1;
            return (int)Math.Ceiling(Base * Multiplyer);            
        }

        public decimal GetTaxIncome()
        {
            return Math.Round((Development / 20m) * (TaxIncome + Control.TaxIncome - 1), 2) + 0.2m;
        }
        public decimal GetTradeProduction()
        {
            decimal TotalMultiplyer = 1;
            TotalMultiplyer += ProductionEf - 1;
            TotalMultiplyer += Control.ProductionEf - 1;
            TotalMultiplyer += Control.TradeGoodEf[Trade] - 1;
            

            return Math.Round(((Development / 100m) + 0.05m) * TotalMultiplyer, 2);
        }
        public decimal GetTradeIncome()
        {
            decimal Mp = 1;
            decimal Production = GetTradeProduction();
            Mp += Control.TradeGoodIncome[Trade] - 1;
            
            return Math.Round(Production * Trade.GetPrice() * Mp, 2);
        }

       

        public void AddModifier(Modifier m)
        {
            Modifiers.Add(m);
            foreach (Modifier.Variable variable in m.Variables)
            {
                if(this[variable.Name] != null)
                    this[variable.Name] = (decimal)this[variable.Name] + variable.Value;
                else
                    ConsoleClass.HandleConsole("\nUnknown variable: " + variable.Name, true);
            }
        }

        public void RemoveModifier(Modifier m)
        {
            foreach (Modifier.Variable variable in m.Variables)
            {
                this[variable.Name] = (decimal)this[variable.Name] - variable.Value;
            }
            Modifiers.Remove(m);
        }

        public List<Modifier> Modifiers = new List<Modifier>();
        
        public class Pathway
        {
            public int TotalTime = 0;
            public List<Province> Path = new List <Province> { };
            public Province Destination = null;
            public Pathway(Province dest, List<Province> pat, int tim)
            {
                TotalTime = tim;
                Path = pat;
                Destination = dest;
            }
        }

    }
}
