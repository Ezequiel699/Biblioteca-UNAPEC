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
import BibliografiasDetail from './Components/Gestion/Bibliografias/BibliografiasDetail'
import BibliografiasAdd from './Components/Gestion/Bibliografias/BibliografiasAdd'
import BibliografiasEdit from './Components/Gestion/Bibliografias/BibliografiasEdit'
import EditorasAdd from './Components/Gestion/Editoras/EditorasAdd'
import EditorasEdit from './Components/Gestion/Editoras/EditorasEdit'
import EditorasDetalle from './Components/Gestion/Editoras/EditorasDetalle'
import CienciasAdd from './Components/Gestion/Ciencias/CienciasAdd'
import CienciasEdit from './Components/Gestion/Ciencias/CienciasEdit'
import IdiomasAdd from './Components/Gestion/Idiomas/IdiomasAdd'
import IdiomasEdit from './Components/Gestion/Idiomas/IdiomasEdit'
import IdiomasDetalle from './Components/Gestion/Idiomas/IdiomasDetalle'
import AutoresAdd from './Components/Gestion/Autores/AutoresAdd'
import AutoresEdit from './Components/Gestion/Autores/AutoresEdit'
import AutoresDetails from './Components/Gestion/Autores/AutoresDetalle'
import UsuariosAdd from './Components/Gestion/Usuarios/UsuariosAdd'
import UsuariosEditar from './Components/Gestion/Usuarios/UsuariosEdit'
import UsuariosDetalle from './Components/Gestion/Usuarios/UsuariosDetalle'


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
          <Route path="/tipos-bibliografia/nuevo" element={<BibliografiasAdd />} />
          <Route path="/tipos-bibliografia/editar/:id" element={<BibliografiasEdit />} />
          <Route path="/tipos-bibliografia/detalle/:id" element={<BibliografiasDetail />} />
          
          {/* Editoras */}
          <Route path="/editoras" element={<Editoras />} />
          <Route path="/editoras/nuevo" element={<EditorasAdd />} />
          <Route path="/editoras/editar/:id" element={<EditorasEdit />} />
          <Route path="/editoras/detalle/:id" element={<EditorasDetalle />} />
          
          {/* Ciencias */}
          <Route path="/ciencias" element={<Ciencias />} />
          <Route path="/ciencias/nuevo" element={<CienciasAdd />} />
          <Route path="/ciencias/editar/:id" element={<CienciasEdit />} />
          <Route path="/ciencias/detalle/:id" element={<CienciasEdit />} />
          
          {/* Idiomas */}
          <Route path="/idiomas" element={<Idiomas />} />
          <Route path="/idiomas/nuevo" element={<IdiomasAdd />} />
          <Route path="/idiomas/editar/:id" element={<IdiomasEdit />} />
          <Route path="/idiomas/detalle/:id" element={<IdiomasDetalle />} />
          
          {/* Autores */}
          <Route path="/autores" element={<Autores />} />
          <Route path="/autores/nuevo" element={<AutoresAdd />} />
          <Route path="/autores/editar/:id" element={<AutoresEdit />} />
          <Route path="/autores/detalle/:id" element={<AutoresDetails />} />
          
          {/* Usuarios */}
          <Route path="/usuarios" element={<Usuarios />} />
          <Route path="/usuarios/nuevo" element={<UsuariosAdd />} />
          <Route path="/usuarios/editar/:id" element={<UsuariosEditar />} />
          <Route path="/usuarios/detalle/:id" element={<UsuariosDetalle />} />
          
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