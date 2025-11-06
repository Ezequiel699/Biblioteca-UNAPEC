// src/components/Gestion/Bibliografias/Bibliografias.jsx
import { FaBook, FaPlus } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { useEffect, useState } from 'react';
import ItemList from '../../Crud/Item';
import api from '../../../Services/api';
import './Usuarios.css';

const Usuarios = () => {
  const [biblios, setBiblios] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filtro, setFiltro] = useState(''); // '' todos | 'true' activos | 'false' inactivos

  useEffect(() => {
    const params = filtro === '' ? {} : { estado: filtro };
    api.get('/api/usuarios', { params })
      .then(res => { setBiblios(res.data.items); setLoading(false); })
      .catch(() => setLoading(false));
  }, [filtro]);

  if (loading) return <p>Cargando usuarios</p>;

  return (
        <div className='usuarios-container'>
          {/* Contenedor único: misma columna que la lista */}
          <div className='shared-wrapper'>
            {/* Fila 1: cabecera */}
            <div className='header-row'>
              <h2 className='header-title'>Autores</h2>

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

            {/* Fila 2: lista */}
            <div className='list-container'>
              {biblios.map(b => (
                <ItemList
                  key={b.id}
                  item={b}
                  icon={<FaBook className='list-icon' />}
                  recurso='usuarios'
                />
              ))}
            </div>
          </div>
        </div>
  );
};

export default Usuarios;