using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace HtmlUtility.UnitTests;

public static class TestData
{
    public static System.Collections.IEnumerable GetToReflectionTypeTestData()
    {
        yield return new TestCaseData(MarkdownTokenType.Block).Returns(typeof(Block));
        yield return new TestCaseData(MarkdownTokenType.BlankLineBlock).Returns(typeof(BlankLineBlock));
        yield return new TestCaseData(MarkdownTokenType.ContainerBlock).Returns(typeof(ContainerBlock));
        yield return new TestCaseData(MarkdownTokenType.CustomContainer).Returns(typeof(Markdig.Extensions.CustomContainers.CustomContainer));
        yield return new TestCaseData(MarkdownTokenType.DefinitionItem).Returns(typeof(Markdig.Extensions.DefinitionLists.DefinitionItem));
        yield return new TestCaseData(MarkdownTokenType.DefinitionList).Returns(typeof(Markdig.Extensions.DefinitionLists.DefinitionList));
        yield return new TestCaseData(MarkdownTokenType.Figure).Returns(typeof(Markdig.Extensions.Figures.Figure));
        yield return new TestCaseData(MarkdownTokenType.FooterBlock).Returns(typeof(Markdig.Extensions.Footers.FooterBlock));
        yield return new TestCaseData(MarkdownTokenType.Footnote).Returns(typeof(Markdig.Extensions.Footnotes.Footnote));
        yield return new TestCaseData(MarkdownTokenType.FootnoteGroup).Returns(typeof(Markdig.Extensions.Footnotes.FootnoteGroup));
        yield return new TestCaseData(MarkdownTokenType.LinkReferenceDefinitionGroup).Returns(typeof(LinkReferenceDefinitionGroup));
        yield return new TestCaseData(MarkdownTokenType.ListBlock).Returns(typeof(ListBlock));
        yield return new TestCaseData(MarkdownTokenType.ListItemBlock).Returns(typeof(ListItemBlock));
        yield return new TestCaseData(MarkdownTokenType.MarkdownDocument).Returns(typeof(MarkdownDocument));
        yield return new TestCaseData(MarkdownTokenType.QuoteBlock).Returns(typeof(QuoteBlock));
        yield return new TestCaseData(MarkdownTokenType.Table).Returns(typeof(Markdig.Extensions.Tables.Table));
        yield return new TestCaseData(MarkdownTokenType.TableCell).Returns(typeof(Markdig.Extensions.Tables.TableCell));
        yield return new TestCaseData(MarkdownTokenType.TableRow).Returns(typeof(Markdig.Extensions.Tables.TableRow));
        yield return new TestCaseData(MarkdownTokenType.LeafBlock).Returns(typeof(LeafBlock));
        yield return new TestCaseData(MarkdownTokenType.Abbreviation).Returns(typeof(Markdig.Extensions.Abbreviations.Abbreviation));
        yield return new TestCaseData(MarkdownTokenType.CodeBlock).Returns(typeof(CodeBlock));
        yield return new TestCaseData(MarkdownTokenType.FencedCodeBlock).Returns(typeof(FencedCodeBlock));
        yield return new TestCaseData(MarkdownTokenType.MathBlock).Returns(typeof(Markdig.Extensions.Mathematics.MathBlock));
        yield return new TestCaseData(MarkdownTokenType.YamlFrontMatterBlock).Returns(typeof(Markdig.Extensions.Yaml.YamlFrontMatterBlock));
        yield return new TestCaseData(MarkdownTokenType.DefinitionTerm).Returns(typeof(Markdig.Extensions.DefinitionLists.DefinitionTerm));
        yield return new TestCaseData(MarkdownTokenType.EmptyBlock).Returns(typeof(EmptyBlock));
        yield return new TestCaseData(MarkdownTokenType.FigureCaption).Returns(typeof(Markdig.Extensions.Figures.FigureCaption));
        yield return new TestCaseData(MarkdownTokenType.HeadingBlock).Returns(typeof(HeadingBlock));
        yield return new TestCaseData(MarkdownTokenType.HtmlBlock).Returns(typeof(HtmlBlock));
        yield return new TestCaseData(MarkdownTokenType.LinkReferenceDefinition).Returns(typeof(LinkReferenceDefinition));
        yield return new TestCaseData(MarkdownTokenType.FootnoteLinkReferenceDefinition).Returns(typeof(Markdig.Extensions.Footnotes.FootnoteLinkReferenceDefinition));
        yield return new TestCaseData(MarkdownTokenType.HeadingLinkReferenceDefinition).Returns(typeof(Markdig.Extensions.AutoIdentifiers.HeadingLinkReferenceDefinition));
        yield return new TestCaseData(MarkdownTokenType.ParagraphBlock).Returns(typeof(ParagraphBlock));
        yield return new TestCaseData(MarkdownTokenType.ThematicBreakBlock).Returns(typeof(ThematicBreakBlock));
        yield return new TestCaseData(MarkdownTokenType.Inline).Returns(typeof(Markdig.Syntax.Inlines.Inline));
        yield return new TestCaseData(MarkdownTokenType.ContainerInline).Returns(typeof(Markdig.Syntax.Inlines.ContainerInline));
        yield return new TestCaseData(MarkdownTokenType.FootnoteLink).Returns(typeof(Markdig.Extensions.Footnotes.FootnoteLink));
        yield return new TestCaseData(MarkdownTokenType.LeafInline).Returns(typeof(Markdig.Syntax.Inlines.LeafInline));
        yield return new TestCaseData(MarkdownTokenType.AbbreviationInline).Returns(typeof(Markdig.Extensions.Abbreviations.AbbreviationInline));
        yield return new TestCaseData(MarkdownTokenType.AutolinkInline).Returns(typeof(Markdig.Syntax.Inlines.AutolinkInline));
        yield return new TestCaseData(MarkdownTokenType.CodeInline).Returns(typeof(Markdig.Syntax.Inlines.CodeInline));
        yield return new TestCaseData(MarkdownTokenType.HtmlEntityInline).Returns(typeof(Markdig.Syntax.Inlines.HtmlEntityInline));
        yield return new TestCaseData(MarkdownTokenType.HtmlInline).Returns(typeof(Markdig.Syntax.Inlines.HtmlInline));
        yield return new TestCaseData(MarkdownTokenType.LineBreakInline).Returns(typeof(Markdig.Syntax.Inlines.LineBreakInline));
        yield return new TestCaseData(MarkdownTokenType.LiteralInline).Returns(typeof(Markdig.Syntax.Inlines.LiteralInline));
        yield return new TestCaseData(MarkdownTokenType.EmojiInline).Returns(typeof(Markdig.Extensions.Emoji.EmojiInline));
        yield return new TestCaseData(MarkdownTokenType.MathInline).Returns(typeof(Markdig.Extensions.Mathematics.MathInline));
        yield return new TestCaseData(MarkdownTokenType.SmartyPant).Returns(typeof(Markdig.Extensions.SmartyPants.SmartyPant));
        yield return new TestCaseData(MarkdownTokenType.TaskList).Returns(typeof(Markdig.Extensions.TaskLists.TaskList));
        yield return new TestCaseData(MarkdownTokenType.DelimiterInline).Returns(typeof(Markdig.Syntax.Inlines.DelimiterInline));
        yield return new TestCaseData(MarkdownTokenType.EmphasisDelimiterInline).Returns(typeof(Markdig.Syntax.Inlines.EmphasisDelimiterInline));
        yield return new TestCaseData(MarkdownTokenType.LinkDelimiterInline).Returns(typeof(Markdig.Syntax.Inlines.LinkDelimiterInline));
        yield return new TestCaseData(MarkdownTokenType.PipeTableDelimiterInline).Returns(typeof(Markdig.Extensions.Tables.PipeTableDelimiterInline));
        yield return new TestCaseData(MarkdownTokenType.EmphasisInline).Returns(typeof(Markdig.Syntax.Inlines.EmphasisInline));
        yield return new TestCaseData(MarkdownTokenType.CustomContainerInline).Returns(typeof(Markdig.Extensions.CustomContainers.CustomContainerInline));
        yield return new TestCaseData(MarkdownTokenType.HtmlAttributes).Returns(typeof(Markdig.Renderers.Html.HtmlAttributes));
        yield return new TestCaseData(MarkdownTokenType.LinkInline).Returns(typeof(Markdig.Syntax.Inlines.LinkInline));
        yield return new TestCaseData(MarkdownTokenType.JiraLink).Returns(typeof(Markdig.Extensions.JiraLinks.JiraLink));
    }
    
