namespace Hub.Transactions.WebAPI.Endpoints.ThreeDSecureDetails
{
    public class ThreeDSecureDetailsRow
    {
        public bool Enrolled { get; set; }

        public string Authenticated { get; set; }

        public string ECI { get; set; }

        public string XID { get; set; }

        public string AuthenticationValue { get; set; }

        public string PAReq { get; set; }

        public string PARes { get; set; }

        public string VEReq { get; set; }

        public string VERes { get; set; }
    }
}
