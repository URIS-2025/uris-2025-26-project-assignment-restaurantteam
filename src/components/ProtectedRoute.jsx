import { Navigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

function ProtectedRoute({ children }) {
  const { token } = useAuth()

  if (!token) {
    return <Navigate to="/login" state={{ from: window.location.pathname }} replace />
  }

  return children
}

export default ProtectedRoute