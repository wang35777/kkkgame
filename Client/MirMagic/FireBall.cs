using Client.MirGraphics;
using Client.MirObjects;
using Client.MirSounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.MirMagic
{
    class FireBall : BaseMagic
    {
        public override void OnMagicBegin(PlayerObject player)
        {
            player.Effects.Add(new Effect(Libraries.Magic, 0, 10, player.Frame.Count * player.FrameInterval, player));
            SoundManager.PlaySound(20000 + (ushort)player.Spell * 10);
        }
    }
}
