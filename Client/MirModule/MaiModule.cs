using Client.MirControls;
using Client.MirObjects;
using Client.MirScenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using S = ServerPackets;
using C = ClientPackets;
using Client.MirScenes.Dialogs;

namespace Client.MirModule
{
    public class MailModule
    {
        private UserObject User;

        public bool NewMail;
        public int NewMailCounter = 0;

        public void ProcessPacket(Packet p)
        {
            switch (p.Index)
            {
                case (short)ServerPacketIds.ReceiveMail:
                    ReceiveMail((S.ReceiveMail)p);
                    break;
                case (short)ServerPacketIds.MailLockedItem:
                    MailLockedItem((S.MailLockedItem)p);
                    break;
                case (short)ServerPacketIds.MailSent:
                    MailSent((S.MailSent)p);
                    break;
                case (short)ServerPacketIds.MailSendRequest:
                    MailSendRequest((S.MailSendRequest)p);
                    break;
            }
        }

        private void ReceiveMail(S.ReceiveMail p)
        {
            NewMail = false;
            NewMailCounter = 0;
            User.Mail.Clear();

            User.Mail = p.Mail.OrderByDescending(e => !e.Locked).ThenByDescending(e => e.DateSent).ToList();

            foreach (ClientMail mail in User.Mail)
            {
                foreach (UserItem itm in mail.Items)
                    GameScene.Bind(itm);
            }

            //display new mail received
            if (User.Mail.Any(e => e.Opened == false))
                NewMail = true;

            GameScene.Scene.MailListDialog.UpdateInterface();
        }

        private void MailLockedItem(S.MailLockedItem p)
        {
            InventoryDialog inventory = GameScene.Scene.InventoryDialog;
            MirItemCell cell = inventory.GetCell(p.UniqueID);
            if (cell != null)
                cell.Locked = p.Locked;
        }

        private void MailSendRequest(S.MailSendRequest p)
        {
            MirInputBox inputBox = new MirInputBox("Please enter the name of the person you would like to mail.");

            inputBox.OKButton.Click += (o1, e1) =>
            {
                GameScene.Scene.MailComposeParcelDialog.ComposeMail(inputBox.InputTextBox.Text);
                GameScene.Scene.InventoryDialog.Show();

                //open letter dialog, pass in name
                inputBox.Dispose();
            };

            inputBox.Show();
        }

        private void MailSent(S.MailSent p)
        {
            InventoryDialog inventory = GameScene.Scene.InventoryDialog;
            BeltDialog beltDialog = GameScene.Scene.BeltDialog;
            for (int i = 0; i < inventory.Grid.Length; i++)
            {
                if (inventory.Grid[i].Locked)
                    inventory.Grid[i].Locked = false;
            }

            for (int i = 0; i < beltDialog.Grid.Length; i++)
            {
                if (beltDialog.Grid[i].Locked)
                    beltDialog.Grid[i].Locked = false;
            }

            GameScene.Scene.MailComposeParcelDialog.Hide();
        }
    }
}
