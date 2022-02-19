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

        public static Iceberg GetClosestIceberg(Game game, Iceberg MyIce)
        {
            // Gets: an iceberg
            // Returns: the cloests Iceberg we have to MyIce (not in actual 
            // distance but with a calculation of the speed in which penguins 
            // will arrive from MyIce to closestIce)
            Iceberg closestIce = game.GetMyIcebergs()[0];
            foreach (var ice in game.GetMyIcebergs())
            {
                if (ice != MyIce && ice.GetTurnsTillArrival(MyIce) < closestIce.GetTurnsTillArrival(MyIce))
                {
                    closestIce = ice;
                }
            }

            return closestIce;
        }
    }
}
