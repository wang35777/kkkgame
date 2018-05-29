using Client.MirControls;
using Client.MirGraphics;
using Client.MirSounds;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.MirScenes.Dialogs
{
    public sealed class AssistDialog : MirImageControl
    {
        public MirButton CloseButton;
        public MirCheckBox checkBoxSmartAttack, checkBoxSmartFire, checkBoxAutoEat, checkBoxSmartSheild, checkBoxAutoPick, checkBoxChangePoison;
        public MirTextBox textBoxPercentHpProtect, textBoxPercentMpProtect;
        public MirTextBox textBoxEatItemName, textBoxEatMpName;

        public AssistDialog()
        {
            Index = 309;
            Library = Libraries.Prguse;

            Movable = true;
            Sort = true;
            Location = Center;

            checkBoxSmartAttack = new MirCheckBox { Index = 2086, UnTickedIndex = 2086, TickedIndex = 2087, Parent = this, Location = new Point(301, 69), Library = Libraries.Prguse };
            //checkBoxSmartAttack.Checked = Settings.smart
            checkBoxSmartAttack.LabelText = "刀刀刺杀";
            checkBoxSmartAttack.Click += CheckBoxSmartAttack;

            checkBoxSmartFire = new MirCheckBox { Index = 2086, UnTickedIndex = 2086, TickedIndex = 2087, Parent = this, Location = new Point(301, 94), Library = Libraries.Prguse };
            checkBoxSmartFire.LabelText = "自动烈火";
            checkBoxSmartFire.Checked = Settings.smartFireHit;
            checkBoxSmartFire.Click += CheckBoxFireClick;

            checkBoxAutoEat = new MirCheckBox { Index = 2086, UnTickedIndex = 2086, TickedIndex = 2087, Parent = this, Location = new Point(151, 69), Library = Libraries.Prguse };
            checkBoxAutoEat.LabelText = "自动吃药";
            checkBoxAutoEat.Checked = Settings.smartFireHit;
            checkBoxAutoEat.Click += CheckBoxAutoEatClick;

            checkBoxSmartSheild = new MirCheckBox { Index = 2086, UnTickedIndex = 2086, TickedIndex = 2087, Parent = this, Location = new Point(301, 119), Library = Libraries.Prguse };
            checkBoxSmartSheild.LabelText = "自动开盾";
            checkBoxSmartSheild.Checked = Settings.smartSheild;
            checkBoxSmartSheild.Click += CheckBoxSheildClick;

           checkBoxChangePoison = new MirCheckBox
            {
                Index = 2086, UnTickedIndex = 2086, TickedIndex = 2087,
                Parent = this, Location = new Point(301, 143), Library = Libraries.Prguse
            };
            checkBoxChangePoison.LabelText = "自动换毒";
            checkBoxChangePoison.Checked = Settings.smartChangePoison;
            checkBoxChangePoison.Click += CheckBoxChangePoisonClick;


            checkBoxAutoPick = new MirCheckBox {
                Index = 2086, UnTickedIndex = 2086, TickedIndex = 2087,
                Parent = this, Location = new Point(26, 69), Library = Libraries.Prguse };
            checkBoxAutoPick.LabelText = "自动拾取";
            checkBoxAutoPick.Checked = Settings.autoPick;
            checkBoxAutoPick.Click += CheckBoxAutoPick;

            textBoxPercentHpProtect = new MirTextBox {
                Location = new Point(238, 95),
                Parent = this,
                Size = new Size(45, 15),
                MaxLength = Globals.MaxPasswordLength,
                OnlyNumber = true,
                CanLoseFocus = true,
                FocusWhenVisible = false,
                Font = new Font(Settings.FontName, 8F)
            };
            textBoxPercentHpProtect.TextBox.TextChanged += percentHpTextBox_changed;
            textBoxPercentHpProtect.Text = String.Format("{0}", Settings.percentHpProtect);

            textBoxEatItemName = new MirTextBox {
                Location = new Point(189, 119),
                Parent = this,
                Size = new Size(93, 15),
                MaxLength = Globals.MaxPasswordLength,
                CanLoseFocus = true,
                FocusWhenVisible = false,
                Font = new Font(Settings.FontName, 8F)
            };
            textBoxEatItemName.TextBox.TextChanged += eatHpItemTextBox_changed;
            textBoxEatItemName.Text = Settings.hpItemName;

            textBoxPercentMpProtect = new MirTextBox { Location = new Point(238, 143),
                Parent = this, Size = new Size(40, 15),
                MaxLength = Globals.MaxPasswordLength, OnlyNumber = true,
                CanLoseFocus = true, FocusWhenVisible= false,
                Font = new Font(Settings.FontName, 8F)
            };

            textBoxPercentMpProtect.TextBox.TextChanged += percentMpTextBox_changed;
            textBoxPercentMpProtect.Text = String.Format("{0}", Settings.percentHpProtect);

            textBoxEatMpName = new MirTextBox {
                Location = new Point(189, 168), Parent = this,
                Size = new Size(93, 15), MaxLength = Globals.MaxPasswordLength,
                CanLoseFocus = true, FocusWhenVisible = false,
                Font = new Font(Settings.FontName, 8F)
            };
            textBoxEatMpName.TextBox.TextChanged += eatMpItemTextBox_changed;
            textBoxEatMpName.Text = Settings.mpItemName;

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(415, 4),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();
        }

        private void CheckBoxChangePoisonClick(object sender, EventArgs e)
        {
            Settings.smartChangePoison = checkBoxChangePoison.Checked;
        }

        private void CheckBoxAutoPick(object sender, EventArgs e)
        {
            Settings.autoPick = checkBoxAutoPick.Checked;
        }

        private void eatMpItemTextBox_changed(object sender, EventArgs e)
        {
            Settings.mpItemName = textBoxEatMpName.Text;
        }

        private void percentMpTextBox_changed(object sender, EventArgs e)
        {
              int.TryParse(textBoxPercentMpProtect.Text, out Settings.percentMpProtect);
        }

        private void CheckBoxSheildClick(object sender, EventArgs e)
        {
            Settings.smartSheild = checkBoxSmartSheild.Checked;
        }

        private void eatHpItemTextBox_changed(object sender, EventArgs e)
        {
            Settings.hpItemName = textBoxEatItemName.Text;
        }

        private void percentHpTextBox_changed(object sender, EventArgs e)
        {
            int.TryParse(textBoxPercentHpProtect.Text, out Settings.percentHpProtect); 
        }

        private void CheckBoxAutoEatClick(object sender, EventArgs e)
        {
            Settings.autoEatItem = checkBoxAutoEat.Checked;
        }

        private void CheckBoxFireClick(object sender, EventArgs e)
        {
            Settings.smartFireHit = checkBoxSmartFire.Checked;
        }

        private void CheckBoxSmartAttack(object sender, EventArgs e)
        {
          //  Settings.smartSheild = checkBoxSmartSheild.Checked;
        }

        public void Hide()
        {
            if (!Visible) return;
            Visible = false;
        }

        public void Show()
        {
            if (Visible) return;
            Visible = true;
        }
    }
}
