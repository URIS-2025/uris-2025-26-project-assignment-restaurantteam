import { createContext, useState, useContext } from 'react'

const AuthContext = createContext()

export function AuthProvider({ children }) {
  const [token, setToken] = useState(localStorage.getItem('token') || null)
  const [role, setRole] = useState(localStorage.getItem('role') || null)

  const login = (newToken, newRole) => {
    setToken(newToken)
    setRole(newRole)
    localStorage.setItem('token', newToken)
    localStorage.setItem('role', newRole)
  }

  const logout = () => {
    setToken(null)
    setRole(null)
    localStorage.removeItem('token')
    localStorage.removeItem('role')
  }

  return (
    <AuthContext.Provider value={{ token, role, login, logout }}>
      {children}
    </AuthContext.Provider>
  )
}

export function useAuth() {
  return useContext(AuthContext)
}