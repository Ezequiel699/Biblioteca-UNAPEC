import './NavBar.css'

const NavBar = () => {
    return(
        <header className='header'>
            <img className='profile-picture' src="https://cdn-icons-png.flaticon.com/512/3135/3135715.png" alt="Profile Picture" />
            <nav className='navbar'>
                <ul className='navbar-list'>
                    <li className='navbar-option'><a href="">Home</a></li>
                    <li className='navbar-option'><a href="">Users</a></li>
                    <li className='navbar-option'><a href="">Books</a></li>
                    <li className='navbar-option'><a href="">🔍</a></li>
                </ul>
            </nav>
        </header>
    );
}

export default NavBar;


/*            <nav className='navbar'>
                <ul className='navbar-list'>
                    <li className='navbar-option'><a href="">🔍</a></li>
                    <li className='navbar-option'><a href="">Home</a></li>
                    <li className='navbar-option'><a href="">Fast Reading</a></li>
                    <li className='navbar-option'><a href="">Inmersive</a></li>
                </ul>
            </nav>
             */