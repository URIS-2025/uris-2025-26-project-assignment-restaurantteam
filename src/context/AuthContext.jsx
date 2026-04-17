import { createContext, useState, useContext } from 'react'
import { jwtDecode } from 'jwt-decode'

const AuthContext = createContext()

export function AuthProvider({ children }) {
  const [token, setToken] = useState(sessionStorage.getItem('token') || null)
  const [role, setRole] = useState(sessionStorage.getItem('role') || null)
  const [userId, setUserId] = useState(sessionStorage.getItem('userId') || null)
  const [username, setUsername] = useState(sessionStorage.getItem('username') || null)

  const login = (newToken) => {
    
    setToken(newToken)
    const decoded = jwtDecode(newToken)
    setRole(decoded.role)
    setUserId(decoded.nameid)
    setUsername(decoded.unique_name)
    sessionStorage.setItem('token', newToken)
    sessionStorage.setItem('role', role)
    sessionStorage.setItem('userId', userId)
    sessionStorage.setItem('username', username)
  }

  const logout = () => {
    setToken(null)
    setRole(null)
    setUserId(null)
    setUsername(null)
    sessionStorage.removeItem('token')
    sessionStorage.removeItem('role')
    sessionStorage.removeItem('userId')
    sessionStorage.removeItem('username')
  }

  return (
    <AuthContext.Provider value={{ token, role, userId, username, login, logout }}>
      {children}
    </AuthContext.Provider>
  )
}

export function useAuth() {
  return useContext(AuthContext)
}