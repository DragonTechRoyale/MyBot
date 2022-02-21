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
                GameInfo.InitializeUpgradeDict(game);
                GameInfo.InitializeAttckedEnemyIcebergs(game);
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
                if (Utils.ShouldUseWalls(game)) { Offensive.CreateWall(game); }
                Offensive.Attack(game);
            }
            GameInfo.EndTurn(game);
        }
    }
}