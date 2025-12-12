// src/components/Gestion/Autores/Autores.jsx
import { FaBook, FaPlus } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { useEffect, useState } from 'react';
import ItemList from '../../Crud/Item';
import ConfirmModal from '../../Crud/ConfirmModal';
import api from '../../../Services/api';
import './Autores.css';

const Autores = () => {
  const [autores, setAutores] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filtro, setFiltro] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [idEliminar, setIdEliminar] = useState(null);

  const cargarAutores = () => {
    setLoading(true);

    const params = filtro === '' ? {} : { estado: filtro };

    api.get('/api/autores', { params })
      .then(res => {
        setAutores(res.data.items || []);
      })
      .catch(err => {
        console.error('Error cargando autores:', err);
        setAutores([]);
      })
      .finally(() => setLoading(false));
  };

  useEffect(() => {
    cargarAutores();
  }, [filtro]);

  const pedirConfirmacion = (id) => {
    setIdEliminar(id);
    setModalOpen(true);
  };

  const confirmarEliminar = async () => {
    try {
      await api.delete(`/api/autores/${idEliminar}`);
      cargarAutores();
    } catch (err) {
      console.error("Error eliminando autor:", err);
    }

    setModalOpen(false);
  };

  if (loading) return <p>Cargando autores…</p>;

  return (
    <div className='autores-container'>
      <div className='shared-wrapper'>
        
        {/* Header */}
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

            <Link to='/autores/nuevo' className='btn-add'>
              <FaPlus /> Añadir
            </Link>
          </div>
        </div>

        {/* Lista */}
        <div className='list-container'>
          {autores.length === 0 ? (
            <p style={{ padding: "1rem" }}>No hay autores registrados.</p>
          ) : (
            autores.map(a => (
              <ItemList
                key={a.id}
                item={a}
                icon={<FaBook className='list-icon' />}
                recurso='autores'
                displayField='nombre'
                onDelete={() => pedirConfirmacion(a.id)}
              />
            ))
          )}
        </div>
      </div>

      {/* Modal Confirmación */}
      <ConfirmModal
        open={modalOpen}
        onClose={() => setModalOpen(false)}
        onConfirm={confirmarEliminar}
        message="¿Realmente deseas eliminar este autor?"
      />
    </div>
  );
};

export default Autores;
