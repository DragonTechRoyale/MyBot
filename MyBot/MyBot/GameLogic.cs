using PenguinGame;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MyBot
{
    public static class GameLogic
    {
        public static void Execute(Game game)
        {
            if (game.Turn == 1)
            {
                game.GetMyIcebergs()[0].Upgrade();
            }
            else if (game.Turn == 7)
            {
                Defensive.DefendIcebergs(game);
                var TargetIce = game.GetNeutralIcebergs()[0];
                game.GetMyIcebergs()[0].SendPenguins(TargetIce, TargetIce.PenguinAmount + 1);
            }
            else if (game.Turn == 12)
            {
                Defensive.DefendIcebergs(game);
                var TargetIce = game.GetNeutralIcebergs()[1];
                game.GetMyIcebergs()[0].SendPenguins(TargetIce, TargetIce.PenguinAmount + 1);
            }
            else if (game.Turn >= 19)
            {
                Defensive.DefendIcebergs(game);
                foreach (var myIceberg in game.GetMyIcebergs())
                {
                    int myPenguinAmount = myIceberg.PenguinAmount;
                    int targetPenguinAmount = 0;
                    List<Iceberg> targets = new List<Iceberg>();

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
                        if (myPenguinAmount > targetPenguinAmount)
                        {
                            // Send penguins to the target.
                            // In order to take control of the iceberg we need to send one penguin more than is currently in the iceberg.
                            System.Console.WriteLine(myIceberg + " sends " + (targetPenguinAmount + 1) + " penguins to " + targetIce);
                            myIceberg.SendPenguins(targetIce, targetPenguinAmount + 1);
                        }
                    }
                }
            }
        }
    }
}