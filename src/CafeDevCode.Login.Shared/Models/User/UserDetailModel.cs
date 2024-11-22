﻿using CafeDevCode.Database.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeDevCode.Logic.Shared.Models
{
    public class UserDetailModel : User
    {
        public List<IdentityRole> roles = new List<IdentityRole>();
        public string Password { get; set; } = string.Empty;
    }
}
