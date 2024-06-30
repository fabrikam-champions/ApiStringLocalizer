using System;
using System.Collections.Generic;
using System.Text;

namespace ApiStringLocalizer
{
    public class ApiStringLocalizerCacheKey : IEquatable<ApiStringLocalizerCacheKey>
    {
        public string ResourceName { get; set; }
        public string Culture { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (ApiStringLocalizerCacheKey)obj;
            return ResourceName == other.ResourceName && Culture == other.Culture;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + (ResourceName != null ? ResourceName.GetHashCode() : 0);
                hash = hash * 23 + (Culture != null ? Culture.GetHashCode() : 0);
                return hash;
            }
        }

        public bool Equals(ApiStringLocalizerCacheKey other)
        {
            if(ReferenceEquals(null, other)) 
                return false;
            return GetHashCode() == other.GetHashCode();
        }

        public static bool operator ==(ApiStringLocalizerCacheKey lhs, ApiStringLocalizerCacheKey rhs)
        {
            if (ReferenceEquals(lhs, null))
            {
                return ReferenceEquals(rhs, null);
            }

            return lhs.Equals(rhs);
        }

        public static bool operator !=(ApiStringLocalizerCacheKey lhs, ApiStringLocalizerCacheKey rhs)
        {
            return !(lhs == rhs);
        }
    }

}
