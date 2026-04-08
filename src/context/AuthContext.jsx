import { createContext, useState, useContext } from 'react'

const AuthContext = createContext()

export function AuthProvider({ children }) {
  const [token, setToken] = useState(sessionStorage.getItem('token') || null)
  const [role, setRole] = useState(sessionStorage.getItem('role') || null)
  const [userId, setUserId] = useState(sessionStorage.getItem('userId') || null)
  const [username, setUsername] = useState(sessionStorage.getItem('username') || null)

  const login = (newToken, newRole, newUserId, newUsername) => {
    setToken(newToken)
    setRole(newRole)
    setUserId(newUserId)
    setUsername(newUsername)
    sessionStorage.setItem('token', newToken)
    sessionStorage.setItem('role', newRole)
    sessionStorage.setItem('userId', newUserId)
    sessionStorage.setItem('username', newUsername)
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