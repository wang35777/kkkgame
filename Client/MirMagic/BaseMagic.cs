using Client.MirObjects;
using Client.MirScenes.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Effect = Client.MirObjects.Effect;
using Client.MirGraphics;
using System.Drawing;

namespace Client.MirMagic
{
    public class BaseMagic
    {
        public virtual void OnUseSpell(PlayerObject player, ClientMagic magic)
        {
            UserObject User = (UserObject)player;
            User.NextMagic = magic;
            User.NextMagicLocation = MapControl.MapLocation;
            User.NextMagicObject = MapObject.MouseObject;
            User.NextMagicDirection = MapControl.MouseDirection();
        }

        public virtual void OnMagicBegin(PlayerObject player)
        {

        }

        public virtual void OnDrawEffect(PlayerObject player, MirAction action)
        {

        }
    }
}
