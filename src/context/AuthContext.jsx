import { createContext, useState, useContext } from 'react'

const AuthContext = createContext()

export function AuthProvider({ children }) {
  const [token, setToken] = useState(sessionStorage.getItem('token') || null)
  const [role, setRole] = useState(sessionStorage.getItem('role') || null)

  const login = (newToken, newRole) => {
    setToken(newToken)
    setRole(newRole)
    sessionStorage.setItem('token', newToken)
    sessionStorage.setItem('role', newRole)
  }

  const logout = () => {
    setToken(null)
    setRole(null)
    sessionStorage.removeItem('token')
    sessionStorage.removeItem('role')
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