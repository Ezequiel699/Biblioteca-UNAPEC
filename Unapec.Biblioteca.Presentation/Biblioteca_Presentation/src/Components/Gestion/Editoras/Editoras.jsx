// src/components/Gestion/Editoras/Editoras.jsx
import { FaBuilding, FaPlus } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { useEffect, useState } from 'react';
import ItemList from '../../Crud/Item';
import api from '../../../Services/api';
import ConfirmModal from '../../Crud/ConfirmModal';
import './Editoras.css';

const Editoras = () => {
  const [editoras, setEditoras] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filtro, setFiltro] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [idEliminar, setIdEliminar] = useState(null);

  const cargarEditoras = () => {
    setLoading(true);
    const params = filtro === '' ? {} : { estado: filtro };

    api.get('/api/editoras', { params })
      .then(res => setEditoras(res.data.items || []))
      .catch(() => setEditoras([]))
      .finally(() => setLoading(false));
  };

  useEffect(() => {
    cargarEditoras();
  }, [filtro]);

  const pedirConfirmarEliminacion = (id) => {
    setIdEliminar(id);
    setModalOpen(true);
  };

  const confirmarEliminar = async () => {
    try {
      await api.delete(`/api/editoras/${idEliminar}`);
      cargarEditoras();
    } catch (error) {
      console.error("Error eliminando editora:", error);
    }
    setModalOpen(false);
  };

  if (loading) return <p>Cargando editoras…</p>;

  return (
    <div className='editoras-container'>
      <div className='shared-wrapper'>

        <div className='header-row'>
          <h2 className='header-title'>Editoras</h2>

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

            <Link to='/editoras/nuevo' className='btn-add'>
              <FaPlus /> Añadir
            </Link>
          </div>
        </div>

        <div className='list-container'>
          {editoras.length === 0 ? (
            <p style={{ padding: "1rem" }}>No hay editoras registradas.</p>
          ) : (
            editoras.map(e => (
              <ItemList
                key={e.id}
                item={e}
                icon={<FaBuilding className='list-icon' />}
                recurso='editoras'
                displayField='descripcion'
                onDelete={() => pedirConfirmarEliminacion(e.id)}
              />
            ))
          )}
        </div>
      </div>

      <ConfirmModal
        open={modalOpen}
        onClose={() => setModalOpen(false)}
        onConfirm={confirmarEliminar}
        message="¿Realmente deseas eliminar esta editora?"
      />
    </div>
  );
};

export default Editoras;
