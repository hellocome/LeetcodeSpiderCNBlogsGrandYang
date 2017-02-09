using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace LeetcodeSpiderCNBlogsGrandYang.Configuration
{
    public sealed class SpiderSettingSection : ConfigurationSection
    {
        public const string SECTION_NAME = "SpiderSettingSection";

        private ConfigurationPropertyCollection mProperties = null;
        public SpiderSettingSection()
        {
            // Property initialization
            mProperties = new ConfigurationPropertyCollection();

            mProperties.Add(mProxyAddress);
            mProperties.Add(mProxyUserName);
            mProperties.Add(mProxyPassword);
            mProperties.Add(mProxyDomain);
        }


        // This is a key customization. 
        // It returns the initialized property bag.
        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return mProperties;
            }
        }

        private static readonly ConfigurationProperty mProxyAddress = new ConfigurationProperty("ProxyAddress", typeof(string), "", ConfigurationPropertyOptions.IsRequired);
        private static readonly ConfigurationProperty mProxyUserName = new ConfigurationProperty("ProxyUserName", typeof(string), "", ConfigurationPropertyOptions.IsRequired);
        private static readonly ConfigurationProperty mProxyPassword = new ConfigurationProperty("ProxyPassword", typeof(string), "", ConfigurationPropertyOptions.IsRequired);
        private static readonly ConfigurationProperty mProxyDomain = new ConfigurationProperty("ProxyDomain", typeof(string), "", ConfigurationPropertyOptions.IsRequired);

        public string ProxyAddress
        {
            get
            {
                return this["ProxyAddress"] as string;
            }
        }
        public string ProxyUserName
        {
            get
            {
                return this["ProxyUserName"] as string;
            }
        }

        public string ProxyPassword
        {
            get
            {
                return this["ProxyPassword"] as string;
            }
        }

        public string ProxyDomain
        {
            get
            {
                return this["ProxyDomain"] as string;
            }
        }

    }
}
