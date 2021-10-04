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
using AKDialogueMaker.Models;
using AKDialogueMaker.Utilities;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace AKDialogueMaker
{
    /// <summary>
    /// Interaction logic for PreviewWindow.xaml
    /// </summary>
    public partial class PreviewWindow : Window
    {
        #region Private, Protected Variables
        MainWindow _MainWindow;
        string _PlayerName;

        #endregion

        #region Constructors

        public PreviewWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _MainWindow = mainWindow;

            Initialize();
        }


        #endregion

        #region Private, Protected Methods

        private void Initialize()
        {
            _MainWindow.OnInputTextChanged += (text) => SetDialogueText(text);
            _MainWindow.OnPlayerNameTextChanged += (playerName) =>
            {
                SetPlayerName(playerName);
                SetDialogueText(_MainWindow.GetInputText());
            };

            SetDialogueText(_MainWindow.GetInputText());
            SetPlayerName(_MainWindow.GetPlayerName());
        }

        private string ParseAsDialogueString(string s)
        {
            var matches = Regex.Matches(s, Config.SCRIPT_PATTERN, RegexOptions.Multiline);
            var _text = "";

            for (int i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                var content = match.Groups["scriptcontent"].Value;
                bool isLast = i >= matches.Count - 1;

                if (content.StartsWith("br")) s = Regex.Replace(s, match.Value, !isLast ? "\n" : "");
                else if (content.StartsWith("char")) s = s.Replace(match.Value, !isLast ? "\n\n[Player action/new page]\n" : "");
                else if (content.StartsWith("npc")) s = s.Replace(match.Value, !isLast ? "\n\n[NPC action/new page]\n" : "");
                else if (content.StartsWith("&7$"))
                {
                    //if (i == 0) s = s.Replace(match.Value, !isLast ? (content.Contains("%n%") ? _PlayerName : _StarterName) : "");
                    if (i != 0) s = s.Replace(match.Value, !isLast ? (content.Contains("%n%") ? $"[{_PlayerName}]" : $"[{NameLabel.Content}]") : "");
                    else
                    {
                        var starterNameMatch = Regex.Match(match.Value, Config.STARTER_NAME_PATTERN);
                        if (starterNameMatch.Success) SetNameLabelText(starterNameMatch.Groups["startername"].Value);
                        s = s.Replace(match.Value, "");
                    }
                }
            }

            _text = string.Join(" ", s.Split(' ').Take(Config.MAX_WORDS_PER_DIALOGUE));
            _text = _text.Replace("%n%", $"[{_PlayerName}]");
            _text = Regex.Replace(_text, Config.WHITESPACE_PATTERN_1, "\n\n");
            return _text;
        }
        #endregion


        #region Public Methods
        public void SetDialogueText(string text)
        {
            PreviewTextBox.SetText(ParseAsDialogueString(text), Brushes.White);
        }

        public void SetNameLabelText(string text)
        {
            NameLabel.Content = text;
        }

        public void SetPlayerName(string playerName)
        {
            _PlayerName = playerName;
        }

        public string GetDialogueText()
        {
            return PreviewTextBox.GetText();
        }

        #endregion
    }
}