    public static System.Collections.IEnumerable GetToReflectionTypesTestData()
    {
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.Block]).Returns(new Type[] { typeof(Block) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.BlankLineBlock]).Returns(new Type[] { typeof(BlankLineBlock) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.EmphasisInline, MarkdownTokenType.TableRow, MarkdownTokenType.ContainerBlock])
            .Returns(new Type[] { typeof(Markdig.Syntax.Inlines.EmphasisInline), typeof(Markdig.Extensions.Tables.TableRow), typeof(ContainerBlock) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.CustomContainer, MarkdownTokenType.Block])
            .Returns(new Type[] { typeof(Markdig.Extensions.CustomContainers.CustomContainer), typeof(Block) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.DefinitionList, MarkdownTokenType.DefinitionItem])
            .Returns(new Type[] { typeof(Markdig.Extensions.DefinitionLists.DefinitionList), typeof(Markdig.Extensions.DefinitionLists.DefinitionItem) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.SmartyPant, MarkdownTokenType.DefinitionList, MarkdownTokenType.LiteralInline])
            .Returns(new Type[] { typeof(Markdig.Extensions.SmartyPants.SmartyPant), typeof(Markdig.Extensions.DefinitionLists.DefinitionList), typeof(Markdig.Syntax.Inlines.LiteralInline) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.Figure, MarkdownTokenType.AutolinkInline])
            .Returns(new Type[] { typeof(Markdig.Extensions.Figures.Figure), typeof(Markdig.Syntax.Inlines.AutolinkInline) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.BlankLineBlock, MarkdownTokenType.FooterBlock, MarkdownTokenType.EmojiInline])
            .Returns(new Type[] { typeof(BlankLineBlock), typeof(Markdig.Extensions.Footers.FooterBlock), typeof(Markdig.Extensions.Emoji.EmojiInline) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.Footnote]).Returns(new Type[] { typeof(Markdig.Extensions.Footnotes.Footnote) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.FootnoteGroup]).Returns(new Type[] { typeof(Markdig.Extensions.Footnotes.FootnoteGroup) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.LinkReferenceDefinitionGroup, MarkdownTokenType.CustomContainerInline, MarkdownTokenType.LeafBlock])
            .Returns(new Type[] { typeof(LinkReferenceDefinitionGroup), typeof(Markdig.Extensions.CustomContainers.CustomContainerInline), typeof(LeafBlock) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.ListBlock, MarkdownTokenType.AutolinkInline, MarkdownTokenType.ContainerInline])
            .Returns(new Type[] { typeof(ListBlock), typeof(Markdig.Syntax.Inlines.AutolinkInline), typeof(Markdig.Syntax.Inlines.ContainerInline) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.ListItemBlock, MarkdownTokenType.LinkInline])
            .Returns(new Type[] { typeof(ListItemBlock), typeof(Markdig.Syntax.Inlines.LinkInline) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.MarkdownDocument, MarkdownTokenType.Figure])
            .Returns(new Type[] { typeof(MarkdownDocument), typeof(Markdig.Extensions.Figures.Figure) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.ParagraphBlock, MarkdownTokenType.QuoteBlock, MarkdownTokenType.CustomContainerInline])
            .Returns(new Type[] { typeof(ParagraphBlock), typeof(QuoteBlock), typeof(Markdig.Extensions.CustomContainers.CustomContainerInline) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.Table, MarkdownTokenType.ParagraphBlock])
            .Returns(new Type[] { typeof(Markdig.Extensions.Tables.Table), typeof(ParagraphBlock) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.TableCell, MarkdownTokenType.MathInline])
            .Returns(new Type[] { typeof(Markdig.Extensions.Tables.TableCell), typeof(Markdig.Extensions.Mathematics.MathInline) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.TableRow]).Returns(new Type[] { typeof(Markdig.Extensions.Tables.TableRow) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.Abbreviation, MarkdownTokenType.LeafBlock])
            .Returns(new Type[] { typeof(Markdig.Extensions.Abbreviations.Abbreviation), typeof(LeafBlock) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.SmartyPant, MarkdownTokenType.Abbreviation])
            .Returns(new Type[] { typeof(Markdig.Extensions.SmartyPants.SmartyPant), typeof(Markdig.Extensions.Abbreviations.Abbreviation) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.PipeTableDelimiterInline, MarkdownTokenType.CodeBlock, MarkdownTokenType.HtmlInline])
            .Returns(new Type[] { typeof(Markdig.Extensions.Tables.PipeTableDelimiterInline), typeof(CodeBlock), typeof(Markdig.Syntax.Inlines.HtmlInline) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.FencedCodeBlock, MarkdownTokenType.AbbreviationInline, MarkdownTokenType.FigureCaption])
            .Returns(new Type[] { typeof(FencedCodeBlock), typeof(Markdig.Extensions.Abbreviations.AbbreviationInline), typeof(Markdig.Extensions.Figures.FigureCaption) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.MathBlock]).Returns(new Type[] { typeof(Markdig.Extensions.Mathematics.MathBlock) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.YamlFrontMatterBlock]).Returns(new Type[] { typeof(Markdig.Extensions.Yaml.YamlFrontMatterBlock) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.DefinitionTerm, MarkdownTokenType.QuoteBlock])
            .Returns(new Type[] { typeof(Markdig.Extensions.DefinitionLists.DefinitionTerm), typeof(QuoteBlock) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.EmptyBlock, MarkdownTokenType.HeadingBlock, MarkdownTokenType.FootnoteLinkReferenceDefinition])
            .Returns(new Type[] { typeof(EmptyBlock), typeof(HeadingBlock), typeof(Markdig.Extensions.Footnotes.FootnoteLinkReferenceDefinition) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.HtmlAttributes, MarkdownTokenType.FigureCaption])
            .Returns(new Type[] { typeof(Markdig.Renderers.Html.HtmlAttributes), typeof(Markdig.Extensions.Figures.FigureCaption) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.HeadingBlock, MarkdownTokenType.Inline])
            .Returns(new Type[] { typeof(HeadingBlock), typeof(Markdig.Syntax.Inlines.Inline) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.HtmlBlock]).Returns(new Type[] { typeof(HtmlBlock) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.LiteralInline, MarkdownTokenType.LinkReferenceDefinition, MarkdownTokenType.HtmlAttributes])
            .Returns(new Type[] { typeof(Markdig.Syntax.Inlines.LiteralInline), typeof(LinkReferenceDefinition), typeof(Markdig.Renderers.Html.HtmlAttributes) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.HtmlInline, MarkdownTokenType.MathBlock, MarkdownTokenType.FootnoteLinkReferenceDefinition])
            .Returns(new Type[] { typeof(Markdig.Syntax.Inlines.HtmlInline), typeof(Markdig.Extensions.Mathematics.MathBlock), typeof(Markdig.Extensions.Footnotes.FootnoteLinkReferenceDefinition) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.HeadingLinkReferenceDefinition]).Returns(new Type[] { typeof(Markdig.Extensions.AutoIdentifiers.HeadingLinkReferenceDefinition) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.ParagraphBlock]).Returns(new Type[] { typeof(ParagraphBlock) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.LinkInline, MarkdownTokenType.ThematicBreakBlock])
            .Returns(new Type[] { typeof(Markdig.Syntax.Inlines.LinkInline), typeof(ThematicBreakBlock) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.AutolinkInline, MarkdownTokenType.Inline])
            .Returns(new Type[] { typeof(Markdig.Syntax.Inlines.AutolinkInline), typeof(Markdig.Syntax.Inlines.Inline) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.LiteralInline, MarkdownTokenType.ContainerInline])
            .Returns(new Type[] { typeof(Markdig.Syntax.Inlines.LiteralInline), typeof(Markdig.Syntax.Inlines.ContainerInline) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.FootnoteLink, MarkdownTokenType.Block])
            .Returns(new Type[] { typeof(Markdig.Extensions.Footnotes.FootnoteLink), typeof(Block) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.FencedCodeBlock, MarkdownTokenType.LeafInline, MarkdownTokenType.LiteralInline])
            .Returns(new Type[] { typeof(FencedCodeBlock), typeof(Markdig.Syntax.Inlines.LeafInline), typeof(Markdig.Syntax.Inlines.LiteralInline) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.LinkInline, MarkdownTokenType.AbbreviationInline])
            .Returns(new Type[] { typeof(Markdig.Syntax.Inlines.LinkInline), typeof(Markdig.Extensions.Abbreviations.AbbreviationInline) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.AutolinkInline]).Returns(new Type[] { typeof(Markdig.Syntax.Inlines.AutolinkInline) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.CodeInline, MarkdownTokenType.EmphasisInline, MarkdownTokenType.HtmlInline])
            .Returns(new Type[] { typeof(Markdig.Syntax.Inlines.CodeInline), typeof(Markdig.Syntax.Inlines.EmphasisInline), typeof(Markdig.Syntax.Inlines.HtmlInline) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.HtmlEntityInline]).Returns(new Type[] { typeof(Markdig.Syntax.Inlines.HtmlEntityInline) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.HtmlInline, MarkdownTokenType.Table])
            .Returns(new Type[] { typeof(Markdig.Syntax.Inlines.HtmlInline), typeof(Markdig.Extensions.Tables.Table) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.Footnote, MarkdownTokenType.LineBreakInline])
            .Returns(new Type[] { typeof(Markdig.Extensions.Footnotes.Footnote), typeof(Markdig.Syntax.Inlines.LineBreakInline) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.LiteralInline, MarkdownTokenType.FooterBlock])
            .Returns(new Type[] { typeof(Markdig.Syntax.Inlines.LiteralInline), typeof(Markdig.Extensions.Footers.FooterBlock) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.EmojiInline]).Returns(new Type[] { typeof(Markdig.Extensions.Emoji.EmojiInline) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.HeadingLinkReferenceDefinition, MarkdownTokenType.LeafBlock, MarkdownTokenType.MathInline])
            .Returns(new Type[] { typeof(Markdig.Extensions.AutoIdentifiers.HeadingLinkReferenceDefinition), typeof(LeafBlock), typeof(Markdig.Extensions.Mathematics.MathInline) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.SmartyPant, MarkdownTokenType.BlankLineBlock, MarkdownTokenType.DefinitionTerm])
            .Returns(new Type[] { typeof(Markdig.Extensions.SmartyPants.SmartyPant), typeof(BlankLineBlock), typeof(Markdig.Extensions.DefinitionLists.DefinitionTerm) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.TaskList, MarkdownTokenType.TaskList])
            .Returns(new Type[] { typeof(Markdig.Extensions.TaskLists.TaskList), typeof(Markdig.Extensions.TaskLists.TaskList) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.Figure, MarkdownTokenType.DelimiterInline])
            .Returns(new Type[] { typeof(Markdig.Extensions.Figures.Figure), typeof(Markdig.Syntax.Inlines.DelimiterInline) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.EmphasisDelimiterInline, MarkdownTokenType.EmptyBlock])
            .Returns(new Type[] { typeof(Markdig.Syntax.Inlines.EmphasisDelimiterInline), typeof(EmptyBlock) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.LinkDelimiterInline, MarkdownTokenType.HtmlInline])
            .Returns(new Type[] { typeof(Markdig.Syntax.Inlines.LinkDelimiterInline), typeof(Markdig.Syntax.Inlines.HtmlInline) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.LinkReferenceDefinition, MarkdownTokenType.PipeTableDelimiterInline, MarkdownTokenType.LinkReferenceDefinitionGroup])
            .Returns(new Type[] { typeof(LinkReferenceDefinition), typeof(Markdig.Extensions.Tables.PipeTableDelimiterInline), typeof(LinkReferenceDefinitionGroup) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.LeafInline, MarkdownTokenType.EmphasisInline])
            .Returns(new Type[] { typeof(Markdig.Syntax.Inlines.LeafInline), typeof(Markdig.Syntax.Inlines.EmphasisInline) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.CustomContainerInline, MarkdownTokenType.MathInline, MarkdownTokenType.EmojiInline])
            .Returns(new Type[] { typeof(Markdig.Extensions.CustomContainers.CustomContainerInline), typeof(Markdig.Extensions.Mathematics.MathInline), typeof(Markdig.Extensions.Emoji.EmojiInline) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.EmphasisDelimiterInline, MarkdownTokenType.YamlFrontMatterBlock, MarkdownTokenType.HtmlAttributes])
            .Returns(new Type[] { typeof(Markdig.Syntax.Inlines.EmphasisDelimiterInline), typeof(Markdig.Extensions.Yaml.YamlFrontMatterBlock), typeof(Markdig.Renderers.Html.HtmlAttributes) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.LinkInline, MarkdownTokenType.HeadingLinkReferenceDefinition])
            .Returns(new Type[] { typeof(Markdig.Syntax.Inlines.LinkInline), typeof(Markdig.Extensions.AutoIdentifiers.HeadingLinkReferenceDefinition) });
        yield return new TestCaseData((List<MarkdownTokenType>)[MarkdownTokenType.JiraLink]).Returns(new Type[] { typeof(Markdig.Extensions.JiraLinks.JiraLink) });
        yield return new TestCaseData((List<MarkdownTokenType>)[]).Returns(null);
        yield return new TestCaseData(null).Returns(null);
    }

    public static string GetExampleMarkdownText() => File.ReadAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestData\Example.md"));

    /*
        $i = -1; $MarkdownDocument | % {
            $t = $_.GetType();
            $i++;
            if ($t.Namespace -ceq 'Markdig.Syntax') {
                "[$i] = $($_.GetType().Name)";
            } else {
                "[$i] = $($_.GetType().FullName)";
            }
        }
        ($MarkdownDocument | % { $t = $_.GetType(); if ($t.Namespace -ceq 'Markdig.Syntax') { "typeof($($t.Name))" } else { "typeof($($t.FullName))" } }) -join ', '
        ($MarkdownDocument | % { "new($($_.Span.Start), $($_.Span.End))" }) -join ', '
        [0] = HeadingBlock
        [1] = ParagraphBlock
        [2] = ParagraphBlock
        [3] = ListBlock
        [4] = HeadingBlock
        [5] = ParagraphBlock
        [6] = HeadingBlock
        [7] = ParagraphBlock
        [8] = HeadingBlock
        [9] = ParagraphBlock
        [10] = ParagraphBlock
        [11] = LinkReferenceDefinitionGroup
        [12] = HeadingBlock
        [13] = ParagraphBlock
        [14] = ParagraphBlock
        [15] = ParagraphBlock
        [16] = HeadingBlock
        [17] = ParagraphBlock
        [18] = ParagraphBlock
        [19] = HeadingBlock
        [20] = ParagraphBlock
        [21] = FencedCodeBlock
        [22] = FencedCodeBlock
        [23] = HeadingBlock
        [24] = ParagraphBlock
        [25] = HeadingBlock
        [26] = ParagraphBlock
        [27] = ParagraphBlock
        [28] = HeadingBlock
        [29] = ParagraphBlock
        [30] = ParagraphBlock

        $HeadingBlock = $MarkdownDocument[0];
        Markdig.Renderers.Html
        $t = $HeadingBlock.Inline.GetType();
        $t.FullName;
        $Inline = $HeadingBlock.Inline;
        $t = $Inline.GetType();
        if ($t.Namespace -ceq 'Markdig.Syntax') { "typeof($($t.Name))" } else { "typeof($($t.FullName))" }
        "new($($HeadingBlock.Inline.Span.Start), $($HeadingBlock.Inline.Span.End))"
        $i = -1; @($Inline) | % {
            $t = $_.GetType();
            $i++;
            if ($t.Namespace -ceq 'Markdig.Syntax') {
                "[$i] = $($_.GetType().Name)";
            } else {
                "[$i] = $($_.GetType().FullName)";
            }
        }
        (@($Inline) | % { $t = $_.GetType(); if ($t.Namespace -ceq 'Markdig.Syntax') { "typeof($($t.Name))" } else { "typeof($($t.FullName))" } }) -join ', '
        (@($Inline) | % { "new($($_.Span.Start), $($_.Span.End))" }) -join ', '
        $HeadingBlock = $MarkdownDocument[4];
        $t = $HeadingBlock.Inline.GetType();
        $t.FullName;
        $Inline = $HeadingBlock.Inline;
        if ($t.Namespace -ceq 'Markdig.Syntax') { "typeof($($t.Name))" } else { "typeof($($t.FullName))" }
        "new($($HeadingBlock.Inline.Span.Start), $($HeadingBlock.Inline.Span.End))"
        $i = -1; @($Inline) | % {
            $t = $_.GetType();
            $i++;
            if ($t.Namespace -ceq 'Markdig.Syntax') {
                "[$i] = $($_.GetType().Name)";
            } else {
                "[$i] = $($_.GetType().FullName)";
            }
        }
        (@($Inline) | % { $t = $_.GetType(); if ($t.Namespace -ceq 'Markdig.Syntax') { "typeof($($t.Name))" } else { "typeof($($t.FullName))" } }) -join ', '
        (@($Inline) | % { "new($($_.Span.Start), $($_.Span.End))" }) -join ', '
    */
    public static MarkdownDocument GetExampleMarkdownDocument() => Markdig.Markdown.Parse(GetExampleMarkdownText(), true);

    public static System.Collections.IEnumerable GetGetChildObjectsTestData()
    {
        MarkdownDocument document = GetExampleMarkdownDocument();
        yield return new TestCaseData(document, new Type[]
        {
            typeof(HeadingBlock), typeof(ParagraphBlock), typeof(ParagraphBlock), typeof(ListBlock), typeof(HeadingBlock), typeof(ParagraphBlock), typeof(HeadingBlock), typeof(ParagraphBlock), typeof(HeadingBlock), typeof(ParagraphBlock),
            typeof(ParagraphBlock), typeof(LinkReferenceDefinition), typeof(HeadingBlock), typeof(ParagraphBlock), typeof(ParagraphBlock), typeof(ParagraphBlock), typeof(HeadingBlock), typeof(ParagraphBlock), typeof(ParagraphBlock),
            typeof(HeadingBlock), typeof(ParagraphBlock), typeof(FencedCodeBlock), typeof(FencedCodeBlock), typeof(HeadingBlock), typeof(ParagraphBlock), typeof(HeadingBlock), typeof(ParagraphBlock), typeof(ParagraphBlock), typeof(HeadingBlock),
            typeof(ParagraphBlock), typeof(ParagraphBlock)
        })
            .Returns(new SourceSpan[]
            {
                new(0, 26), new(31, 84), new(89, 110), new(115, 168), new(171, 199), new(204, 274), new(279, 285), new(290, 321), new(326, 352), new(357, 416), new(421, 432), new(437, 479), new(484, 490), new(495, 570), new(575, 660), new(665, 813),
                new(818, 835), new(840, 923), new(928, 990), new(995, 1001), new(1006, 1027), new(1032, 1071), new(1076, 1194), new(1199, 1216), new(1221, 1285), new(1290, 1301), new(1306, 1366), new(1371, 1446), new(1451, 1462), new(1467, 1484),
                new(1489, 1499)
            });
        HeadingBlock headingBlock = document.OfType<HeadingBlock>().First();

        yield return new TestCaseData(headingBlock, new Type[]
        {
            typeof(Markdig.Syntax.Inlines.ContainerInline)
        })
            .Returns(new SourceSpan[]
            {
                new(0, 0)
            });
        yield return new TestCaseData(headingBlock.Inline, new Type[]
        {
            typeof(Markdig.Syntax.Inlines.LiteralInline)
        })
            .Returns(new SourceSpan[]
            {
                new(0, 0)
            });
    }

    // public static System.Collections.IEnumerable GetData()
    // {
        
    // }

    // public static System.Collections.IEnumerable GetData()
    // {
        
    // }

    // public static System.Collections.IEnumerable GetData()
    // {
        
    // }
}