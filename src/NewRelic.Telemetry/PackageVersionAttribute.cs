﻿using System;

namespace NewRelic.Telemetry
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class PackageVersionAttribute : Attribute
    {
        public string PackageVersion { get; }
        public PackageVersionAttribute(string version) 
        {
            PackageVersion = version;
        }
    }
}
