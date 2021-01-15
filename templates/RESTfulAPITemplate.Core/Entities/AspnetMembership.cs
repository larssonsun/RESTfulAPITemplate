using System;
#if (DISTRIBUTEDCACHE)
using MessagePack;
#endif

namespace RESTfulAPITemplate.Core.Entity
{
#if (DISTRIBUTEDCACHE)

    [MessagePackObject(keyAsPropertyName: true)]

#endif

    public partial class AspnetMembership
    {
        public Guid ApplicationId { get; set; }
        public Guid UserId { get; set; }
        public string Password { get; set; }
        public int PasswordFormat { get; set; }
        public string PasswordSalt { get; set; }
        public string MobilePin { get; set; }
        public string Email { get; set; }
        public string LoweredEmail { get; set; }
        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        public DateTime LastLockoutDate { get; set; }
        public int FailedPasswordAttemptCount { get; set; }
        public DateTime FailedPasswordAttemptWindowStart { get; set; }
        public int FailedPasswordAnswerAttemptCount { get; set; }
        public DateTime FailedPasswordAnswerAttemptWindowStart { get; set; }
        public string Comment { get; set; }
        public Guid? BoundToUnitType { get; set; }
        public Guid? BoundToUnitId { get; set; }
        public Guid? BoundToCertificateType { get; set; }
        public Guid? BoundToCertificateId { get; set; }
        public int? IfTurnover { get; set; }
        public DateTime? LastGetYzmTime { get; set; }
    }
}
