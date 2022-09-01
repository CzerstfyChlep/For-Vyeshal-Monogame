using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForVyeshal___Monogame
{
    public class TradeGood
    {
        public string Name = "";
        decimal BasePrice = 0;
        decimal Multiplier = 1;
        public Color Color = new Color(0, 0, 0);
        public Color InvertedColor = new Color(255, 255, 255);
        List<TradeGoodModifier> Modifiers = new List<TradeGoodModifier>();
        public Modifier ProvinceModifier = new Modifier("Tradegood modifier", new List<Modifier.Variable>());
        public Modifier CountryModifier = new Modifier("Tradegood modifier", new List<Modifier.Variable>());

        public decimal GetPrice()
        {
            return BasePrice * Multiplier;
        }
        public void SetPrice(decimal Value)
        {
            BasePrice = Value;
        }
        public decimal GetTotalProduction()
        {
            decimal v = 0;
            foreach(Province p in Game.Provinces)
            {
                if(p.Trade == this)
                    v += p.GetTradeProduction(); 
            }
            return (decimal)Math.Round(v, 2);

        }

        public class TradeGoodModifier : Modifier
        {
            public decimal Value = 0;
            public bool Percentage = false;
            public TradeGoodModifier(string title, decimal value, int duration = -1, bool percentage = false, string description = "") : base(title, new List<Variable> { new Variable(title, value) }, duration, description)
            {
                Title = title;
                Description = description;
                Value = value;
                Duration = duration;
                Percentage = percentage;
            }
        }
        public void AddModifier(TradeGoodModifier m)
        {
            Modifiers.Add(m);
            if (!m.Percentage)
            {
                BasePrice += m.Value;
            }
            else
            {
                Multiplier += m.Value;
            }
        }
        public void RemoveModifier(TradeGoodModifier m)
        {
            if (!m.Percentage)
            {
                BasePrice -= m.Value;
            }
            else
            {
                Multiplier -= m.Value;
            }
            Modifiers.Remove(m);
        }

    }
}
