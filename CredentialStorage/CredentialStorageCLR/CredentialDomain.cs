﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace CredentialStorageCLR
{
    [XmlRoot(ElementName = CredentialDomain.ElementName, Namespace = CredentialDomain.NamespaceURI)]
    [Serializable]
    public class CredentialDomain : CredentialComponent, ICredentialContentItem
    {
        public const string ElementName = "";

        private System.Collections.Generic.List<StoredCredential> _credentials = new List<StoredCredential>();
        private Guid? _id = null;
        private string _displayText = "";

        [XmlArrayItem(ElementName = StoredCredential.ElementName, Namespace = StoredCredential.NamespaceURI)]
        public List<StoredCredential> Credentials
        {
            get { return this._credentials; }
            set { this._credentials = value ?? new List<StoredCredential>(); }
        }

        [XmlAttribute]
        public string DisplayText
        {
            get { return this._displayText; }
            set { this._displayText = value ?? ""; }
        }

        [XmlAttribute]
        public Guid Id
        {
            get
            {
                if (!this._id.HasValue)
                    this._id = Guid.NewGuid();

                return this._id.Value;
            }
            set { this._id = value; }
        }

        bool ICredentialContentItem.IsFolder { get { return false; } }

        protected override CredentialComponent Clone()
        {
            CredentialDomain clone = new CredentialDomain();
            clone._id = this.Id;
            clone._displayText = this._displayText;
            lock (this._credentials)
            {
                foreach (StoredCredential item in this._credentials)
                    clone._credentials.Add((item == null) ? null : (item as ICloneable).Clone() as StoredCredential);
            }

            return clone;
        }

        ICredentialContentItem ICredentialContentItem.Clone() { return this.Clone() as ICredentialContentItem; }
    }
}