using PenguinGame;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MyBot
{
    public static class GameInfo
    {
        private static Dictionary<int, bool> upgradedThisTurn = new Dictionary<int, bool>(); // int - Iceberg id, bool - true: upgraded
        private static Dictionary<int, bool> attackedIcebergsByUs = new Dictionary<int, bool>();

        public static void InitializeAttckedEnemyIcebergs(Game game)
        {
            foreach (var ice in game.GetEnemyIcebergs())
            {
                attackedIcebergsByUs.Add(ice.UniqueId, false);
            }
            foreach (var ice in game.GetNeutralIcebergs())
            {
                attackedIcebergsByUs.Add(ice.UniqueId, false);
            }
        }

        public static void UpdateAttackedIcebergsByUs(Iceberg enemyIceberg, bool state)
        {
            // should always be true 
            if (attackedIcebergsByUs.ContainsKey(enemyIceberg.UniqueId))
            {
                attackedIcebergsByUs[enemyIceberg.UniqueId] = state;
            }
            else
            {
                System.Console.WriteLine($"attackedIcebergsByUs does not contain {enemyIceberg.UniqueId}");
            }
        }

        public static bool IsAttackedByUs(Iceberg enemyIceberg)
        {
            return attackedIcebergsByUs[enemyIceberg.UniqueId];
        }

        public static void InitializeUpgradeDict(Game game)
        {
            foreach (var myIce in game.GetAllIcebergs())
            {
                upgradedThisTurn.Add(myIce.UniqueId, false);
            }
        }

        public static void UpdateUpgradeDict(int uniqueId)
        {
            // should always be true 
            if (upgradedThisTurn.ContainsKey(uniqueId))
            {
                upgradedThisTurn[uniqueId] = true;
            }
            else
            {
                System.Console.WriteLine($"upgradedThisTurn does not contain {uniqueId}");
            }
        }

        public static bool UpgradedThisTurn(int uniqueId)
        {
            // should always be true but isnt for some reason
            if (upgradedThisTurn.ContainsKey(uniqueId))
            {
                return upgradedThisTurn[uniqueId];
            }
            else
            {
                System.Console.WriteLine($"upgradedThisTurn does not contain {uniqueId}");
            }
            return false;
        }

        public static void EndTurn(Game game)
        {
            foreach (var iceberg in game.GetAllIcebergs())
            {
                upgradedThisTurn[iceberg.UniqueId] = false;
            }
        }
    }
}