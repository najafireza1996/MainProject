namespace Common.Constants {

    public static class SubscriptionType
    {
        public const string Free = "Free";
        public const string Premium = "Premium";

        public static string[] GetSubscriptionTypes()
        {
            return new string[] { Free, Premium };
        }

    }
}
