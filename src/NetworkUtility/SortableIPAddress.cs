using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace NetworkUtility;

public class SortableIPAddress : IEquatable<SortableIPAddress>, IEquatable<IPAddress>, IComparable<SortableIPAddress>, IComparable<IPAddress>, IComparable
{
    private readonly bool _isIPv4;
    private readonly long _scopeId;

    public bool IsIPv6 { get; }
    public bool IsIPv4MappedToIPv6 => IsIPv6 && _isIPv4;
    public ImmutableArray<byte> AddressBytes { get; }
    public long? ScopeId
    {
        get
        {
            if (IsIPv6) return _scopeId;
            return null;
        }
    }

    public SortableIPAddress(IPAddress address)
    {
        ArgumentNullException.ThrowIfNull(address);
        if (address.AddressFamily == AddressFamily.InterNetwork)
        {
            _isIPv4 = true;
            IsIPv6 = false;
            _scopeId = 0L;
        }
        else
        {
            IsIPv6 = true;
            _isIPv4 = address.IsIPv4MappedToIPv6;
            _scopeId = address.ScopeId;
        }
        AddressBytes = [.. address.GetAddressBytes()];
    }

    public SortableIPAddress(byte[] address)
    {
        ArgumentNullException.ThrowIfNull(address);
        IPAddress iPAddress;
        try { iPAddress = new(address); }
        catch (Exception exception) { throw new ArgumentException(exception.Message, nameof(address), exception); }
        if (iPAddress.AddressFamily == AddressFamily.InterNetwork)
        {
            _isIPv4 = true;
            IsIPv6 = false;
            _scopeId = 0L;
        }
        else
        {
            IsIPv6 = true;
            _isIPv4 = iPAddress.IsIPv4MappedToIPv6;
            _scopeId = iPAddress.ScopeId;
        }
        AddressBytes = [.. iPAddress.GetAddressBytes()];
    }

    public SortableIPAddress(long newAddress)
    {
        IPAddress iPAddress;
        try { iPAddress = new(newAddress); }
        catch (Exception exception) { throw new ArgumentException(exception.Message, nameof(newAddress), exception); }
        if (iPAddress.AddressFamily == AddressFamily.InterNetwork)
        {
            _isIPv4 = true;
            IsIPv6 = false;
            _scopeId = 0L;
        }
        else
        {
            IsIPv6 = true;
            _isIPv4 = iPAddress.IsIPv4MappedToIPv6;
            _scopeId = iPAddress.ScopeId;
        }
        AddressBytes = [.. iPAddress.GetAddressBytes()];
    }

    public SortableIPAddress(byte[] address, long scopeid)
    {
        ArgumentNullException.ThrowIfNull(address);
        IPAddress iPAddress;
        try { iPAddress = new(address, scopeid); }
        catch (ArgumentOutOfRangeException argExc)
        {
            throw new ArgumentOutOfRangeException((argExc.ParamName == nameof(address)) ? argExc.ParamName : nameof(scopeid), argExc);
        }
        catch (ArgumentException argExc)
        {
            throw new ArgumentException(argExc.Message, (argExc.ParamName == nameof(scopeid)) ? argExc.ParamName : nameof(address), argExc);
        }
        catch (Exception exception) { throw new ArgumentException(exception.Message, nameof(address), exception); }
        IsIPv6 = true;
        _isIPv4 = iPAddress.IsIPv4MappedToIPv6;
        _scopeId = iPAddress.ScopeId;
        AddressBytes = [.. iPAddress.GetAddressBytes()];
    }

    private int CompareIPv4ToIPv4(IList<byte> other)
    {
        int diff;
        for (int i = 0; i < AddressBytes.Length; i++)
            if ((diff = AddressBytes[i].CompareTo(other[i])) != 0)
                return diff;
        return 0;
    }

    private int CompareIPv4MappedToIPv4(IList<byte> other)
    {
        int offset = AddressBytes.Length - other.Count;
        int diff;
        for (int i = 0; i < other.Count; i++)
            if ((diff = AddressBytes[i + offset].CompareTo(other[i])) != 0)
                return diff;
        return 0;
    }

