using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace LteDev.XsdTypeGen.maml
{
    [Serializable()]
    [XmlRoot("para", Namespace = Constants.Xmlns_maml)]
    public class para
    {
        // TODO: Reference from maml/textBlockType.cs: maml:para
    }
}