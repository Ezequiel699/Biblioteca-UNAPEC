// src/components/Gestion/Editoras/Editoras.jsx
import { FaBuilding, FaPlus } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { useEffect, useState } from 'react';
import ItemList from '../../Crud/Item';
import api from '../../../Services/api';
import './Editoras.css';

const Editoras = () => {
  const [editoras, setEditoras] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filtro, setFiltro] = useState('');

  useEffect(() => {
    const params = filtro === '' ? {} : { estado: filtro };
    api.get('/api/editoras', { params })
      .then(res => { setEditoras(res.data.items); setLoading(false); })
      .catch(() => setLoading(false));
  }, [filtro]);

  if (loading) return <p>Cargando editoras…</p>;

  return (
    <div className='editoras-container'>
      <div className='shared-wrapper'>
        <div className='header-row'>
          <h2 className='header-title'>Editoras</h2>

          <div className='header-controls'>
            <select value={filtro} onChange={e => setFiltro(e.target.value)} className='filter-select'>
              <option value=''>Todos</option>
              <option value='true'>Activos</option>
              <option value='false'>Inactivos</option>
            </select>

            <Link to='/editoras/nuevo' className='btn-add'>
              <FaPlus /> Añadir
            </Link>
          </div>
        </div>

        <div className='list-container'>
          {editoras.map(e => (
            <ItemList key={e.id} item={e} icon={<FaBuilding className='list-icon' />} recurso='editoras' />
          ))}
        </div>
      </div>
    </div>
  );
};

export default Editoras;