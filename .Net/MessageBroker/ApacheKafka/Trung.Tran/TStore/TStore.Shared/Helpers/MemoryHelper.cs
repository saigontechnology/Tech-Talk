using System;

namespace TStore.Shared.Helpers
{
    public static class MemoryHelper
    {
        public static void CheckMemoryUsage(int? forceCollectThreshold = null, int? throwIfGreaterThanBytes = null)
        {
            long usedMemory = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory usage: {usedMemory} bytes");

            if (usedMemory > forceCollectThreshold)
            {
                GC.Collect();
            }

            if (usedMemory > throwIfGreaterThanBytes)
            {
                throw new Exception("Exceeded maximum memory usage");
            }
        }
    }
}
