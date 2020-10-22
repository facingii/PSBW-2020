using System;

using EFFundamentals.Models;

namespace EFFundamentals
{
    class Program
    {
        static void Main (string[] args)
        {
            // obtenemos el contexto que nos permite realizar operaciones CRUD sobre la conexión a la BD 
            employeesContext context = new employeesContext ();

            // Recorre la tabla departamentos interando por cada uno de los elementos
            // SELECT * FROM Departments;
            //
            // context.Departments
            foreach (var d in context.Departments)
            {
                //Console.WriteLine (d.DeptNo);
            }

            // La función find realiza una búsqueda utilizando el campo llave de la tabla, su representación en SQL es
            // SELECT * FROM Departments WHERE dept_no = 'd005';
            //
            // context.Departments.Find (new Object [] { "d005" });
            Departments department = context.Departments.Find (new object[] { "d005" });
            //Console.WriteLine(department.DeptName);

            var task = context.Departments.FindAsync (new object[] { "d005" });
            //Console.WriteLine("OP. adicionales");

            //Console.WriteLine("Mas OP. adicionales");

            //Console.WriteLine ("Departamento encontrado {0}", task.Result.DeptName);

            //
            // INSERT INTO employees VALUES (10, '21/10/2018', 'AAA', 'BBB', 'M', '21/10/2018');
            //
            //context.Employees.Add (new Employees()
            //{
            //    EmpNo = 10,
            //    BirthDate = DateTime.Now,
            //    FirstName = "AAA",
            //    LastName = "BBB",
            //    Gender = "M",
            //    HireDate = DateTime.Now
            //});

            //
            // INSERT INTO departments VALUES ('9999', 'SISTEMA WEB');
            //
            //context.Departments.Add (new Departments()
            //{
            //    DeptNo = "9999",
            //    DeptName = "SISTEMA WEB"
            //});

            //context.SaveChanges ();

            //
            // DELETE FROM employees WHERE no_emp = 10;
            //
            //Employees emp = context.Employees.Find(new object[] { 10 });
            //context.Employees.Remove(emp);
            //context.SaveChanges();

            // SELECT employees.first_name FROM employees INNER JOIN
            // (SELECT dept_manager.emp_no FROM dept_manager INNER JOIN departments ON
            // dept_manager.dept_no = departments.dept_no WHERE departments.dept_no = 'd005') deptos ON employees.emp_no = deptos.emp_no
        }
    }
}
