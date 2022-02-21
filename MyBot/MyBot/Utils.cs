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

        public static Iceberg[] GetWalls(Game game)
        {
            // returns a two item array with the the walls (the two
            // icebergs with the most penguins)
            if (game.GetMyIcebergs().Length < 2)
            {
                return null;
            }

            Iceberg[] Walls = new Iceberg[2];
            List<Iceberg> MyIces = game.GetMyIcebergs().ToList();

            Walls[0] = StrongestIce(game, MyIces);
            MyIces.Remove(Walls[0]);
            Walls[1] = StrongestIce(game, MyIces);

            return Walls;
        }

        public static Iceberg StrongestIce(Game game, List<Iceberg> Ices)
        {
            // gets: list of icebergs
            // returns: the iceberg with the most penguins
            Iceberg strongest = Ices[0];

            foreach (Iceberg Ice in Ices)
            {
                if (Ice.PenguinAmount > strongest.PenguinAmount)
                {
                    strongest = Ice;
                }
            }

            return strongest;
        }

        public static int WallRatio(Game game)
        {
            // returns: the ratio between the PenguinAmount of the walls
            Iceberg[] Walls = GetWalls(game);
            if (Walls == null || Walls[0].PenguinAmount == 0 || Walls[1].PenguinAmount == 0)
            {
                return 0;
            }
            return Walls[1].PenguinAmount / Walls[0].PenguinAmount;
        }

        public static bool ShouldUseWalls(Game game)
        {
            // if there are natural icebergs and we have less than 3 icebergs
            // there is no need for walls
            if (game.GetNeutralIcebergs().Length == 0 &&
                game.GetMyIcebergs().Length > 2) { return true; }
            return false;
        }

        public static List<Iceberg> AboutToBeConquered(Game game, int which = 0)
        {
            // return a list of icebergs that are about to be conquered
            List<Iceberg> IcesList = game.GetAllIcebergs().ToList();
            List<(Iceberg, List<int>)> AboutToBeConqueredIces = new List<(Iceberg, List<int>)>();
            // Iceberg - the ice that is about to be conquered
            // List<int> - the turns in which it will be conqured  
            int sum = 0; // the sum of penguins in the Ice we checking
            PenguinGroup LastGroup = new PenguinGroup(); /* Refering to the
            loop foreach (Iceberg Ice in IcesList): the last group that will
            arrive at Ice */
            List<PenguinGroup> AllAttackingGroups = new List<PenguinGroup>(); /*
            Refering to the loop foreach (Iceberg Ice in IcesList): all the
            groups which are destined to arrive at Ice */

            switch (which)
            {
                case 1:
                    IcesList = game.GetMyIcebergs().ToList();
                    break;
                case 2:
                    IcesList = game.GetEnemyIcebergs().ToList();
                    break;
                case 3:
                    IcesList = game.GetNeutralIcebergs().ToList();
                    break;
            }

            // TODO: dont sum all at once 
            foreach (Iceberg Ice in IcesList)
            {
                AllAttackingGroups = AllGroupsAttackingIceberg(game, Ice, 2);
                LastGroup = AllAttackingGroups[0];
                sum = Ice.PenguinAmount;

                foreach (PenguinGroup group in AllGroupsAttackingIceberg(game, Ice, 2))
                {
                    if (LastGroup.TurnsTillArrival < group.TurnsTillArrival)
                    {
                        LastGroup = group;
                    }
                    sum -= group.PenguinAmount;
                }
                foreach (PenguinGroup group in AllGroupsAttackingIceberg(game, Ice, 1))
                {
                    if (LastGroup.TurnsTillArrival < group.TurnsTillArrival)
                    {
                        LastGroup = group;
                    }
                    sum += group.PenguinAmount;
                }
                sum += LastGroup.TurnsTillArrival * Ice.Level;

                if (sum <= 0) { AboutToBeConqueredIces.Add(Ice); };
            }

            return AboutToBeConqueredIces;
        }

        public static List<PenguinGroup> AllGroupsAttackingIceberg(Game game, Iceberg ice, int which = 0)
        {
            // Return a list of all (or per which) groups attacking the Iceberg
            // ice. Enemy groups supporting enemy ices or my groups supporting
            // my ices can be included.
            List<PenguinGroup> AttackingGroups = new List<PenguinGroup>();
            List<PenguinGroup> groups = game.GetAllPenguinGroups().ToList();
            
            switch(which)
            {
                case 1:
                    groups = game.GetMyPenguinGroups().ToList();
                    break;
                case 2:
                    groups = game.GetEnemyPenguinGroups().ToList();
                    break;
            }

            foreach(PenguinGroup group in groups)
            {
                if (group.Destination == ice)
                {
                    AttackingGroups.Add(group);
                }
            }

            return AttackingGroups;
        }
    }
}