    private int CompareIPv4ToIPv4Mapped(IList<byte> other)
    {
        int offset = other.Count - AddressBytes.Length;
        int diff;
        for (int i = 0; i < AddressBytes.Length; i++)
            if ((diff = AddressBytes[i].CompareTo(other[i + offset])) != 0)
                return diff;
        return 0;
    }

    private int CompareIPv4ToIPv6(IList<byte> other, long scopeId)
    {
        if (scopeId != 0L) return -1;
        int offset = other.Count - AddressBytes.Length;
        for (int i = 0; i < offset; i++)
            if (other[i] != 0)
                return -1;
        int diff;
        for (int i = 0; i < AddressBytes.Length; i++)
            if ((diff = AddressBytes[i].CompareTo(other[i + offset])) != 0)
                return diff;
        return -1;
    }

    private int CompareIPv6ToIPv4(IList<byte> other)
    {
        if (ScopeId != 0L) return 1;
        int offset = AddressBytes.Length - other.Count;
        for (int i = 0; i < offset; i++)
            if (AddressBytes[i] != 0)
                return 1;
        int diff;
        for (int i = 0; i < AddressBytes.Length; i++)
            if ((diff = AddressBytes[i + offset].CompareTo(other[i])) != 0)
                return diff;
        return 1;
    }

    private int CompareIPv6ToIPv6(IList<byte> other, long scopeId)
    {
        int diff;
        for (int i = 0; i < AddressBytes.Length; i++)
            if ((diff = AddressBytes[0].CompareTo(other[0])) != 0) return diff;
        return _scopeId.CompareTo(scopeId);
    }

    public int CompareTo(SortableIPAddress other)
    {
        if (other is null) return 1;
        if (ReferenceEquals(this, other)) return 0;
        if (IsIPv6)
            return other.IsIPv6 ? CompareIPv6ToIPv6(other.AddressBytes, other._scopeId) : _isIPv4 ? CompareIPv4MappedToIPv4(other.AddressBytes) : CompareIPv6ToIPv4(other.AddressBytes);
        return other.IsIPv6 ? (other._isIPv4 ? CompareIPv4ToIPv4Mapped(other.AddressBytes) : CompareIPv4ToIPv6(other.AddressBytes, other._scopeId)) : CompareIPv4ToIPv4(other.AddressBytes);
    }

    public int CompareTo(IPAddress other)
    {
        if (other is null) return 1;
        if (IsIPv6)
            return (other.AddressFamily == AddressFamily.InterNetworkV6) ? CompareIPv6ToIPv6(other.GetAddressBytes(), other.ScopeId) : _isIPv4 ? CompareIPv4MappedToIPv4(other.GetAddressBytes()) :
                CompareIPv6ToIPv4(other.GetAddressBytes());
        return (other.AddressFamily == AddressFamily.InterNetworkV6) ? (other.IsIPv4MappedToIPv6 ? CompareIPv4ToIPv4(other.GetAddressBytes()) :
            CompareIPv4ToIPv6(other.GetAddressBytes(), other.ScopeId)) : CompareIPv4ToIPv4Mapped(other.GetAddressBytes());
    }

    public int CompareTo(object obj)
    {
        if (obj is null) return 1;
        if (obj is SortableIPAddress other) return CompareTo(other);
        if (obj is IPAddress iPAddress) return CompareTo(iPAddress);
        return (obj is string s || (s = obj.ToString()) is not null) ? ToString().CompareTo(s) : 1;
    }

    public SortableIPAddress Decrement()
    {
        int decIdx = AddressBytes.Length - 1;
        byte value = AddressBytes[decIdx];
        if (value > 0)
        {
            value--;
            byte[] addressBytes = new byte[AddressBytes.Length];
            AddressBytes.CopyTo(addressBytes);
            addressBytes[decIdx] = value;
            return IsIPv6 ? new SortableIPAddress(addressBytes, _scopeId) : new SortableIPAddress(addressBytes);
        }
        int flipEnd = decIdx;
        decIdx--;
        int lBound = (IsIPv6 && _isIPv4) ? AddressBytes.Length - 3 : 0;
        do
        {
            if ((value = AddressBytes[decIdx]) > 0)
            {
                value--;
                byte[] addressBytes = new byte[AddressBytes.Length];
                AddressBytes.CopyTo(addressBytes);
                addressBytes[decIdx] = value;
                for (int i = decIdx + 1; i <= flipEnd; i++)
                    addressBytes[i] = 255;
                return IsIPv6 ? new SortableIPAddress(addressBytes, _scopeId) : new SortableIPAddress(addressBytes);
            }
            decIdx--;
        }
        while (decIdx >= lBound);
        throw new OverflowException("IP address could not be decremented further");
    }

