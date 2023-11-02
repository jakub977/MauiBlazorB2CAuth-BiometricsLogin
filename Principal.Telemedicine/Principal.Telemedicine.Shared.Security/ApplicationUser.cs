using Microsoft.AspNetCore.Identity;
using Principal.Telemedicine.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Principal.Telemedicine.Shared.Security;
public class ApplicationUser : IdentityUser<string>
{
    public UserContract user  { get; set; }         
}
