using ComputerInterface;
using ComputerInterface.ViewLib;

namespace MonoSandbox.ComputerInterface
{
    class MonoSandboxView : ComputerView
    {
        public static MonoSandboxView instance;
        private readonly UISelectionHandler selectionHandler;
        //Yes english spelling of colour get over it
        //thank you mono for not making me add a U to a word
        const string mainColour = "383FC7";

        public MonoSandboxView()
        {
            instance = this;

            selectionHandler = new UISelectionHandler(EKeyboardKey.Up, EKeyboardKey.Down, EKeyboardKey.Enter);

            selectionHandler.MaxIdx = 3;

            selectionHandler.OnSelected += OnEntrySelected;

            selectionHandler.ConfigureSelectionIndicator($"<color=#{mainColour}>></color> ", "", "  ", "");
        }

        public override void OnShow(object[] args)
        {
            base.OnShow(args);
            UpdateScreen();
        }

        public void UpdateScreen()
        {
            SetText(str =>
            {
                str.BeginCenter();
                str.MakeBar('-', SCREEN_WIDTH, 0, "e6e6ff10");
                str.AppendClr("Mono Sandbox Mod", mainColour).EndColor().AppendLine();
                str.AppendLine("Created by Monosphere");
                str.MakeBar('-', SCREEN_WIDTH, 0, "e6e6ff10");
                str.EndAlign().AppendLines(1);
                str.AppendClr("  Thruster Strength:", mainColour).EndColor();
                str.AppendLine(selectionHandler.GetIndicatedText(0, Plugin.thrusterManager.multiplier.ToString()));
                str.AppendClr("  Explosion Strength:", mainColour).EndColor();
                str.AppendLine(selectionHandler.GetIndicatedText(1, Plugin.C4Control.multiplier.ToString()));
                str.AppendClr("  Balloon Strength: ", mainColour).EndColor();
                str.AppendLine(selectionHandler.GetIndicatedText(2, Plugin.balloonManager.balloonPower.ToString()));
                str.AppendClr("  Weapon Strength: ", mainColour).EndColor();
                str.AppendLine(selectionHandler.GetIndicatedText(3, Plugin.weaponManager.weaponForce.ToString()));
                str.AppendLines(3);
                str.MakeBar('-', SCREEN_WIDTH, 0, "e6e6ff10");
                str.AppendClr("Special thanks to Walter Bennet", mainColour).EndColor().AppendLine();
                });
        }

        private void OnEntrySelected(int index)
        {
        
        }

        public override void OnKeyPressed(EKeyboardKey key)
        {
            if (selectionHandler.HandleKeypress(key))
            {
                UpdateScreen();
                return;
            }
            
            switch(selectionHandler.CurrentSelectionIndex)
            {
                case 0:
                    switch (key)
                    {
                        case EKeyboardKey.Left:
                            Plugin.thrusterManager.UpdateMultiplier(false);
                            break;
                        case EKeyboardKey.Right:
                            Plugin.thrusterManager.UpdateMultiplier(true);
                            break;
                    }
                    UpdateScreen();
                    break;
                    case 1:
                        switch (key)
                        {

                            case EKeyboardKey.Left:
                            Plugin.C4Control.UpdateMultiplier(false);
                            break;
                            case EKeyboardKey.Right:
                            Plugin.C4Control.UpdateMultiplier(true);
                            break;

                        }
                        UpdateScreen();
                        break;
                case 2:
                    switch (key)
                    {

                        case EKeyboardKey.Left:
                            Plugin.balloonManager.UpdateMultiplier(false);
                            break;
                        case EKeyboardKey.Right:
                            Plugin.balloonManager.UpdateMultiplier(true);
                            break;

                    }
                    UpdateScreen();
                    break;
                case 3:
                    switch (key)
                    {

                        case EKeyboardKey.Left:
                            Plugin.weaponManager.UpdateMultiplier(false);
                            break;
                        case EKeyboardKey.Right:
                            Plugin.weaponManager.UpdateMultiplier(true);
                            break;

                    }
                    UpdateScreen();
                    break;
            }

            switch (key)
            {
                case EKeyboardKey.Back:
                    ReturnToMainMenu();
                    break;
                case EKeyboardKey.Up:
                    selectionHandler.MoveSelectionUp();
                    UpdateScreen();
                    break;
                case EKeyboardKey.Down:
                    selectionHandler.MoveSelectionDown();
                    UpdateScreen();
                    break;
            }
        }
    }
}