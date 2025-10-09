import { Link } from 'react-router-dom';
import './Home.css';

const Home = () => (
  <section className="home">
    <h1 className="home__title">Panel de administración</h1>
    <article className="home__sub">Sistema de gestión de bibliotecas</article>

    <div className="home__grid">
      {/* ---- Bibliografías ---- */}
      <Link to="/bibliografias" className="home__card">
        <span className="home__ico">📚</span>
        <span className="home__name">Bibliografías</span>
        <span className="home__desc">Administra autores, títulos y ediciones.</span>
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
    </div>
  </section>
);

export default Home;