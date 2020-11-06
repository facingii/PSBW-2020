using System;
using System.Collections.Generic;

namespace EmployeesWebService.Models
{
    public partial class CurrentDeptEmp
    {
        public int EmpNo { get; set; }
        public string DeptNo { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
