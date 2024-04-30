using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Common.Models
{
    public enum LoginStatus
    {
        Success,
        Failure,
        EmailNotConfirmed
    }
    public class LoginResult
    {
        public LoginStatus Status { get; set; }
        public string ErrorMessage { get; set; }
    }

}
