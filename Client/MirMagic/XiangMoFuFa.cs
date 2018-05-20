using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.MirObjects;
using Client.MirScenes;
using Client.MirNetwork;

using S = ServerPackets;
using C = ClientPackets;
using System.Drawing;
using Client.MirGraphics;

namespace Client.MirMagic
{
    // 仿烈火
    class XiangMoFuFa : BaseMagic
    {
        public override void OnUseSpell(PlayerObject player, ClientMagic magic)
        {
            GameScene scene = GameScene.Scene;

            if (CMain.Time < scene.ToggleTime) return;
            scene.ToggleTime = CMain.Time + 500;

            int cost = magic.Level * magic.LevelCost + magic.BaseCost;
            if (cost > MapObject.User.MP)
            {
                scene.OutputMessageTr("Not Enough Mana to cast.");
                return;
            }
            Network.Enqueue(new C.SpellToggle { Spell = magic.Spell, CanUse = true });
        }

        public override void OnMagicBegin(PlayerObject player)
        {
        }

        public override void OnDrawEffect(PlayerObject player, MirAction action)
        {
            if (action != MirAction.Attack1)
                return;

            MirDirection Direction = player.Direction;
            int SpellLevel = player.SpellLevel;
            int FrameIndex = player.FrameIndex * 9 / 6;
            Point DrawLocation = player.DrawLocation;

            Libraries.Magic4.DrawBlend(292 + (int)Direction * 16 + FrameIndex, DrawLocation, Color.White, true, 0.7F);
        }
    }
}
