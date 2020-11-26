
import EmployeesList from './employees/listEmployees.js';
import AddEmployees from './employees/addEmployees.js';
import EditEmployees from './employees/editEmployees.js';

import './App.css'

import {
	BrowserRouter as Router,
	Link,
	Switch,
	Route
} from 'react-router-dom'; 

function App () {
	return (
		<Router>
			<div className="container">
				<nav className="navbar navbar-expand-lg navheader">
					<div className="collapse navbar-collapse">
						<ul className="navbar-nav mr-auto">
							<li className="nav-item">
								<Link to={'/addEmployee'} className="nav-link">Add Employee</Link>
							</li>
							<li className="nav-item">
								<Link to={'/EmployeesList'} className="nav-link">Employee List</Link>
							</li>
						</ul>
					</div>
				</nav>
				<br />
				<Switch>
					<Route exact path='/AddEmployee' component={AddEmployees} />
					<Route path='/edit/:id' component={EditEmployees} />
					<Route path='/EmployeesList' component={EmployeesList} /> 
				</Switch>	
			</div>
		</Router>
		
	);
}

export default App;
