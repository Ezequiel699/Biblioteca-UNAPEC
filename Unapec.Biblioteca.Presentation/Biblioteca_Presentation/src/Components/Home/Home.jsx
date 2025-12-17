import { Link } from 'react-router-dom';
import './Home.css';

const Home = () => (
  <section className="home">
    <h1 className="home__title">Panel de administración</h1>
    <article className="home__sub">Sistema de gestión de bibliotecas</article>

    <div className="home__grid">
      {/* ---- Tipos de Bibliografía ---- */}
      <Link to="/tipos-bibliografia" className="home__card">
        <span className="home__ico">📚</span>
        <span className="home__name">Tipos de Bibliografía</span>
        <span className="home__desc">Administra tipos de bibliografía.</span>
      </Link>

      {/* ---- Editoras ---- */}
      <Link to="/editoras" className="home__card">
        <span className="home__ico">🏢</span>
        <span className="home__name">Editoras</span>
        <span className="home__desc">Gestiona editoriales y contactos.</span>
      </Link>

      {/* ---- Ciencias ---- */}
      <Link to="/ciencias" className="home__card">
        <span className="home__ico">🔬</span>
        <span className="home__name">Ciencias</span>
        <span className="home__desc">Revisa áreas y categorías científicas.</span>
      </Link>

      {/* ---- Idiomas ---- */}
      <Link to="/idiomas" className="home__card">
        <span className="home__ico">🌐</span>
        <span className="home__name">Idiomas</span>
        <span className="home__desc">Controla traducciones e idiomas disponibles.</span>
      </Link>

      {/* ---- Autores ---- */}
      <Link to="/autores" className="home__card">
        <span className="home__ico">✍🏻</span>
        <span className="home__name">Autores</span>
        <span className="home__desc">Gestiona los autores disponibles.</span>
      </Link>

      {/* ---- Usuarios ---- */}
      <Link to="/usuarios" className="home__card">
        <span className="home__ico">👨🏻‍💻</span>
        <span className="home__name">Usuarios</span>
        <span className="home__desc">Gestiona los usuarios del sistema.</span>
      </Link>

      {/* ---- Libros ---- */}
      <Link to="/libros" className="home__card">
        <span className="home__ico">📖</span>
        <span className="home__name">Libros</span>
        <span className="home__desc">Gestiona el catálogo de libros.</span>
      </Link>

      {/* ---- Empleados ---- */}
      <Link to="/empleados" className="home__card">
        <span className="home__ico">👔</span>
        <span className="home__name">Empleados</span>
        <span className="home__desc">Gestiona el personal de la biblioteca.</span>
      </Link>

          <Link to="/prestamos" className="home__card">
        <span className="home__ico">⏳</span>
        <span className="home__name">Préstamos</span>
        <span className="home__desc">Registra y gestiona préstamos de libros.</span>
      </Link>
    </div>
  </section>
);

export default Home;