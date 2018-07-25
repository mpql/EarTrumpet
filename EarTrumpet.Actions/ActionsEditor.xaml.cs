﻿using EarTrumpet.UI.Views;
using EarTrumpet_Actions.DataModel.Actions;
using EarTrumpet_Actions.DataModel.Conditions;
using EarTrumpet_Actions.DataModel.Triggers;
using EarTrumpet_Actions.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EarTrumpet_Actions
{
    public partial class ActionsEditor : UserControl
    {
        private readonly ActionsEditorViewModel _viewModel;

        public ActionsEditor()
        {
            InitializeComponent();
            _viewModel = new ActionsEditorViewModel();
            DataContext = _viewModel;
        }

        private void OpenContextMenu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var btn = (Button)sender;
            if (!btn.ContextMenu.IsOpen)
            {
                e.Handled = true;

                var mouseRightClickEvent = new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Right)
                {
                    RoutedEvent = Mouse.MouseUpEvent,
                    Source = sender,
                };
                InputManager.Current.ProcessInput(mouseRightClickEvent);
            }
        }

        private void MenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var item = (MenuItem)sender;
            
            var dt = (PartViewModel)item.DataContext;
            if (dt.Part is BaseTrigger)
            {
                _viewModel.SelectedAction.Triggers.Add(dt);
            }
            else if (dt.Part is BaseCondition)
            {
                _viewModel.SelectedAction.Conditions.Add(dt);
            }
            else if (dt.Part is BaseAction)
            {
                _viewModel.SelectedAction.Actions.Add(dt);
            }
            dt.Part.Loaded();
            _viewModel.SelectedPart = dt;
            
        }

        private void PartView_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var lst = (ListView)sender;
            var item = (PartViewModel)lst.SelectedItem;
            if (item != null)
            {
                item.Part.Loaded();
                _viewModel.SelectedPart = item;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            if (btn.DataContext is PartViewModel)
            {
                var dt = (PartViewModel)btn.DataContext;
                if (dt.Part is BaseTrigger)
                {
                    _viewModel.SelectedAction.Triggers.Remove(dt);
                }
                else if (dt.Part is BaseCondition)
                {
                    _viewModel.SelectedAction.Conditions.Remove(dt);
                }
                else if (dt.Part is BaseAction)
                {
                    _viewModel.SelectedAction.Actions.Remove(dt);
                }
            }
            else
            {
                var actionViewModel = (EarTrumpetActionViewModel)btn.DataContext;
                _viewModel.EarTrumpetActions.Remove(actionViewModel);
            }
        }

        private void HotkeySelect_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var dt = (HotkeyTrigger)btn.DataContext;

            var win = new HotkeySelectionWindow();
            win.Owner = Window.GetWindow(this);
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if ((bool)win.ShowDialog())
            {
                dt.Hotkey = win.Hotkey;
            }
        }
    }
}
