import axios from 'axios'

const reservationClient = axios.create({
  baseURL: 'https://localhost:44349'
})

const getAuthHeader = (token) => ({
  headers: { Authorization: `Bearer ${token}` }
})

// Rezervacije
export const getAllReservations = (token) => {
  return reservationClient.get('/api/reservations', getAuthHeader(token))
}

export const getReservationById = (id, token) => {
  return reservationClient.get(`/api/reservations/${id}`, getAuthHeader(token))
}

export const createReservation = (data, token) => {
  return reservationClient.post('/api/reservations', data, getAuthHeader(token))
}

export const updateReservation = (id, data, token) => {
  return reservationClient.put(`/api/reservations/${id}`, data, getAuthHeader(token))
}

export const deleteReservation = (id, token) => {
  return reservationClient.delete(`/api/reservations/${id}`, getAuthHeader(token))
}

// Stolovi
export const getAllTables = () => {
  return reservationClient.get('/api/reservations/tables')
}

export const createTable = (data, token) => {
  return reservationClient.post('/api/reservations/tables', data, getAuthHeader(token))
}

export const updateTable = (id, data, token) => {
  return reservationClient.put(`/api/reservations/tables/${id}`, data, getAuthHeader(token))
}

export const deleteTable = (id, token) => {
  return reservationClient.delete(`/api/reservations/tables/${id}`, getAuthHeader(token))
}