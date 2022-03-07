﻿using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace LteDev.RegexParsing
{
    //public interface IStaticCharElement : IRegexPatternElement
    //{
    //    int Count { get; }
    //    char Value { get; }
    //    bool IsEscaped { get; }
    //}

    public class RegexParser
    {
        /// <summary>Matches the first tokens of a character class pattern.</summary>
        /// <remarks>
        /// Matches at least one, two or all of the following groups:
        /// <list type="bullet">
        ///   <item><c>neg</c> = Character class is negated</item>
        ///   <item><c>EM</c> = Maches one or more escaped meta character sequences</item>
        ///   <item><c>lit</c> = Maches one or more literal characters</item>
        ///   <item><c>r</c> = Matches range character (<c>-</c>) after a <c>lit</c> group match</item>
        /// </list>
        /// </remarks>
        public static readonly Regex CharacterClassFirstTokenRegex = new Regex(@"\G(
    (?<neg>\^)
    (
        (
            (?<lit>-[^\\-]*((?=-$)-)?)
        |
            (?<esc>\\\-((\\[^rntabefv\]\\^-]))*)
        )
        (?<r>(?=-.)-)?
    )?
|
    (
        (?<em>\\\^(\\[\\\]-])*)
    |
        (?<esc>\\\-((\\[^rntabefv\]\\^-]))*)
    |
        (?<lit>-[^\\-]*((?=-$)-)?)
    )
    (?<r>(?=-.)-)?
)", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        /// <summary>Matches subsequent character class pattern tokens.</summary>
        /// <remarks>
        /// Matches one the following groups:
        /// <list type="bullet">
        ///   <item><c>ce</c> = One or more character escape sequences (<c>\n</c>, <c>\t</c>, etc)</item>
        ///   <item><c>em</c> = One or more metacharacter escape sequences  (<c>\]</c>, <c>\\</c>, etc)</item>
        ///   <item><c>oct</c> = One or more octal escape sequences</item>
        ///   <item><c>nul</c> = One or more NUL character escape sequences (<c>\0</c>)</item>
        ///   <item><c>ctl</c> = One or more control character escape sequence</item>
        ///   <item><c>hex</c> = One or more hexidecimal escape sequence</item>
        ///   <item><c>uni</c> = One or more unicode escape sequence</item>
        ///   <item><c>cat</c> = One or more unicode category sequence</item>
        ///   <item><c>esc</c> = One or more escaped literal sequences</item>
        ///   <item><c>lit</c> = One or more literal characters</item>
        /// </list>
        /// Additional possible matche
        /// <list type="bullet">
        ///   <item><c>r</c> = Matches range character (<c>-</c>) after a <c>ce</c>, <c>em</c> or <c>esc</c> group match</item>
        /// </list>
        /// </remarks>
        public static readonly Regex CharacterClassNextTokenRegex = new Regex(@"\G(
    
    (
        (?<ce>(\\[rntabefv])+)
    |
        (?<em>(\\[\\\]-])+)
    |
        (?<oct>(\\(((?=0[1-7])0[1-7]|[1-3][0-7]?|[4-7]))[0-7]?)+)
    |
        (?<nul>\\0((?!\\0[1-7]|[1-7])\\0)*)
    |
        (?<ctl>(\\c[A-Z])+)
    |
        (?<hex>(\\x[\da-fA-F]{2})+)
    |
        (?<uni>(\\u[\dA-Fa-f]{4})+)
    |
        (?<cat>\\[Pp]\{(?<n>[\w-]+)?\})
    |
        (?<esc>\\.((?!\\(
            c[A-Z]
            |x[\da-fA-F]{2}
            |u[\dA-Fa-f]{4}
            |[Pp]\{[\w-]*\}
            |[0-7rntabefv\]\\-]
        ))\\.)*)
    )
