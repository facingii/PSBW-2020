using System;
using System.Collections.Generic;
using EmployeesWebService.Models;

namespace EmployeesWebService.Helpers
{
    public interface IUserService
    {
        User Authenticate (string username, string password);
        IEnumerable<User> GetAll ();
    }
}
