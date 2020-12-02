// import libraries
import React from 'react';
import axios from 'axios';
import { Redirect, useHistory } from 'react-router-dom';
// import css
//import './Addemployees.css';

// import widgets
import {
	Container,
	Col,
	Form,
	FormGroup,
	Label,
	Input,
	Button
} from 'reactstrap';

// clase modelo para los datos del formulario
class AddEmployees extends React.Component {
	constructor (props) {
		super(props);

		const tk = localStorage.getItem('ACCESS_TOKEN');

		// mantiene el estado del formulario
		this.state = {
			EmpNo: '',
			FirstName: '',
			LastName: '',
			BirthDate: '',
			HireDate: '',
			Token: tk
		}
	}

	// función de envío de datos al backend
	Add = () => {
		const data = {
			"empNo":parseInt(this.state.EmpNo),
			"birthDate":this.state.BirthDate,
		    "firstName":this.state.FirstName,
			"lastName":this.state.LastName,
			"gender":"F",
			"hireDate":this.state.HireDate	
		}

		axios.post ('http://localhost:5001/api/employees', JSON.stringify(data), {
			headers: {
				'Accept': 'application/json',
				'Content-type': 'application/json',
				'Authorization': 'Bearer ' + this.state.Token
			}
		}).then (json => {
			if (json.data.status === 'Success') {
				alert("Data saved!");
				//this.props.history.push('/employeesList');
			} else {
				alert('Data not saved!');
				//debugger;
				//this.props.history.push('employeesList');
			}
		})

	}

	// modifica el estado del formulario de acuerdo a los valores
	// de los campos
	handleChange = (e) => {
		this.setState ({
			[e.target.name]:e.target.value
		});
	}

	// dibuja al componente
	render () {
		if (!this.state.Token) {
			return (
				<Redirect to={{ 
					pathname: '/login', 
					state: { 
						from: this.props.location 
					} 
				}} />
			);
		} else {
			return (
				<Container className="App">
					<h4 className="PageHeading">Enter employee infomation</h4>
					<Form className="form">
						<Col>
							<FormGroup row>
								<Label for="name" sm={2}>No. Employee</Label>
								<Col sm={2}>
									<Input type="text" name="EmpNo" onChange={this.handleChange} value={this.state.EmpNo} />
								</Col>
							</FormGroup>
							<FormGroup row>
								<Label for="name" sm={2}>First Name</Label>
								<Col sm={2}>
									<Input type="text" name="FirstName" onChange={this.handleChange} value={this.state.FirstName} />
								</Col>
							</FormGroup>
							<FormGroup row>
								<Label for="name" sm={2}>Last Name</Label>
								<Col sm={2}>
									<Input type="text" name="LastName" onChange={this.handleChange} value={this.state.LastName} />
								</Col>
							</FormGroup>
							<FormGroup row>
								<Label for="name" sm={2}>Birth Date</Label>
								<Col sm={2}>
									<Input bsSize="lg" type="date" name="BirthDate" value={this.state.BirthDate} onChange={this.handleChange} />
								</Col>
							</FormGroup>
							<FormGroup row>
								<Label for="name" sm={2}>Hire Date</Label>
								<Col sm={2}>
									<Input type="text" name="HireDate" onChange={this.handleChange} value={this.state.HireDate} />
								</Col>
							</FormGroup>
						</Col>
						<Col>
							<FormGroup row>
								<Col sm={5}>
								</Col>
								<Col sm={1}>
									<button type="button" onClick={this.Add} className="btn btn-success">Submit</button>
								</Col>
								<Col sm={1}>
									<Button color="danger">Cancel</Button>{' '}
								</Col>
								<Col sm={5}>
								</Col>
							</FormGroup>
						</Col>
					</Form>
				</Container>
			);
		}
	}
}

export default AddEmployees;