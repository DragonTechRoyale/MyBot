using PenguinGame;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MyBot
{
    public static class Defensive
    {
        public static void DefendIcebergs(Game game)
        {
            // TODO: prioritise ices that are about to be conquered 
            var AttackedIces = Utils.GetIcebergssBeingAttacked(game); // TODO: changed to icebergs in actual danger
            // TODO: add priority order
            bool WallsDefenfding = Utils.ShouldUseWalls(game);
            Iceberg[] Walls = Utils.GetWalls(game);

            if (WallsDefenfding && Walls != null)
            {
                AttackedIces = AttackedIces.OrderByDescending(ice => Walls[0].GetTurnsTillArrival(ice.Item1)).ToList();
            }

            foreach (var AttackedIceTuple in AttackedIces)
            {
                Iceberg AttackedIce = AttackedIceTuple.Item1;
                PenguinGroup AttackingGroup = AttackedIceTuple.Item2;

                if (WallsDefenfding && Walls != null)
                {
                    
                    int TotalSendAmount = AttackingGroup.PenguinAmount;
                    int Wall1SendAmount = TotalSendAmount * Utils.WallRatio(game);
                    int Wall0SendAmount = TotalSendAmount - Wall1SendAmount;

                    Walls[0].SendPenguins(AttackedIce, Wall0SendAmount);
                    Walls[1].SendPenguins(AttackedIce, Wall1SendAmount);
                }
                else if (AttackingGroup.PenguinAmount >= AttackedIce.PenguinAmount && !Utils.ShouldUseWalls(game))
                {
                    Utils.GetClosestIceberg(game, AttackedIce, 1).SendPenguins(AttackedIce, AttackingGroup.PenguinAmount + 1);
                }
            }
        }
    }
}