    // public SortableIPAddress Decrement(int count)
    // {
    //     throw new NotImplementedException();
    // }

    // public SortableIPAddress Decrement(long count)
    // {
    //     throw new NotImplementedException();
    // }

    // public SortableIPAddress Decrement(BigInteger count)
    // {
    //     throw new NotImplementedException();
    // }

    public bool Equals([NotNullWhen(true)] SortableIPAddress other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (IsIPv6)
            return other.IsIPv6 ? IPv6EqualsIPv6(other.AddressBytes, other._scopeId) : _isIPv4 && IPv4MappedEqualsIPv4(other.AddressBytes);
        return other.IsIPv6 ? (other._isIPv4 && IPv4EqualsIPv4Mapped(other.AddressBytes)) : IPv4EqualsIPv4(other.AddressBytes);
    }

    public bool Equals([NotNullWhen(true)] IPAddress other)
    {
        if (other is null) return false;
        if (IsIPv6)
            return (other.AddressFamily == AddressFamily.InterNetworkV6) ? IPv6EqualsIPv6(other.GetAddressBytes(), other.ScopeId) : _isIPv4 && IPv4MappedEqualsIPv4(other.GetAddressBytes());
        return (other.AddressFamily == AddressFamily.InterNetworkV6) ? (other.IsIPv4MappedToIPv6 && IPv4EqualsIPv4Mapped(other.GetAddressBytes())) : IPv4EqualsIPv4(other.GetAddressBytes());
    }

    public override bool Equals([NotNullWhen(true)] object obj)
    {
        if (obj is null) return false;
        if (obj is SortableIPAddress other) return Equals(other);
        if (obj is IPAddress iPAddress) return Equals(iPAddress);
        return (obj is string s || (s = obj.ToString()) is not null) && ToString().Equals(s);
    }

    public override int GetHashCode()
    {
        HashCode hashCode = new();
        hashCode.AddBytes(AddressBytes.ToArray());
        if (IsIPv6 && !_isIPv4)
            hashCode.Add(ScopeId);
        return hashCode.ToHashCode();
    }

    public SortableIPAddress Increment()
    {
        int decIdx = AddressBytes.Length - 1;
        byte value = AddressBytes[decIdx];
        if (value < 255)
        {
            value++;
            byte[] addressBytes = new byte[AddressBytes.Length];
            AddressBytes.CopyTo(addressBytes);
            addressBytes[decIdx] = value;
            return IsIPv6 ? new SortableIPAddress(addressBytes, _scopeId) : new SortableIPAddress(addressBytes);
        }
        int flipEnd = decIdx;
        decIdx--;
        int lBound = (IsIPv6 && _isIPv4) ? AddressBytes.Length - 3 : 0;
        do
        {
            if ((value = AddressBytes[decIdx]) < 255)
            {
                value++;
                byte[] addressBytes = new byte[AddressBytes.Length];
                AddressBytes.CopyTo(addressBytes);
                addressBytes[decIdx] = value;
                for (int i = decIdx + 1; i <= flipEnd; i++)
                    addressBytes[i] = 0;
                return IsIPv6 ? new SortableIPAddress(addressBytes, _scopeId) : new SortableIPAddress(addressBytes);
            }
            decIdx--;
        }
        while (decIdx >= lBound);
        throw new OverflowException("IP address could not be incremented further");
    }

    // public SortableIPAddress Increment(int count)
    // {
    //     throw new NotImplementedException();
    // }

    // public SortableIPAddress Increment(long count)
    // {
    //     throw new NotImplementedException();
    // }

