import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { registerUser } from '../api/authApi'

function RegisterPage() {
  const [username, setUsername] = useState('')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const [success, setSuccess] = useState('')
  const [loading, setLoading] = useState(false)

  const navigate = useNavigate()

  const handleRegister = async (e) => {
    e.preventDefault()
    setError('')
    setSuccess('')
    setLoading(true)

    try {
      await registerUser(username, email, password)
      setSuccess('Registracija uspješna! Preusmjeravamo na login...')
      setTimeout(() => navigate('/login'), 2000)
    } catch (err) {
      setError('Greška pri registraciji. Pokušaj ponovo.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="row justify-content-center">
      <div className="col-md-4">
        <h2 className="mb-4">Registracija</h2>
        {error && <div className="alert alert-danger">{error}</div>}
        {success && <div className="alert alert-success">{success}</div>}
        <form onSubmit={handleRegister}>
          <div className="mb-3">
            <label className="form-label">Korisničko ime</label>
            <input
              type="text"
              className="form-control"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
            />
          </div>
          <div className="mb-3">
            <label className="form-label">Email</label>
            <input
              type="email"
              className="form-control"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>
          <div className="mb-3">
            <label className="form-label">Lozinka</label>
            <input
              type="password"
              className="form-control"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>
          <button type="submit" className="btn btn-dark w-100" disabled={loading}>
            {loading ? 'Učitavanje...' : 'Registruj se'}
          </button>
        </form>
        <p className="mt-3 text-center">
          Već imaš nalog? <a href="/login">Prijavi se</a>
        </p>
      </div>
    </div>
  )
}

export default RegisterPage