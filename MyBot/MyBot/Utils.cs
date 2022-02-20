using PenguinGame;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MyBot
{
    public static class Utils
    {
        public static PenguinGroup IsAttackingIceberg(Game game, Iceberg ice)
        {
            // Returns null if there are no attack on the iceberg ice, return the 
            // group attacking our iceberg otherise
            var enemyGroups = game.GetEnemyPenguinGroups();
            foreach (var group in enemyGroups)
            {
                if (group.Destination == ice)
                {
                    return group;
                }
            }
            return null;
        }


        public static List<(Iceberg, PenguinGroup)> GetIcebergssBeingAttacked(Game game)
        {
            // Returns all of our icebegrs that are being attacked
            List<(Iceberg, PenguinGroup)> AttackedIces = new List<(Iceberg, PenguinGroup)>();
            var myIces = game.GetMyIcebergs();

            foreach (var myIce in myIces)
            {
                PenguinGroup AttackingGroup = IsAttackingIceberg(game, myIce);
                if (AttackingGroup != null)
                {
                    AttackedIces.Add((myIce, AttackingGroup));
                }
            }

            return AttackedIces;
        }

        public static Iceberg GetClosestIceberg(Game game, Iceberg MyIce, int which = 0)
        {
            // Gets: an iceberg
            // Returns: the cloests Iceberg to MyIce (not in actual 
            // distance but with a calculation of the speed in which penguins 
            // will arrive from MyIce to closestIce)
            List<Iceberg> Ices = game.GetAllIcebergs().ToList();
            Iceberg closestIce = null;
            if (which == 1)
            {
                Ices = game.GetMyIcebergs().ToList();
            }
            else if (which == 2)
            {
                Ices = game.GetEnemyIcebergs().ToList();
            }
            else if (which == 3)
            {
                Ices = game.GetNeutralIcebergs().ToList();
            }
            closestIce = Ices[0];
            foreach (var ice in Ices)
            {
                if (ice != MyIce && ice.GetTurnsTillArrival(MyIce) < closestIce.GetTurnsTillArrival(MyIce))
                {
                    closestIce = ice;
                }
            }

            return closestIce;
        }

        public static int AverageDistanceFromEnemy(Game game, Iceberg MyIce)
        {
            // gets: Iceberg MyIce
            // returns: the average distance from it to the enemy icebergs
            int enemyIceAmount = game.GetEnemyIcebergs().Length;
            int enemyIceSum = 0;
            foreach (var EnemyIce in game.GetEnemyIcebergs())
            {
                if (EnemyIce != MyIce)
                {
                    enemyIceSum += MyIce.GetTurnsTillArrival(EnemyIce);
                }
            }
            return enemyIceSum / enemyIceAmount;
        }
    }
}