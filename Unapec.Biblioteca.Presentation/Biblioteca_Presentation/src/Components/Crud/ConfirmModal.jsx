// src/components/Crud/ConfirmModal.jsx
import './ConfirmModal.css';

const ConfirmModal = ({ show, title, children, onConfirm, onCancel }) => {
  if (!show) return null;

  return (
    <div className='modal-overlay'>
      <div className='modal-box'>
        <h3>{title}</h3>
        <div className='modal-body'>{children}</div>

        <div className='modal-actions'>
          <button className='btn-confirm' onClick={onConfirm}>Eliminar</button>
          <button className='btn-cancel' onClick={onCancel}>Cancelar</button>
        </div>
      </div>
    </div>
  );
};

export default ConfirmModal;