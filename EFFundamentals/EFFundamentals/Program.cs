using System;
using System.Linq;

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
            //Employees emp = context.Employees.Find (new object [] { 10 });
            //context.Employees.Remove (emp);
            //context.SaveChanges();


            // esta forma de consulta utiliza la función extendida Where, utilizando la función lambda como
            // forma de filtrado
            Employees employee = context.Employees.Where<Employees>(e => e.FirstName == "AAA").FirstOrDefault<Employees>();
            employee.LastName = "CCC";
            context.SaveChanges();

            // en esta forma se utiliza la forma de selección que utiliza una estructura tipo sql para definir
            // los parámetros de la consulta
            Departments dept = (from d in context.Departments
                               where d.DeptNo == "9999" select d).FirstOrDefault<Departments> ();

            dept.DeptName = "PROGRAMACION DE SISTEMAS BASADOS EN WEB";
            context.SaveChanges();

            // la función First devuelve el primer registro de la colección consultada
            Employees e1 = context.Employees.First<Employees>();
            Console.WriteLine("{0} {1}", e1.FirstName, e1.LastName);


            // SELECT employees.first_name FROM employees INNER JOIN
            // (SELECT dept_manager.emp_no FROM dept_manager INNER JOIN departments ON
            // dept_manager.dept_no = departments.dept_no WHERE departments.dept_no = 'd005') deptos ON employees.emp_no = deptos.emp_no
        }
    }
}
