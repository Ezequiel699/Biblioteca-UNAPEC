import './NavBar.css'
import { Link } from 'react-router-dom'

const NavBar = () => {
    return(
        <header className='header'>
            <div className='navbar-container'>
                <img className='profile-picture' src="https://cdn-icons-png.flaticon.com/512/3135/3135715.png" alt="Profile Picture" />
                <nav className='navbar'>
                    <ul className='navbar-list'>
                        <li className='navbar-option'><Link to="/">Home</Link></li>
                        <li className='navbar-option'><a href="">Books</a></li>
                        <li className='navbar-option'><a href="">Users</a></li>
                        <li className='navbar-option'><a href="">Employee</a></li>
                        <li className='navbar-option'><a href="">🔍</a></li>
                    </ul>
                </nav>
            </div>
            <div className='bar'></div>
        </header>
    );
}

export default NavBar;