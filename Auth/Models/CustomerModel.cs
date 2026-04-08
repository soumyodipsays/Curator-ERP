using System;

namespace Auth.Models
{
    public class CustomerModel
    {
        public long CustomerID { get; set; }

        public string CustomerName { get; set; }
        public string ApplicationName { get; set; }

        public string Address { get; set; }

        public string Phone1 { get; set; }
        public string Phone2 { get; set; }

        public string Email { get; set; }

        public string DBServerName { get; set; }
        public string DatabaseName { get; set; }
        public string DBUserName { get; set; }
        public string DBPassword { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public long? PaymentGatewayID { get; set; }

        public DateTime? CreatedOn { get; set; }
    }
}