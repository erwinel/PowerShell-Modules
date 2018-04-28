﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LteDev.AssemblyBrowser
{
#pragma warning disable 1591 // Missing XML comment for publicly visible type or member
    public class TypeDefinitionItem
    {
        private Type _type;
        private string _baseName = null;
        private string _fullName = null;
        private string _name = null;
        public Dictionary<string, TypeDefinitionItem> _nestedTypes = new Dictionary<string, TypeDefinitionItem>();
        public Dictionary<string, TypeDefinitionItem> _genericTypes = null;
        public Dictionary<string, MethodDefinitionItem> _methods = null;

        public Type Type { get { return this._type; } }

        public string BaseName { get { return this._baseName; } }

        public string FullName
        {
            get
            {
                if (this._fullName != null)
                    return this._fullName;

                if (this._type.IsGenericParameter)
                    this._fullName = this.Name;
                else
                {
                    if (this._type.IsNested)
                    {
                        TypeDefinitionItem declaringType = AssemblyLookupCache.Lookup(this._type.DeclaringType);
                        this._fullName = String.Format("{0}+{1}", declaringType.FullName, this.Name);
                    }
                }
#warning Not implemented
                throw new NotImplementedException();
            }
        }

        public string Name { get { return this._name; } }

        public TypeDefinitionItem(Type type)
        {
            this._type = type;
        }

        public TypeDefinitionItem Get(Type type)
        {
            if (!(type.IsNested && type.DeclaringType.Equals(this.Type)))
                return AssemblyLookupCache.Lookup(type);
            
            lock (this._nestedTypes)
            {
                if (this._nestedTypes.ContainsKey(type.AssemblyQualifiedName))
                    return this._nestedTypes[type.AssemblyQualifiedName];

                TypeDefinitionItem typeLookupCache = new TypeDefinitionItem(type);
                this._nestedTypes[type.AssemblyQualifiedName] = typeLookupCache;

                return typeLookupCache;
            }
        }
    }
#pragma warning restore 1591 // Missing XML comment for publicly visible type or member
}