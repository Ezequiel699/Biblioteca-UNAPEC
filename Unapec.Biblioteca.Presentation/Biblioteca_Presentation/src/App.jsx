import './App.css'
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import NavBar from './Components/NavBar/NavBar'
import Bibliografias from './Components/Gestion/Bibliografias/Bibliografias'
import Editoras from './Components/Gestion/Editoras/Editoras'
import Home from './Components/Home/Home'
import Ciencias from './Components/Gestion/Ciencias/Ciencias'
import Idiomas from './Components/Gestion/Idiomas/Idiomas'
import Editar from './Components/Crud/Edit'
import Detalle from './Components/Crud/Details'
import Add from './Components/Crud/Add'

function App() {
  return (
    <BrowserRouter>
      <div className='page-container'>
        <NavBar />
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/tipos-bibliografia" element={<Bibliografias />} />
          <Route path="/editoras" element={<Editoras />} />
          <Route path="/ciencias" element={<Ciencias />} />
          <Route path="/idiomas" element={<Idiomas />} />
          <Route path="/:recurso/editar/:id" element={<Editar />} />
          <Route path="/:recurso/detalle/:id" element={<Detalle />} />
          <Route path='/:recurso/nuevo' element={<Add />} />
        </Routes>
      </div>
    </BrowserRouter>
  )
}

export default App