|
    (?<lit>(-[^\\-]*|[^\\-]+)(-(?=$))?)
)(?<r>(?=-.)-)?", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        /// <summary>Uses named groups to match regular expression patterns.</summary>
        /// <remarks>
        /// Matches one of the following groups:
        /// <list type="bullet">
        ///   <item><c>cmt</c> = Comment. Optionally matches the following group:
        ///     <list type="bullet">
        ///         <item><c>t</c> - Text of comment</item>
        ///     </list>
        ///   </item>
        ///   <item><c>anc</c> = One or more anchor sequences (<c>^</c>, <c>\A</c>)</item>
        ///   <item><c>alt</c> = One or more alternation characters (<c>|</c>)</item>
        ///   <item><c>grp</c> = A group pattern. Also matches the following group:
        ///     <list type="bullet">
        ///         <item><c>ptn</c> - Contents of group.</item>
        ///     </list>
        ///   </item>
        ///   <item><c>any</c> = One or more Dot characters (<c>.</c>)</item>
        ///   <item><c>lit</c> = One or more one or more literal characters</item>
        ///   <item><c>ce</c> = One or more character escape sequences (<c>\n</c>, <c>\t</c>, etc)</item>
        ///   <item><c>em</c> = One or more metacharacter escape sequences (<c>\{</c>, <c>\\</c>, etc)</item>
        ///   <item><c>oct</c> = One or more octal escape sequences</item>
        ///   <item><c>nul</c> = One or more NUL character escape sequences (<c>\0</c>)</item>
        ///   <item><c>ctl</c> = One or more control character escape sequence</item>
        ///   <item><c>hex</c> = One or more hexidecimal escape sequence</item>
        ///   <item><c>uni</c> = One or more unicode escape sequence</item>
        ///   <item><c>cat</c> = One or more unicode category sequence</item>
        ///   <item><c>nbr</c> = A named backreference sequence</item>
        ///   <item><c>dbr</c> = One or more numbered backreference sequences</item>
        ///   <item><c>esc</c> = One or more escaped literals</item>
        ///   <item><c>cc</c> = A character class pattern. Optionally matches the following group:
        ///     <list type="bullet">
        ///         <item><c>ptn</c> - Contents of class pattern.</item>
        ///     </list>
        ///   </item>
        /// </list>
        /// The following group may also match:
        /// <list type="bullet">
        ///   <item><c>q</c> = Matches a qualifier sequence. Follows any other group match from the first list except <c>anc</c>, <c>alt</c> or <c>grp</c>. One of the following will always match as well:
        ///     <list type="bullet">
        ///       <item><c>opt</c> = Matches optional quantifier (<c>?</c>)</item>
        ///       <item><c>mlt</c> = Matches multiple quantifier (<c>+</c>)</item>
        ///       <item><c>min</c> = First number in a <c>{N}</c>, <c>{N,}</c> or <c>{N,N}</c> match. The following may match as well:
        ///         <list type="bullet">
        ///           <item><c>qr</c> = The <c>,</c> character following the first digit of an explicit repeat quantifier.
        ///             <list type="bullet">
        ///               <item><c>max</c> = The second number in a <c>{N,N}</c> match.</item>
        ///             </list>
        ///           </item>
        ///         </list>
        ///       </item>
        ///     </list>
        ///     Additionally, the <c>lzy</c> group will match if it is a lazy quantifier.
        ///   </item>
        /// </list>
        /// </remarks>
        public static readonly Regex SingleLinePatternTokenRegex = new Regex(@"\G
(
    (?<anc>([$^]+|\\[GAZz])+)
|
    (?<alt>\|+)
|
    (?<grp>\(
        (?<ptn>
            ([^\\()]+|(\\.)+)*
            (?>
                (?>(?'g'\()[^\\()]+|(\\.)+)+
                (?>(?'-g'\))[^\\()]+|(\\.)+)+
            )*
            (?(g)(?!))
        )
    \))
|
    (
        (?<any>\.+)
    |
        (?<lit>
            ({|[^[\\^$.|?*+(){])
            (
                [^[\\^$.|?*+(){]+
                |
                (?!\{\d+(,\d*)?\}){
            )*
        )
    |
        (?<ce>(\\[rntaefv])+)
    |
        (?<em>(\\[[\\^$.|?*+(){}])+)
    |
        (?<oct>(\\((?=0[1-7])0[1-7][0-7]?|[1-3][0-7]{2}))+)
    |
        (?<nul>\\0((?!\\0[1-7]|[1-3][0-7]{2})\\0)*)
    |
        (?<ctl>(\\c[A-Z])+)
    |
        (?<hex>(\\x[\da-fA-F]{2})+)
    |
        (?<uni>(\\u[\dA-Fa-f]{4})+)
    |
        (?<cat>\\[Pp]\{(?<n>[\w-]+)?\})
    |
        (?<dbr>((?!\\[1-3][0-7]{2})\\[1-9]\d?)+)
    |
        (?<dbr>\\k(<(?<n>[1-9]\d?)>|'(?<n>[1-9]\d?)'))
    |    
        (?<nbr>\\k(?=[<']\d*\D[\w-]*[>'])(<(?<n>[\w-]+)>|'(?<n>[\w-]+)'))
    |
        (?<esc>\\.((?!\\(
            c[A-Z]
            |x[\da-fA-F]{2}
            |u[\dA-Fa-f]{4}
            |[Pp]\{[\w-]*\}
            |[\drntaefv\]\\-]
        ))\\.)*)
    |
        (?<cc>\[
            (?<ptn>
                (
                    (\\.)+
                |
                    [^\\\]]+
                )+
            )?
        \])
    )
    (?<q>
        (
            (?<opt>\?)
        |
            (?<mlt>\+)
        |
            \{(?<min>\d+)(?<qr>,(?<max>\d+)?)?\}
        )
        (?<lzy>\?)?
    )?
)", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        /// <summary>Uses named groups to match various multiline regular expression patterns.</summary>
        /// <remarks>
        /// Matches one of the following groups:
        /// <list type="bullet">
        ///   <item><c>cmt</c> = Comment. Optionally matches the following group:
        ///     <list type="bullet">
        ///         <item><c>t</c> - Text of comment</item>
        ///     </list>
        ///   </item>
        ///   <item><c>anc</c> = One or more anchor sequences (<c>^</c>, <c>\A</c>)</item>
        ///   <item><c>alt</c> = One or more alternation characters (<c>|</c>)</item>
        ///   <item><c>grp</c> = A group pattern. Also matches the following group:
        ///     <list type="bullet">
        ///         <item><c>ptn</c> - Contents of group.</item>
        ///     </list>
        ///   </item>
        ///   <item><c>any</c> = One or more Dot characters (<c>.</c>)</item>
        ///   <item><c>lit</c> = One or more one or more literal characters</item>
        ///   <item><c>ce</c> = One or more character escape sequences (<c>\n</c>, <c>\t</c>, etc)</item>
        ///   <item><c>em</c> = One or more metacharacter escape sequences (<c>\{</c>, <c>\\</c>, etc)</item>
        ///   <item><c>oct</c> = One or more octal escape sequences</item>
        ///   <item><c>nul</c> = One or more NUL character escape sequences (<c>\0</c>)</item>
        ///   <item><c>ctl</c> = One or more control character escape sequence</item>
        ///   <item><c>hex</c> = One or more hexidecimal escape sequence</item>
        ///   <item><c>uni</c> = One or more unicode escape sequence</item>
        ///   <item><c>cat</c> = One or more unicode category sequence</item>
        ///   <item><c>nbr</c> = A named backreference sequence</item>
        ///   <item><c>dbr</c> = One or more numbered backreference sequences</item>
        ///   <item><c>esc</c> = One or more escaped literals</item>
        ///   <item><c>cc</c> = A character class pattern. Optionally matches the following group:
        ///     <list type="bullet">
        ///         <item><c>ptn</c> - Contents of class pattern.</item>
        ///     </list>
        ///   </item>
        /// </list>
        /// The following group may also match:
        /// <list type="bullet">
        ///   <item><c>q</c> = Matches a qualifier sequence. Follows any other group match from the first list except <c>anc</c>, <c>alt</c> or <c>grp</c>. One of the following will always match as well:
        ///     <list type="bullet">
        ///       <item><c>opt</c> = Matches optional quantifier (<c>?</c>)</item>
        ///       <item><c>mlt</c> = Matches multiple quantifier (<c>+</c>)</item>
        ///       <item><c>min</c> = First number in a <c>{N}</c>, <c>{N,}</c> or <c>{N,N}</c> match. The following may match as well:
        ///         <list type="bullet">
        ///           <item><c>qr</c> = The <c>,</c> character following the first digit of an explicit repeat quantifier.
        ///             <list type="bullet">
        ///               <item><c>max</c> = The second number in a <c>{N,N}</c> match.</item>
        ///             </list>
        ///           </item>
        ///         </list>
        ///       </item>
        ///     </list>
        ///     Additionally, the <c>lzy</c> group will match if it is a lazy quantifier.
        ///   </item>
        /// </list>
        /// </remarks>
        public static readonly Regex MultilinePatternTokenRegex = new Regex(@"\G
(
    (?<cmt>\#(?<t>[^\r\n]+)?(\r\n?|\n|$))
|
    (?<anc>([$^]+|\\[GAZz])+)
|
    (?<alt>\|+)
|
    (?<grp>\(
        (?<ptn>
            ([^\\()#]+|(\\.)+|\#[^\r\n]+)*
            (?>
                (?>(?'g'\()[^\\()#]+|(\\.)+|\#[^\r\n]+)+
                (?>(?'-g'\))[^\\()#]+|(\\.)+|\#[^\r\n]+)+
            )*
            (?(g)(?!))
        )
    \))
|
    (
        (?<any>\.+)
    |
        (?<lit>
            ({|[^#[\\^$.|?*+(){])
            (
                [^#[\\^$.|?*+(){]+
                |
                (?!\{\d+(,\d*)?\}){
            )*
        )
    |
        (?<ce>(\\[rntaefv])+)
    |
        (?<em>(\\[#[\\^$.|?*+(){}])+)
    |
        (?<oct>(\\((?=0[1-7])0[1-7][0-7]?|[1-3][0-7]{2}))+)
    |
        (?<nul>\\0((?!\\0[1-7]|[1-3][0-7]{2})\\0)*)
    |
        (?<ctl>(\\c[A-Z])+)
    |
        (?<hex>(\\x[\da-fA-F]{2})+)
    |
        (?<uni>(\\u[\dA-Fa-f]{4})+)
    |
        (?<cat>\\[Pp]\{(?<n>[\w-]+)?\})
    |
        (?<dbr>((?!\\[1-3][0-7]{2})\\[1-9]\d?)+)
    |
        (?<dbr>\\k(<(?<n>[1-9]\d?)>|'(?<n>[1-9]\d?)'))
    |    
        (?<nbr>\\k(?=[<']\d*\D[\w-]*[>'])(<(?<n>[\w-]+)>|'(?<n>[\w-]+)'))
    |
        (?<esc>\\.((?!\\(
            c[A-Z]
            |x[\da-fA-F]{2}
            |u[\dA-Fa-f]{4}
            |[Pp]\{[\w-]*\}
            |[\drntaefv\]\\-]
        ))\\.)*)
    |
        (?<cc>\[
            (?<ptn>
                (
                    (\\.)+
                |
                    [^\\\]]+
                )+
            )?
        \])
    )
    (?<q>
        (
            (?<opt>\?)
        |
            (?<mlt>\+)
        |
            \{(?<min>\d+)(?<qr>,(?<max>\d+)?)?\}
        )
        (?<lzy>\?)?
    )?
)", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        /// <summary>Matches a qualifier.</summary>
        /// <remarks>
        /// One of the following will always match:
        /// <list type="bullet">
        ///   <item><c>opt</c> = Matches optional quantifier (<c>?</c>)</item>
        ///   <item><c>mlt</c> = Matches multiple quantifier (<c>+</c>)</item>
        ///   <item><c>min</c> = First number in a <c>{N}</c>, <c>{N,}</c> or <c>{N,N}</c> match. The following may match as well:
        ///     <list type="bullet">
        ///       <item><c>qr</c> = The <c>,</c> character following the first digit of an explicit repeat quantifier.
        ///         <list type="bullet">
        ///           <item><c>max</c> = The second number in a <c>{N,N}</c> match.</item>
        ///         </list>
        ///       </item>
        ///     </list>
        ///   </item>
        /// </list>
        /// Additionally, the <c>lzy</c> group will match if it is a lazy quantifier.
        /// </remarks>
        public static readonly Regex QuantifierRegex = new Regex(@"\G(
    (?<opt>\?)
|
    (?<mlt>\+)
|
    \{(?<min>\d+)(?<qr>,(?<max>\d+)?)?\}
)(?<lzy>\?)?", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        /// <summary>Matches the pattern at the start of a group which indicates the group type.</summary>
        /// <remarks>
        ///   <list type="bullet">
        ///     <item><c>ap</c> = Pattern for a look-ahead positive</item>
        ///     <item><c>an</c> = Pattern for a look-ahead negative</item>
        ///     <item><c>bp</c> = Pattern for a look-behind positive</item>
        ///     <item><c>bn</c> = Pattern for a look-behind negative</item>
        ///     <item><c>ng</c> = Pattern for a named group.
        ///       <list type="bullet">
        ///         <item><c>d</c> = Opening delimiter for name.</item>
        ///       </list>
        ///     </item>
        ///     <item><c>x</c> = Pattern for a non-capturing group</item>
        ///     <item>
        ///       <item><c>x</c> = Pattern for a pattern modifier group</item>
        ///       <item><c>m</c> = Matches the modifier characters. If this group<c>m</c>matches as well, then it is a group modifier; otherwise group it is a pattern modifier and the whole match will contain the closing parenthesis.</item>
        ///       <item><c>c</c> = Matches the start of a conditional
        ///         <list type="bullet">
        ///           <item><c>cr</c> = Matches the start of a conditional that references a numbered group</item>
        ///           <item><c>cn</c> = Matches the start of a conditional that references a named group</item>
        ///           <item><c>cl</c> = Matches the start of a lookaround conditional</item>
        ///         </list>
        ///       </item>
        ///     </item>
        ///   </list>
        /// </remarks>
        public static readonly Regex GroupTypeRegex = new Regex(@"^\?(
    (?<x>:)
|
    (?=<?[=!])
    (
        (?<ap>=)
    |
        (?<an>!)
    |
        <
        (
            (?<bp>=)
        |
            (?<bn>!)
        )
    )
|
    (?='-?\w+'|<-?\w+>)
    (?<d>['<])(?<ng>[\w-]+)['>]
|
    (?<m>
        (?=([ixsmn]+(-[ixsmn]+)?)(:|$))
        (?=[xsmn-]*i[xsmn-]*(:|$))
        (?=[ismn-]*x[ismn-]*(:|$))
        (?=[ixmn-]*s[ixmn-]*(:|$))
        (?=[ixsn-]*m[ixsn-]*(:|$))
        (?=[ixsm-]*n[ixsm-]*(:|$))
        [ixsmn]*(-[ixsmn]+)?
        ((?<x>:)|$)
    )
|
    (?<at>>)
|
    (?<c>
        \(
        (?=\w*\))
        (
            (?=\d+\))(?<cr>\d+)
        |
            (?<cn>.*)
        )\)
    )?
)?(?<ptn>.+)?", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        private void ParseCharacterClassTokens(string pattern, Collection<IRegexPatternToken> collection)
        {
            Match match = CharacterClassFirstTokenRegex.Match(pattern);
            int startAt;
            if (match.Success)
            {
                if (match.Groups["neg"].Success)
                    collection.Add(new CharTokens(RegexTokenType.CharacterClassOpen, '[', '^'));
                else
                    collection.Add(new CharTokens(RegexTokenType.CharacterClassOpen, '['));
                if (match.Groups["lit"].Success)
                {
                    collection.Add(new CharTokens(RegexTokenType.LiteralCharacter, match.Groups["lit"].Value));
                    if (match.Groups["r"].Success)
                        collection.Add(new CharTokens(RegexTokenType.CharacterClassOpen, '-'));
                }
                startAt = match.Length;
            }
            else
            {
                startAt = 0;
                collection.Add(new CharTokens(RegexTokenType.CharacterClassOpen, '['));
            }
            while (startAt < pattern.Length)
            {
                match = CharacterClassNextTokenRegex.Match(pattern, startAt);
                if (match.Groups["ce"].Success)
                    collection.Add(new CharTokens(RegexTokenType.CharacterEscape, match.Value));
                else if (match.Groups["em"].Success) // One or more metacharacter escape sequences (<c>\{</c>, <c>\\</c>, etc)
                    collection.Add(new CharTokens(RegexTokenType.MetaCharacterEscape, match.Value));
                else if (match.Groups["oct"].Success) // One or more octal escape sequences
                    collection.Add(new CharTokens(RegexTokenType.OctalEscape, match.Value));
                else if (match.Groups["nul"].Success) // One or more NUL character escape sequences (<c>\0</c>)
                    collection.Add(new CharTokens(RegexTokenType.NullEscape, match.Value));
                else if (match.Groups["ctl"].Success) // One or more control character escape sequence
                    collection.Add(new CharTokens(RegexTokenType.ControlCharacterEscape, match.Value));
                else if (match.Groups["hex"].Success) // One or more hexidecimal escape sequence
                    collection.Add(new CharTokens(RegexTokenType.HexidecimalEscape, match.Value));
                else if (match.Groups["uni"].Success) // One or more unicode escape sequence
                    collection.Add(new CharTokens(RegexTokenType.UnicodeCategory, match.Value));
                else if (match.Groups["cat"].Success) // One unicode category sequence
                    collection.Add(new NamedToken(RegexTokenType.UnicodeCategory, match.Value.ToCharArray(0, 3), match.Groups["n"].Value, '}'));
                else
                    collection.Add(new CharTokens(RegexTokenType.LiteralCharacter, match.Value));
                startAt = match.Index + match.Length;
            }
            collection.Add(new CharTokens(RegexTokenType.CharacterClassClose, ']'));
        }

        private void ParseTokens(string pattern, Collection<IRegexPatternToken> collection, int groupLevel, Regex regex)
        {
            Collection<IRegexPatternToken> result = new Collection<IRegexPatternToken>();
            int startIndex = 0;
            while (startIndex < pattern.Length)
            {
                Match match = regex.Match(pattern, startIndex);
                if (!match.Success)
                {
                    int endIndex = startIndex + 1;
                    if (endIndex < pattern.Length)
                    {
                        while (!match.Success && ++endIndex < pattern.Length)
                            match = regex.Match(pattern, endIndex);
                    }
                    result.Add(new CharTokens(RegexTokenType.Unknown, pattern.ToCharArray(startIndex, endIndex - startIndex)));
                    if (!match.Success)
                        break;
                }

                if (match.Groups["grp"].Success) // A group pattern
                {
                    startIndex = match.Index + match.Length;
                    string ptn = match.Groups["ptn"].Success ? match.Groups["ptn"].Value : "";
                    if (ptn.Length > 0)
                    {
                        match = GroupTypeRegex.Match(ptn);
                        if (match.Success)
                        {
                            if (match.Groups["ap"].Success)
                            {
                                result.Add(new OpenGroupToken(RegexTokenType.LookaheadOpen, groupLevel, '?', '=', null));
                                if (match.Groups["ptn"].Success && (ptn = match.Groups["ptn"].Value).Length > 0)
                                    ParseTokens(ptn, collection, groupLevel + 1, regex);
                                result.Add(new CloseGroupToken(RegexTokenType.GroupClose, groupLevel));
                            }
                            else if (match.Groups["an"].Success)
                            {
                                result.Add(new OpenGroupToken(RegexTokenType.LookaheadOpen, groupLevel, '?', '!', null));
                                if (match.Groups["ptn"].Success && (ptn = match.Groups["ptn"].Value).Length > 0)
                                    ParseTokens(ptn, collection, groupLevel + 1, regex);
                                result.Add(new CloseGroupToken(RegexTokenType.GroupClose, groupLevel));
                            }
                            else if (match.Groups["bp"].Success)
                            {
                                result.Add(new OpenGroupToken(RegexTokenType.LookbehindOpen, groupLevel, '?', '<', '=', null));
                                if (match.Groups["ptn"].Success && (ptn = match.Groups["ptn"].Value).Length > 0)
                                    ParseTokens(ptn, collection, groupLevel + 1, regex);
                                result.Add(new CloseGroupToken(RegexTokenType.GroupClose, groupLevel));
                            }
                            else if (match.Groups["bn"].Success)
                            {
                                result.Add(new OpenGroupToken(RegexTokenType.LookbehindOpen, groupLevel, '?', '<', '!', null));
                                if (match.Groups["ptn"].Success && (ptn = match.Groups["ptn"].Value).Length > 0)
                                    ParseTokens(ptn, collection, groupLevel + 1, regex);
                                result.Add(new CloseGroupToken(RegexTokenType.GroupClose, groupLevel));
                            }
                            else if (match.Groups["m"].Success && !match.Groups["x"].Success)
                                result.Add(new CharTokens(RegexTokenType.ModeModifier, match.Value));
                            else
                            {
                                if (match.Groups["at"].Success)
                                    result.Add(new OpenGroupToken(RegexTokenType.AtomicGroupOpen, groupLevel, '?', '>', null));
                                else if (match.Groups["ng"].Success)
                                    result.Add(new OpenGroupToken(RegexTokenType.NamedGroupOpen, groupLevel, '?', match.Groups["d"].Value[0], match.Groups["ng"].Value, (match.Groups["d"].Value[0] == '<') ? '>' : '\''));
                                else if (match.Groups["x"].Success)
                                {
                                    if (match.Groups["m"].Success)
                                        result.Add(new OpenGroupToken(RegexTokenType.NonCapturingGroupOpen, groupLevel, match.Groups["m"].Value));
                                    else
                                        result.Add(new OpenGroupToken(RegexTokenType.NonCapturingGroupOpen, groupLevel, '?', ':', null));
                                }
                                else
                                    result.Add(new OpenGroupToken(RegexTokenType.GroupOpen, groupLevel, '?', null));
                                if (match.Groups["ptn"].Success && (ptn = match.Groups["ptn"].Value).Length > 0)
                                    ParseTokens(ptn, collection, groupLevel + 1, regex);
                                result.Add(new CloseGroupToken(RegexTokenType.GroupClose, groupLevel));
                                Match qm = QuantifierRegex.Match(pattern, startIndex);
                                if (qm.Success)
                                {
                                    if (qm.Groups["opt"].Success)
                                    {
                                        if (match.Groups["lzy"].Success)
                                            result.Add(new CharTokens(RegexTokenType.GreedyOptionalQuantifier, '?'));
                                        else
                                            result.Add(new CharTokens(RegexTokenType.LazyOptionalQuantifier, '?', '?'));
                                    }
                                    else if (qm.Groups["mlt"].Success)
                                    {
                                        if (match.Groups["lzy"].Success)
                                            result.Add(new CharTokens(RegexTokenType.GreedyMultipleQuantifier, '+'));
                                        else
                                            result.Add(new CharTokens(RegexTokenType.LazyMultipleQuantifier, '+', '?'));
                                    }
                                    else
                                    {
                                        if (qm.Groups["max"].Success)
                                        {
                                            throw new NotImplementedException();
                                        }
                                        else if (qm.Groups["qr"].Success)
                                        {
                                            throw new NotImplementedException();
                                        }
                                        else
                                        {
                                            throw new NotImplementedException();
                                        }
                                    }
                                    match = qm;
                                }
                            }
                            //if (match.Groups["ptn"].Success && (ptn = match.Groups["ptn"].Value).Length > 0)
                            //    ParseTokens(ptn, collection, groupLevel + 1);
                            //result.Add(new CloseGroupToken(RegexTokenType.GroupClose, groupLevel));
                        }
                        else
                        {
                            result.Add(new OpenGroupToken(RegexTokenType.GroupOpen, groupLevel, null));
                            ParseTokens(ptn, collection, groupLevel + 1, regex);
                            result.Add(new CloseGroupToken(RegexTokenType.GroupClose, groupLevel));
                        }
                    }
                    else
                    {
                        result.Add(new OpenGroupToken(RegexTokenType.GroupOpen, groupLevel, null));
                        result.Add(new CloseGroupToken(RegexTokenType.GroupClose, groupLevel));
                    }
                    continue;
                }

                if (match.Groups["cmt"].Success) // Comment
                    result.Add(new CharTokens(RegexTokenType.CommentType, match.Groups["t"].Success ? match.Groups["t"].Value : ""));
                else if (match.Groups["anc"].Success) // One or more anchor sequences (<c>^</c>, <c>\A</c>)
                    result.Add(new CharTokens(RegexTokenType.Anchor, match.Value));
                else if (match.Groups["alt"].Success) // One or more alternation characters (<c>|</c>)
                    result.Add(new CharTokens(RegexTokenType.Alternation, match.Value));
                else
                {
                    if (match.Groups["any"].Success) // One or more Dot characters (<c>.</c>)
                        result.Add(new CharTokens(RegexTokenType.AnyCharacters, match.Value));
                    else if (match.Groups["ce"].Success) // One or more character escape sequences (<c>\n</c>, <c>\t</c>, etc)
                        result.Add(new CharTokens(RegexTokenType.CharacterEscape, match.Value));
                    else if (match.Groups["em"].Success) // One or more metacharacter escape sequences (<c>\{</c>, <c>\\</c>, etc)
                        result.Add(new CharTokens(RegexTokenType.MetaCharacterEscape, match.Value));
                    else if (match.Groups["oct"].Success) // One or more octal escape sequences
                        result.Add(new CharTokens(RegexTokenType.OctalEscape, match.Value));
                    else if (match.Groups["nul"].Success) // One or more NUL character escape sequences (<c>\0</c>)
                        result.Add(new CharTokens(RegexTokenType.NullEscape, match.Value));
                    else if (match.Groups["ctl"].Success) // One or more control character escape sequence
                        result.Add(new CharTokens(RegexTokenType.ControlCharacterEscape, match.Value));
                    else if (match.Groups["hex"].Success) // One or more hexidecimal escape sequence
                        result.Add(new CharTokens(RegexTokenType.HexidecimalEscape, match.Value));
                    else if (match.Groups["uni"].Success) // One or more unicode escape sequence
                        result.Add(new CharTokens(RegexTokenType.UnicodeCategory, match.Value));
                    else if (match.Groups["cat"].Success) // One unicode category sequence
                        result.Add(new NamedToken(RegexTokenType.UnicodeCategory, match.Value.ToCharArray(0, 3), match.Groups["n"].Value, '}'));
                    else if (match.Groups["nbr"].Success) // One named backreference sequences
                        result.Add(new NamedToken(RegexTokenType.Backreference, match.Value.ToCharArray(0, 3), match.Groups["n"].Value, match.Value[match.Length - 1]));
                    else if (match.Groups["dbr"].Success) // One or more numbered backreference sequences
                    {
                        if (match.Groups["n"].Success)
                            result.Add(new NamedToken(RegexTokenType.Backreference, match.Value.ToCharArray(0, 3), match.Groups["n"].Value, match.Value[match.Length - 1]));
                        else
                            result.Add(new CharTokens(RegexTokenType.Backreference, match.Value));
                    }
                    else if (match.Groups["esc"].Success) // One or more escaped literals
                        result.Add(new CharTokens(RegexTokenType.EscapedLiteral, match.Value));
                    else if (match.Groups["cc"].Success) // Character class
                    {
                        if (match.Groups["ptn"].Success)
                            ParseCharacterClassTokens(match.Groups["ptn"].Value, result);
                        else
                        {
                            result.Add(new CharTokens(RegexTokenType.CharacterClassOpen, '['));
                            result.Add(new CharTokens(RegexTokenType.CharacterClassClose, ']'));
                        }
                    }
                    else // One or more one or more literal characters
                        result.Add(new CharTokens(RegexTokenType.LiteralCharacter, match.Value));
                }
                startIndex = match.Index + match.Length;
            }
        }

        public Collection<IRegexPatternToken> ParseTokens(string pattern, bool isMultiLine)
        {
            Collection<IRegexPatternToken> result = new Collection<IRegexPatternToken>();
            if (pattern != null)
                ParseTokens(pattern, result, 0, isMultiLine ? MultilinePatternTokenRegex : SingleLinePatternTokenRegex);
            return result;
        }
    }
}
