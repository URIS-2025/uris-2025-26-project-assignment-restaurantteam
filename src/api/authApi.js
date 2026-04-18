import axios from 'axios'
const hostUrl = "https://accountservice-py1t.onrender.com"
const localUrl = "https://localhost:7276"
const authClient = axios.create({
  baseURL: hostUrl
})

export const loginUser = (username, password) => {
  return authClient.post('/api/authentication/login', { username, password })
}

export const registerUser = (username, email, password) => {
  return authClient.post('/api/users/', { username, email, password })
}