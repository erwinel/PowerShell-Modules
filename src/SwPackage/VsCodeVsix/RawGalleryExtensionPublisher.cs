using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace SwPackage.VsCodeVsix;

// Was RawVsCodeGalleryExtensionPublisher
public class RawGalleryExtensionPublisher
{
    private string _publisherId;
    private string _publisherName;
    private string _displayName;

    public string PublisherId
    {
        get => _publisherId; set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            _publisherId = value;
        }
    }

    public string PublisherName
    {
        get => _publisherName; set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            _publisherName = value;
        }
    }

    public string DisplayName
    {
        get => _displayName; set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            _displayName = value;
        }
    }

    public string? Domain { get; set; }

    public bool? IsDomainVerified { get; set; }

    public RawGalleryExtensionPublisher(string publisherId, string publisherName, string displayName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(publisherId);
        ArgumentException.ThrowIfNullOrWhiteSpace(publisherName);
        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);
        _publisherId = publisherId;
        _publisherName = publisherName;
        _displayName = displayName;
    }

    public static bool TryCreate(JsonObject publisherJson, [NotNullWhen(true)] out RawGalleryExtensionPublisher? publisher)
    {
        // "publisherId": "eed56242-9699-4317-8bc7-e9f4b9bdd3ff",
        // "publisherName": "redhat",
        // "displayName": "Red Hat",
        // "flags": "verified",
        // "domain": "https://redhat.com",
        // "isDomainVerified": true
        throw new NotImplementedException();
    }
}