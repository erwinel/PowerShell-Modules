using System.Diagnostics;
using Markdig.Syntax;

namespace HtmlUtility;

public partial class Select_MarkdownObject
{
    /// <summary>
    /// Returns the <paramref name="inputObject"/> if it is <see cref="HtmlAttributes" />; otherwise the <see cref="HtmlAttributes" /> of the <paramref name="inputObject"/> is returned, if present.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void AttributesInputObjAndDirectDesc(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        // TODO: Implement AttributesInputObjAndDirectDesc
        // DepthRange: Select-MarkdownObject -MinDepth 0 -MaxDepth 1 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes -MinDepth 0 -MaxDepth 1
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes -MinDepth 0 -MaxDepth 1 -IncludeAttributes
        // RecurseUnmatched: Select-MarkdownObject -Type HtmlAttributes -MinDepth 0 -MaxDepth 1 -RecurseUnmatchedOnly
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns all recursive descendant <see cref="HtmlAttributes" /> up to the specified <see cref="MaxDepth"/> from <paramref name="inputObject"/>.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void AttributesToDepth(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(MaxDepth > 1);
        // TODO: Implement AttributesToDepth
        // DepthRange: Select-MarkdownObject -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -MinDepth 1 -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes -MinDepth 1 -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes -MinDepth 1 -MaxDepth 2 -IncludeAttributes
        // RecurseUnmatched: Select-MarkdownObject -Type HtmlAttributes -MaxDepth 2 -RecurseUnmatchedOnly
        // RecurseUnmatched: Select-MarkdownObject -Type HtmlAttributes -MinDepth 1 -MaxDepth 2 -RecurseUnmatchedOnly
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the <paramref name="inputObject"/> if it is <see cref="HtmlAttributes" />; otherwise all recursive descendant <see cref="HtmlAttributes" /> up to the specified <see cref="MaxDepth"/>
    /// will be returned.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void AttributesToDepthInclInputObj(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(MaxDepth > 1);
        // TODO: Implement AttributesToDepthInclInputObj
        // DepthRange: Select-MarkdownObject -MinDepth 0 -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes -MinDepth 0 -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes -MinDepth 0 -MaxDepth 2 -IncludeAttributes
        // RecurseUnmatched: Select-MarkdownObject -Type HtmlAttributes -MinDepth 0 -MaxDepth 2 -RecurseUnmatchedOnly
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns all recursive descendant <see cref="HtmlAttributes" /> within the specified <see cref="MinDepth"/> range from <paramref name="inputObject"/>.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void AttributesInRange(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(MinDepth > 1);
        Debug.Assert(MaxDepth > MinDepth);
        // TODO: Implement AttributesInRange
        // DepthRange: Select-MarkdownObject -MinDepth 2 -MaxDepth 3 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes -MinDepth 2 -MaxDepth 3
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes -MinDepth 2 -MaxDepth 3 -IncludeAttributes
        // RecurseUnmatched: Select-MarkdownObject -Type HtmlAttributes -MinDepth 2 -MaxDepth 3 -RecurseUnmatchedOnly
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the <paramref name="inputObject"/> if it is <see cref="HtmlAttributes" />; otherwise all recursive descendant <see cref="HtmlAttributes" /> will be returned.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void AttributesInputObjAndAllDesc(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        // TODO: Implement AttributesInputObjAndAllDesc
        // DepthRange: Select-MarkdownObject -MinDepth 0 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes -MinDepth 0
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes -MinDepth 0 -IncludeAttributes
        // RecurseUnmatched: Select-MarkdownObject -Type HtmlAttributes -MinDepth 0 -RecurseUnmatchedOnly
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns all recursive descendant <see cref="HtmlAttributes" /> starting from the specified <see cref="MinDepth"/> from <paramref name="inputObject"/>.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void AttributesFromDepth(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(MinDepth > 1);
        // TODO: Implement AttributesFromDepth
        // DepthRange: Select-MarkdownObject -MinDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes -MinDepth 2
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes -MinDepth 2 -IncludeAttributes
        // RecurseUnmatched: Select-MarkdownObject -Type HtmlAttributes -MinDepth 2 -RecurseUnmatchedOnly
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the <paramref name="inputObject"/> if it is *NOT* <see cref="HtmlAttributes" />, along with all recursive descendants up to the specified <see cref="MaxDepth"/>,
    /// *NOT* including <see cref="HtmlAttributes" />.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void AnyTypeToDepthInclInputObj(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(MaxDepth > 1);
        // TODO: Implement AnyTypeToDepthInclInputObj
        // DepthRange: Select-MarkdownObject -MinDepth 0 -MaxDepth 2
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the <paramref name="inputObject"/> along with all recursive descendants up to the specified <see cref="MaxDepth"/>, including <see cref="HtmlAttributes" />.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void AnyTypePlusAttribToDepthInclInputObj(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(MaxDepth > 1);
        // TODO: Implement AnyTypePlusAttribToDepthInclInputObj
        // DepthRange: Select-MarkdownObject -Type Any -MinDepth 0 -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Any -MinDepth 0 -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Inline, Any -MinDepth 0 -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes, Any -MinDepth 0 -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes, Any -MinDepth 0 -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes, Any -MinDepth 0 -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes, Any -MinDepth 0 -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes, Any -MinDepth 0 -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes, Any -MinDepth 0 -MaxDepth 2 -IncludeAttributes
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the <paramref name="inputObject"/> if it is *NOT* <see cref="HtmlAttributes" />, along with all direct descendants, *NOT* including <see cref="HtmlAttributes" />.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void AnyTypeInputObjAndDirectDesc(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        // TODO: Implement AnyTypeInputObjAndDirectDesc
        // DepthRange: Select-MarkdownObject -MinDepth 0 -MaxDepth 1
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the <paramref name="inputObject"/>, along with all direct descendants, including <see cref="HtmlAttributes" />.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void AnyTypePlusAttribInputObjAndDirectDesc(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        // TODO: Implement AnyTypePlusAttribInputObjAndDirectDesc
        // DepthRange: Select-MarkdownObject -Type Any -MinDepth 0 -MaxDepth 1 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Any -MinDepth 0 -MaxDepth 1 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Inline, Any -MinDepth 0 -MaxDepth 1 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes, Any -MinDepth 0 -MaxDepth 1
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes, Any -MinDepth 0 -MaxDepth 1 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes, Any -MinDepth 0 -MaxDepth 1
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes, Any -MinDepth 0 -MaxDepth 1 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes, Any -MinDepth 0 -MaxDepth 1
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes, Any -MinDepth 0 -MaxDepth 1 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Any -MinDepth 0 -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type Block, Any -MinDepth 0 -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type Block, Inline, Any -MinDepth 0 -MaxDepth 2
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns all recursive descendants up to the specified <see cref="MaxDepth"/> from <paramref name="inputObject"/>, *NOT* including <see cref="HtmlAttributes" />.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void AnyTypeToDepth(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(MaxDepth > 1);
        // TODO: Implement AnyTypeToDepth
        // DepthRange: Select-MarkdownObject -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type Any -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type Block, Any -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type Block, Inline, Any -MaxDepth 2
        // DepthRange: Select-MarkdownObject -MinDepth 1 -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type Any -MinDepth 1 -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type Block, Any -MinDepth 1 -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type Block, Inline, Any -MinDepth 1 -MaxDepth 2
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns all recursive descendants, up to the specified <see cref="MaxDepth"/> from <paramref name="inputObject"/> that match the specified <see cref="_singleType"/>,
    /// along with all <see cref="HtmlAttributes" /> to that depth.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void AnyTypePlusAttribToDepth(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(MaxDepth > 1);
        // TODO: Implement AnyTypePlusAttribToDepth
        // DepthRange: Select-MarkdownObject -Type Any -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Any -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Inline, Any -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes, Any -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes, Any -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes, Any -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes, Any -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes, Any -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes, Any -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Any -MinDepth 0 -MaxDepth 1
        // DepthRange: Select-MarkdownObject -Type Block, Any -MinDepth 0 -MaxDepth 1
        // DepthRange: Select-MarkdownObject -Type Block, Inline, Any -MinDepth 0 -MaxDepth 1
        // DepthRange: Select-MarkdownObject -Type Any -MinDepth 1 -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Any -MinDepth 1 -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Inline, Any -MinDepth 1 -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes, Any -MinDepth 1 -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes, Any -MinDepth 1 -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes, Any -MinDepth 1 -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes, Any -MinDepth 1 -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes, Any -MinDepth 1 -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes, Any -MinDepth 1 -MaxDepth 2 -IncludeAttributes
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns all recursive descendants from the specified <see cref="MinDepth"/> to the <see cref="MaxDepth"/> from <paramref name="inputObject"/>, *NOT* including <see cref="HtmlAttributes" />.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void AnyTypeInRange(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(MinDepth > 1);
        Debug.Assert(MaxDepth > MinDepth);
        // TODO: Implement AnyTypeInRange
        // DepthRange: Select-MarkdownObject -MinDepth 2 -MaxDepth 3
        // DepthRange: Select-MarkdownObject -Type Any -MinDepth 2 -MaxDepth 3
        // DepthRange: Select-MarkdownObject -Type Block, Any -MinDepth 2 -MaxDepth 3
        // DepthRange: Select-MarkdownObject -Type Block, Inline, Any -MinDepth 2 -MaxDepth 3
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns all recursive descendants from the specified <see cref="MinDepth"/> to the <see cref="MaxDepth"/> from <paramref name="inputObject"/>, including <see cref="HtmlAttributes" />.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void AnyTypePlusAttribInRange(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(MinDepth > 1);
        Debug.Assert(MaxDepth > MinDepth);
        // TODO: Implement AnyTypePlusAttribInRange
        // DepthRange: Select-MarkdownObject -Type Any -MinDepth 2 -MaxDepth 3 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Any -MinDepth 2 -MaxDepth 3 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Inline, Any -MinDepth 2 -MaxDepth 3 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes, Any -MinDepth 2 -MaxDepth 3
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes, Any -MinDepth 2 -MaxDepth 3 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes, Any -MinDepth 2 -MaxDepth 3
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes, Any -MinDepth 2 -MaxDepth 3 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes, Any -MinDepth 2 -MaxDepth 3
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes, Any -MinDepth 2 -MaxDepth 3 -IncludeAttributes
        // RecurseUnmatched: Select-MarkdownObject -Type Block -MinDepth 1 -RecurseUnmatchedOnly
        // RecurseUnmatched: Select-MarkdownObject -Type Block, Inline -MinDepth 1 -RecurseUnmatchedOnly
        // RecurseUnmatched: Select-MarkdownObject -Type Block, HtmlAttributes -MinDepth 1 -RecurseUnmatchedOnly
        // RecurseUnmatched: Select-MarkdownObject -Type Block, Inline, HtmlAttributes -MinDepth 1 -RecurseUnmatchedOnly
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the <paramref name="inputObject"/> if it matches any of the specified <see cref="_multiTypes"/>, along with all recursive descendants, up to the specified <see cref="MaxDepth"/>,
    /// that match any of the specified types.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void MultiTypeToDepthInclInputObj(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(_multiTypes is not null);
        Debug.Assert(_multiTypes.Count > 1);
        Debug.Assert(MaxDepth > 1);
        // TODO: Implement MultiTypeToDepthInclInputObj
        // DepthRange: Select-MarkdownObject -Type Block, Inline -MinDepth 0 -MaxDepth 2
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the <paramref name="inputObject"/> if it is <see cref="HtmlAttributes" /> or it matches any of the specified <see cref="_multiTypes"/>, along with all recursive descendants,
    /// up to the specified <see cref="MaxDepth"/>, that match any of the specified types, along with all descendant <see cref="HtmlAttributes" /> up to that depth.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void MultiTypePlusAttribToDepthInclInputObj(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(_multiTypes is not null);
        Debug.Assert(_multiTypes.Count > 1);
        Debug.Assert(MaxDepth > 1);
        // TODO: Implement MultiTypePlusAttribToDepthInclInputObj
        // DepthRange: Select-MarkdownObject -Type Block, Inline -MinDepth 0 -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes -MinDepth 0 -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes -MinDepth 0 -MaxDepth 2 -IncludeAttributes
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the <paramref name="inputObject"/> if it matches any of the specified <see cref="_multiTypes"/>, along with all direct descendants that match any of the specified types,
    /// irregardless of whether the <paramref name="inputObject"/> was a match.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void MultiTypeInputObjAndDirectDesc(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(_multiTypes is not null);
        Debug.Assert(_multiTypes.Count > 1);
        // TODO: Implement MultiTypeInputObjAndDirectDesc
        // DepthRange: Select-MarkdownObject -Type Block, Inline -MinDepth 0 -MaxDepth 1
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the <paramref name="inputObject"/> if it is <see cref="HtmlAttributes" /> or it matches any of the specified <see cref="_multiTypes"/>,
    /// along with the <see cref="HtmlAttributes" /> of the <paramref name="inputObject"/> and all direct descendants that match any of the specified types,
    /// irregardless of whether the <paramref name="inputObject"/> was a match.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void MultiTypePlusAttribInputObjAndDirectDesc(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(_multiTypes is not null);
        Debug.Assert(_multiTypes.Count > 1);
        // TODO: Implement MultiTypePlusAttribInputObjAndDirectDesc
        // DepthRange: Select-MarkdownObject -Type Block, Inline -MinDepth 0 -MaxDepth 1 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes -MinDepth 0 -MaxDepth 1
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes -MinDepth 0 -MaxDepth 1 -IncludeAttributes
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns all recursive descendants, up to the specified <see cref="MaxDepth"/> from <paramref name="inputObject"/> that match any of the specified <see cref="_multiTypes"/>.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void MultiTypeToDepth(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(_multiTypes is not null);
        Debug.Assert(_multiTypes.Count > 1);
        Debug.Assert(MaxDepth > 1);
        // TODO: Implement MultiTypeToDepth
        // DepthRange: Select-MarkdownObject -Type Block, Inline -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type Block, Inline -MinDepth 1 -MaxDepth 2
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns all recursive descendants, up to the specified <see cref="MaxDepth"/> from <paramref name="inputObject"/> that match any of the specified <see cref="_multiTypes"/>,
    /// along with all <see cref="HtmlAttributes" /> to that depth.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void MultiTypePlusAttribToDepth(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(_multiTypes is not null);
        Debug.Assert(_multiTypes.Count > 1);
        Debug.Assert(MaxDepth > 1);
        // TODO: Implement MultiTypePlusAttribToDepth
        // DepthRange: Select-MarkdownObject -Type Block, Inline -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Inline -MinDepth 1 -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes -MinDepth 1 -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes -MinDepth 1 -MaxDepth 2 -IncludeAttributes
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns all recursive descendants, from the specified <see cref="MinDepth"/> to the <see cref="MaxDepth"/> from <paramref name="inputObject"/> that match any of the
    /// specified <see cref="_multiTypes"/>.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void MultiTypeInRange(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(_multiTypes is not null);
        Debug.Assert(_multiTypes.Count > 1);
        Debug.Assert(MinDepth > 1);
        Debug.Assert(MaxDepth > MinDepth);
        // TODO: Implement MultiTypeInRange
        // DepthRange: Select-MarkdownObject -Type Block, Inline -MinDepth 2 -MaxDepth 3
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns all recursive descendants, from the specified <see cref="MinDepth"/> to the <see cref="MaxDepth"/> from <paramref name="inputObject"/> that match any of the
    /// specified <see cref="_multiTypes"/>, along with all <see cref="HtmlAttributes" /> within that range.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void MultiTypePlusAttribInRange(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(_multiTypes is not null);
        Debug.Assert(_multiTypes.Count > 1);
        Debug.Assert(MinDepth > 1);
        Debug.Assert(MaxDepth > MinDepth);
        // TODO: Implement MultiTypePlusAttribInRange
        // DepthRange: Select-MarkdownObject -Type Block, Inline -MinDepth 2 -MaxDepth 3 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes -MinDepth 2 -MaxDepth 3
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes -MinDepth 2 -MaxDepth 3 -IncludeAttributes
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the <paramref name="inputObject"/> if it matches the specified <see cref="_singleType"/>, along with all recursive descendants, up to the specified <see cref="MaxDepth"/>,
    /// that match the specified type.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void SingleTypeToDepthInclInputObj(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(_singleType is not null);
        Debug.Assert(MaxDepth > 1);
        // TODO: Implement SingleTypeToDepthInclInputObj
        // DepthRange: Select-MarkdownObject -Type Block -MinDepth 0 -MaxDepth 2
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the <paramref name="inputObject"/> if it is <see cref="HtmlAttributes" /> or it matches the specified <see cref="_singleType"/>, along with all recursive descendants,
    /// up to the specified <see cref="MaxDepth"/>, that match the specified type, along with all descendant <see cref="HtmlAttributes" /> up to that depth.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void SingleTypePlusAttribToDepthInclInputObj(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(_singleType is not null);
        Debug.Assert(MaxDepth > 1);
        // TODO: Implement SingleTypePlusAttribToDepthInclInputObj
        // DepthRange: Select-MarkdownObject -Type Block -MinDepth 0 -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes -MinDepth 0 -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes -MinDepth 0 -MaxDepth 2 -IncludeAttributes
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the <paramref name="inputObject"/> if it matches the specified <see cref="_singleType"/>, along with all direct descendants that match the specified type,
    /// irregardless of whether the <paramref name="inputObject"/> was a match.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void SingleTypeInputObjAndDirectDesc(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(_singleType is not null);
        // TODO: Implement SingleTypeInputObjAndDirectDesc
        // DepthRange: Select-MarkdownObject -Type Block -MinDepth 0 -MaxDepth 1
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the <paramref name="inputObject"/> if it is <see cref="HtmlAttributes" /> or it matches the specified <see cref="_singleType"/>,
    /// along with the <see cref="HtmlAttributes" /> of the <paramref name="inputObject"/> and all direct descendants that match the specified type,
    /// irregardless of whether the <paramref name="inputObject"/> was a match.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void SingleTypePlusAttribInputObjAndDirectDesc(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(_singleType is not null);
        // TODO: Implement SingleTypePlusAttribInputObjAndDirectDesc
        // DepthRange: Select-MarkdownObject -Type Block -MinDepth 0 -MaxDepth 1 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes -MinDepth 0 -MaxDepth 1
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes -MinDepth 0 -MaxDepth 1 -IncludeAttributes
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns all recursive descendants, up to the specified <see cref="MaxDepth"/> from <paramref name="inputObject"/> that match the specified <see cref="_singleType"/>.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void SingleTypeToDepth(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(_singleType is not null);
        Debug.Assert(MaxDepth > 1);
        // TODO: Implement SingleTypeToDepth
        // DepthRange: Select-MarkdownObject -Type Block -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type Block -MinDepth 1 -MaxDepth 2
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns all recursive descendants, up to the specified <see cref="MaxDepth"/> from <paramref name="inputObject"/> that match the specified <see cref="_singleType"/>,
    /// along with all <see cref="HtmlAttributes" /> to that depth.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void SingleTypePlusAttribToDepth(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(_singleType is not null);
        Debug.Assert(MaxDepth > 1);
        // TODO: Implement SingleTypePlusAttribToDepth
        // DepthRange: Select-MarkdownObject -Type Block -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block -MinDepth 1 -MaxDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes -MinDepth 1 -MaxDepth 2
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes -MinDepth 1 -MaxDepth 2 -IncludeAttributes
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns all recursive descendants, within the specified <see cref="MinDepth"/> range from <paramref name="inputObject"/> that match the specified <see cref="_singleType"/>.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void SingleTypeInRange(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(_singleType is not null);
        Debug.Assert(MinDepth > 1);
        Debug.Assert(MaxDepth > MinDepth);
        // TODO: Implement SingleTypeInRange
        // DepthRange: Select-MarkdownObject -Type Block -MinDepth 2 -MaxDepth 3
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns all recursive descendants, within the specified <see cref="MinDepth"/> range from <paramref name="inputObject"/> that match the specified <see cref="_singleType"/>,
    /// along with all <see cref="HtmlAttributes" /> within that range.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void SingleTypePlusAttribInRange(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(_singleType is not null);
        Debug.Assert(MinDepth > 1);
        Debug.Assert(MaxDepth > MinDepth);
        // TODO: Implement SingleTypePlusAttribInRange
        // DepthRange: Select-MarkdownObject -Type Block -MinDepth 2 -MaxDepth 3 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes -MinDepth 2 -MaxDepth 3
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes -MinDepth 2 -MaxDepth 3 -IncludeAttributes
        // RecurseUnmatched: Select-MarkdownObject -Type Block, HtmlAttributes -MinDepth 2 -MaxDepth 3 -RecurseUnmatchedOnly
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the <paramref name="inputObject"/> if it is *NOT* <see cref="HtmlAttributes" />, along with all recursive descendants, *NOT* including <see cref="HtmlAttributes" />.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void AnyTypeInputObjAndAllDesc(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        // TODO: Implement AnyTypeInputObjAndAllDesc
        // DepthRange: Select-MarkdownObject -MinDepth 0
        // DepthRange: Select-MarkdownObject -Type Any -MinDepth 0
        // DepthRange: Select-MarkdownObject -Type Block, Any -MinDepth 0
        // DepthRange: Select-MarkdownObject -Type Block, Inline, Any -MinDepth 0
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the <paramref name="inputObject"/> along with all recursive descendants, including <see cref="HtmlAttributes" />.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void AnyTypePlusAttribInputObjAndAllDesc(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        // TODO: Implement AnyTypePlusAttribInputObjAndAllDesc
        // DepthRange: Select-MarkdownObject -Type Any -MinDepth 0 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Any -MinDepth 0 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Inline, Any -MinDepth 0 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes, Any -MinDepth 0
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes, Any -MinDepth 0 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes, Any -MinDepth 0
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes, Any -MinDepth 0 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes, Any -MinDepth 0
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes, Any -MinDepth 0 -IncludeAttributes
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns all recursive descendants starting from the specified <see cref="MinDepth"/> from <paramref name="inputObject"/>, *NOT* including <see cref="HtmlAttributes" />.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void AnyTypeFromDepth(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(MinDepth > 1);
        // TODO: Implement AnyTypeFromDepth
        // DepthRange: Select-MarkdownObject -MinDepth 2
        // DepthRange: Select-MarkdownObject -Type Any -MinDepth 2
        // DepthRange: Select-MarkdownObject -Type Block, Any -MinDepth 2
        // DepthRange: Select-MarkdownObject -Type Block, Inline, Any -MinDepth 2
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns all recursive descendants starting from the specified <see cref="MinDepth"/> from <paramref name="inputObject"/>, including <see cref="HtmlAttributes" />.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void AnyTypePlusAttribFromDepth(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(MinDepth > 1);
        // TODO: Implement AnyTypePlusAttribFromDepth
        // DepthRange: Select-MarkdownObject -Type Any -MinDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Any -MinDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Inline, Any -MinDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes, Any -MinDepth 2
        // DepthRange: Select-MarkdownObject -Type HtmlAttributes, Any -MinDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes, Any -MinDepth 2
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes, Any -MinDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes, Any -MinDepth 2
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes, Any -MinDepth 2 -IncludeAttributes
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the <paramref name="inputObject"/> if it matches any of the specified <see cref="_multiTypes"/>, along with all recursive descendants that match any of the specified types.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void MultiTypeInputObjAndAllDesc(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        // TODO: Implement MultiTypeInputObjAndAllDesc
        // DepthRange: Select-MarkdownObject -Type Block, Inline -MinDepth 0
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the <paramref name="inputObject"/> if it is <see cref="HtmlAttributes" /> or it matches any of the specified <see cref="_multiTypes"/>,
    /// along with all recursive descendants that match any of the specified types, along with all descendant <see cref="HtmlAttributes" />.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void MultiTypePlusAttribInputObjAndAllDesc(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        // TODO: Implement MultiTypePlusAttribInputObjAndAllDesc
        // DepthRange: Select-MarkdownObject -Type Block, Inline -MinDepth 0 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes -MinDepth 0
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes -MinDepth 0 -IncludeAttributes
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns all recursive descendants, starting from the specified <see cref="MinDepth"/> from <paramref name="inputObject"/> that match any of the specified <see cref="_multiTypes"/>.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void MultiTypeFromDepth(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(MinDepth > 1);
        // TODO: Implement MultiTypeFromDepth
        // DepthRange: Select-MarkdownObject -Type Block, Inline -MinDepth 2
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns all recursive descendants, starting from the specified <see cref="MinDepth"/> from <paramref name="inputObject"/> that match any of the specified <see cref="_multiTypes"/>,
    /// along with all <see cref="HtmlAttributes" /> from that depth.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void MultiTypePlusAttribFromDepth(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(MinDepth > 1);
        // TODO: Implement MultiTypePlusAttribFromDepth
        // DepthRange: Select-MarkdownObject -Type Block, Inline -MinDepth 2 -IncludeAttributes
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the <paramref name="inputObject"/> if it matches the specified <see cref="_singleType"/>, along with all recursive descendants that match the specified type.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void SingleTypeInputObjAndAllDesc(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        // TODO: Implement SingleTypeInputObjAndAllDesc
        // DepthRange: Select-MarkdownObject -Type Block -MinDepth 0
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the <paramref name="inputObject"/> if it is <see cref="HtmlAttributes" /> or it matches the specified <see cref="_singleType"/>,
    /// along with all recursive descendants that match the specified type, along with all descendant <see cref="HtmlAttributes" />.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void SingleTypePlusAttribInputObjAndAllDesc(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        // TODO: Implement SingleTypePlusAttribInputObjAndAllDesc
        // DepthRange: Select-MarkdownObject -Type Block -MinDepth 0 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes -MinDepth 0
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes -MinDepth 0 -IncludeAttributes
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns all recursive descendants, starting from the specified <see cref="MinDepth"/> from <paramref name="inputObject"/> that match the specified <see cref="_singleType"/>.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void SingleTypeFromDepth(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(MinDepth > 1);
        // TODO: Implement SingleTypeFromDepth
        // DepthRange: Select-MarkdownObject -Type Block -MinDepth 2
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns all recursive descendants, starting from the specified <see cref="MinDepth"/> from <paramref name="inputObject"/> that match the specified <see cref="_singleType"/>,
    /// along with all <see cref="HtmlAttributes" /> from that depth.
    /// </summary>
    /// <param name="inputObject">The <see cref="MarkdownObject"/> to process.</param>
    private void SingleTypePlusAttribFromDepth(MarkdownObject inputObject)
    {
        Debug.Assert(inputObject is not null);
        Debug.Assert(MinDepth > 1);
        // TODO: Implement SingleTypePlusAttribFromDepth
        // DepthRange: Select-MarkdownObject -Type Block -MinDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes -MinDepth 2
        // DepthRange: Select-MarkdownObject -Type Block, HtmlAttributes -MinDepth 2 -IncludeAttributes
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes -MinDepth 2
        // DepthRange: Select-MarkdownObject -Type Block, Inline, HtmlAttributes -MinDepth 2 -IncludeAttributes
        throw new NotImplementedException();
    }

    private void BeginProcessing_DepthRange(List<Type>? types, int minDepth, int maxDepth, bool includeAttributes)
    {
        Debug.Assert(types is null || types.Count > 0);
        Debug.Assert(maxDepth >= minDepth);
        if (maxDepth > minDepth)
        {
            if (types is null)
                _processInputObject = minDepth switch
                {
                    0 => (maxDepth > 1) ? (includeAttributes ? AnyTypePlusAttribToDepthInclInputObj : AnyTypeToDepthInclInputObj) :
                                                includeAttributes ? AnyTypePlusAttribInputObjAndDirectDesc : AnyTypeInputObjAndDirectDesc,
                    1 => includeAttributes ? AnyTypePlusAttribToDepth : AnyTypeToDepth,
                    _ => includeAttributes ? AnyTypePlusAttribInRange : AnyTypeInRange,
                };
            else if (types.Count > 0)
            {
                _multiTypes = types;
                _processInputObject = minDepth switch
                {
                    0 => (maxDepth > 1) ? (includeAttributes ? MultiTypePlusAttribToDepthInclInputObj : MultiTypeToDepthInclInputObj) :
                                                includeAttributes ? MultiTypePlusAttribInputObjAndDirectDesc : MultiTypeInputObjAndDirectDesc,
                    1 => includeAttributes ? MultiTypePlusAttribToDepth : MultiTypeToDepth,
                    _ => includeAttributes ? MultiTypePlusAttribInRange : MultiTypeInRange,
                };
            }
            else
            {
                _singleType = types[0];
                _processInputObject = minDepth switch
                {
                    0 => (maxDepth > 1) ? (includeAttributes ? SingleTypePlusAttribToDepthInclInputObj : SingleTypeToDepthInclInputObj) :
                                                includeAttributes ? SingleTypePlusAttribInputObjAndDirectDesc : SingleTypeInputObjAndDirectDesc,
                    1 => includeAttributes ? SingleTypePlusAttribToDepth : SingleTypeToDepth,
                    _ => includeAttributes ? SingleTypePlusAttribInRange : SingleTypeInRange,
                };
            }
        }
        else
            BeginProcessing_ExplicitDepth(types, minDepth, includeAttributes);
    }

    private void BeginProcessing_DepthRange(List<Type>? types, int minDepth, bool includeAttributes)
    {
        Debug.Assert(types is null || types.Count > 0);
        if (types is null)
            _processInputObject = minDepth switch
            {
                0 => includeAttributes ? AnyTypePlusAttribInputObjAndAllDesc : AnyTypeInputObjAndAllDesc,
                1 => includeAttributes ? AnyTypePlusAttrib : AnyType,
                _ => includeAttributes ? AnyTypePlusAttribFromDepth : AnyTypeFromDepth,
            };
        else if (types.Count > 0)
        {
            _multiTypes = types;
            _processInputObject = minDepth switch
            {
                0 => includeAttributes ? MultiTypePlusAttribInputObjAndAllDesc : MultiTypeInputObjAndAllDesc,
                1 => includeAttributes ? MultiTypePlusAttrib : MultiType,
                _ => includeAttributes ? MultiTypePlusAttribFromDepth : MultiTypeFromDepth,
            };
        }
        else
        {
            _singleType = types[0];
            _processInputObject = minDepth switch
            {
                0 => includeAttributes ? SingleTypePlusAttribInputObjAndAllDesc : SingleTypeInputObjAndAllDesc,
                1 => includeAttributes ? SingleTypePlusAttrib : SingleType,
                _ => includeAttributes ? SingleTypePlusAttribFromDepth : SingleTypeFromDepth,
            };
        }
    }

    private void BeginProcessing_NoRecursion(List<Type>? types, bool includeAttributes)
    {
        Debug.Assert(types is null || types.Count > 0);
        if (types is null)
            _processInputObject = includeAttributes ? AnyTypePlusAttribDirectDesc : AnyTypeDirectDesc;
        else if (types.Count > 1)
        {
            _multiTypes = types;
            _processInputObject = includeAttributes ? MultiTypePlusAttribDirectDesc : MultiTypeDirectDesc;
        }
        else
        {
            _singleType = types[0];
            _processInputObject = includeAttributes ? SingleTypePlusAttribDirectDesc : SingleTypeDirectDesc;
        }
    }
}