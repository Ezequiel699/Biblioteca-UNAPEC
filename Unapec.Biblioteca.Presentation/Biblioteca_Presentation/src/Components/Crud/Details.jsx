// src/components/Crud/Detalle.jsx
import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import api from '../../Services/api';
import './Edit.css';   // re-usamos los estilos del formulario

const Detalle = () => {
  const { recurso, id } = useParams();
  const navigate = useNavigate();

  const [item, setItem] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    api.get(`/api/${recurso}/${id}`)
      .then(res => {
        setItem(res.data);
        setLoading(false);
      })
      .catch(err => {
        console.error(err);
        setLoading(false);
      });
  }, [recurso, id]);

  if (loading) return <p>Cargando…</p>;
  if (!item)   return <p>No encontrado.</p>;

  return (
    <div className="editarForm">
      <h2>Detalle de {recurso}</h2>

      <div className="field">
        <label>Descripción</label>
        <p>{item.descripcion}</p>
      </div>

      <div className="field">
        <label>Estado</label>
        <p>{item.estado ? 'Activo' : 'Inactivo'}</p>
      </div>

      <div className="field">
        <label>Creado</label>
        <p>{new Date(item.creadoEn).toLocaleString()}</p>
      </div>

      <div className="field">
        <label>Última actualización</label>
        <p>{new Date(item.actualizadoEn).toLocaleString()}</p>
      </div>

      <div className="actions">
        <button type="button" className="submitBtn" onClick={() => navigate(`/${recurso}`)}>
          Volver
        </button>
      </div>
    </div>
  );
};

export default Detalle;