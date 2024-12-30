using System.Reflection;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace HtmlUtility;

public static partial class MarkdownExtensionMethods
{
    public static Type ToReflectionType(this MarkdownTokenType type) => _markdownTokenTypeMap[type];

    public static List<Type>? ToReflectionTypes(this IList<MarkdownTokenType>? source)
    {
        if (source is null || source.Count == 0) return null;
        var types = source.Distinct().Select(ToReflectionType).ToList();
        for (int end = 1; end < types.Count; end++)
        {
            var x = types[end];
            for (int n = 0; n < end; n++)
            {
                var y = types[n];
                if (x.IsAssignableFrom(y))
                {
                    types.RemoveAt(n);
                    end--;
                    break;
                }
                if (y.IsAssignableFrom(x))
                {
                    types.RemoveAt(end);
                    end--;
                    break;
                }
            }
        }
        return types;
    }

    public static IEnumerable<(MarkdownObject Parent, HtmlAttributes Attribute)> AllAttributes(this MarkdownObject? source)
    {
        if (source is not null)
        {
            foreach (MarkdownObject parent in source.Descendants())
            {
                var attr = parent.TryGetAttributes();
                if (attr is not null)
                    yield return (parent, attr);
            }
        }
    }

    /// <summary>
    /// Searches for descendants that match a specified type, ignoring nested descendants of matching markdown objects.
    /// </summary>
    /// <param name="source">The prospective parent markdown object.</param>
    /// <param name="type">The object type to search for.</param>
    /// <param name="emitAttributesofUnmatched">Whether to emit <see cref="HtmlAttributes"/> of unmatched tokens.</param>
    /// <returns>The <see cref="MarkdownObject"/>s that descend from the <paramref name="source"/> object that is an instance of the specified <paramref name="type"/>,
    /// except for any that have an ancestor that has already been yielded.</returns>
    public static IEnumerable<MarkdownObject> DescendantBranchesMatchingType(this MarkdownObject? source, Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        if (source is null) return [];
        if (type == MarkdownObjectType)
            return source.Descendants();
        if (type.IsNonAttributeMarkdownObjectType())
        {
            if (source is ContainerBlock containerBlock)
                return GetBranches(containerBlock, type.IsInstanceOfType);
            if (source is ContainerInline containerInline)
                return GetBranches(containerInline, type.IsInstanceOfType);
            if (source is LeafBlock leafBlock)
                return (leafBlock.Inline is null) ? [] : GetBranches(leafBlock.Inline, type.IsInstanceOfType);
        }
        
        return [];
    }

    /// <summary>
    /// Searches for descendants tht match any of the specified types, ignoring nested descendants of matching markdown objects.
    /// </summary>
    /// <param name="source">The prospective parent markdown object.</param>
    /// <param name="types">The object types to search for.</param>
    /// <param name="emitAttributesofUnmatched">Whether to emit <see cref="HtmlAttributes"/> of unmatched tokens.</param>
    /// <returns>The <see cref="MarkdownObject"/>s that descend from the <paramref name="source"/> object that is an instance of any of the specified <paramref name="types"/>,
    /// except for any that have an ancestor that has already been yielded.</returns>
    public static IEnumerable<MarkdownObject> DescendantBranchesMatchingType(this MarkdownObject? source, IEnumerable<Type> types)
    {
        ArgumentNullException.ThrowIfNull(types);
        if (source is null) return [];
        types = types.Where(IsNonAttributeMarkdownObjectType).CollapseTypes(out int typeCount);
        if (typeCount == 1)
        {
            Type singleType = types.First();
            if (singleType == MarkdownObjectType)
                return source.Descendants();
            if (source is ContainerBlock containerBlock)
                return GetBranches(containerBlock, singleType.IsInstanceOfType);
            if (source is ContainerInline containerInline)
                return GetBranches(containerInline, singleType.IsInstanceOfType);
            if (source is LeafBlock leafBlock)
                return (leafBlock.Inline is null) ? [] : GetBranches(leafBlock.Inline, singleType.IsInstanceOfType);
        }
        else if (typeCount > 0)
        {
            if (source is ContainerBlock containerBlock)
                return GetBranches(containerBlock, obj => types.Any(t => t.IsInstanceOfType(obj)));
            if (source is ContainerInline containerInline)
                return GetBranches(containerInline, obj => types.Any(t => t.IsInstanceOfType(obj)));
            if (source is LeafBlock leafBlock)
                return (leafBlock.Inline is null) ? [] : GetBranches(leafBlock.Inline, obj => types.Any(t => t.IsInstanceOfType(obj)));
        }

        return [];
    }

