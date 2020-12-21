using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using EmployeesWebService.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Ganss.XSS;
using System.Net;
using Microsoft.AspNetCore.Cors;

namespace EmployeesWebService.Controllers
{
    public class Empleado
    {
        public int EmpNo { get; set; }
        public String Nombre { get; set; }
        public String Apellidos { get; set; }
        public String Titulo { get; set; }
        public int Salario { get; set; }
        //public DateTime DepaEmpleado { get; set; }
        //public DateTime DepaManager { get; set; }
    }

    //[Authorize]
    [Route("api/[controller]")]
    //[EnableCors (Startup.MY_CORS)]  
    public class EmployeesController : Controller
    {
        private const string EMPLOYEES_KEY = "employeeKey";
        private const string EMPLOYEES_LIST = "employeeList";
        private IMemoryCache cache;

        public EmployeesController (IMemoryCache cache, IDataProtectionProvider provider)
        {
            this.cache = cache;
        }

        [HttpGet]
        public IEnumerable<Empleado> Get ()
        {
            //var cachado = cache.Get<List<Empleado>>(EMPLOYEES_LIST);
            //if (cachado != null) {
            //    return cachado;
            //}
             
            var context = new employeesContext ();
            //var employees = context.Employees.Where<Employees>(e => e.LastName.Contains("Smith"));

            //
            // SELECT employees.first_name, employees.last_name, titles.title, salaries.salary
            // FROM employees INNER JOIN titles ON employees.emp_no = titles.emp_no
            // INNER JOIN salaries ON employees.emp_no = salaries.emp_no
            // INNER JOIN inner join dept_emp ON employees.emp_no = dept_emp.emp_no
            // INNER JOIN dept_manager ON employees.emp_no = dept_manager.emp_no
            // WHERE employees.last_name like 'smith'
            var employees = from e in context.Employees
                                //join s in context.Salaries on e.EmpNo equals s.EmpNo
                                //join t in context.Titles on e.EmpNo equals t.EmpNo
                            orderby e.EmpNo ascending
                            select new Empleado
                            {
                                EmpNo = e.EmpNo,
                                Nombre = WebUtility.HtmlEncode (e.FirstName),
                                Apellidos = WebUtility.HtmlEncode (e.LastName),
                                Titulo = "",
                                Salario = 0, 
                            };

            employees = employees.Take(50);

            //var options = new MemoryCacheEntryOptions {
            //    Priority = CacheItemPriority.High,
            //    AbsoluteExpiration = DateTime.Now.AddMinutes (3)
            //};

            //cache.Set (EMPLOYEES_LIST, employees.ToList (), options);

            return employees;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Employees Get (int id)
        {
            //var cachado = cache.Get<Employees> (id);

            //if (cachado != null) {
            //    return cachado;
            //}  

            var context = new employeesContext ();

            Employees employee = context.Employees.Where<Employees>(e => e.EmpNo == id).FirstOrDefault<Employees>(); 
            if (employee == null) {
                return null;
            }

            //var options = new MemoryCacheEntryOptions {
            //     Priority = CacheItemPriority.Normal,
            //     AbsoluteExpiration = DateTime.Now.AddMinutes (30)
            //};

            //cache.Set<Employees>(employee.EmpNo, employee, options);

            employee.FirstName = "<img src=\"http://url.to.file.which/not.exist\" onerror=alert(document.cookie);>";
            return employee;
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post ([FromBody] Employees value)
        {
            bool error = false;

            string firstName = WebUtility.HtmlEncode(value.FirstName);
            string lastName = WebUtility.HtmlEncode(value.LastName);

            try {
                var context = new employeesContext ();

                context.Employees.Add (value);
                context.SaveChanges   ();
            } catch (Exception ex) {
                Console.WriteLine(ex.InnerException.Message);
                error = true;
            }

            var result = new {
                Status = !error ? "Success" : "Fail"
            };

            return new JsonResult (result);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put (int id, [FromBody] Employees value)
        {
            bool error = false;

            try
            {
                var context = new employeesContext();

                var employee = context.Employees.Where<Employees>(e => e.EmpNo == id).FirstOrDefault();
                if (employee == null) {
                    return new JsonResult (new { Status = "Fail" });;
                }

                employee.BirthDate = value.BirthDate;
                employee.FirstName = WebUtility.HtmlEncode (value.FirstName);
                employee.LastName = WebUtility.HtmlEncode (value.LastName);
                employee.Gender = value.Gender;
                employee.HireDate = value.HireDate;

                context.SaveChanges();
            } catch {
                error = true;
            }

            var result = new {
                Status = !error ? "Success" : "Fail"
            };

            return new JsonResult (result);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete (int id)
        {
            var context = new employeesContext ();

            var employee = context.Employees.Where<Employees>(e => e.EmpNo == id).FirstOrDefault();
            if (employee == null) return;

            context.Employees.Remove (employee);
            context.SaveChanges ();
        }
    }


    public class Mixed
    {
        public int emp_no { get; set; }
        public DateTime birth_name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime HireDate { get; set; }
        public string DeptNo { get; set; }
        public string DeptName { get; set; }
    }
}
