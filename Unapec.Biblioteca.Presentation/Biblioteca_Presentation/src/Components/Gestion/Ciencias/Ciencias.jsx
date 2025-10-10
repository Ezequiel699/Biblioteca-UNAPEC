// src/components/Gestion/Ciencias/Ciencias.jsx
import { FaFlask, FaPlus } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { useEffect, useState } from 'react';
import ItemList from '../../Crud/Item';
import api from '../../../Services/api';
import './Ciencias.css';

const Ciencias = () => {
  const [ciencias, setCiencias] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filtro, setFiltro] = useState('');

  useEffect(() => {
    const params = filtro === '' ? {} : { estado: filtro };
    api.get('/api/ciencias', { params })
      .then(res => { setCiencias(res.data.items); setLoading(false); })
      .catch(() => setLoading(false));
  }, [filtro]);

  if (loading) return <p>Cargando ciencias…</p>;

  return (
    <div className='ciencias-container'>
      <div className='shared-wrapper'>
        <div className='header-row'>
          <h2 className='header-title'>Ciencias</h2>

          <div className='header-controls'>
            <select value={filtro} onChange={e => setFiltro(e.target.value)} className='filter-select'>
              <option value=''>Todos</option>
              <option value='true'>Activos</option>
              <option value='false'>Inactivos</option>
            </select>

            <Link to='/ciencias/nuevo' className='btn-add'>
              <FaPlus /> Añadir
            </Link>
          </div>
        </div>

        <div className='list-container'>
          {ciencias.map(c => (
            <ItemList key={c.id} item={c} icon={<FaFlask className='list-icon' />} recurso='ciencias' />
          ))}
        </div>
      </div>
    </div>
  );
};

export default Ciencias;