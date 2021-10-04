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
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace AKDialogueMaker.Utilities
{
    public static class DialogueUtility
    {
        public static string SetUpAsDialogueText(this string s)
        {
            var matches = Regex.Matches(s, Config.SCRIPT_PATTERN, RegexOptions.Multiline);
            var _text = "";

            for (int i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                var content = match.Groups["scriptcontent"].Value;
                bool isLast = i >= matches.Count - 1;

                if (content.StartsWith("br")) s = Regex.Replace(s, match.Value, !isLast ? "\n" : "");
                else if (content.StartsWith("npc")) s = s.Replace(match.Value, !isLast ? "\n\n[NPC action/new page]\n" : "");
                else if (content.StartsWith("&7$")) s = s.Replace(match.Value, !isLast ? (content.Contains("%n%") ? "[Player name]" : "[NPC name]") : "");
            }

            _text = string.Join(" ", s.Split(' ').Take(Config.MAX_WORDS_PER_DIALOGUE));
            _text = _text.Replace("%n%", "[Player name]");
            _text = Regex.Replace(_text, Config.WHITESPACE_PATTERN_1, "\n\n");
            return _text;
        }

        public static string GetUntilOrEmpty(this string text, string stopAt = "-")
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return text.Substring(0, charLocation);
                }
            }

            return string.Empty;
        }
    }
}
