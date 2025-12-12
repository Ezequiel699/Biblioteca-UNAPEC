import './App.css'
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import NavBar from './Components/NavBar/NavBar'
import Bibliografias from './Components/Gestion/Bibliografias/Bibliografias'
import Editoras from './Components/Gestion/Editoras/Editoras'
import Home from './Components/Home/Home'
import Ciencias from './Components/Gestion/Ciencias/Ciencias'
import Idiomas from './Components/Gestion/Idiomas/Idiomas'
import Autores from './Components/Gestion/Autores/Autores'
import Usuarios from './Components/Gestion/Usuarios/Usuarios'
import Libros from './Components/Gestion/Libros/Libros'
import Empleados from './Components/Gestion/Empleados/Empleados'
import Login from './Components/Login/Login'
import Editar from './Components/Crud/Edit'
import Detalle from './Components/Crud/Details'
import Add from './Components/Crud/Add'
import LibrosAdd from './Components/Gestion/Libros/LibrosAdd'
import LibrosEditar from './Components/Gestion/Libros/LibrosEditar'
import LibrosDetalle from './Components/Gestion/Libros/LibrosDetalle'

function App() {
  return (
    <BrowserRouter>
      <div className='page-container'>
        <NavBar />
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/" element={<Home />} />
          
          {/* Tipos de Bibliografía */}
          <Route path="/tipos-bibliografia" element={<Bibliografias />} />
          <Route path="/tipos-bibliografia/nuevo" element={<Add />} />
          <Route path="/tipos-bibliografia/editar/:id" element={<Editar />} />
          <Route path="/tipos-bibliografia/detalle/:id" element={<Detalle />} />
          
          {/* Editoras */}
          <Route path="/editoras" element={<Editoras />} />
          <Route path="/editoras/nuevo" element={<Add />} />
          <Route path="/editoras/editar/:id" element={<Editar />} />
          <Route path="/editoras/detalle/:id" element={<Detalle />} />
          
          {/* Ciencias */}
          <Route path="/ciencias" element={<Ciencias />} />
          <Route path="/ciencias/nuevo" element={<Add />} />
          <Route path="/ciencias/editar/:id" element={<Editar />} />
          <Route path="/ciencias/detalle/:id" element={<Detalle />} />
          
          {/* Idiomas */}
          <Route path="/idiomas" element={<Idiomas />} />
          <Route path="/idiomas/nuevo" element={<Add />} />
          <Route path="/idiomas/editar/:id" element={<Editar />} />
          <Route path="/idiomas/detalle/:id" element={<Detalle />} />
          
          {/* Autores */}
          <Route path="/autores" element={<Autores />} />
          <Route path="/autores/nuevo" element={<Add />} />
          <Route path="/autores/editar/:id" element={<Editar />} />
          <Route path="/autores/detalle/:id" element={<Detalle />} />
          
          {/* Usuarios */}
          <Route path="/usuarios" element={<Usuarios />} />
          <Route path="/usuarios/nuevo" element={<Add />} />
          <Route path="/usuarios/editar/:id" element={<Editar />} />
          <Route path="/usuarios/detalle/:id" element={<Detalle />} />
          
          {/* Libros */}
          <Route path="/libros" element={<Libros />} />
          <Route path="/libros/nuevo" element={<LibrosAdd />} />
          <Route path="/libros/editar/:id" element={<LibrosEditar />} />
          <Route path="/libros/detalle/:id" element={<LibrosDetalle />} />
          
          {/* Empleados */}
          <Route path="/empleados" element={<Empleados />} />
          <Route path="/empleados/nuevo" element={<Add />} />
          <Route path="/empleados/editar/:id" element={<Editar />} />
          <Route path="/empleados/detalle/:id" element={<Detalle />} />
        </Routes>
      </div>
    </BrowserRouter>
  )
}

export default App