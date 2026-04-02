import axios from 'axios'

const authClient = axios.create({
  baseURL: 'http://localhost:5107'
})

export const loginUser = (username, password) => {
  return authClient.post('/api/Authentication/login', { username, password })
}

export const registerUser = (username, email, password) => {
  return authClient.post('/api/Authentication/register', { username, email, password })
}