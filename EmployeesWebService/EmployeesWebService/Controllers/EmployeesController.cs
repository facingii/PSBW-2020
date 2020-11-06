using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using EmployeesWebService.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace EmployeesWebService.Controllers
{
    public class Empleado
    {
        public String Nombre { get; set; }
        public String Apellidos { get; set; }
        public String Titulo { get; set; }
        public int Salario { get; set; }
        public DateTime DepaEmpleado { get; set; }
        public DateTime DepaManager { get; set; }
    }

    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
        private const string EMPLOYEES_KEY = "empleadodelmes";
        private const string EMPLOYEES_LIST = "listadeempleados";
        private IMemoryCache cache;

        public EmployeesController (IMemoryCache cache)
        {
            this.cache = cache;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Empleado> Get ()
        {
            var cachado = cache.Get<List<Empleado>>(EMPLOYEES_LIST);
            if (cachado != null)
            {
                return cachado;
            }


            var context = new employeesContext ();
            //var employees = context.Employees.Where<Employees> (e => e.LastName.Contains ("Smith"));

            //
            // SELECT employees.first_name, employees.last_name, titles.title, salaries.salary
            // FROM employees INNER JOIN titles on employees.emp_no = titles.emp_no
            // INNER JOIN salaries on employees.emp_no = salaries.emp_no
            // INNER JOIN inner join dept_emp on employees.emp_no = dept_emp.emp_no
            // INNER JOIN dept_manager on employees.emp_no = dept_manager.emp_no
            // WHERE employees.last_name like 'smith';
            //
            var employees = from e in context.Employees
                            join t in context.Titles on e.EmpNo equals t.EmpNo
                            join s in context.Salaries on e.EmpNo equals s.EmpNo
                            join de in context.DeptEmp on e.EmpNo equals de.EmpNo
                            join dm in context.DeptManager on e.EmpNo equals dm.EmpNo
                            /*where e.LastName.Contains ("Smith")*/ select new Empleado
                            {
                                Nombre = e.FirstName,
                                Apellidos =  e.LastName,
                                Titulo = t.Title,
                                Salario = s.Salary,
                                DepaEmpleado = de.FromDate,
                                DepaManager = dm.FromDate
                            };


            var options = new MemoryCacheEntryOptions ()
            {
                Priority = CacheItemPriority.High,
                AbsoluteExpiration = DateTime.Now.AddMinutes(3)
            };
      
            cache.Set (EMPLOYEES_LIST, employees.ToList (), options);

            return employees;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Employees Get (int id)
        {
            var cachado = cache.Get<Employees> (EMPLOYEES_KEY);
            if (cachado != null)
            {
                return cachado;
            }  

            var context = new employeesContext ();

            Employees employee = context.Employees.Where<Employees>(e => e.EmpNo == id).FirstOrDefault<Employees>(); 
            if (employee == null)
            {
                return null;
            }

            var options = new MemoryCacheEntryOptions
            {
                 Priority = CacheItemPriority.Normal,
                 AbsoluteExpiration = DateTime.Now.AddMinutes (30)
            };

            cache.Set<Employees>(EMPLOYEES_KEY, employee, options);
            return employee;
        }

        // POST api/values
        [HttpPost]
        public void Post ([FromBody] Mixed value)
        {
            var context = new employeesContext ();

            using (var transaction = context.Database.BeginTransaction ())
            {
                Employees employee = new Employees
                {
                    EmpNo = value.emp_no,
                    BirthDate = value.birth_name,
                    FirstName = value.FirstName,
                    LastName = value.LastName,
                    Gender = value.Gender,
                    HireDate = value.HireDate
                };

                Departments deparment = new Departments
                {
                    DeptNo = value.DeptNo,
                    DeptName = value.DeptName
                };

                context.Employees.Add (employee);
                context.Departments.Add (deparment);

                try
                {
                    context.SaveChanges ();
                    transaction.Commit ();
                } catch (Exception ex)
                {
                    //logger.LogError (ex.Message);
                }
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put (int id, [FromBody] Employees value)
        {
            var context = new employeesContext ();

            var employee = context.Employees.Where<Employees> (e => e.EmpNo == id).FirstOrDefault ();
            if (employee == null) return;

            employee.BirthDate = value.BirthDate;
            employee.FirstName = value.FirstName;
            employee.LastName = value.LastName;
            employee.Gender = value.Gender;
            employee.HireDate = value.HireDate;

            context.SaveChanges ();
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
