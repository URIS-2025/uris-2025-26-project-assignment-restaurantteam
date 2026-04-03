import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

function Navbar() {
  const { token, role, logout } = useAuth()
  const navigate = useNavigate()

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
      <div className="container">
        <Link className="navbar-brand" to="/">🍽️ Restaurant App</Link>
        <div className="navbar-nav ms-auto">
          <Link className="nav-link" to="/menu">Meni</Link>
          {token && <Link className="nav-link" to="/orders">Narudžbine</Link>}
          {token && <Link className="nav-link" to="/reservations">Rezervacije</Link>}
          {token && role === 'ADMIN' && (
            <Link className="nav-link" to="/users">Korisnici</Link>
          )}
          {!token && <Link className="nav-link" to="/login">Login</Link>}
          {!token && <Link className="nav-link" to="/register">Register</Link>}
          {token && (
            <button className="btn btn-outline-light btn-sm ms-2" onClick={handleLogout}>
              Odjavi se
            </button>
          )}
        </div>
      </div>
    </nav>
  )
}

export default Navbar