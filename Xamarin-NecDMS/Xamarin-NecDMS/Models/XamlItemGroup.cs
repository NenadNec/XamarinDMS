using System;
using System.Collections.Generic;
using System.Text;

namespace NecDMS.Models
{
    [Serializable]
    public class XamlItemGroup
    {
        public List<XamlItemGroup> Children { get; set; } = new List<XamlItemGroup>();
        public List<XamlItem> XamlItems { get; set; } = new List<XamlItem>();
        public string Name { get; set; }
    }

    [Serializable]
    public class XamlItem
    {
        public string Key { get; set; }
    }
}
