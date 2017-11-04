﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LteDev.XsdTypeGen.maml
{
    [Serializable()]
    public class textBlockType
    {
        [XmlElement("para", Namespace = Constants.Xmlns_maml, Type = typeof(para))]
        [XmlElement("list", Namespace = Constants.Xmlns_maml, Type = typeof(list))]
        [XmlElement("table", Namespace = Constants.Xmlns_maml, Type = typeof(table))]
        [XmlElement("example", Namespace = Constants.Xmlns_maml, Type = typeof(example))]
        [XmlElement("alertSet", Namespace = Constants.Xmlns_maml, Type = typeof(alertSet))]
        [XmlElement("quote", Namespace = Constants.Xmlns_maml, Type = typeof(quote))]
        [XmlElement("definitionList", Namespace = Constants.Xmlns_maml, Type = typeof(definitionList))]
        public List<object> Contents { get; set; }
    }
}