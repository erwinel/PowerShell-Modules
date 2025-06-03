using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;

namespace NetworkUtility;

public class IPNetworkList : IReadOnlyCollection<SortableIPAddress>, ICollection<SortableIPAddress>
{
    public SortableIPAddress FirstIPAddress { get; }

    public int PrefixLength { get; }

    public SortableIPAddress LastIPAddress { get; }

    public BigInteger Count { get; }

    bool ICollection<SortableIPAddress>.IsReadOnly => true;

    int IReadOnlyCollection<SortableIPAddress>.Count => (Count > int.MaxValue) ? int.MaxValue : (int)Count;

    int ICollection<SortableIPAddress>.Count => (Count > int.MaxValue) ? int.MaxValue : (int)Count;

    public IPNetworkList(IPNetwork network)
    {
        PrefixLength = network.PrefixLength;
        FirstIPAddress = new(network.BaseAddress);
        int maxPrefix = FirstIPAddress.AddressBytes.Length << 3;
        if (PrefixLength == maxPrefix)
        {
            Count = BigInteger.One;
            LastIPAddress = FirstIPAddress;
        }
        else
        {
            Count = BigInteger.One << (maxPrefix - PrefixLength);
            if (PrefixLength == 0)
            {
                LastIPAddress = FirstIPAddress.ScopeId.HasValue ? new SortableIPAddress([.. Enumerable.Repeat<byte>(255, FirstIPAddress.AddressBytes.Length)], FirstIPAddress.ScopeId.Value) :
                    new SortableIPAddress([.. Enumerable.Repeat<byte>(255, FirstIPAddress.AddressBytes.Length)]);
            }
            else
            {
                int mixedBitIndex = PrefixLength >> 3;
                byte[] lastIpBytes = new byte[FirstIPAddress.AddressBytes.Length];
                for (int i = 0; i < mixedBitIndex; i++)
                    lastIpBytes[i] = FirstIPAddress.AddressBytes[i];
                byte shift = (byte)(PrefixLength & 7);
                lastIpBytes[mixedBitIndex] = (byte)((FirstIPAddress.AddressBytes[mixedBitIndex] & (255 << (8 - shift))) | (255 >> shift));
                for (int i = mixedBitIndex + 1; i < lastIpBytes.Length; i++)
                    lastIpBytes[i] = 0;
                LastIPAddress = FirstIPAddress.ScopeId.HasValue ? new SortableIPAddress(lastIpBytes, FirstIPAddress.ScopeId.Value) : new SortableIPAddress(lastIpBytes);
            }
        }
    }

    public IPNetworkList(SortableIPAddress address, int prefixLength)
    {
        ArgumentNullException.ThrowIfNull(address);
        ArgumentOutOfRangeException.ThrowIfNegative(prefixLength);
        int maxPrefix = address.AddressBytes.Length << 3;
        if (prefixLength == maxPrefix)
        {
            Count = BigInteger.One;
            FirstIPAddress = LastIPAddress = address;
        }
        else
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(prefixLength, maxPrefix);
            Count = BigInteger.One << (maxPrefix - prefixLength);
            if (prefixLength > 0)
            {
                int mixedBitIndex = prefixLength >> 3;
                byte[] lastIpBytes = new byte[address.AddressBytes.Length];
                for (int i = 0; i < mixedBitIndex; i++)
                    lastIpBytes[i] = address.AddressBytes[i];
                byte shift = (byte)(prefixLength & 7);
                byte expected = (byte)(address.AddressBytes[mixedBitIndex] & (255 << (8 - shift)));
                lastIpBytes[mixedBitIndex] = (byte)(expected | (255 >> shift));
                byte[] addressBytes = null;
                if (addressBytes[mixedBitIndex] != expected)
                {
                    addressBytes ??= new byte[address.AddressBytes.Length];
                    addressBytes[mixedBitIndex] = expected;
                }
                for (int i = mixedBitIndex + 1; i < addressBytes.Length; i++)
                {
                    lastIpBytes[i] = 0;
                    if (addressBytes[i] != 0)
                    {
                        addressBytes ??= new byte[address.AddressBytes.Length];
                        addressBytes[i] = 0;
                        for (i++; i < addressBytes.Length; i++)
                        {
                            addressBytes[i] = 0;
                            lastIpBytes[i] = 0;
                        }
                        break;
                    }
                }
                if (addressBytes == null)
                    FirstIPAddress = address;
                else
                    FirstIPAddress = address.ScopeId.HasValue ? new SortableIPAddress(addressBytes, address.ScopeId.Value) : new SortableIPAddress(addressBytes);
                LastIPAddress = address.ScopeId.HasValue ? new SortableIPAddress(lastIpBytes, address.ScopeId.Value) : new SortableIPAddress(lastIpBytes);
            }
            else
            {
                if (address.AddressBytes.Any(b => b != 0))
                {
                    byte[] addressBytes = [.. Enumerable.Repeat<byte>(0, address.AddressBytes.Length)];
                    FirstIPAddress = address.ScopeId.HasValue ? new SortableIPAddress(addressBytes, address.ScopeId.Value) : new SortableIPAddress(addressBytes);
                }
                else
                    FirstIPAddress = address;
                LastIPAddress = address.ScopeId.HasValue ? new SortableIPAddress([.. Enumerable.Repeat<byte>(255, address.AddressBytes.Length)], address.ScopeId.Value) :
                    new SortableIPAddress([.. Enumerable.Repeat<byte>(255, address.AddressBytes.Length)]);
            }
        }
    }

    void ICollection<SortableIPAddress>.Add(SortableIPAddress item) => throw new NotSupportedException();

    void ICollection<SortableIPAddress>.Clear() => throw new NotSupportedException();

    public bool Contains(SortableIPAddress item) => item != null && (PrefixLength == 0 || ((Count == 1) ? FirstIPAddress.Equals(item) : FirstIPAddress.CompareTo(item) <= 0 && LastIPAddress.CompareTo(item) >= 0));

    void ICollection<SortableIPAddress>.CopyTo(SortableIPAddress[] array, int arrayIndex)
    {
        if (Count <= int.MaxValue)
        {
            int count = (int)Count;
            if ((array.Length - arrayIndex) >= count)
            {
                int end = arrayIndex + count;
                SortableIPAddress address = FirstIPAddress;
                array[arrayIndex] = address;
                arrayIndex++;
                while (arrayIndex < end)
                {
                    address = address.Increment();
                    array[arrayIndex] = address;
                    arrayIndex++;
                }
                return;
            }
        }
        throw new ArgumentException($"Not enough room in the target array for {Count} items", nameof(array));
    }

    public IEnumerator<SortableIPAddress> GetEnumerator() => new Enumerator(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    bool ICollection<SortableIPAddress>.Remove(SortableIPAddress item) => throw new NotSupportedException();

    class Enumerator : IEnumerator<SortableIPAddress>
    {
        private IPNetworkList _target;
        public SortableIPAddress Current { get; private set; }

        object IEnumerator.Current => Current;

        internal Enumerator(IPNetworkList target)
        {
            ArgumentNullException.ThrowIfNull(target);
            _target = target;
        }

        public void Dispose() => _target = null;

        public bool MoveNext()
        {
            IPNetworkList target = _target;
            ObjectDisposedException.ThrowIf(target is null, this);
            SortableIPAddress current = Current;
            if (current is null)
                Current = target.FirstIPAddress;
            else if (current < target.LastIPAddress)
                Current = current.Increment();
            else
                return false;
            return true;
        }

        public void Reset()
        {
            ObjectDisposedException.ThrowIf(_target is null, this);
            Current = null;
        }
    }
}

