﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeDevCode.Common.Shared.Model
{
    public class BaseEntity
    {
        public string? CreatedBy { get; set; }
        public DateTime? CreateAt { get; set; }
        public string? LastUpdatedBy { get;set; }
        public DateTime? LastUpdatedAt { get;set; }   
        public bool IsDeleted { get; set; }
        public BaseEntity SetCreateInfo(string user, DateTime date)
        {
            this.CreatedBy = user;
            this.CreateAt = date;
            return this;
        }
        public BaseEntity SetUpdateInfo(string user, DateTime date)
        {
            this.LastUpdatedBy = user;
            this.LastUpdatedAt = date;
            return this;
        }
        public BaseEntity MarkAsDelete(string user, DateTime date)
        {
            this.IsDeleted = true;
            this.LastUpdatedBy = user;
            this.LastUpdatedAt = date;  
            return this;
        }
    }
}
