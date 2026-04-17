import axios from 'axios'

const accountClient = axios.create({
  baseURL: 'https://localhost:7276'
})

const getAuthHeader = (token) => ({
  headers: { Authorization: `Bearer ${token}` }
})

export const getAllUsers = (token) => {
  return accountClient.get('/api/users', getAuthHeader(token))
}

export const getUserById = (id, token) => {
  return accountClient.get(`/api/users/${id}`, getAuthHeader(token))
}

export const updateUser = (id, data, token) => {
  return accountClient.put(`/api/users/${id}`, data, getAuthHeader(token))
}

export const deleteUser = (id, token) => {
  return accountClient.delete(`/api/users/${id}`, getAuthHeader(token))
}

export const updateUserRole = (id, role, token) => {
  return accountClient.patch(`/api/users/${id}`, { role }, getAuthHeader(token))
}