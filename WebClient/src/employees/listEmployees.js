import React from 'react';

class EmployeesList extends React.Component {

	constructor (props) {
		super(props);

		this.state = {
			items: [],
			isFetched: false,
			error: null
		};

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
				<table className="table">
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
										<td>Editar</td>
										<td>Eliminar</td>
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