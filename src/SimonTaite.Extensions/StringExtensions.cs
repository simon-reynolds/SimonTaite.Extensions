using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace SimonTaite.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Truncates a string containing HTML to a number of text characters, keeping whole words.
        /// The result contains HTML and any tags left open are closed.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxCharacters"></param>
        /// <param name="trailingText"></param>
        /// <returns></returns>
        public static string TruncateHtml(this string html, int maxCharacters, string trailingText = "")
        {
            if (string.IsNullOrEmpty(html))
                    return html;

                // find the spot to truncate
                // count the text characters and ignore tags
                var textCount = 0;
                var charCount = 0;
                var ignore = false;
                foreach (char c in html)
                {
                    charCount++;
                    if (c == '<')
                        ignore = true;
                    else if (!ignore)
                        textCount++;

                    if (c == '>')
                        ignore = false;

                    // stop once we hit the limit
                    if (textCount >= maxCharacters)
                        break;
                }

                // Truncate the html and keep whole words only
                var trunc = new StringBuilder(html.TruncateWords(charCount));

                // keep track of open tags and close any tags left open
                var tags = new Stack<string>();
                var matches = Regex.Matches(trunc.ToString(),
                    @"<((?<tag>[^\s/>]+)|/(?<closeTag>[^\s>]+)).*?(?<selfClose>/)?\s*>",
                    RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);

                foreach (Match match in matches)
                {
                    if (match.Success)
                    {
                        var tag = match.Groups["tag"].Value;
                        var closeTag = match.Groups["closeTag"].Value;

                        // push to stack if open tag and ignore it if it is self-closing, i.e. <br />
                        if (!string.IsNullOrEmpty(tag) && string.IsNullOrEmpty(match.Groups["selfClose"].Value))
                            tags.Push(tag);

                        // pop from stack if close tag
                        else if (!string.IsNullOrEmpty(closeTag))
                        {
                            // pop the tag to close it.. find the matching opening tag
                            // ignore any unclosed tags
                            while (tags.Pop() != closeTag && tags.Count > 0)
                            { }
                        }
                    }
                }

                if (html.Length > charCount)
                    // add the trailing text
                    trunc.Append(trailingText);

                // pop the rest off the stack to close remainder of tags
                while (tags.Count > 0)
                {
                    trunc.Append("</");
                    trunc.Append(tags.Pop());
                    trunc.Append('>');
                }

                return trunc.ToString();
        }
        
        /// <summary>
        /// Truncates a string containing HTML to a number of text characters, keeping whole words.
        /// The result contains HTML and any tags left open are closed.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxCharacters"></param>
        /// <returns></returns>
        public static string TruncateHtml(this string html, int maxCharacters)
        {
            return html.TruncateHtml(maxCharacters, null);
        }

        /// <summary>
        /// Strips all HTML tags from a string
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string StripHtml(this string html)
        {
            if (string.IsNullOrEmpty(html))
                return html;

            return Regex.Replace(html, @"<(.|\n)*?>", string.Empty);
        }

        /// <summary>
        /// Truncates text to a number of characters
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxCharacters"></param>
        /// <returns></returns>
        public static string Truncate(this string text, int maxCharacters)
        {
            return text.Truncate(maxCharacters, null);
        }

        /// <summary>
        /// Truncates text to a number of characters and adds trailing text, i.e. elipses, to the end
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxCharacters"></param>
        /// <param name="trailingText"></param>
        /// <returns></returns>
        public static string Truncate(this string text, int maxCharacters, string trailingText)
        {
            if (string.IsNullOrEmpty(text) || maxCharacters <= 0 || text.Length <= maxCharacters)
                return text;
            else
                return text.Substring(0, maxCharacters) + trailingText;
        }


        /// <summary>
        /// Truncates text and discars any partial words left at the end
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxCharacters"></param>
        /// <param name="trailingText"></param>
        /// <returns></returns>
        public static string TruncateWords(this string text, int maxCharacters)
        {
            return text.TruncateWords(maxCharacters, null);
        }

        /// <summary>
        /// Truncates text and discars any partial words left at the end
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxCharacters"></param>
        /// <param name="trailingText"></param>
        /// <returns></returns>
        public static string TruncateWords(this string text, int maxCharacters, string trailingText)
        {
            if (string.IsNullOrEmpty(text) || maxCharacters <= 0 || text.Length <= maxCharacters)
                return text;

            // trunctate the text, then remove the partial word at the end
            return Regex.Replace(text.Truncate(maxCharacters),
                @"\s+[^\s]+$", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled) + trailingText;
        }
        
        public static string CalculateMD5Hash(this string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input), @"input cannot be null");
            }
            
            // step 1, calculate MD5 hash from input
            var md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input.ToLowerInvariant().Trim());
            byte[] hash = md5.ComputeHash(inputBytes);
         
            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }
        
        public static string Slugify(this string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input), @"input cannot be null");
            }
            
            const int SlugLength = 45;
            var str = RemoveAccent(input).ToLower();
            str = Regex.Replace(str, @"\s\(.*\)$", "");

            str = Regex.Replace(str, @"[^a-z0-9\s-]", ""); // invalid chars           
            str = Regex.Replace(str, @"\s+", " ").Trim(); // convert multiple spaces into one space   
            str = str.Substring(0, str.Length <= SlugLength ? str.Length : SlugLength).Trim(); // cut and trim it   
            str = Regex.Replace(str, @"\s", "-"); // hyphens   

            return str;
        }

        public static string RemoveAccent(this string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input), @"input cannot be null");
            }
            
            var bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(input);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}