    /// <summary>
    /// Searches for descendants, up to a specified recursion depth, which match the specifie type, ignoring nested descendants of matching markdown objects.
    /// </summary>
    /// <param name="source">The prospective parent markdown object.</param>
    /// <param name="type">The object type to search for.</param>
    /// <param name="maximumDepth">The maximum number of times to recurse into nested child objects.</param>
    /// <param name="emitAttributesofUnmatched">Whether to emit <see cref="HtmlAttributes"/> of unmatched tokens.</param>
    /// <returns>The <see cref="MarkdownObject"/>s that descend from the <paramref name="source"/> object that is an instance of the specified <paramref name="type"/>,
    /// except for any that have an ancestor that has already been yielded or are beyond the specified <paramref name="maximumDepth"/>.</returns>
    /// <remarks>If <paramref name="maximumDepth"/> is less than <c>1</c>, nothing will be yielded.</remarks>
    public static IEnumerable<MarkdownObject> DescendantBranchesMatchingType(this MarkdownObject? source, Type type, int maximumDepth)
    {
        ArgumentNullException.ThrowIfNull(type);
        if (source is null || maximumDepth < 1) return [];
        if (type == MarkdownObjectType)
            return source.Descendants();
        if (type.IsNonAttributeMarkdownObjectType())
        {
            if (source is ContainerBlock containerBlock)
                return GetBranchesToDepth(containerBlock, maximumDepth, type.IsInstanceOfType);
            if (source is ContainerInline containerInline)
                return GetBranchesToDepth(containerInline, maximumDepth, type.IsInstanceOfType);
            if (source is LeafBlock leafBlock)
                return GetBranchesToDepth(leafBlock, maximumDepth, type.IsInstanceOfType);
        }

        return [];
    }

    /// <summary>
    /// Searches for descendants, up to a specified recursion depth, which match any of the specified types, ignoring nested descendants of matching markdown objects.
    /// </summary>
    /// <param name="source">The prospective parent markdown object.</param>
    /// <param name="types">The object types to search for.</param>
    /// <param name="maximumDepth">The maximum number of times to recurse into nested child objects.</param>
    /// <param name="emitAttributesofUnmatched">Whether to emit <see cref="HtmlAttributes"/> of unmatched tokens.</param>
    /// <returns>The <see cref="MarkdownObject"/>s that descend from the <paramref name="source"/> object that is an instance of any of the specified <paramref name="types"/>,
    /// except for any that have an ancestor that has already been yielded or are beyond the specifed <paramref name="maximumDepth"/>.</returns>
    /// <remarks>If <paramref name="maximumDepth"/> is less than <c>1</c>, nothing will be yielded.</remarks>
    public static IEnumerable<MarkdownObject> DescendantBranchesMatchingType(this MarkdownObject? source, IEnumerable<Type> types, int maximumDepth)
    {
        ArgumentNullException.ThrowIfNull(types);
        if (source is null || maximumDepth < 1) return [];
        types = types.Where(IsNonAttributeMarkdownObjectType).CollapseTypes(out int typeCount);
        if (typeCount == 0) return [];
        if (typeCount == 1)
        {
            Type type = types.First();
            if (type == MarkdownObjectType)
                return source.Descendants();
            if (source is ContainerBlock containerBlock)
                return GetBranchesToDepth(containerBlock, maximumDepth, type.IsInstanceOfType);
            if (source is ContainerInline containerInline)
                return GetBranchesToDepth(containerInline, maximumDepth, type.IsInstanceOfType);
            if (source is LeafBlock leafBlock)
                return GetBranchesToDepth(leafBlock, maximumDepth, type.IsInstanceOfType);
        }
        else
        {
            if (source is ContainerBlock containerBlock)
                return GetBranchesToDepth(containerBlock, maximumDepth, obj => types.Any(t => t.IsInstanceOfType(obj)));
            if (source is ContainerInline containerInline)
                return GetBranchesToDepth(containerInline, maximumDepth, obj => types.Any(t => t.IsInstanceOfType(obj)));
            if (source is LeafBlock leafBlock)
                return GetBranchesToDepth(leafBlock, maximumDepth, obj => types.Any(t => t.IsInstanceOfType(obj)));
        }

        return [];
    }

