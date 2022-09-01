using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForVyeshal___Monogame
{
    public class Country
    {
        public string TAG = "";
        public string FullName = "";
        public string FactionTAG = "";
        public string FactionFullName = "";
        public Color Color;
        public Color InvertedColor;

        public Province Capital = null;

        /// <summary>
        /// !!!DO NOT USE THIS, IT IS ONLY USED FOR FILE READING!!!
        /// </summary>
        public int CapitalInt = 0;
        /// <summary>
        /// !!!DO NOT USE THIS, IT IS ONLY USED FOR FILE READING!!!
        /// </summary>
        public List<UnitTemplateModifier> UnitTemplatesINITIALIZE = new List<UnitTemplateModifier>();

        public bool Player = false;
        public decimal Gold = 0;
             
        public int MilitaryEquipment = 100;
        public int Coal = 0;

        public List<Unit> Units = new List<Unit>();

        public Dictionary<Country, int> Relations = new Dictionary<Country, int>();

        public decimal GetTotalTradeIncome(TradeGood t)
        {
           decimal total = 0;
           foreach(Province p in Game.Provinces)
            {
                if(p.Control == this && p.Trade == t)
                {
                    total += p.GetTradeIncome();
                }
            }
            return Math.Round(total,2);
        }

        public void CreateUnit(Province p, UnitTemplate t)
        {
            Unit u = new Unit();
            u.Attack = t.Attack;
            u.Defence = t.Defence;
            u.Movement = t.Movement;
            u.Name = t.Name;

            switch (t.Class)
            {
                case "Infantry":
                    u.Name = InfantryUnitsMade + "th Infantry Division of " + FullName;
                    InfantryUnitsMade++;
                    break;
                case "Cavalry":
                    u.Name = InfantryUnitsMade + "th Cavalry Brigade of " + FullName;
                    CavalryUnitsMade++;
                    break;
                case "Artillery":
                    u.Name = InfantryUnitsMade + "th Artillery Group of " + FullName;
                    ArtilleryUnitsMade++;
                    break;

            }

            u.Class = t.Class;

            u.MaxMorale = t.MaxMorale;
            u.Morale = t.MaxMorale;

            u.MaxEnterenchment = t.MaxEntrenchment;

            u.GoldBuildCost = t.GoldBuildCost;
            u.GoldUpKeep = t.GoldUpkeep;
            u.MEBuildCost = t.MEBuildCost;
            u.MEUpkeep = t.MEUpkeep;
            u.HorseBuildCost = t.HorseBuildCost;
            u.HorseUpkeep = t.HorseUpkeep;

            u.NormalTerrain = t.NormalTerrain;
            u.AridTerrain = t.AridTerrain;
            u.RoughTerrain = t.RoughTerrain;
            u.MountainTerrain = t.MountainTerrain;

            u.InfantryBonus = t.InfantryBonus;
            u.CavalryBonus = t.CavalryBonus;
            u.ArtilleryBonus = t.ArtilleryBonus;

            u.MaxSize = t.Size;
            u.Size = t.Size;

            p.Units.Add(u);
        }

        public decimal GetTotalProduction(TradeGood t)
        {
            decimal Total = 0;
            foreach(Province p in Game.Provinces)
            {
                if(p.Control == this && p.Trade == t)
                {
                    Total += p.GetTradeProduction();
                }
            }
            return Math.Round(Total, 2);
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

        public decimal Stability = 100;
        public decimal MaxStability = 100;
        public decimal StabilityGain = 0;
        public decimal LastTurnIncome = 0;
        public decimal LastTurnMilitaryEquipmentProduction = 0;
        public decimal ProductionEf = 0;
        public decimal Support = 100;
        public decimal PartisansEf = 1;
        public decimal MilitaryEquipmentEf = 1;
        public decimal RailroadsCost = 1;
        public decimal BuildingsCost = 1;
        public decimal TaxIncome = 1;
        public decimal RelationsOverTime = 2;
        public decimal DiplomaticReputation = 3;

        public Dictionary<TradeGood, decimal> TradeGoodIncome = new Dictionary<TradeGood, decimal>();
        public Dictionary<TradeGood, decimal> TradeGoodEf = new Dictionary<TradeGood, decimal>();
        public Dictionary<string, UnitTemplate> UnitTemplates = new Dictionary<string, UnitTemplate>();

        public decimal MaxMoraleFlat = 0;
        public decimal MaxMoraleModifier = 1;

        public decimal ArmyExpenses = 1;

        public decimal NormalTerrainModifier = 1;
        public decimal AridTerrainModifier = 1;
        public decimal RoughTerrainModifier = 1;
        public decimal MountainTerrainModifier = 1;

        public int InfantryUnitsMade = 1;
        public int CavalryUnitsMade = 1;
        public int ArtilleryUnitsMade = 1;

        public Country()
        {
            foreach (TradeGood t in Game.TradeGoods.Values)
            {
                TradeGoodIncome.Add(t, 1);
                TradeGoodEf.Add(t, 1);
            }                  
        }
                    
        public void AddModifier(CountryModifier m)
        {
            Modifiers.Add(m);
            foreach (Modifier.Variable variable in m.Variables)
            {
                if(!variable.Name.Contains("TradeGoodEf:") && !variable.Name.Contains("TradeGoodIncome:"))
                {
                    if(this[variable.Name] != null)
                        this[variable.Name] = (decimal)this[variable.Name] + variable.Value;
                }
                else if(variable.Name.Contains("TradeGoodEf:"))
                {
                    string[] n = variable.Name.Split(':');
                    TradeGoodEf[Game.TradeGoods[n[1]]] += variable.Value;
                }
                else if (variable.Name.Contains("TradeGoodIncome:"))
                {
                    string[] n = variable.Name.Split(':');
                    TradeGoodIncome[Game.TradeGoods[n[1]]] += variable.Value;
                }
            }
        }

        public void RemoveModifier(CountryModifier m)
        {
            if (Modifiers.Contains(m))
            {
                foreach (Modifier.Variable variable in m.Variables)
                {
                    if (!variable.Name.Contains("TradeGoodEf:") && !variable.Name.Contains("TradeGoodIncome:"))
                    {
                        if (this[variable.Name] != null)
                            this[variable.Name] = (decimal)this[variable.Name] - variable.Value;
                    }
                    else if (variable.Name.Contains("TradeGoodEf:"))
                    {
                        string[] n = variable.Name.Split(':');
                        TradeGoodEf[Game.TradeGoods[n[1]]] -= variable.Value;
                    }
                    else if (variable.Name.Contains("TradeGoodIncome:"))
                    {
                        string[] n = variable.Name.Split(':');
                        TradeGoodIncome[Game.TradeGoods[n[1]]] -= variable.Value;
                    }
                }
                Modifiers.Remove(m);
            }
        }

        public void AddModifier(UnitTemplateModifier m)
        {
            UnitModifiers.Add(m);
            foreach (Modifier.Variable variable in m.Variables)
            {
                if (m.Units == "a")
                {
                    foreach(UnitTemplate ut in UnitTemplates.Values)
                    {
                        ut[variable.Name] = (decimal)ut[variable.Name] + variable.Value;
                    }
                }
                else if (m.Units.Contains("class:"))
                {
                    foreach(UnitTemplate ut in UnitTemplates.Values)
                    {
                        if(ut.Class == m.Units.Split(':')[1])
                            ut[variable.Name] = (decimal)ut[variable.Name] + variable.Value;
                    }
                }
                else if (m.Units.Contains("unit:"))
                {
                    foreach (UnitTemplate ut in UnitTemplates.Values)
                    {
                        if (ut.Name == m.Units.Split(':')[1])
                            ut[variable.Name] = (decimal)ut[variable.Name] + variable.Value;
                    }
                }
            }
        }

        public void RemoveModifier(UnitTemplateModifier m)
        {
            if (UnitModifiers.Contains(m))
            {
                foreach (Modifier.Variable variable in m.Variables)
                {
                    if (m.Units == "a")
                    {
                        foreach (UnitTemplate ut in UnitTemplates.Values)
                        {
                            ut[variable.Name] = (decimal)ut[variable.Name] - variable.Value;
                        }
                    }
                    else if (m.Units.Contains("class:"))
                    {
                        foreach (UnitTemplate ut in UnitTemplates.Values)
                        {
                            if (ut.Class == m.Units.Split(':')[1])
                                ut[variable.Name] = (decimal)ut[variable.Name] - variable.Value;
                        }
                    }
                    else if (m.Units.Contains("unit:"))
                    {
                        foreach (UnitTemplate ut in UnitTemplates.Values)
                        {
                            if (ut.Name == m.Units.Split(':')[1])
                                ut[variable.Name] = (decimal)ut[variable.Name] - variable.Value;
                        }
                    }
                }
                UnitModifiers.Remove(m);
            }
        }


        public List<CountryModifier> Modifiers = new List<CountryModifier>();
        public List<UnitTemplateModifier> UnitModifiers = new List<UnitTemplateModifier>();

        public class CountryModifier : Modifier
        {           
            public string Character = "";
            public CountryModifier(string title, List<Variable> variables, int duration = -1, string description = "", string character = "") : base(title, variables, duration, description)
            {               
                Title = title;
                Description = description;
                Variables = new List<Variable>(variables);
                Duration = duration;
                Character = character;
            }           
        }
        public class UnitTemplateModifier : Modifier
        {
            public string Units = "";
            public UnitTemplateModifier(string title, List<Variable> variables, string unit = "a", int duration = -1, string description = "") : base(title, variables, duration, description)
            {
                Title = title;
                Description = description;
                Variables = new List<Variable>(variables);
                Duration = duration;
                Units = unit;
            }
        }

        public class UnitTemplate
        {
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

            //base info
            public decimal Attack = 0;            
            public decimal Defence = 0;
            public int Movement = 1;
            public string Name = "";
            public string Class = "";

            //Morale
            public decimal MaxEntrenchment = 0;
            public decimal MaxMorale = 0;

            //Upkeep
            public decimal GoldBuildCost = 0;
            public decimal GoldUpkeep = 0;
            public int MEBuildCost = 0;
            public int MEUpkeep = 0;
            public int HorseBuildCost = 0;
            public int HorseUpkeep = 0;

            //Terrain
            public decimal NormalTerrain = 1;
            public decimal AridTerrain = 1;
            public decimal RoughTerrain = 1;
            public decimal MountainTerrain = 1;

            //Class bonuses
            public decimal InfantryBonus = 1;
            public decimal CavalryBonus = 1;
            public decimal ArtilleryBonus = 1;

            //Size
            public int Size = 200;          
            public string Description = "";
            
        }
    }
}
