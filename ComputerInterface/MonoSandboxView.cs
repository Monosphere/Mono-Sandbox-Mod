using System;
using System.Collections.Generic;
using Utilla;
using ComputerInterface;
using ComputerInterface.ViewLib;
using Photon.Pun;
using UnityEngine;
using MonoSandbox;
namespace MonoSandbox.ComputerInterface
{
    class MonoSandboxView : ComputerView
    {
        public static MonoSandboxView instance;
        private readonly UISelectionHandler selectionHandler;
        //Yes english spelling of colour get over it
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
                str.AppendLine(selectionHandler.GetIndicatedText(0, MonoSandbox.Plugin.thrusterManager.multiplier.ToString()));
                str.AppendClr("  Explosion Strength:", mainColour).EndColor();
                str.AppendLine(selectionHandler.GetIndicatedText(1, MonoSandbox.Plugin.C4Control.multiplier.ToString()));
                str.AppendClr("  Balloon Strength: ", mainColour).EndColor();
                str.AppendLine(selectionHandler.GetIndicatedText(2, MonoSandbox.Plugin.balloonManager.balloonPower.ToString()));
                str.AppendClr("  Weapon Strength: ", mainColour).EndColor();
                str.AppendLine(selectionHandler.GetIndicatedText(3, MonoSandbox.Plugin.weaponManager.weaponForce.ToString()));
                str.AppendLines(3);
                str.MakeBar('-', SCREEN_WIDTH, 0, "e6e6ff10");
                str.AppendClr("Special thanks to Walter Bennet", mainColour).EndColor().AppendLine();
                });
        }

        private void OnEntrySelected(int index)
        {
            /*
            try
            {
                switch (index)
                {
                    case 0:
                        if (MBConfig.Modded)
                            BazookaManager.Instance.UpdateEnabled();
                        UpdateScreen();
                        break;
                    case 1:
                        if (MBConfig.Modded)
                            BazookaManager.Instance.UpdateLeft();
                        UpdateScreen();
                        break;
                }
            }
            
            catch (Exception e) { Console.WriteLine(e); }
            */
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
                            MonoSandbox.Plugin.thrusterManager.UpdateMultiplier(false);
                            break;
                        case EKeyboardKey.Right:
                            MonoSandbox.Plugin.thrusterManager.UpdateMultiplier(true);
                            break;
                    }
                    UpdateScreen();
                    break;
                    case 1:
                        switch (key)
                        {

                            case EKeyboardKey.Left:
                            MonoSandbox.Plugin.C4Control.UpdateMultiplier(false);
                            break;
                            case EKeyboardKey.Right:
                            MonoSandbox.Plugin.C4Control.UpdateMultiplier(true);
                            break;

                        }
                        UpdateScreen();
                        break;
                case 2:
                    switch (key)
                    {

                        case EKeyboardKey.Left:
                            MonoSandbox.Plugin.balloonManager.UpdateMultiplier(false);
                            break;
                        case EKeyboardKey.Right:
                            MonoSandbox.Plugin.balloonManager.UpdateMultiplier(true);
                            break;

                    }
                    UpdateScreen();
                    break;
                case 3:
                    switch (key)
                    {

                        case EKeyboardKey.Left:
                            MonoSandbox.Plugin.weaponManager.UpdateMultiplier(false);
                            break;
                        case EKeyboardKey.Right:
                            MonoSandbox.Plugin.weaponManager.UpdateMultiplier(true);
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