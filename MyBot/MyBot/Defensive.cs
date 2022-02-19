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
            var AttackedIces = Utils.GetIcebergssBeingAttacked(game); // TODO: changed to icebergs in actual danger
            // TODO: add priority order
            foreach (var AttackedIceTuple in AttackedIces)
            {
                Iceberg AttackedIce = AttackedIceTuple.Item1;
                PenguinGroup AttackingGroup = AttackedIceTuple.Item2;
                if (AttackingGroup.PenguinAmount >= AttackedIce.PenguinAmount)
                {
                    Utils.GetClosestIceberg(game, AttackedIce).SendPenguins(AttackedIce, AttackingGroup.PenguinAmount + 1);
                }
            }
        }
    }
}