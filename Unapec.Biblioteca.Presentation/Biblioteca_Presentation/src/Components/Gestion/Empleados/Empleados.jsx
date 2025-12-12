// src/components/Gestion/Empleados/Empleados.jsx
import { FaUserTie, FaPlus } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { useEffect, useState } from 'react';
import ItemList from '../../Crud/Item';
import api from '../../../Services/api';
import './Empleados.css';

const Empleados = () => {
  const [empleados, setEmpleados] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filtro, setFiltro] = useState('');

  useEffect(() => {
    const params = filtro === '' ? {} : { estado: filtro };
    api.get('/api/empleados', { params })
      .then(res => { 
        setEmpleados(res.data.items); 
        setLoading(false); 
      })
      .catch((error) => {
        console.error('Error cargando empleados:', error);
        setEmpleados([]);
        setLoading(false);
      });
  }, [filtro]);

  if (loading) return <p>Cargando empleados…</p>;

  return (
    <div className='empleados-container'>
      <div className='shared-wrapper'>
        <div className='header-row'>
          <h2 className='header-title'>Empleados</h2>

          <div className='header-controls'>
            <select
              value={filtro}
              onChange={e => setFiltro(e.target.value)}
              className='filter-select'
            >
              <option value=''>Todos</option>
              <option value='true'>Activos</option>
              <option value='false'>Inactivos</option>
            </select>

            <Link to='/empleados/nuevo' className='btn-add'>
              <FaPlus /> Añadir
            </Link>
          </div>
        </div>

        <div className='list-container'>
          {empleados.map(empleado => (
            <ItemList
              key={empleado.id}
              item={empleado}
              icon={<FaUserTie className='list-icon' />}
              recurso='empleados'
              displayField='nombre'
            />
          ))}
        </div>
      </div>
    </div>
  );
};

export default Empleados;