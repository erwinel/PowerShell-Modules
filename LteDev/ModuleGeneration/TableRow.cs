﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LteDev.ModuleGeneration
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [Serializable]
    public class TableRow
    {
        public const string ElementName_C = "C";
        private Collection<ParagraphHost> _cells = new Collection<ParagraphHost>();
        [XmlElement(ElementName_C)]
        public Collection<ParagraphHost> Cells
        {
            get { return _cells; }
            set { _cells = value ?? new Collection<ParagraphHost>(); }
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
