import axios from 'axios'

const reservationClient = axios.create({
  baseURL: 'https://localhost:7149'
})

const getAuthHeader = (token) => ({
  headers: { Authorization: `Bearer ${token}` }
})

// Rezervacije
export const getAllReservations = (token) => {
  return reservationClient.get('/api/v2/reservations', getAuthHeader(token))
}

export const getReservationById = (id, token) => {
  return reservationClient.get(`/api/v2/reservations/${id}`, getAuthHeader(token))
}

export const createReservation = (id, data, token) => {
  return reservationClient.post(`/api/v2/users/${id}/reservations/`, data, getAuthHeader(token))
}

export const updateReservation = (id, data, token) => {
  return reservationClient.put(`/api/v2/reservations/${id}`, data, getAuthHeader(token))
}

export const deleteReservation = (id, token) => {
  return reservationClient.delete(`/api/v2/reservations/${id}`, getAuthHeader(token))
}

// Stolovi
export const getAllTables = () => {
  return reservationClient.get('/api/tables')
}

export const createTable = (data, token) => {
  return reservationClient.post('/api/tables', data, getAuthHeader(token))
}

export const updateTable = (id, data, token) => {
  return reservationClient.put(`/api/tables/${id}`, data, getAuthHeader(token))
}

export const deleteTable = (id, token) => {
  return reservationClient.delete(`/api/tables/${id}`, getAuthHeader(token))
}