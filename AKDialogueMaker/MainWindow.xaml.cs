/*
Copyright (c) 2021 Eperty123

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE
OR OTHER DEALINGS IN THE SOFTWARE.
*/
using AKDialogueMaker.Utilities;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AKDialogueMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private, Protected Variables
        PreviewWindow PreviewWindow;
        #endregion

        #region Public Variables
        public Action<string> OnInputTextChanged;
        public Action<string> OnPlayerNameTextChanged;

        #endregion

        #region Constructors
        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }
        #endregion

        #region Private, Protected Methods
        private void Initialize()
        {
            Title = Application.ResourceAssembly.GetName().Name;
            ShowPreviewWindow();
        }

        private void ShowPreviewWindow()
        {
            if (PreviewWindow == null)
            {
                PreviewWindow = new PreviewWindow(this);
                PreviewWindow.Show();

                PreviewWindow.Closing += (o, e) =>
                {
                    PreviewWindow = null;
                };
            }
            else PreviewWindow.Focus();
        }

        private void UpdateText()
        {
            OnInputTextChanged?.Invoke(GetInputText());
        }

        private void PreviewBtn_Click(object sender, RoutedEventArgs e)
        {
            ShowPreviewWindow();
        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateText();
        }

        private void CopyToClipboardBtn_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(GetInputText());
        }

        private void PlayerNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnPlayerNameTextChanged?.Invoke(PlayerNameTextBox.Text);
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            InputTextBox.SetText("", Brushes.White);
        }

        #endregion

        #region Public Methods
        public string GetInputText()
        {
            return InputTextBox.GetText();
        }

        public string GetPlayerName()
        {
            return PlayerNameTextBox.Text;
        }
        #endregion
    }
}
