namespace Mobile.Models;

public static class Dictionary
{
    public static class SqlColumn
    {
        public static class PaymentFrequency
        {
            public static readonly string Daily = "DAILY";
            public static readonly string Weekly = "WEEKLY";
            public static readonly string Monthly = "MONTHLY";
            public static readonly string Quarterly = "QUARTERLY";
            public static readonly string Yearly = "YEARLY";
        }
        public static class Period
        {
            public static readonly string Morning = "MORNING";
            public static readonly string Afternoon = "AFTERNOON";
            public static readonly string Evening = "EVENING";
        }
        public static class Presence
        {
            public static readonly string Pending = "PENDING";
            public static readonly string Confirmed = "CONFIRMED";
            public static readonly string NotListed = "NOTLISTED";
        }
    }

}

