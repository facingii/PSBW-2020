import React from 'react';
import {Link} from 'react-router-dom';
import axios from 'axios';

class EmployeesList extends React.Component {

	constructor (props) {
		super(props);

		this.state = {
			items: [],
			isFetched: false,
			error: null
		};

		this.deleteEmployee = this.deleteEmployee.bind(this);

	}

	componentDidMount () {
		fetch("http://localhost:5001/api/employees/")
		.then(res => res.json())
		.then(
			(result) => {
				this.setState (
					{
						items: result,
						isFetched: true,
						error: null
					}
				);
			},
			(error) => {
				this.setState (
					{
						items: [],
						isFetched: true,
						error: error
					}
				);
			}
		)
	}

	deleteEmployee (id) {
		console.log(id);

		axios.delete ("http://localhost:5001/api/employees/")
			.then (response => {
				if (response.status === 200) {
					if (response.data.staus === "Success") {
						alert("Registro eliminado!");
					}
				}
			});
	}

	render () {
		const { items, isFetched, error } = this.state;


		if (error) {
			return (<div><p>{error.message}</p></div>);
		} else if (!isFetched) {
			return (<div>
						<img src="https://media.giphy.com/media/sSgvbe1m3n93G/giphy.gif" />
					</div>);
		} else {
			return (
				<table className="table table stripped">
					<thead className="listHeader">
						<tr>
							<th>Nombre</th>
							<th>Apellidos</th>
							<th>Título</th>
							<th>Salario</th>
							<th colSpan="2">Acciones</th>
						</tr>
					</thead>
					<tbody>
						{
							items.map(i => 
									<tr>
										<td>{i.nombre}</td>
										<td>{i.apellidos}</td>
										<td>{i.titulo}</td>
										<td>{i.salario}</td>
										<td><Link to={"/edit/"+i.empNo} className="btn btn-success">Editar</Link></td>
										<td><button type="button" onClick={() => this.deleteEmployee(i.empNo)} className="btn btn-danger">Eliminar</button></td>
									</tr>
							)
						}
				    </tbody>
				</table>
				
			);
		} //else
		
	} // render

} // class


export default EmployeesList;