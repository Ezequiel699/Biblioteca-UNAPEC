// src/components/Gestion/Usuarios/Usuarios.jsx
import { FaUser, FaPlus } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { useEffect, useState } from 'react';
import ItemList from '../../Crud/Item';
import api from '../../../Services/api';
import './Usuarios.css';

const Usuarios = () => {
  const [usuarios, setUsuarios] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filtro, setFiltro] = useState('');

  useEffect(() => {
    const params = filtro === '' ? {} : { estado: filtro };
    api.get('/api/usuarios', { params })
      .then(res => { 
        setUsuarios(res.data);
        setLoading(false); 
      })
      .catch((error) => {
        console.error('Error cargando usuarios:', error);
        setUsuarios([]);
        setLoading(false);
      });
  }, [filtro]);

  if (loading) return <p>Cargando usuarios…</p>;

  return (
    <div className='usuarios-container'>
      <div className='shared-wrapper'>
        <div className='header-row'>
          <h2 className='header-title'>Usuarios</h2>

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

            <Link to='/usuarios/nuevo' className='btn-add'>
              <FaPlus /> Añadir
            </Link>
          </div>
        </div>

        <div className='list-container'>
          {usuarios.map(u => (
            <ItemList
              key={u.id}
              item={u}
              icon={<FaUser className='list-icon' />}
              recurso='usuarios'
              displayField='nombre'
            />
          ))}
        </div>
      </div>
    </div>
  );
};

export default Usuarios;