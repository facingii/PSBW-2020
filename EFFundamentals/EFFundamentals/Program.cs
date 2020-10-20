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
            foreach (var e in context.Departments)
            {
                //Console.WriteLine (e.DeptNo);
            }

            // La función find realiza una búsqueda utilizando el campo llave de la tabla, su representación en SQL es
            // SELECT * FROM Departments WHERE dept_no = 'd005';
            //
            // context.Departments.Find (new Object [] { "d005" });
            Departments department = context.Departments.Find (new object[] { "d005" });
            Console.WriteLine(department.DeptName);

            // SELECT employees.first_name FROM employees INNER JOIN
            // (SELECT dept_manager.emp_no FROM dept_manager INNER JOIN departments ON
            // dept_manager.dept_no = departments.dept_no WHERE departments.dept_no = 'd005') deptos ON employees.emp_no = deptos.emp_no
        }
    }
}
