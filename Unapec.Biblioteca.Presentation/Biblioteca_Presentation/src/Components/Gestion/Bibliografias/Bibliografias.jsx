// src/components/Gestion/Bibliografias/Bibliografias.jsx
import { FaBook, FaPlus } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { useEffect, useState } from 'react';
import ItemList from '../../Crud/Item';
import api from '../../../Services/api';
import ConfirmModal from '../../Crud/ConfirmModal';
import './Bibliografias.css';

const Bibliografias = () => {
  const [biblios, setBiblios] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filtro, setFiltro] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [idEliminar, setIdEliminar] = useState(null);

  const cargarBiblios = () => {
    setLoading(true);
    const params = filtro === '' ? {} : { estado: filtro };

    api.get('/api/tipos-bibliografia', { params })
      .then(res => setBiblios(res.data.items || []))
      .catch(err => {
        console.error('Error cargando bibliografías:', err);
        setBiblios([]);
      })
      .finally(() => setLoading(false));
  };

  useEffect(() => {
    cargarBiblios();
  }, [filtro]);

  // 🟢 Abrir modal y guardar el ID
  const pedirConfirmarEliminacion = (id) => {
    setIdEliminar(id);
    setModalOpen(true);
  };

  // 🔴 Confirmar eliminación
  const confirmarEliminar = async () => {
    try {
      await api.delete(`/api/tipos-bibliografia/${idEliminar}`);
      cargarBiblios();
    } catch (err) {
      console.error('Error eliminando bibliografía:', err);
    }
    setModalOpen(false);
  };

  if (loading) return <p>Cargando bibliografías…</p>;

  return (
    <div className='bibliographics-container'>
      <div className='shared-wrapper'>

        {/* Header */}
        <div className='header-row'>
          <h2 className='header-title'>Bibliografías</h2>

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

            <Link to='/tipos-bibliografia/nuevo' className='btn-add'>
              <FaPlus /> Añadir
            </Link>
          </div>
        </div>

        {/* Lista */}
        <div className='list-container'>
          {biblios.length === 0 ? (
            <p style={{ padding: "1rem" }}>No hay bibliografías registradas.</p>
          ) : (
            biblios.map(b => (
              <ItemList
                key={b.id}
                item={b}
                icon={<FaBook className='list-icon' />}
                recurso='tipos-bibliografia'
                displayField='descripcion'
                onDelete={() => pedirConfirmarEliminacion(b.id)}
              />
            ))
          )}
        </div>
      </div>

      {/* Modal de confirmación */}
      <ConfirmModal
        open={modalOpen}
        onClose={() => setModalOpen(false)}
        onConfirm={confirmarEliminar}
        message="¿Realmente deseas eliminar esta bibliografía?"
      />
    </div>
  );
};

export default Bibliografias;