    /// <summary>
    /// Gets descendents which exist at a specified recursion depth.
    /// </summary>
    /// <param name="source">The prospective parent markdown object.</param>
    /// <param name="depth">The number of times to recurse into child markdown objects.</param>
    /// <returns>The <see cref="MarkdownObject"/>s that exist at the specified recursion <paramref name="depth"/>.</returns>
    /// <param name="includeAttributes">Whether to include <see cref="HtmlAttributes"/>.</param>
    /// <remarks>If <paramref name="depth"/> is less than <c>1</c>, nothing will be yielded.</remarks>
    public static IEnumerable<MarkdownObject> DescendantsAtDepth(this MarkdownObject? source, int depth, bool includeAttributes = false)
    {
        if (source is null || depth < 1) return [];
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets descendents which exists at or beyond a specified recursion depth.
    /// </summary>
    /// <param name="source">The prospective parent markdown object.</param>
    /// <param name="minimumDepth">The number of to recurse into child objects before yielding descendant markdown objects.</param>
    /// <param name="includeAttributes">Whether to include <see cref="HtmlAttributes"/>.</param>
    /// <returns>The <see cref="MarkdownObject"/>s that exist at or beyond the specified recursion <paramref name="minimumDepth"/>.</returns>
    /// <remarks>If <paramref name="minimumDepth"/> is less than <c>1</c>, nothing will be yielded.</remarks>
    public static IEnumerable<MarkdownObject> DescendantsFromDepth(this MarkdownObject? source, int minimumDepth, bool includeAttributes = false)
    {
        if (source is null) return [];
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets descendants that exist at or below the specified recursion depth.
    /// </summary>
    /// <param name="source">The prospective parent markdown object.</param>
    /// <param name="maximumDepth">The maximum number of times to recurse into nested child objects.</param>
    /// <returns>The <see cref="MarkdownObject"/>s that exist at or below the specified recursion <paramref name="maximumDepth"/>.</returns>
    /// <param name="includeAttributes">Whether to include <see cref="HtmlAttributes"/>.</param>
    /// <remarks>If <paramref name="minimumDepth"/> is less than <c>1</c>, nothing will be yielded.</remarks>
    public static IEnumerable<MarkdownObject> DescendantsUpToDepth(this MarkdownObject? source, int maximumDepth, bool includeAttributes = false)
    {
        if (source is null || maximumDepth < 1) return [];
        throw new NotImplementedException();
    }

    public static IEnumerable<MarkdownObject> DescendantsInDepthRange(this MarkdownObject? source, int minimumDepth, int maximumDepth, bool includeAttributes = false)
    {
        if (source is null || maximumDepth < 1 || maximumDepth < minimumDepth) return [];
        throw new NotImplementedException();
    }

    public static IEnumerable<MarkdownObject> DescendantBranchesMatchingType(this MarkdownObject? source, Type type, int minimumDepth, int maximumDepth)
    {
        ArgumentNullException.ThrowIfNull(type);
        if (source is null || maximumDepth < 1 || maximumDepth < minimumDepth) return [];
        throw new NotImplementedException();
    }

    public static IEnumerable<MarkdownObject> DescendantBranchesMatchingType(this MarkdownObject? source, IEnumerable<Type> types, int minimumDepth, int maximumDepth)
    {
        ArgumentNullException.ThrowIfNull(types);
        if (source is null || minimumDepth < 1 || maximumDepth < minimumDepth) return [];
        types = types.Where(IsNonAttributeMarkdownObjectType).CollapseTypes(out int typeCount);
        if (typeCount == 0) return [];
        throw new NotImplementedException();
    }
}