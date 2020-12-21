
import EmployeesList from './employees/listEmployees.js';
import AddEmployees from './employees/addEmployees.js';
import EditEmployees from './employees/editEmployees.js';
import Login from './login/login.js'

import { useState } from 'react';
import IdleTimer from 'react-idle-timer';

import './App.css';

import {
	BrowserRouter as Router,
	Link,
	Switch,
	Route
} from 'react-router-dom'; 


function App () {
	const [timeout, setTimeout] = useState (10000);
	const [userLoggedIn, setUserLoggedIn] = useState (false);
	const [isTimeout, setIsTimeout] = useState (false);
	const [idleTimer, setIdleTimer] = useState (null);	

	function onAction (e) {
		console.log("User did something");
		setTimeout(false);
	}

	function onActive (e) {
		console.log("User is active");
		setIsTimeout(false);
	}

	function onIdle (e) {
		console.log("User is idle");
		localStorage.setItem('ACCESS_TOKEN', '');
		//redirect land page
	}

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
				<IdleTimer
					ref = {ref => { setIdleTimer (ref) }}
					element = {document}
					onActive = {onActive}
					onAction = {onActive}
					onIdle = {onIdle}
					debounce = {250}
					timeout = {timeout} />

				
				<Switch>
					<Route exact path='/AddEmployee' component={AddEmployees} />
					<Route path='/edit/:id' component={EditEmployees} />
					<Route path='/EmployeesList' component={EmployeesList} />
					<Route exact path='/login' component={Login} />
				</Switch>
			</div>
		</Router>
		
	);
	
}

export default App;
