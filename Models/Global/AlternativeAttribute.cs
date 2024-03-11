using System;

namespace BoostifySolution.Models.Global
{
    internal class AlternativeAttribute : Attribute
    {
        public string Alternative;

        public AlternativeAttribute(string v)
        {
            this.Alternative = v;
        }
    }
}