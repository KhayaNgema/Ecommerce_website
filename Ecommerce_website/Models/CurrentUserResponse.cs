using System;
using System.Collections.Generic;

public class CurrentUserResponse
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string ProfilePicture { get; set; }
    public string Address { get; set; }
    public bool IsActive { get; set; }
    public bool IsSuspended { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsFirstTimeLogin { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime ModifiedDateTime { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
    public List<string> Roles { get; set; }
}
