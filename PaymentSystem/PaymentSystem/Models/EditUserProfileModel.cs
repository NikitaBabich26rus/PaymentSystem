using System.ComponentModel.DataAnnotations;
using PaymentSystem.Data;

namespace PaymentSystem.Models;

public class EditUserProfileModel
{
    public UserProfileModel UserProfile { get; set; }

    public List<RoleRecord> Roles { get; set; }
}