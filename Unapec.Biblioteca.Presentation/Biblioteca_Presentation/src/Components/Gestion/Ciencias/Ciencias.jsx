// src/components/Gestion/Ciencias/Ciencias.jsx
import { FaFlask, FaPlus } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { useEffect, useState } from 'react';
import ItemList from '../../Crud/Item';
import api from '../../../Services/api';
import ConfirmModal from '../../Crud/ConfirmModal';
import './Ciencias.css';

const Ciencias = () => {
  const [ciencias, setCiencias] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filtro, setFiltro] = useState('');

  const [modalOpen, setModalOpen] = useState(false);
  const [idEliminar, setIdEliminar] = useState(null);

  // 🔄 Cargar Ciencias
  const cargarCiencias = () => {
    setLoading(true);

    const params = filtro === '' ? {} : { estado: filtro };

    api.get('/api/ciencias', { params })
      .then(res => { 
        setCiencias(res.data.items || []); 
      })
      .catch(err => {
        console.error("Error cargando ciencias:", err);
        setCiencias([]);
      })
      .finally(() => setLoading(false));
  };

  useEffect(() => {
    cargarCiencias();
  }, [filtro]);

  // 🟢 Pedir confirmación antes de eliminar
  const pedirConfirmarEliminacion = (id) => {
    setIdEliminar(id);
    setModalOpen(true);
  };

  // 🔴 Confirmar eliminación
  const confirmarEliminar = async () => {
    try {
      await api.delete(`/api/ciencias/${idEliminar}`);
      cargarCiencias();
    } catch (error) {
      console.error("Error eliminando ciencia:", error);
    }

    setModalOpen(false);
  };

  if (loading) return <p>Cargando ciencias…</p>;

  return (
    <div className='ciencias-container'>
      <div className='shared-wrapper'>

        {/* Header */}
        <div className='header-row'>
          <h2 className='header-title'>Ciencias</h2>

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

            <Link to='/ciencias/nuevo' className='btn-add'>
              <FaPlus /> Añadir
            </Link>
          </div>
        </div>

        {/* Lista */}
        <div className='list-container'>
          {ciencias.length === 0 ? (
            <p style={{ padding: "1rem" }}>No hay ciencias registradas.</p>
          ) : (
            ciencias.map(c => (
              <ItemList
                key={c.id}
                item={c}
                icon={<FaFlask className='list-icon' />}
                recurso='ciencias'
                displayField='descripcion'
                onDelete={() => pedirConfirmarEliminacion(c.id)}  // 👉 AHORA SÍ TIENE EL BOTÓN
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
        message="¿Realmente deseas eliminar esta ciencia?"
      />
    </div>
  );
};

export default Ciencias;
