import './NavBar.css'
import { Link, useNavigate } from 'react-router-dom'
import authService from '../Login/authService'

const NavBar = () => {
    const navigate = useNavigate();

    const handleLogout = () => {
        if (window.confirm('¿Está seguro que desea cerrar sesión?')) {
            authService.logout();
            navigate('/login');
        }
    };

    return(
        <header className='header'>
            <div className='navbar-container'>
                <img className='profile-picture' src="https://cdn-icons-png.flaticon.com/512/3135/3135715.png" alt="Profile Picture" />
                <nav className='navbar'>
                    <ul className='navbar-list'>
                        <li className='navbar-option'><Link to="/home">Home</Link></li>
                        <li className='navbar-option'><Link to="/libros">Books</Link></li>
                        <li className='navbar-option'><Link to="/usuarios">Users</Link></li>
                        <li className='navbar-option'><Link to="/empleados">Employee</Link></li>
                        <li className='navbar-option'><a href="">🔍</a></li>
                    </ul>
                </nav>
                <button className='btn-logout' onClick={handleLogout}>
                    Logout
                </button>
            </div>
            <div className='bar'></div>
        </header>
    );
}

export default NavBar;