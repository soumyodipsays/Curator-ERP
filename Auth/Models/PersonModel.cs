using System;

namespace Auth.Models
{
    public class PersonModel
    {
        public long PersonID { get; set; }
        public long CustomerID { get; set; }

        public string PersonTypeCode { get; set; }

        public string FirstName { get; set; }
        public string MiddleInitials { get; set; }
        public string LastName { get; set; }

        public string ParentsFirstName { get; set; }
        public string ParentsMiddleInitials { get; set; }
        public string ParentsLastName { get; set; }

        public string SpouseFirstName { get; set; }
        public string SpouseMiddleInitials { get; set; }
        public string SpouseLastName { get; set; }

        public string Child1FirstName { get; set; }
        public string Child1MiddleInitials { get; set; }
        public string Child1LastName { get; set; }

        public string Child2FirstName { get; set; }
        public string Child2MiddleInitials { get; set; }
        public string Child2LastName { get; set; }

        public short? TotalAdultFamilyMember { get; set; }
        public short? TotalMinorFamilyMember { get; set; }

        public DateTime? DOB { get; set; }
        public string PlaceOfBirth { get; set; }

        public string GenderCode { get; set; }
        public string Occupation { get; set; }

        public short? BloodGroupID { get; set; }

        public string WorkingIn { get; set; }
        public string KnownMember { get; set; }
        public string ReasonToJoin { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}