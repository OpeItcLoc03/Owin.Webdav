﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Soukoku.Owin.Webdav.Models
{
    public class DateProperty : Property<DateTime>
    {
        public DateProperty(string name) : base(name, Consts.XmlNamespace) { }
        public DateProperty(string name, string @namespace) : base(name, @namespace) { }

        public string FormatString { get; set; }
        public Func<DateTime, string> Formatter { get; set; }

        public override XmlElement Serialize(XmlDocument doc)
        {
            var node = doc.CreateElement(Name, Namespace);
            if (Formatter != null)
            {
                node.InnerText = Formatter(Value);
            }
            else if (string.IsNullOrWhiteSpace(FormatString))
            {
                node.InnerText = Value.ToString();
            }
            else
            {
                node.InnerText = Value.ToString(FormatString);
            }
            return node;
        }
    }

    public class ReadOnlyDateProperty : DerivedProperty<DateTime>
    {
        public ReadOnlyDateProperty(string name) : base(name, Consts.XmlNamespace) { }
        public ReadOnlyDateProperty(string name, string @namespace) : base(name, @namespace) { }
        
        public string FormatString { get; set; }
        public Func<DateTime, string> Formatter { get; set; }


        public override XmlElement Serialize(XmlDocument doc)
        {
            if (SerializeRoutine == null)
            {
                var node = doc.CreateElement(Name, Namespace);
                if (Formatter != null)
                {
                    node.InnerText = Formatter(Value);
                }
                else if (string.IsNullOrWhiteSpace(FormatString))
                {
                    node.InnerText = Value.ToString();
                }
                else
                {
                    node.InnerText = Value.ToString(FormatString);
                }
                return node;
            }
            return SerializeRoutine(this, doc);
        }
    }
}
