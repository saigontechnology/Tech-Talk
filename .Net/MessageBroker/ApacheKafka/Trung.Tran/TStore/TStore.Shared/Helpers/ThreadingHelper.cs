using System;
using System.Threading;

namespace TStore.Shared.Helpers
{
    public static class ThreadingHelper
    {
        public static bool TryRelease(this SemaphoreSlim semaphoreSlim)
        {
            try
            {
                semaphoreSlim.Release();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
