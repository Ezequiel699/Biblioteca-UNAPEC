import './Content.css';

const Content = () => {
  const cards = [
    { title: 'Bibliografías', img: 'https://www.julianmarquina.es/wp-content/uploads/Los-libros-ocupan-un-lugar-muy-importante-en-la-vida-de-las-personas.jpg', href: '/bibliografias' },
    { title: 'Editoras',      img: 'https://antinomiaslibro.wordpress.com/wp-content/uploads/2016/03/maracs-edit.jpg', href: '/editoras' },
    { title: 'Ciencias',      img: 'https://i0.wp.com/www.atmosfera.unam.mx/wp-content/uploads/2019/05/imagen.png?fit=1177%2C869&ssl=1', href: '/ciencias' },
    { title: 'Idiomas',       img: 'https://cdn-icons-png.flaticon.com/512/2037/2037492.png', href: '/idiomas' },
  ];

  return (
    <div className="content">
      {cards.map(c => (
        <div key={c.href} className="card-wrapper">
          <h3 className="card-title">{c.title}</h3>
          <a href={c.href} className="card">
            <img src={c.img} alt={c.title} className="card__bg" />
          </a>
        </div>
      ))}
    </div>
  );
};

export default Content;