import axios from 'axios'

const authClient = axios.create({
  baseURL: 'https://localhost:7276'
})

export const loginUser = (username, password) => {
  return authClient.post('/api/authentication/login', { username, password })
}

export const registerUser = (username, email, password) => {
  return authClient.post('/api/users/', { username, email, password })
}