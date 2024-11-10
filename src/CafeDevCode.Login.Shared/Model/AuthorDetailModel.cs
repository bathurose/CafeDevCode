﻿using CafeDevCode.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeDevCode.Login.Shared.Model
{
    public class AuthorDetailModel : Author
    {
        public List<Post> Posts { get; set; } = new List<Post>(); 
    }
}