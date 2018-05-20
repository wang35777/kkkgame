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
    class LuoHanGunFa : BaseMagic
    {
        public override void OnUseSpell(PlayerObject player, ClientMagic magic)
        {
            GameScene scene = GameScene.Scene;
            if (CMain.Time < scene.ToggleTime) return;
            GameScene.LuoHanGunFa = !GameScene.LuoHanGunFa;
            scene.ChatDialog.ReceiveChat(GameScene.LuoHanGunFa ? "Use LuoHanGunFa." : "Do not use LuoHanGunFa.", ChatType.Hint);
            scene.ToggleTime = CMain.Time + 1000;
            Network.Enqueue(new C.SpellToggle { Spell = magic.Spell, CanUse = GameScene.LuoHanGunFa });
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
            int FrameIndex = player.FrameIndex;
            Point DrawLocation = player.DrawLocation;
            Libraries.Magic4.DrawBlend((int)Direction * 6 + SpellLevel * 90 + FrameIndex, DrawLocation, Color.White, true, 0.7F);
        }
    }
}