    // public SortableIPAddress Increment(BigInteger count)
    // {
    //     throw new NotImplementedException();
    // }

    private bool IPv4EqualsIPv4(IList<byte> other)
    {
        for (int i = 0; i < AddressBytes.Length; i++)
            if (AddressBytes[i] != other[i])
                return false;
        return true;
    }

    private bool IPv4MappedEqualsIPv4(IList<byte> other)
    {
        int offset = AddressBytes.Length - other.Count;
        for (int i = 0; i < AddressBytes.Length; i++)
            if (AddressBytes[i] != other[i + offset])
                return false;
        return true;
    }

    private bool IPv4EqualsIPv4Mapped(IList<byte> other)
    {
        int offset = other.Count - AddressBytes.Length;
        for (int i = 0; i < AddressBytes.Length; i++)
            if (AddressBytes[i] != other[i + offset])
                return false;
        return true;
    }

    private bool IPv6EqualsIPv6(IList<byte> other, long scopeId)
    {
        if (_scopeId != scopeId) return false;
        for (int i = 0; i < AddressBytes.Length; i++)
            if (AddressBytes[i] != other[i])
                return false;
        return true;
    }

    public override string ToString() => (IsIPv6 ? new IPAddress([.. AddressBytes], _scopeId) : new IPAddress([.. AddressBytes])).ToString();

    public static SortableIPAddress operator ++(SortableIPAddress address)
    {
        ArgumentNullException.ThrowIfNull(address);
        return address.Increment();
    }

    // public static SortableIPAddress operator +(SortableIPAddress left, int right)
    // {
    //     ArgumentNullException.ThrowIfNull(left);
    //     return left.Increment(right);
    // }

    // public static SortableIPAddress operator +(SortableIPAddress left, long right)
    // {
    //     ArgumentNullException.ThrowIfNull(left);
    //     return left.Increment(right);
    // }

    // public static SortableIPAddress operator +(SortableIPAddress left, BigInteger right)
    // {
    //     ArgumentNullException.ThrowIfNull(left);
    //     return left.Increment(right);
    // }

    // public static SortableIPAddress operator +(int left, SortableIPAddress right)
    // {
    //     ArgumentNullException.ThrowIfNull(right);
    //     return right.Increment(left);
    // }

    // public static SortableIPAddress operator +(long left, SortableIPAddress right)
    // {
    //     ArgumentNullException.ThrowIfNull(right);
    //     return right.Increment(left);
    // }

    // public static SortableIPAddress operator +(BigInteger left, SortableIPAddress right)
    // {
    //     ArgumentNullException.ThrowIfNull(right);
    //     return right.Increment(left);
    // }

    public static SortableIPAddress operator --(SortableIPAddress address)
    {
        ArgumentNullException.ThrowIfNull(address);
        return address.Decrement();
    }

    // public static SortableIPAddress operator -(SortableIPAddress left, int right)
    // {
    //     ArgumentNullException.ThrowIfNull(left);
    //     return left.Decrement(right);
    // }

    // public static SortableIPAddress operator -(SortableIPAddress left, long right)
    // {
    //     ArgumentNullException.ThrowIfNull(left);
    //     return left.Decrement(right);
    // }

    // public static SortableIPAddress operator -(SortableIPAddress left, BigInteger right)
    // {
    //     ArgumentNullException.ThrowIfNull(left);
    //     return left.Decrement(right);
    // }

    public static bool operator ==(SortableIPAddress left, SortableIPAddress right) => (left is null) ? right is null : left.Equals(right);

    public static bool operator !=(SortableIPAddress left, SortableIPAddress right) => (left is null) ? right is not null : !left.Equals(right);

    public static bool operator <(SortableIPAddress left, SortableIPAddress right) => left is null ? right is not null : left.CompareTo(right) < 0;

    public static bool operator <=(SortableIPAddress left, SortableIPAddress right) => left is null || left.CompareTo(right) <= 0;

    public static bool operator >(SortableIPAddress left, SortableIPAddress right) => left is not null && left.CompareTo(right) > 0;

    public static bool operator >=(SortableIPAddress left, SortableIPAddress right) => left is null ? right is null : left.CompareTo(right) >= 0;
}