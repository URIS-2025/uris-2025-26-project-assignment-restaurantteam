import axios from 'axios'

const accountClient = axios.create({
  baseURL: 'https://localhost:44339'
})

const getAuthHeader = (token) => ({
  headers: { Authorization: `Bearer ${token}` }
})

export const getAllUsers = (token) => {
  return accountClient.get('/api/Users', getAuthHeader(token))
}

export const getUserById = (id, token) => {
  return accountClient.get(`/api/Users/${id}`, getAuthHeader(token))
}

export const updateUser = (id, data, token) => {
  return accountClient.put(`/api/Users/${id}`, data, getAuthHeader(token))
}

export const deleteUser = (id, token) => {
  return accountClient.delete(`/api/Users/${id}`, getAuthHeader(token))
}

export const updateUserRole = (id, role, token) => {
  return accountClient.put(`/api/Users/${id}/role`, { role }, getAuthHeader(token))
}