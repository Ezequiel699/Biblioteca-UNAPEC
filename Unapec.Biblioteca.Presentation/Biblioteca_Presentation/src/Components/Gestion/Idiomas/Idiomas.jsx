// src/components/Gestion/Idiomas/Idiomas.jsx
import { FaLanguage, FaPlus } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { useEffect, useState } from 'react';
import ItemList from '../../Crud/Item';
import api from '../../../Services/api';
import './Idiomas.css';

const Idiomas = () => {
  const [idiomas, setIdiomas] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filtro, setFiltro] = useState(''); // '' todos | 'true' activos | 'false' inactivos

  useEffect(() => {
    const params = filtro === '' ? {} : { estado: filtro };
    api.get('/api/idiomas', { params })
      .then(res => { setIdiomas(res.data.items); setLoading(false); })
      .catch(() => setLoading(false));
  }, [filtro]);

  if (loading) return <p>Cargando idiomas…</p>;

  return (
    <div className='idiomas-container'>
      <div className='shared-wrapper'>
        <div className='header-row'>
          <h2 className='header-title'>Idiomas</h2>

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

            <Link to='/idiomas/nuevo' className='btn-add'>
              <FaPlus /> Añadir
            </Link>
          </div>
        </div>

        <div className='list-container'>
          {idiomas.map(i => (
            <ItemList
              key={i.id}
              item={i}
              icon={<FaLanguage className='list-icon' />}
              recurso='idiomas'
            />
          ))}
        </div>
      </div>
    </div>
  );
};

export default Idiomas;