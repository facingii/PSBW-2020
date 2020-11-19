// import libraries
import React from 'react';
import axios from 'axios';

// import css
//import './Addemployees.css';

// import widgets
import {
	Container,
	Col,
	Form,
	Row,
	FormGroup,
	Label,
	Input,
	Button
} from 'reactstrap';

// clase modelo para los datos del formulario
class AddEmployees extends React.Component {
	constructor (props) {
		super(props);

		// mantiene el estado del formulario
		this.state = {
			EmpNo: '',
			FirstName: '',
			LastName: '',
			BirthDate: '',
			HireDate: ''
		}
	}

	// función de envío de datos al backend
	Addemployees = () => {
		axios.post ('http://localhost:5000/api/employees/add',
		{
			EmpNo: this.state.EmpNo,
			FirstName: this.state.FirstName,
			LastName: this.state.LastName,
			BirthDate: this.state.BirthDate,
			HireDate: this.state.BirthDate
		}).then (json => {
			if (json.data.Status === 'Success') {
				console.log(json.data.Status);
				alert("Data saved!");
				this.props.history.push('/employeesList');
			} else {
				alert('Data not saved!');
				debugger;
				this.props.history.push('employeesList');
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
		return (
			<Container className="App">
				<h4 className="PageHeading">Enter employee infomation</h4>
				<Form className="form">
					<Col>
						<FormGroup row>
							<Label for="name" sm={2}>No. Employee</Label>
							<Col sm={2}>
								<Input type="text" name="empno" onChange={this.handleChange} value={this.state.EmpNo} />
							</Col>
						</FormGroup>
						<FormGroup row>
							<Label for="name" sm={2}>Name</Label>
							<Col sm={2}>
								<Input type="text" name="firstName" onChange={this.handleChange} value={this.state.FirstName} />
							</Col>
						</FormGroup>
						<FormGroup row>
							<Label for="name" sm={2}>First Name</Label>
							<Col sm={2}>
								<Input type="text" name="lastName" onChange={this.handleChange} value={this.state.LastName} />
							</Col>
						</FormGroup>
						<FormGroup row>
							<Label for="name" sm={2}>Birth Date</Label>
							<Col sm={2}>
								<Input type="text" name="firstName" onChange={this.handleChange} value={this.state.BirthDate} />
							</Col>
						</FormGroup>
						<FormGroup row>
							<Label for="name" sm={2}>Hire Date</Label>
							<Col sm={2}>
								<Input type="text" name="firstName" onChange={this.handleChange} value={this.state.FirstName} />
							</Col>
						</FormGroup>
					</Col>
					<Col>
						<FormGroup row>
							<Col sm={5}>
							</Col>
							<Col sm={1}>
								<button type="button" onClick={this.Addstudent} className="btn btn-success">Submit</button>
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

export default AddEmployees;