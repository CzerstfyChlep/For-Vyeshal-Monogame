using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForVyeshal___Monogame
{
    public class Unit
    {
        //base info
        public decimal Attack = 0;
        public decimal Defence = 0;
        public int Movement = 0;
        public string Name = "";
        public string Class = "";

        //Morale
        public decimal Morale = 0;
        public decimal Enternchment = 0;

        public decimal MaxEnterenchment = 0;
        public decimal MaxMorale = 0;

        //Upkeep
        public decimal GoldBuildCost = 0;
        public decimal GoldUpKeep = 0;
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
        public int MaxSize = 0;
        public int Size = 0;
    }

    public class Battle
    {
        public List<Unit> Attacker = new List<Unit>();

        public List<Unit> Defender = new List<Unit>();

        public void BattleTick()
        {
            Random r = new Random();
            int attackerroll = r.Next(0, 3);
            if(attackerroll == 2)
            {

            }
            else
            {
                float AttackerInfantryDamage = 0, AttackerCavalryDamage = 0, AttackerArtilleryDamage = 0;
                float DefenderInfantryDamage = 0, DefenderCavalryDamage = 0, DefenderArtilleryDamage = 0;

                int AttackerGeneralRoll = r.Next(1, 10);
                int DefenderGeneralRoll = r.Next(1, 10);



                foreach(Unit u in Attacker)
                {
                    float damage = 0;
                    damage = (float)u.Attack * ((float)u.Size / (float)u.MaxSize) * AttackerGeneralRoll * r.Next(1, 7);
                    switch (u.Class)
                    {
                        case "Infantry":
                            AttackerInfantryDamage += damage;
                            break;
                        case "Cavalry":
                            AttackerCavalryDamage += damage;
                            break;
                        case "Artillery":
                            AttackerArtilleryDamage += damage;
                            break;
                    }
                }


            }
        }
    }
    

}
