import axios from 'axios'

const orderClient = axios.create({
  baseURL: 'https://localhost:44310'
})

const getAuthHeader = (token) => ({
  headers: { Authorization: `Bearer ${token}` }
})

export const getAllOrders = (token) => {
  return orderClient.get('/api/orders', getAuthHeader(token))
}

export const getOrderById = (id, token) => {
  return orderClient.get(`/api/orders/${id}`, getAuthHeader(token))
}

export const createOrder = (data, token) => {
  return orderClient.post('/api/orders', data, getAuthHeader(token))
}

export const deleteOrder = (id, token) => {
  return orderClient.delete(`/api/orders/${id}`, getAuthHeader(token))
}

export const updateOrderStatus = (id, status, token) => {
  return orderClient.put(`/api/orders/${id}/status`, { status }, getAuthHeader(token))
}