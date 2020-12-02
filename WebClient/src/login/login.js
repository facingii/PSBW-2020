import React from "react";
import axios from "axios";

import { 
	Form, 
	FormGroup,
	Label, 
	Input, 
	Button 
} from "reactstrap";

import "./login.css";

class Login extends React.Component {

	constructor (props) {
		super(props);

		this.state = {
			User: {},
			UserName: '',
			Password: '',
			Prev: props.location.state.from.pathname
		}
		
		this.doLogin = this.doLogin.bind(this);
	}

	doLogin () {
		axios.post ('http://localhost:5001/api/users/authenticate',
		{
			"firstName": "",
			"lastName": "",
			"userName": this.state.UserName,
			"password": this.state.Password,
			"token": ""
		}).then (
			(response) => {
				if (response.status === 200) {
					const json = response.data;
					localStorage.setItem("ACCESS_TOKEN", json.token);
					this.props.history.push(this.state.Prev);
				}
			},
			(error) => {
				console.log("Exception " + error);
			}
		);
	}

	handleChange = (e) => {
		this.setState (
			{
				[e.target.name]: e.target.value
			}
		);
	}

	render () {
		return (
			<div className="Login">
				<Form>
					<FormGroup size="lg">
						<Label>Usuario</Label>
						<Input name="UserName" type="text" onChange={this.handleChange} value={this.state.UserName} />
					</FormGroup>
					<FormGroup size="lg">
						<Label>Contraseña</Label>
						<Input type="password" name="Password" onChange={this.handleChange} value={this.state.Password} />
					</FormGroup>
					<Button block size="lg" type="button" onClick={this.doLogin}>
						login
					</Button>
				</Form>
			</div>
		);
	}

}

export default Login;