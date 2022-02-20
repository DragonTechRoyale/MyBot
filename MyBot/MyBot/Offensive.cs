using PenguinGame;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MyBot
{
    public static class Offensive
    {
        public static void CreateWall(Game game)
        {
            if (Utils.GetWalls(game) ==  null) { return; }
            Iceberg[] Wall = new Iceberg[2];
            Wall[0] = game.GetMyIcebergs().OrderBy(ice => Utils.AverageDistanceFromEnemy(game, ice)).ToList()[0];
            Wall[1] = game.GetMyIcebergs().OrderBy(ice => Utils.AverageDistanceFromEnemy(game, ice)).ToList()[1];
            Iceberg WeakerWall = null;

            foreach (Iceberg myIce in game.GetMyIcebergs())
            {
                if (myIce.Level != myIce.UpgradeLevelLimit && myIce.CanUpgrade())
                {
                    myIce.Upgrade();
                    GameInfo.UpdateUpgradeDict(myIce.Id);
                }
                else if (!(myIce == Wall[0]) || !(myIce == Wall[1]))
                {
                    if (Wall[0].PenguinAmount > Wall[1].PenguinAmount)
                    {
                        WeakerWall = Wall[1];
                    }
                    else
                    {
                        WeakerWall = Wall[0];
                    }
                    if (WeakerWall != null)
                    {
                        myIce.SendPenguins(WeakerWall, myIce.PenguinAmount);
                    }
                    else
                    {
                        System.Console.WriteLine("wtf");
                    }
                }
            }
        }
        public static void WallAttack(Game game)
        {
            Iceberg[] Walls = Utils.GetWalls(game);
            if (game.GetNeutralIcebergs().Length != 0)
            {
                foreach (var naturalIce in game.GetNeutralIcebergs())
                {
                    int TotalSendAmount = naturalIce.PenguinAmount + 1;
                    int Wall1SendAmount = TotalSendAmount * Utils.WallRatio(game);
                    int Wall0SendAmount = TotalSendAmount - Wall1SendAmount;

                    Walls[0].SendPenguins(naturalIce, Wall0SendAmount);
                    Walls[1].SendPenguins(naturalIce, Wall1SendAmount);

                }
            }
            else
            {
                foreach (var EnemyIce in game.GetEnemyIcebergs())
                {
                    int TotalSendAmount = EnemyIce.PenguinAmount + 1;
                    int Wall1SendAmount = TotalSendAmount * Utils.WallRatio(game);
                    int Wall0SendAmount = TotalSendAmount - Wall1SendAmount;

                    Walls[0].SendPenguins(EnemyIce, Wall0SendAmount);
                    Walls[1].SendPenguins(EnemyIce, Wall1SendAmount);
                }
            }
        }

        public static void Attack(Game game)
        {
            if (Utils.GetWalls(game) != null)
            {
                WallAttack(game);
            }
            else
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
                    if (game.GetEnemyIcebergs().Length > 0 && game.GetNeutralIcebergs().Length == 0)
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
                            //System.Console.WriteLine(myIceberg + " sends " + (targetPenguinAmount + 1) + " penguins to " + targetIce);
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
}