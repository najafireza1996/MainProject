using System.ComponentModel.DataAnnotations.Schema;
using Features.User.Domain.Entities;

namespace Features.Authentication.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevorked { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.Now;
        public DateTime ExpiryDate { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }
    }
}
