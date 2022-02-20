using PenguinGame;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MyBot
{
    public static class Offensive
    {
        public static void Attack(Game game)
        {
            foreach (var myIceberg in game.GetMyIcebergs())
            {
                int myPenguinAmount = myIceberg.PenguinAmount;
                int targetPenguinAmount = 0;
                List<Iceberg> targets = new List<Iceberg>();
                bool upgraded = GameInfo.UpgradedThisTurn(myIceberg.Id);

                if (game.GetNeutralIcebergs().Length > 0)
                {
                    targets.AddRange(game.GetNeutralIcebergs().ToList());
                }
                if (game.GetEnemyIcebergs().Length > 0)
                {
                    targets.AddRange(game.GetEnemyIcebergs().ToList());
                }

                foreach (Iceberg targetIce in targets)
                {
                    targetPenguinAmount = targetIce.PenguinAmount;
                    if (targetIce.Owner.Id == game.GetEnemy().Id)
                    {
                        // enemy 
                        targetPenguinAmount = targetPenguinAmount + targetIce.PenguinsPerTurn * myIceberg.GetTurnsTillArrival(targetIce);
                    }
                    if (myPenguinAmount > targetPenguinAmount && !upgraded)
                    {
                        // Send penguins to the target.
                        // In order to take control of the iceberg we need to send one penguin more than is currently in the iceberg.
                        System.Console.WriteLine(myIceberg + " sends " + (targetPenguinAmount + 1) + " penguins to " + targetIce);
                        myIceberg.SendPenguins(targetIce, targetPenguinAmount + 1);
                    }
                }

                if (!upgraded && myIceberg.CanUpgrade())
                {
                    myIceberg.Upgrade();
                    GameInfo.UpdateUpgradeDict(myIceberg.Id);
                }
            }
        }
    }
}