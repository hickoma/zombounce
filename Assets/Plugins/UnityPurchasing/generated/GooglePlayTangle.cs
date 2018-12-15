#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("v32G4+TeHRXUHhELGbIx0i9EveIb4Xrr8KiuemLiutAMN3S8zoy27LMBgqGzjoWKqQXLBXSOgoKChoOAZgKPLtkYqj2xI3uOycQjNvucmM3ePg9AjrWrNj7/0xMpFpbX4b3kDcFbtbSbx6V/SAVSRuZdhwpuQO9vdXIzofqIe+pq1GshFBC8nJXHzuEOFPIVhMOK//bkUp4gxnzNJyHrPO70av6EQXMy8vlXUjjPMSj7InZ2zdpGWwgWEVj0S2B0qJpkET+acyhJ4xNCFPg7eb+SMv9IdOW7O2n6cvc4GwrbNqGX4JsYKia+DugHSF0EAYKMg7MBgomBAYKCgx79a+EzN61Ud8dNSBzEAU5+c50Y/pR7iFv4qK0LjJYESxw8LIGAgoOC");
        private static int[] order = new int[] { 3,4,3,3,12,11,13,8,8,10,11,11,12,13,14 };
        private static int key = 131;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
