﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Requests.Identity
{
    public class UpdatePasswordRequest
    {
        public string UserId { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
