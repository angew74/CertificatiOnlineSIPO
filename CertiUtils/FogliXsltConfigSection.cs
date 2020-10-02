using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Com.Unisys.CdR.Certi.Utils
{

    /// <summary>
    /// The Class that will have the XML config file data loaded into it via the configuration Manager.
    /// </summary>
    public class FogliCertiConfigSection : ConfigurationSection
    {
        /// <summary>
        /// The value of the property here "FogliCerti" needs to match that of the config file section
        /// </summary>
        [ConfigurationProperty("Fogli")]
        public FogliXsltCollection FogliCertiItems
        {
            get { return ((FogliXsltCollection)(base["Fogli"])); }
        }

    }

    /// <summary>
    /// The Class that will have the XML config file data loaded into it via the configuration Manager.
    /// </summary>
    public class FogliDecoderConfigSection : ConfigurationSection
    {
        /// <summary>
        /// The value of the property here "FogliDecoder" needs to match that of the config file section
        /// </summary>
        [ConfigurationProperty("Fogli")]
        public FogliXsltCollection FogliDecoderItems
        {
            get { return ((FogliXsltCollection)(base["Fogli"])); }
        }
    }


    /// <summary>
    /// The collection class that will store the list of each element/item that
    ///        is returned back from the configuration manager.
    /// </summary>
    [ConfigurationCollection(typeof(FoglioXsltElement))]
    public class FogliXsltCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FoglioXsltElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FoglioXsltElement)(element)).Name;
        }

        public FoglioXsltElement this[int idx]
        {
            get
            {
                return (FoglioXsltElement)BaseGet(idx);
            }
        }
    }

    /// <summary>
    /// The class that holds onto each element returned by the configuration manager.
    /// </summary>
    public class FoglioXsltElement : ConfigurationElement
    {

        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Name
        {
            get
            {
                return ((string)(base["name"]));
            }

            set
            {
                base["name"] = value;
            }
        }

        [ConfigurationProperty("value", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string Value
        {
            get
            {
                return ((string)(base["value"]));
            }
            set
            {
                base["value"] = value;
            }
        }
    }
}
