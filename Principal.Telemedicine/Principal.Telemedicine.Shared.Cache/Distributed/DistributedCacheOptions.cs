using Principal.Telemedicine.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Principal.Telemedicine.Shared.Cache;
/// <summary>
/// Nastavení defaultního záznamu do cache
/// </summary>
public  class DistributedCacheOptions
{
    public CacheEntryOptions DefaultCacheEntryOption { get; set; }
}
